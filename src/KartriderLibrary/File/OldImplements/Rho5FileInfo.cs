using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using KartLibrary.Encrypt;

namespace KartLibrary.File
{
    [Obsolete("Rho5FileInfo class is deprecated. Use Rho5File instead.")]
    public class Rho5FileInfo
    {
        internal Rho5 BaseRho5 { get; init; }
        public string FullPath { get; set; }
        public int Offset { get; set; }
        public int CompressedSize { get; set; }
        public int DecompressedSize { get; set; }
        public int Unknown { get; set; }
        public int FileInfoChecksum { get; set; }
        public byte[] Key { get; set; }

        public byte[] GetData()
        {
            byte[] data = new byte[CompressedSize];
            byte[] outdata = new byte[DecompressedSize];
            byte[] decryptKey = Rho5Key.GetPackedFileKey(Key, Rho5Key.GetFileKey_U1(BaseRho5.anotherData), FullPath);
            Rho5DecryptStream decryptStream = new Rho5DecryptStream(BaseRho5.BaseStream, decryptKey);
            decryptStream.Seek(Offset * 0x400 + BaseRho5.DataBaseOffset, SeekOrigin.Begin);
            decryptStream.Read(data, 0, data.Length >= 0x400 ? 0x400 : data.Length);
            if (data.Length >= 0x400)
                BaseRho5.BaseStream.Read(data, 0x400, data.Length - 0x400);
            MemoryStream ms = new MemoryStream(data);
            decryptStream = new Rho5DecryptStream(ms, decryptKey);
            decryptStream.Read(data, 0, data.Length);
            using (MemoryStream ms2 = new MemoryStream(data))
            {
                ZLibStream ds = new ZLibStream(ms2, CompressionMode.Decompress);
                ds.Read(outdata, 0, outdata.Length);
            }
            return outdata;
        }

        private void dump_data(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in data)
                sb.Append($"{b:x2} ");
            sb.Append($"\n");
            System.Diagnostics.Debug.Write(sb.ToString());
        }
    }
}
