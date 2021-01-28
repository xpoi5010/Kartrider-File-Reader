using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Kartrider.IO
{
    public static class BinaryWriterExt
    {
        public static void WriteString(this BinaryWriter br, Encoding encoding,string Text)
        {
            byte[] data = encoding.GetBytes(Text);
            br.Write(Text.Length);
            br.Write(data);
            data = null;
        }

        public static void Write(this BinaryWriter br, Encoding encoding, string Key,string Value)
        {
            br.WriteString(encoding, Key);
            br.WriteString(encoding,Value);
        }
    }
}
