using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KartRider.File
{
    public class RhoStreamInfo
    {
        public uint Index { get; set; }

        public uint Offset { get; set; }

        public uint FileSize { get; set; }

        public uint OriginalSize { get; set; }

        public uint CryptInfomation { get; set; }

        public uint Hash { get; set; }

        public RhoStreamInfo(byte[] data,uint FileKey)
        {
            byte[] DecryptData = Crypt.RhoCrypt.DecryptHeader(data, FileKey);

            using (MemoryStream ms = new MemoryStream(DecryptData))
            {
                BinaryReader br = new BinaryReader(ms);
                Index = br.ReadUInt32();
                Offset = br.ReadUInt32() * 0x100;
                FileSize = br.ReadUInt32();
                OriginalSize = br.ReadUInt32();
                CryptInfomation = br.ReadUInt32();
                Hash = br.ReadUInt32();
            }
        }


    }
}
