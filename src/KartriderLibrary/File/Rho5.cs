using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using KartRider.Encrypt;
using KartRider.IO;
using KartRider;

namespace KartRider.File
{
    public class Rho5 : IDisposable
    {
        public byte PackageVersion { get; set; }
        public Stream BaseStream { get; set; }
        public Rho5FileInfo[] Files { get; private set; } = new Rho5FileInfo[0];
        internal int DataBaseOffset = 0;
        internal string anotherData = "";
        public Rho5()
        {

        }
        public Rho5(string FileName, RegionCode region)
        {
            BaseStream = new FileStream(FileName,FileMode.Open);
            FileInfo fileInfo = new FileInfo(FileName);
            anotherData = "";
            switch (region)
            {
                case RegionCode.Korea:
                    anotherData = "y&errfV6GRS!e8JL";
                    break;
                case RegionCode.China:
                    anotherData = "d$Bjgfc8@dH4TQ?k";
                    break;
                case RegionCode.Taiwan:
                    anotherData = "t5rHKg-g9BA7%=qD";
                    break;
            }
            Rho5DecryptStream decryptStream = new Rho5DecryptStream(BaseStream, fileInfo.Name, anotherData);
            BinaryReader br = new BinaryReader(decryptStream);
            int headerOffset = GetHeaderOffset(fileInfo.Name);
            int fileNameOffset = headerOffset + GetFileNamesOffset(fileInfo.Name);
            decryptStream.Seek(headerOffset, SeekOrigin.Begin);
            int u1 = br.ReadInt32();
            int fileCounts = br.ReadInt32();
            PackageVersion = (byte)(fileCounts & 0xFF);
            fileCounts >>= 8;
            decryptStream.Seek(fileNameOffset, SeekOrigin.Begin);
            //decryptStream.SetToFileInfoKey(fileInfo.Name, "t5rHKg-g9BA7%=qD"); china: d$Bjgfc8@dH4TQ?k korea: y&errfV6GRS!e8JL taiwan: t5rHKg-g9BA7%=qD
            decryptStream.SetToFileInfoKey(fileInfo.Name, anotherData);
            Files = new Rho5FileInfo[fileCounts];
            for(int i =0;i< fileCounts; i++)
            {
                Rho5FileInfo file = new Rho5FileInfo()
                {
                    FullPath = br.ReadText(Encoding.GetEncoding("UTF-16")),
                    FileInfoChecksum = br.ReadInt32(),
                    Unknown = br.ReadInt32(),
                    Offset = br.ReadInt32(),
                    DecompressedSize = br.ReadInt32(),
                    CompressedSize = br.ReadInt32(),
                    Key = br.ReadBytes(0x10),
                    BaseRho5 = this
                };
                Files[i] = file;
            }
            DataBaseOffset = (((int)decryptStream.Position + 0x3FF) >> 10) << 10;
        }
        private int GetHeaderOffset(string name)
        {
            name = name.ToLower();
            int sum = 0;
            foreach (char c in name) sum += c;
            long mpl = (sum * 0xA41A41A5L) >> 32;
            int result = (sum - (int)mpl);
            result >>= 1;
            result += (int)mpl;
            result >>= 8;
            result *= 0x138;
            result = (sum - result + 0x1E);
            return result;
        }

        private int GetFileNamesOffset(string name)
        {
            name = name.ToLower();
            int sum = 0;
            foreach (char c in name) sum += c;
            sum *= 3;
            long mpl = (sum * 0x3521CFB3L) >> 32;
            int result = (sum - (int)mpl);
            result >>= 1;
            result += (int)mpl;
            result >>= 7;
            result *= 0xD4;
            result = (sum - result + 0x2A);
            return result;
        }

        public void Dispose()
        {
            if (BaseStream is null)
                return;
            BaseStream.Close();
            BaseStream.Dispose();
        }
    }
}
