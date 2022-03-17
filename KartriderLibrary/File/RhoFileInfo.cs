using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KartRider.Encrypt;

namespace KartRider.File
{
    public class RhoFileInfo
    {
        public Rho BaseRho { get; set; }
        public string Name { get; set; }
        private string _ext = "";
        private int extnum = -1;
        public string Extension
        {
            get
            {
                return _ext;
            }
            set
            {
                _ext = value;
                extnum = GetExtNum();
            }
        }
        public int ExtNum => extnum;
        public uint FileBlockIndex { get; set; }
        public RhoFileProperty FileProperty { get; set; }
        public int FileSize { get; set; }
        public RhoFileInfo(Rho baseRho)
        {
            BaseRho = baseRho;
        }
        public byte[] GetData()
        {
            uint DecryptKey = RhoKey.GetDataKey(BaseRho.GetFileKey(), this);
            byte[] Data = BaseRho.GetBlockData(FileBlockIndex, DecryptKey);
            return Data;
        }
        public int GetExtNum()
        {
            int output = 0;
            byte[] arr = Encoding.UTF8.GetBytes(_ext);
            for(int i =0;i< arr.Length; i++)
            {
                output |= arr[i]<<(i<<3);
            }
            return output;
        }

        public string FullFileName
        {
            get
            {
                if (_ext == "")
                    return Name;
                else
                    return $"{Name}.{_ext}";
            }
        }
    }

    public enum RhoFileProperty
    {
        None = 0x00, CompressedNonCryption = 0x01, FullCryption = 0x04, PartCryption = 0x05, CompressedFullCryption = 0x06
    }
}
