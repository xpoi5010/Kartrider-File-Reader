using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KartLibrary.Text;
using KartLibrary.IO;
using System.Xml;

namespace KartLibrary.Xml
{
    public class BinaryXmlTag
    {
        #region Members
        private string _name;
        private string _text;
        private Dictionary<string, string> _attributes;
        private List<BinaryXmlTag> _children;
        #endregion

        #region Properties
        public string Name
        {
            get => _name; 
            set => _name = value;
        }

        public string Text
        {
            get => _text;
            set => _text = value;
        }

        public IReadOnlyDictionary<string,string> Attributes => _attributes;

        public List<BinaryXmlTag> Children => _children;

        public BinaryXmlTag this[string t]
        {
            get
            {
                return _children.Find(x => x.Name == t);
            }
            set
            {
                int index = Children.FindIndex(x => x.Name == t);
                if (index == -1)
                    Children.Add(value);
                else
                    Children[index] = value;
            }
        }
        
        #endregion

        #region Constructor
        public BinaryXmlTag()
        {
            _children = new List<BinaryXmlTag>();
            _attributes = new Dictionary<string, string>();
            _name = "";
            _text = "";
        }

        public BinaryXmlTag(string name): this()
        {
            _name = name;
        }

        public BinaryXmlTag(string name, string text): this()
        {
            _name = name;
            _text = text;
        }

        public BinaryXmlTag(string name, params BinaryXmlTag[] children): this()
        {
            _name = name;
            _children.AddRange(children);
        }
        #endregion

        public string? GetAttribute(string Attribute)
        {
            if (!Attributes.ContainsKey(Attribute))
                return null;
            return Attributes[Attribute];
        }

        public void SetAttribute(string name,string value)
        {
            if (!Attributes.ContainsKey(name))
                _attributes.Add(name, value);
            else
                _attributes[name] = value;
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
                bw.Write(Children.Count);
                foreach (BinaryXmlTag sub in Children)
                    bw.Write(sub.ToBinary(encoding));
                return ms.ToArray();
            }
        }

        public override string ToString()
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
            bool HaveSubTag = Children.Count > 0;
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
                foreach (BinaryXmlTag sub in Children)
                {
                    sub.ToString(ref formater, nowLevel + 1);
                }
                formater.AddString(nowLevel, TextAlign.Top, End);
            }

        }

        public static explicit operator BinaryXmlTag(XmlNode node)
        {
            if (node.NodeType != XmlNodeType.Element)
                throw new InvalidOperationException();
            BinaryXmlTag output = new BinaryXmlTag();
            output.Name = node.Name;
            output.Text = node.InnerText;
            foreach (XmlAttribute xmlAttribute in node.Attributes)
            {
                output._attributes.Add(xmlAttribute.Name, xmlAttribute.Value);
            }
            foreach (XmlNode xmlNode in node.ChildNodes)
            {
                if (xmlNode.NodeType == XmlNodeType.Element)
                {
                    output.Children.Add((BinaryXmlTag)xmlNode);
                }
            }
            return output;
        }

    }
}
