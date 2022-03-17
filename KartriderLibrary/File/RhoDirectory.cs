using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KartRider.Encrypt;
using System.IO;

namespace KartRider.File
{
    public class RhoDirectory
    {
        public Rho BaseRho { get; set; }
        public string DirectoryName { get; set; }
        public RhoDirectory Parent { get; set; }
        public List<RhoDirectory> Directories { get;  } = new List<RhoDirectory>();
        public List<RhoFileInfo> Files { get; } = new List<RhoFileInfo>();
        public uint DirIndex
        {
            get;set;
        }

        public string GetFullPathName()
        {
            return "";
        }
        public RhoDirectory(Rho BaseRho)
        {
            this.BaseRho = BaseRho;
        }
        public void GetFromDirInfo(byte[] DirInfoData)
        {
            using (MemoryStream ms = new MemoryStream(DirInfoData))
            {
                BinaryReader msReader = new BinaryReader(ms);
                int DirCount = msReader.ReadInt32();
                for(int i = 0; i < DirCount; i++)
                {
                    RhoDirectory dir = new RhoDirectory(this.BaseRho);
                    StringBuilder strBuilder = new StringBuilder();
                    char tempChar = (char)msReader.ReadInt16();
                    while (tempChar != 0)
                    {
                        strBuilder.Append(tempChar);
                        tempChar = (char)msReader.ReadInt16();
                    } 
                    uint dirInd = msReader.ReadUInt32();
                    dir.DirectoryName = strBuilder.ToString();
                    dir.DirIndex = dirInd;
                    this.AddDirectory(dir);
                }
                int FileCount = msReader.ReadInt32();
                for (int i = 0; i < FileCount; i++)
                {
                    RhoFileInfo rfi = new RhoFileInfo(this.BaseRho);
                    StringBuilder strBuilder = new StringBuilder();
                    char tempChar = (char)msReader.ReadInt16();
                    while (tempChar != 0)
                    {
                        strBuilder.Append(tempChar);
                        tempChar = (char)msReader.ReadInt16();
                    }
                    rfi.Name = strBuilder.ToString();
                    strBuilder.Clear();
                    uint extInt = msReader.ReadUInt32();
                    rfi.FileProperty = (RhoFileProperty)msReader.ReadInt32();
                    rfi.FileBlockIndex = msReader.ReadUInt32();
                    rfi.FileSize = msReader.ReadInt32();
                    for (int j = 0; j < 4; j++)
                    {
                        tempChar = (char)((extInt >> (j<<3)) & 0xFF);
                        if(tempChar != '\0')
                            strBuilder.Append(tempChar);
                    }
                    rfi.Extension = strBuilder.ToString();
                    this.AddFile(rfi);
                }
            }
        }
        public void AddDirectory(RhoDirectory directory)
        {
            Directories.Add(directory);
            directory.Parent = this;
        }
        public void AddFile(RhoFileInfo file)
        {
            Files.Add(file);
        }
    }
}
