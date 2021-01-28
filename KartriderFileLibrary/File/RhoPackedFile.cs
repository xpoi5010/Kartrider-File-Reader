using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartRider.File
{
    public class RhoPackedFileInfo: IPackedObject
    {
        public string FileName = "";

        public uint Ext = 0;

        public uint Index = 0;

        public CryptMode CryptMode = CryptMode.None;

        public uint Path;

        public int FileSize { get; set; }

        public string Extension
        {
            get
            {
                if (Ext == 0x00)
                    return "";
                uint ext_Temp = Ext;
                List<char> output = new List<char>();
                for(int i = 3; i >= 0; i++)
                {
                    if (((ext_Temp) & 0xFF) == 0x00)
                        break;
                    output.Add((char)((ext_Temp) & 0xFF));
                    ext_Temp >>= 8;
                }
                return new string(output.ToArray());
            }
        }

        public ObjectType Type => ObjectType.File;
    }

    /*
     * dds: none
     * tga: none
     * png: 
     */
    public enum CryptMode
    {
        None = 0x00, CompressedNonCryption = 0x01,FullCryption = 0x04, PartCryption = 0x05, CompressedFullCryption = 0x06

    }
}
