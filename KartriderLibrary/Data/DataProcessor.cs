using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zlib;
using System.IO;
using KartRider.Encrypt;

namespace KartRider
{
    // KR is means "KartRider and Raycity".
    public static class DataProcessor
    {
        public static byte[] ReadKRData(this BinaryReader br,int TotalLength)
        {
            long initialPos = br.BaseStream.Position;
            byte checkCode = br.ReadByte();
            if (checkCode != 0x53)
                throw new Exception("It is not KRData Format.");
            byte ProcessMode = br.ReadByte();
            uint Hash = br.ReadUInt32();
            bool Encrypted = (ProcessMode & 2) == 2;
            bool Compressed = (ProcessMode & 1) == 1;
            uint EncryptKey = Encrypted ? br.ReadUInt32() : 0;
            int DecompressSize = Compressed ? br.ReadInt32() : 0;
            byte[] originalData = br.ReadBytes((int)(TotalLength - (br.BaseStream.Position - initialPos)));
            byte[] processedData = originalData;
            if (Encrypted)
            {
                processedData = RhoEncrypt.DecryptData(EncryptKey, processedData);
            }
            if (Compressed)
            {
                using (MemoryStream ms = new MemoryStream(processedData))
                {
                    processedData = new byte[DecompressSize];
                    ZlibStream zs = new ZlibStream(ms, Ionic.Zlib.CompressionMode.Decompress);
                    zs.Read(processedData, 0, processedData.Length);
                }
            }
            uint CheckHash = Adler.Adler32(0, processedData, 0, processedData.Length);
            if (CheckHash != Hash)
                throw new Exception("Exception: KRData hash is not qualified.");
            return processedData;
        }

        public static int WriteKRData(this BinaryWriter bw,byte[] Data,bool Encrypted,bool Compressed,uint EncryptKey = 0)
        {
            long initialPos = bw.BaseStream.Position;
            const byte checkCode = 0x53;
            byte ProcessMode = (byte)((Encrypted ? 2 : 0) | (Compressed ? 1 : 0));
            uint Hash = 0x00;
            int DecompressSize = 0x00;
            byte[] processedData = Data;
            if (Compressed)
            {
                using (MemoryStream ms = new MemoryStream(processedData))
                {
                    processedData = new byte[DecompressSize];
                    ZlibStream zs = new ZlibStream(ms, Ionic.Zlib.CompressionMode.Compress);
                    zs.Read(processedData, 0, processedData.Length);
                    Array.Resize(ref processedData, (int)zs.TotalOut);
                }
            }
            if (Encrypted)
            {
                processedData = RhoEncrypt.DecryptData(EncryptKey, processedData);
            }
            uint CheckHash = Adler.Adler32(0, processedData, 0, processedData.Length);
            Hash = CheckHash;
            DecompressSize = Data.Length;
            bw.Write(checkCode);
            bw.Write(ProcessMode);
            bw.Write(Hash);
            if (Encrypted)
                bw.Write(EncryptKey);
            if (Compressed)
                bw.Write(DecompressSize);
            bw.Write(processedData);
            return (int)(bw.BaseStream.Position - initialPos);
        }

    }
}
