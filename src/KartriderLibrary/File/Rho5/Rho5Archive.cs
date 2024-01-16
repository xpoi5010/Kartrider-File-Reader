using KartLibrary.Consts;
using KartLibrary.Encrypt;
using KartLibrary.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KartLibrary.File
{
    /// <summary>
    /// Rho5Archive represent a set of same data pack Rho5 files. 
    /// </summary>
    public class Rho5Archive : IRhoArchive<Rho5Folder, Rho5File>
    {
        #region Members
        private Rho5Folder _rootFolder;
        // private FileStream? _rho5Stream;
        private Dictionary<int, FileStream> _rho5Streams;
        private HashSet<Rho5FileHandler> _fileHandlers;
        private Dictionary<int, int> _dataBeginPoses;
        private bool _closed;
        #endregion

        #region Properties
        public Rho5Folder RootFolder => _rootFolder;

        public bool IsClosed => _closed;
        #endregion

        #region Constructors
        public Rho5Archive()
        {
            _rootFolder = new Rho5Folder();
            _fileHandlers = new HashSet<Rho5FileHandler>();
            _rho5Streams = new Dictionary<int, FileStream>();
            _dataBeginPoses = new Dictionary<int, int>();
        }
        #endregion

        #region Methods
        public void Open(string dataPackPath, string dataPackName, CountryCode region)
        {
            if (!Directory.Exists(dataPackPath))
                throw new Exception($"{dataPackPath} doesn't exists.");
            DirectoryInfo dataPackDir = new DirectoryInfo(dataPackPath);
            foreach (FileInfo fileInfo in dataPackDir.GetFiles())
            {
                Regex parrern = new Regex($@"^{dataPackName}_(\d{{5}})\.rho5$");
                Match match = parrern.Match(fileInfo.Name);
                if (match.Success)
                {
                    string dataPackIDStr = match.Groups[1].Value;
                    int dataPackID = Convert.ToInt32(dataPackIDStr);
                    openSingleFile(dataPackID, fileInfo.FullName, region);
                }
            }
        }

        public void Save(string dataPackPath, string dataPackName, CountryCode region, SavePattern savePattern = SavePattern.Auto)
        {
            int maxOpenedPartID = _rho5Streams.Count == 0 ? -1 : _rho5Streams.Select(x => x.Key).Max();
            string mixingStr = getMixingString(region);
            Dictionary<int, bool> isPartModified = new Dictionary<int, bool>();
            Dictionary<int, Queue<Rho5File>> oldFilesQueues = new Dictionary<int, Queue<Rho5File>>();
            Queue<Rho5File> newFilesQueue = new Queue<Rho5File>();
            Dictionary<int, int> approxiPartSize = new Dictionary<int, int>();
            for (int i = 0; i <= maxOpenedPartID; i++)
            {
                isPartModified.Add(i, false);
                oldFilesQueues.Add(i, new Queue<Rho5File>());
                approxiPartSize.Add(i, 0);
            }
            Queue<Rho5Folder> folderQueue = new Queue<Rho5Folder>();
            folderQueue.Enqueue(_rootFolder);
            while(folderQueue.Count > 0)
            {
                Rho5Folder curFolder = folderQueue.Dequeue();
                foreach(Rho5Folder subFolder in curFolder.Folders)
                    folderQueue.Enqueue(subFolder);
                foreach(Rho5File file in curFolder.Files)
                {
                    if(file.IsModified && file._dataPackID >= 0)
                    {
                        isPartModified[file._dataPackID] = true;
                    }
                    if(file._dataPackID < 0)
                        newFilesQueue.Enqueue(file);
                    else
                        oldFilesQueues[file._dataPackID].Enqueue(file);
                }
            }

            for(int i = 0; i <= maxOpenedPartID; i++)
            {
                if(savePattern == SavePattern.AlwaysRegeneration || (savePattern == SavePattern.GenerateIfModified && isPartModified[i]))
                {
                    saveSingleFileTo(dataPackPath, $"{dataPackName}", i, mixingStr, oldFilesQueues[i], int.MaxValue);
                }
            }

            for(int i = maxOpenedPartID + 1; newFilesQueue.Count > 0; i++)
            {
                saveSingleFileTo(dataPackPath, $"{dataPackName}", i, mixingStr, newFilesQueue, 10485760);
            }
        }

        private void openSingleFile(int dataPackID, string filePath, CountryCode region)
        {
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException($"");
            FileStream rho5Stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            string mixingStr = getMixingString(region);
            string fileName = Path.GetFileName(filePath);
            Rho5DecryptStream decryptStream = new Rho5DecryptStream(rho5Stream, fileName, mixingStr);
            BinaryReader reader = new BinaryReader(decryptStream);

            int headerOffset = getHeaderOffset(fileName);
            int filesInfoOffset = headerOffset + getFilesInfoOffset(fileName);

            // Reads header
            decryptStream.Seek(headerOffset, SeekOrigin.Begin);
            int headerCrc = reader.ReadInt32();
            int packageVersion = reader.ReadByte();
            int fileCount = reader.ReadInt32();
            if (headerCrc != packageVersion + fileCount)
                throw new Exception("Rho5 header crc mismatch.");
            if (packageVersion != 2)
                throw new Exception("unsupported package version.");

            // Reads files info
            decryptStream.Seek(filesInfoOffset, SeekOrigin.Begin);
            decryptStream.SetToFilesInfoKey(fileName, mixingStr);
            for (int i = 0; i < fileCount; i++)
            {
                string fileFullPath = reader.ReadText();
                int fileInfoChecksum = reader.ReadInt32();
                int unknown = reader.ReadInt32();
                int offset = reader.ReadInt32();
                int decompressedSize = reader.ReadInt32();
                int compressedSize = reader.ReadInt32();
                byte[] fileChksum = reader.ReadBytes(0x10);

                int verifyChksum = unknown + offset + decompressedSize + compressedSize;
                foreach (byte b in fileChksum)
                    verifyChksum += b;

                if (fileInfoChecksum != verifyChksum)
                    throw new Exception("fileInfo checksum mismatch.");

                byte[] decryptKey = Rho5Key.GetPackedFileKey(fileChksum, Rho5Key.GetFileKey_U1(mixingStr), fileFullPath);

                Rho5FileHandler fileHandler = new Rho5FileHandler(this, dataPackID, offset, decompressedSize, compressedSize, decryptKey, fileChksum);

                Rho5Folder curFolder = _rootFolder;
                string[] splitedPath = fileFullPath.Split('/');
                for (int j = 0; j < splitedPath.Length - 1; j++)
                {
                    string curFolderName = splitedPath[j];
                    if (!curFolder.ContainsFolder(curFolderName))
                    {
                        curFolder.AddFolder(new Rho5Folder()
                        {
                            Name = curFolderName
                        });
                        curFolder.appliedChanges();
                    }
                    curFolder = curFolder.GetFolder(curFolderName);
                }
                Rho5File rho5File = new Rho5File();
                rho5File.DataSource = new Rho5DataSource(fileHandler);
                rho5File.Name = splitedPath[^1];
                rho5File._dataPackID = dataPackID;
                rho5File.appliedChanges();
                curFolder.AddFile(rho5File);
                curFolder.appliedChanges();

                _fileHandlers.Add(fileHandler);
            }
            if (!_dataBeginPoses.ContainsKey(dataPackID))
                _dataBeginPoses.Add(dataPackID, 0);
            _dataBeginPoses[dataPackID] = (int)decryptStream.Position + 0x3FF & 0x7FFFFC00;

            _rho5Streams.Add(dataPackID, rho5Stream);

        }

        private void saveSingleFileTo(string dataPackPath, string dataPackName, int dataPackID, string mixingStr, Queue<Rho5File> fileQueue, int maxSize)
        {
            string fullName = $"{Path.GetFullPath(dataPackPath)}\\{dataPackName}_{dataPackID:00000}.rho5";
            string fullDirName = Path.GetDirectoryName(fullName) ?? "";
            if (!Directory.Exists(fullDirName))
            {
                throw new Exception("directory not exists.");
            }
            string outFileName = Path.GetFileName(fullName);
            string outFileNameWithoutExt = Path.GetFileNameWithoutExtension(fullName);
            string tmpFileName = $"tmptau_{outFileNameWithoutExt}_{Environment.TickCount64:x16}.tmp";
            string tmpFileFullName = Path.Combine(dataPackPath, outFileName);

            FileStream tmpOutStream = new FileStream(tmpFileFullName, FileMode.Create);
            BufferedStream tmpBUfferedStream = new BufferedStream(tmpOutStream, 0x1000000);
            Rho5EncryptStream outEncryptStream = new Rho5EncryptStream(tmpBUfferedStream);
            BinaryWriter outEncryptWriter = new BinaryWriter(outEncryptStream);
            
            int headerOffset = getHeaderOffset(outFileName);
            int filesInfoOffset = headerOffset + getFilesInfoOffset(outFileName);

            outEncryptStream.SetLength(headerOffset);
            outEncryptStream.Seek(headerOffset, SeekOrigin.Begin);
            outEncryptStream.SetToHeaderKey(outFileName, mixingStr);
            int headerCrc = 2 + fileQueue.Count;
            outEncryptWriter.Write(headerCrc);
            outEncryptWriter.Write((byte)2);
            outEncryptWriter.Write(fileQueue.Count);
            
            // Enqueue all files
            int filesInfoDataLen = 0;
            foreach (Rho5File file in fileQueue)
                filesInfoDataLen += (0x28) + (file.FullName.Length << 1);
            MemoryStream filesInfoStream = new MemoryStream(filesInfoDataLen);
            BinaryWriter filesInfoWriter = new BinaryWriter(filesInfoStream);

            int dataBeginOffset = filesInfoOffset + filesInfoDataLen + 0x3FF & 0x7FFFFC00;
            int dataOffset = dataBeginOffset;
            outEncryptStream.SetLength(dataBeginOffset);
            while (fileQueue.Count > 0 && tmpOutStream.Length <= maxSize)
            {
                Rho5File file = fileQueue.Dequeue();
                if (file.DataSource is null)
                    throw new Exception();
                byte[] data = file.DataSource.GetBytes();
                byte[] processedData;
                byte[] fileChksum = MD5.HashData(data);
                byte[] encryptKey = Rho5Key.GetPackedFileKey(fileChksum, Rho5Key.GetFileKey_U1(mixingStr), file.FullName);
                using (MemoryStream memStream = new MemoryStream())
                {
                    Ionic.Zlib.ZlibStream compressStream = new Ionic.Zlib.ZlibStream(memStream, Ionic.Zlib.CompressionMode.Compress, true);
                    compressStream.Write(data, 0, data.Length);
                    compressStream.Flush();
                    compressStream.Close();
                    processedData = memStream.ToArray();
                    Rho5EncryptStream encryptStream = new Rho5EncryptStream(memStream, encryptKey);
                    encryptStream.Seek(0, SeekOrigin.Begin);
                    encryptStream.Write(processedData, 0, Math.Min(processedData.Length, 0x400));
                    encryptStream.Flush();
                    processedData = memStream.ToArray();
                }
                int fileInfoChksum = 7 + (dataOffset - dataBeginOffset >> 10) + data.Length + processedData.Length;
                foreach (byte b in fileChksum)
                    fileInfoChksum += b;
                filesInfoWriter.WriteKRString(file.FullName);
                filesInfoWriter.Write(fileInfoChksum);
                filesInfoWriter.Write(0x00000007);
                filesInfoWriter.Write(dataOffset - dataBeginOffset >> 10);
                filesInfoWriter.Write(data.Length);
                filesInfoWriter.Write(processedData.Length);
                filesInfoWriter.Write(fileChksum);

                outEncryptStream.SetKey(encryptKey);
                outEncryptStream.Seek(dataOffset, SeekOrigin.Begin);
                outEncryptStream.Write(processedData, 0, processedData.Length);
                outEncryptStream.Flush();

                dataOffset = dataOffset + processedData.Length + 0x3FF & 0x7FFFFC00;
                outEncryptStream.SetLength(dataOffset);


                Rho5FileHandler fileHandler = new Rho5FileHandler(this, dataPackID, dataOffset - dataBeginOffset >> 10, data.Length, processedData.Length, encryptKey, fileChksum);
                Rho5DataSource rho5DataSource = new Rho5DataSource(fileHandler);
                if (file.DataSource is not Rho5DataSource)
                    file.DataSource?.Dispose();
                file.DataSource = rho5DataSource;
                _fileHandlers.Add(fileHandler);
            }


            outEncryptStream.Seek(filesInfoOffset, SeekOrigin.Begin);
            outEncryptStream.SetToFilesInfoKey(outFileName, mixingStr);
            byte[] filesInfoData = filesInfoStream.ToArray();
            filesInfoStream.Close();
            outEncryptStream.Write(filesInfoData, 0, filesInfoData.Length);

            outEncryptStream.Flush();
            tmpBUfferedStream.Flush();
            tmpOutStream.Close();
        }

        public void Close()
        {
            if (_closed)
                throw new Exception("this archive is close or is not open from rho5 file.");
            foreach (FileStream rho5Stream in _rho5Streams.Values)
            {
                if (rho5Stream.CanRead)
                    rho5Stream.Close();
                rho5Stream.Dispose();
            }
            _rho5Streams.Clear();
            _rootFolder.Clear();
            releaseAllHandles();
            _closed = true;
        }

        public void Dispose()
        {
            foreach (FileStream rho5Stream in _rho5Streams.Values)
            {
                if (rho5Stream.CanRead)
                    rho5Stream.Close();
                rho5Stream.Dispose();
            }
            _rho5Streams.Clear();
            releaseAllHandles();
        }

        private int getHeaderOffset(string fileName)
        {
            fileName = fileName.ToLower();
            int sum = 0;
            foreach (char c in fileName) sum += c;
            long mpl = sum * 0xA41A41A5L >> 32;
            int result = sum - (int)mpl;
            result >>= 1;
            result += (int)mpl;
            result >>= 8;
            result *= 0x138;
            result = sum - result + 0x1E;
            return result;
        }

        private int getFilesInfoOffset(string fileName)
        {
            fileName = fileName.ToLower();
            int sum = 0;
            foreach (char c in fileName) sum += c;
            sum *= 3;
            long mpl = sum * 0x3521CFB3L >> 32;
            int result = sum - (int)mpl;
            result >>= 1;
            result += (int)mpl;
            result >>= 7;
            result *= 0xD4;
            result = sum - result + 0x2A;
            return result;
        }

        private string getMixingString(CountryCode region)
        {
            switch (region)
            {
                case CountryCode.KR:
                    return "y&errfV6GRS!e8JL";
                case CountryCode.CN:
                    return "d$Bjgfc8@dH4TQ?k";
                case CountryCode.TW:
                    return "t5rHKg-g9BA7%=qD";
                default:
                    throw new Exception("");
            }
        }

        private void releaseAllHandles()
        {
            foreach (Rho5FileHandler fileHandler in _fileHandlers)
                fileHandler.releaseHandler();
            _fileHandlers.Clear();
        }

        internal byte[] getData(Rho5FileHandler handler)
        {
            if (!_rho5Streams.ContainsKey(handler._dataPackID) || !_dataBeginPoses.ContainsKey(handler._dataPackID))
                throw new Exception("Invalid data pack id in file handler.");
            FileStream rho5Stream = _rho5Streams[handler._dataPackID];
            if (rho5Stream is null || !rho5Stream.CanRead)
                throw new Exception("");
            FileStream clonedStream = new FileStream(rho5Stream.SafeFileHandle, FileAccess.Read);
            Rho5DecryptStream decryptStream = new Rho5DecryptStream(clonedStream, handler._key);
            int offset = _dataBeginPoses[handler._dataPackID] + (handler._offset << 10);
            decryptStream.Seek(offset, SeekOrigin.Begin);
            byte[] compressedData = new byte[handler._compressedSize];
            byte[] decompressedData = new byte[handler._decompressedSize];
            decryptStream.Read(compressedData, 0, compressedData.Length >= 0x400 ? 0x400 : compressedData.Length);
            if (compressedData.Length >= 0x400)
                clonedStream.Read(compressedData, 0x400, compressedData.Length - 0x400);
            using (MemoryStream memStream = new MemoryStream(compressedData))
            {
                decryptStream = new Rho5DecryptStream(memStream, handler._key);
                Ionic.Zlib.ZlibStream decompressStream = new Ionic.Zlib.ZlibStream(decryptStream, Ionic.Zlib.CompressionMode.Decompress);
                decompressStream.Read(decompressedData, 0, decompressedData.Length);
            }
            return decompressedData;
        }
        #endregion

        #region Structs
        private class DataSavingInfo
        {
            public Rho5File? File;
            public byte[] Data;
        }
        #endregion
    }
}
