using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KartRider.IO;

namespace KartRider.Record
{
    public static class KartRecord
    {
        public static KSVInfo ReadKSVFile(string FileName)
        {
            FileStream fs = new FileStream(FileName, FileMode.Open);
            BinaryReader reader = new BinaryReader(fs);
            int FileSize = reader.ReadInt32();
            byte[] originalData = reader.ReadKRData(FileSize);
            MemoryStream ms = new MemoryStream(originalData);
            BinaryReader memReader = new BinaryReader(ms);
            KSVInfo output = memReader.ReadKSVInfo();
            ms.Close();
            fs.Close();
            return output;
        }

        public static KSVInfo ReadKSVFromBytes(byte[] data)
        {
            MemoryStream dataMS= new MemoryStream(data);
            BinaryReader reader = new BinaryReader(dataMS);
            int FileSize = reader.ReadInt32();
            byte[] originalData = reader.ReadKRData(FileSize);
            MemoryStream ms = new MemoryStream(originalData);
            BinaryReader memReader = new BinaryReader(ms);
            KSVInfo output = memReader.ReadKSVInfo();
            ms.Close();
            dataMS.Close();
            return output;
        }

        public static void SaveKSVFile(string FileName,KSVInfo ksvFile)
        {

        }
    }
}
