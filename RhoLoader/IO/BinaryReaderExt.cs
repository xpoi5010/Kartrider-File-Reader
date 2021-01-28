using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Kartrider.Xml;

namespace Kartrider.IO
{
    public static class BinaryReaderExt
    {
        public static string ReadText(this BinaryReader br,Encoding encoding,int Count)
        {
            byte[] data = br.ReadBytes(Count);
            return encoding.GetString(data);
        }

        public static string ReadText(this BinaryReader br, Encoding encoding)
        {
            int count = br.ReadInt32() << 1;
            byte[] data = br.ReadBytes(count);
            return encoding.GetString(data);
        }

        public static BinaryXmlTag ReadBinaryXmlTag(this BinaryReader br, Encoding encoding)
        {
            BinaryXmlTag tag = new BinaryXmlTag();
            tag.Name = br.ReadText(encoding);
            //Text
            tag.Text = br.ReadText(encoding);
            //Attributes
            int attCount = br.ReadInt32();
            for (int i = 0; i < attCount; i++)
                tag.Attributes.Add(br.ReadText(encoding), br.ReadText(encoding));
            //SubTags
            int SubCount = br.ReadInt32();
            for (int i = 0; i < SubCount; i++)
                tag.SubTags.Add(br.ReadBinaryXmlTag(encoding));
            return tag;
        }
    }
}
