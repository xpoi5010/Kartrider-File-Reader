using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KartRider.Text;
using KartRider.IO;

namespace KartRider.Xml
{
    public class BinaryXmlTag
    {
        public string Name { get; set; }

        public string Text { get; set; }

        public Dictionary<string,string> Attributes = new Dictionary<string, string>();

        public List<BinaryXmlTag> SubTags = new List<BinaryXmlTag>();

        public BinaryXmlTag this[string t]
        {
            get
            {
                return SubTags.Find(x => x.Name == t);
            }
            set
            {
                int index = SubTags.FindIndex(x => x.Name == t);
                if (index == -1)
                    SubTags.Add(value);
                else
                    SubTags[index] = value;
            }
        }

        public string GetAttribute(string Attribute)
        {
            if (!Attributes.ContainsKey(Attribute))
                return null;
            return Attributes[Attribute];
        }

        public void SetAttribute(string Attribute,string Value)
        {
            if (!Attributes.ContainsKey(Attribute))
                Attributes.Add(Attribute, Value);
            else
                Attributes[Attribute] = Value;
        }

        public byte[] ToBinary(Encoding encoding)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryWriter bw = new BinaryWriter(ms);
                //TagName
                bw.WriteString(encoding, Name);
                //Text
                bw.WriteString(encoding, Text);
                //Attributes
                bw.Write(Attributes.Count);
                foreach (KeyValuePair<string, string> key in Attributes)
                    bw.Write(encoding, key.Key, key.Value);
                //SubTags
                bw.Write(SubTags.Count);
                foreach (BinaryXmlTag sub in SubTags)
                    bw.Write(sub.ToBinary(encoding));
                return ms.ToArray();
            }
        }

        public new string ToString()
        {
            TextFormater tf = new TextFormater()
            {
                LevelDelta = 4
            };
            ToString(ref tf, 0);
            return tf.StartFormat();
        }

        

        public void ToString(ref TextFormater formater,int nowLevel)
        {
            bool HaveText = Text != null && Text != "";
            bool HaveAttributes = Attributes.Count > 0;
            bool HaveSubTag = SubTags.Count > 0;
            string Start = "";
            string Att = "";
            string End = "";
            string addition = "";
            bool OneLine = true;
            if((HaveText || HaveSubTag))
            {
                End = $"</{Name}>";
                OneLine = !HaveSubTag;
            }
            else
            {
                End = $"";
                OneLine = true;
                addition = "/";
            }
            if(HaveAttributes)
            {
                List<string> attFormat = new List<string>();
                foreach(KeyValuePair<string,string> KeyPair in Attributes)
                {
                    attFormat.Add($"{KeyPair.Key}=\"{KeyPair.Value}\"");
                }
                Att =$" {String.Join(" ",attFormat)}";
            }
            Start = $"<{Name}{Att}{addition}>";
            if (OneLine)
            {
                formater.AddString(nowLevel, TextAlign.Top,$"{Start}{Text??""}{End}");
            }
            else
            {
                formater.AddString(nowLevel, TextAlign.Top, $"{Start}{Text ?? ""}");
                foreach (BinaryXmlTag sub in SubTags)
                {
                    sub.ToString(ref formater, nowLevel + 1);
                }
                formater.AddString(nowLevel, TextAlign.Top, End);
            }

        }

        
    }
}
