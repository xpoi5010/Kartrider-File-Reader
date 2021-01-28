using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Kartrider.IO;
using System.Xml;

namespace Kartrider.Xml
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
            xd.ToString();
        }
    }
}
