using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Ionic.Zlib;
using KartRider.Encrypt;

namespace KartRider.File
{
    public class Rho : IDisposable
    {
        Stream baseStream;
        private (double,string)[] MagicString = 
        { 
            (1.0d, "Rh layer spec 1.0") ,
            (1.1d, "Rh layer spec 1.1") ,
        };
        public double Version { get; private set; }
        public string FileName { get; private set; }

        private uint RhoFileKey = 0;

        private uint BlockWhiteningKey = 0;

        public RhoBlockInfo[] Blocks { get; private set; }
        public RhoDirectory RootDirectory { get; set; }

        public Rho(string FileName)
        {
            if (!System.IO.File.Exists(FileName))
                throw new FileNotFoundException($"Exception: Could't find the file:{FileName}.",FileName);
            //Test
            FileStream fileStream = new FileStream(FileName, FileMode.Open);
            baseStream = new BufferedStream(fileStream, 16384); //Default: 16 KiB
            this.FileName = FileName;
            BinaryReader reader = new BinaryReader(baseStream);
            FileInfo fileInfo = new FileInfo(FileName);
            RhoFileKey = RhoKey.GetRhoKey(fileInfo.Name.Replace(".rho", ""));
            // Read Magic String
            byte[] magicStrBytes = reader.ReadBytes(0x22);
            string magicStr = Encoding.GetEncoding("UTF-16").GetString(magicStrBytes);
            int verIndex = Array.FindIndex(MagicString, x => x.Item2 == magicStr);
            if (verIndex == -1)
                throw new NotSupportedException("Exception: This file is not supported.");
            Version = MagicString[verIndex].Item1;
            baseStream.Seek(0x80, SeekOrigin.Begin);
            // Part 2
            byte[] part2Data = reader.ReadBytes(0x80);
            if (Version == 1.0d)
                RhoEncrypt.DecryptData(RhoFileKey, part2Data, 0, part2Data.Length);
            else if (Version == 1.1d)
                part2Data = RhoEncrypt.DecryptHeaderInfo(part2Data, RhoFileKey);
            int BlockCount = 0;
            byte[] BlockInfoKeyOld = new byte[0]; // For 1.0 version
            uint BlockInfoKey = RhoKey.GetBlockFirstKey(RhoFileKey); // For 1.1 version
            using (MemoryStream ms = new MemoryStream(part2Data))
            {
                BinaryReader br = new BinaryReader(ms);
                uint part2Hash = br.ReadUInt32();
                uint checkHash = Adler.Adler32(0, part2Data, 4, 0x7C);
                if(part2Hash != checkHash)
                    throw new NotSupportedException("Exception: This file was modified. [ Part 2 Hash not euqal ]");
                int MagicCode = br.ReadInt32();
                if((Version == 1.0d && MagicCode != 0x00010000)|| (Version == 1.1d && MagicCode != 0x00010001))
                    throw new NotSupportedException("Exception: This file is not Rho File. [ Header check failure ]");
                BlockCount = br.ReadInt32();
                BlockWhiteningKey = br.ReadUInt32(); //
                //1.1: 14 bytes unknown
                if(Version == 1.0d)
                {
                    BlockInfoKeyOld = br.ReadBytes(32);
                }
                else if(Version == 1.1d)
                {
                    int u1a = br.ReadInt32(); //=1
                    int u2a = br.ReadInt32(); //=RhoKey - 397E40C3
                    int DataHash = br.ReadInt32(); // in aaa.pk file
                }
                int EndMagicCode = br.ReadInt32(); // = FC1F9778

                Blocks = new RhoBlockInfo[BlockCount];
            }
            
            baseStream.Seek(0x100,SeekOrigin.Begin);
            // Part 3
            for (int i = 0; i < BlockCount; i++)
            {
                if (Version == 1.0d)
                {
                    Blocks[i] = reader.ReadBlockInfoOld(BlockInfoKeyOld);
                }
                else if (Version == 1.1d)
                {
                    Blocks[i] = reader.ReadBlockInfo(BlockInfoKey);
                    BlockInfoKey++;
                }
            }
            // Part 4
            RootDirectory = new RhoDirectory(this);
            RootDirectory.DirectoryName = "";
            RootDirectory.DirIndex = 0xFFFFFFFF;
            Queue<RhoDirectory> processQueue = new Queue<RhoDirectory>();
            processQueue.Enqueue(RootDirectory);
            int[] tempAAr = new int[30];
            while(processQueue.Count > 0)
            {
                RhoDirectory curDir = processQueue.Dequeue();
                byte[] dirData = reader.ReadBlock(this, curDir.DirIndex, RhoKey.GetDirectoryDataKey(RhoFileKey));
                
                curDir.GetFromDirInfo(dirData);
                foreach(RhoDirectory subdir in curDir.Directories)
                {
                    processQueue.Enqueue(subdir);
                }
            }
            foreach(var xaa in RootDirectory.Files)
            {
                tempAAr[0] += (int)xaa.FileBlockIndex;
            }
            foreach (var xaa in Blocks)
            {
                tempAAr[1] += (int)xaa.Hash;
                tempAAr[2] += (int)xaa.Index;
            }
            /*
            for(int ts = 0;ts < baseStream.Length; ts++)
            {
                for(int te = ts+1;te < baseStream.Length; te++)
                {
                    baseStream.Seek(ts, SeekOrigin.Begin);
                    byte[] encData = new byte[te-ts];
                    baseStream.Read(encData, 0, encData.Length);
                    uint testHash = Adler.Adler32(0, encData, 0, encData.Length);
                    if (testHash == 2519818464)
                        System.Diagnostics.Debug.Print($"{ts}:{te} Hash = {testHash}");
                }
            }
            
            */
        }
        public uint GetFileKey()
        {
            return RhoFileKey;
        }
        public RhoBlockInfo GetBlockInfo(uint Index)
        {
            return Array.Find(Blocks, x => x.Index == Index);
        }

        public byte[] GetBlockData(uint BlockIndex,uint Key)
        {
            BinaryReader reader = new BinaryReader(baseStream);
            byte[] output = reader.ReadBlock(this, BlockIndex, Key);
            return output;
        }

        public RhoFileInfo GetFile(string Path)
        {
            string[] PathSplit = Path.Split('/');
            RhoDirectory rd = RootDirectory;
            for(int i = 1; i < (PathSplit.Length-1); i++)
            {
                string curPathName = PathSplit[i].Trim();
                if (curPathName == "")
                    continue;
                RhoDirectory nextDir = rd.Directories.Find(x => x.DirectoryName == curPathName);
                if (nextDir is null)
                    return null;
                rd = nextDir;
            }
            return rd.Files.Find(x => x.FullFileName == PathSplit[PathSplit.Length - 1]);
        }

        public void Dispose()
        {
            this.baseStream.Close();
            this.baseStream.Dispose();
            Blocks = null;
        }

        ~Rho()
        {
            this.Dispose();
        }
    }
}
