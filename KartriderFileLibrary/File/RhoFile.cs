using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static System.IO.File;
using KartRider.Crypt;
using Ionic.Zlib;
using System.Diagnostics;

namespace KartRider.File
{
    public class RhoFile
    {
        public string Path { get; set; }

        public RhoHeader Header { get; set; }

        public List<RhoStreamInfo> StreamInfos { get; set; }

        public IPackedObject[] RhoPackedFiles { get; set; }

        public uint HeaderKey { get; set; }

        public bool FixedMode { get; private set; }

        public RhoPackedFolderInfo ParentFolder { get; set; }

        private Stack<RhoPackedFolderInfo> FolderStack = new Stack<RhoPackedFolderInfo>();

        public RhoPackedFolderInfo NowFolder => FolderStack.Peek();

        public IPackedObject[] NowFolderContent { get; set; }

        private Stack<string> PathStack = new Stack<string>();

        public string NowPath => PathStack.Count == 1? "/": string.Join("/", PathStack.ToArray().Reverse());

        public RhoFile(string path)
        {
            if (!Exists(path))
                throw new FileNotFoundException($"File:{path} can not be found.");
            Path = path;
            FileInfo fi = new FileInfo(path);
            FileStream fs = new FileStream(path, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            string FileName = fi.Extension != "" ? fi.Name.Replace(fi.Extension, "") : fi.Name;
            HeaderKey = KeyGenerator.GetHeaderKey(FileName);
            byte[] block1 = br.ReadBytes(0x80);
            if (!CheckFile(block1))
                throw new Exception($"File:{path} is not a correct RhoFile,check if this file is changed.");
            byte[] block2 = br.ReadBytes(0x80);
            Header = new RhoHeader(block2, HeaderKey);
            if (!Header.IsCorrectRhoFile)
            {
                throw new Exception("It's not a correct Rho File!!!!");
            }
            uint Block3Count = Header.StreamInfoCount;
            StreamInfos = new List<RhoStreamInfo>();
            uint Block3FirstKey = KeyGenerator.GetStreamInfoFirstKey(HeaderKey);
            for (int i = 0; i < Block3Count; i++)
            {
                byte[] bk3 = br.ReadBytes(0x20);
                StreamInfos.Add(new RhoStreamInfo(bk3, Block3FirstKey));
                Block3FirstKey++;
            }
            RhoStreamInfo bk4Info = GetStreamInfo(0xFFFFFFFF);
            fs.Seek(bk4Info.Offset, SeekOrigin.Begin);
            byte[] bk4 = new byte[bk4Info.FileSize];
            fs.Read(bk4, 0, bk4.Length);
            NowFolderContent = RhoPackedFilesInfoDecoder.GetRhoPackedFileInfos(bk4, HeaderKey,0xFFFFFFFF);
            this.FolderStack.Push(new RhoPackedFolderInfo()
            {
                FolderName = "",
                Index = 0xFFFFFFFF,
                ParentIndex = 0
            });
            PathStack.Push("");
            fs.Close();
        }

        public RhoStreamInfo GetStreamInfo(uint index)
        {
            return StreamInfos.Find(x => x.Index == index);
        }

        
        private bool CheckFile(byte[] data)
        {
            using(MemoryStream ms = new MemoryStream(data))
            {
                BinaryReader br = new BinaryReader(ms);
                short a = br.ReadInt16();
                List<char> temp_str = new List<char>();
                while(a != 0x00)
                {
                    temp_str.Add((char)a);
                    a = br.ReadInt16();
                }
                string str = new string(temp_str.ToArray());
                return str == "Rh layer spec 1.1";
            }
        }

        public byte[] GetPackedFile(RhoPackedFileInfo info)
        {
            FileStream fs = new FileStream(Path, FileMode.Open);
            if(info.CryptMode == CryptMode.None || (int)info.CryptMode == -1)
            {
                RhoStreamInfo streaminfo = this.GetStreamInfo(info.Index);
                fs.Seek(streaminfo.Offset, SeekOrigin.Begin);
                byte[] data = new byte[streaminfo.FileSize];
                fs.Read(data, 0, data.Length);
                fs.Close();
                if(info.CryptMode != CryptMode.None)
                {
                    Debug.Print($"File: {info.FileName} is encrypted by a unsupport encryption method: {info.CryptMode}.");
                }
                return data;
            }
            else if(info.CryptMode == CryptMode.FullCryption)
            {
                RhoStreamInfo streaminfo = this.GetStreamInfo(info.Index);
                fs.Seek(streaminfo.Offset, SeekOrigin.Begin);
                byte[] data = new byte[streaminfo.FileSize];
                fs.Read(data, 0, data.Length);
                fs.Close();
                uint key = Crypt.KeyGenerator.GetDataKey(HeaderKey,info);
                data = RhoCrypt.Decrypt(data, key);
                return data;
            }
            else if(info.CryptMode == CryptMode.PartCryption)
            {
                List<byte> output = new List<byte>();
                RhoStreamInfo streaminfo = this.GetStreamInfo(info.Index);
                fs.Seek(streaminfo.Offset, SeekOrigin.Begin);
                byte[] data = new byte[streaminfo.FileSize];
                fs.Read(data, 0, data.Length);
                uint key = Crypt.KeyGenerator.GetDataKey(HeaderKey, info);
                data = RhoCrypt.Decrypt(data, key);
                output.AddRange(data);
                streaminfo = this.GetStreamInfo(info.Index + 1);
                fs.Seek(streaminfo.Offset, SeekOrigin.Begin);
                data = new byte[streaminfo.FileSize];
                fs.Read(data, 0, data.Length);
                output.AddRange(data);
                fs.Close();
                return output.ToArray();
            }
            else if (info.CryptMode == CryptMode.CompressedNonCryption)
            {
                RhoStreamInfo streaminfo = this.GetStreamInfo(info.Index);
                fs.Seek(streaminfo.Offset, SeekOrigin.Begin);
                byte[] data = new byte[(int)streaminfo.FileSize];
                byte[] buf = new byte[(int)streaminfo.OriginalSize];
                fs.Read(data, 0, (int)streaminfo.FileSize);
                fs.Close();
                using (MemoryStream ms = new MemoryStream(data))
                {
                    Ionic.Zlib.ZlibStream zs = new ZlibStream(ms, CompressionMode.Decompress);
                    zs.Read(buf, 0, buf.Length);
                    Debug.Print($"Method: 01 output:{streaminfo.OriginalSize} totalOutput:{zs.TotalOut}");
                }
                return buf;
            }
            else if (info.CryptMode == CryptMode.CompressedFullCryption)
            {
                RhoStreamInfo streaminfo = this.GetStreamInfo(info.Index);
                fs.Seek(streaminfo.Offset, SeekOrigin.Begin);
                byte[] data = new byte[streaminfo.FileSize];
                byte[] buf = new byte[(int)streaminfo.OriginalSize];
                fs.Read(data, 0, (int)streaminfo.FileSize);
                fs.Close();
                using(MemoryStream ms = new MemoryStream(data))
                {
                    Ionic.Zlib.ZlibStream zs = new ZlibStream(ms, CompressionMode.Decompress);
                    zs.Read(buf, 0, buf.Length);
                    Debug.Print($"Method: 06 output:{streaminfo.OriginalSize} totalOutput:{zs.TotalOut}");
                }
                uint key = Crypt.KeyGenerator.GetDataKey(HeaderKey, info);
                buf = RhoCrypt.Decrypt(buf, key);
                return buf;
            }
            else 
            {
                fs.Close();
                throw new Exception("Unknown CryptMode.");
            }
        }

        public byte[] GetStreamData(uint index)
        {
            RhoStreamInfo info = GetStreamInfo(index);
            FileStream fs = new FileStream(Path, FileMode.Open);
            fs.Seek(info.Offset, SeekOrigin.Begin);
            byte[] data = new byte[(int)info.FileSize];
            fs.Read(data, 0, (int)info.FileSize);
            fs.Close();
            return data;
        }

        
        public bool EnterToFolder(string FolderName)
        {
            RhoPackedFolderInfo FolderInfo = (RhoPackedFolderInfo)Array.Find(NowFolderContent, x => x.Type == ObjectType.Folder && ((RhoPackedFolderInfo)x).FolderName == FolderName);
            if (FolderName is null)
                return false;
            this.ParentFolder = this.NowFolder;
            this.NowFolderContent = RhoPackedFilesInfoDecoder.GetRhoPackedFileInfos(GetStreamData(FolderInfo.Index), this.HeaderKey, this.NowFolder.Index);
            this.FolderStack.Push(FolderInfo);
            this.PathStack.Push(FolderInfo.FolderName);
            return true;
        }

        public bool BackToParentFolder()
        {
            if (NowFolder.Index == 0xFFFFFFFF)
                return false;
            RhoPackedFolderInfo nowFolder = this.FolderStack.Pop();
            this.NowFolderContent = RhoPackedFilesInfoDecoder.GetRhoPackedFileInfos(GetStreamData(nowFolder.ParentIndex), this.HeaderKey, nowFolder.ParentIndex);
            this.PathStack.Pop();
            return true;
        }

        
    }
}
