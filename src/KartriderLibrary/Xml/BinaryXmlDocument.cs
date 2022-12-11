using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KartRider.IO;
using System.Xml;

namespace KartRider.Xml
{
    public class BinaryXmlDocument
    {
        public BinaryXmlTag RootTag { get; set; }

        public void Read(Encoding encoding,byte[] array)
        {
            BinaryXmlTag tag = new BinaryXmlTag();
            using(MemoryStream ms = new MemoryStream(array))
            {
                BinaryReader br = new BinaryReader(ms);
                RootTag = br.ReadBinaryXmlTag(encoding);
            }
        }

        public void ReadFromXml(string XML)
        {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(XML);
        }
        public void ReadFromXml(byte[] EncodedXML)
        {
            XmlDocument xd = new XmlDocument();
            using(MemoryStream ms = new MemoryStream(EncodedXML))
            {
                xd.Load(ms);
            }
            
        }
    }
}
