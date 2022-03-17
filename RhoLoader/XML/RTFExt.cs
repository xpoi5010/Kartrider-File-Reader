using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KartRider.Xml;
using KartRider.Text;

namespace RhoLoader.XML
{
    public static class RTFExt
    {
        public static void ApplyToRichTextBox(this BinaryXmlTag bxt,System.Windows.Forms.RichTextBox rtb)
        {
            RichTextFormater tf = new RichTextFormater()
            {
                LevelDelta = 4
            };
            bxt.ApplyToRichText(tf, 0);
            tf.StartFormat(rtb);
        }
        public static void ApplyToRichText(this BinaryXmlTag bxt, RichTextFormater formater, int nowLevel)
        {
            bool HaveText = bxt.Text != null && bxt.Text != "";
            bool HaveAttributes = bxt.Attributes.Count > 0;
            bool HaveSubTag = bxt.SubTags.Count > 0;
            string Start = "";
            string Att = "";
            string End = "";
            string addition = "";
            bool OneLine = true;
            if ((HaveText || HaveSubTag))
            {
                End = $"\\cf1 </\\cf2 {bxt.Name}\\cf1 >";
                OneLine = !HaveSubTag;
            }
            else
            {
                End = $"";
                OneLine = true;
                addition = "/";
            }
            if (HaveAttributes)
            {
                List<string> attFormat = new List<string>();
                foreach (KeyValuePair<string, string> KeyPair in bxt.Attributes)
                {
                    attFormat.Add($"\\cf4 {KeyPair.Key}\\cf3 =\"\\cf1 {KeyPair.Value.Replace("\\", "\\\\").Replace("\"", "&quot;")}\\cf3 \"");
                }
                Att = $" {String.Join(" ", attFormat)}";
            }
            Start = $"\\cf1 <\\cf2 {bxt.Name}{Att}\\cf1 {addition}>";
            if (OneLine)
            {
                formater.AddString(nowLevel, TextAlign.Top, $"{Start}\\cf3 {bxt.Text.Replace("\\", "\\\\").Replace("\"", "&quot;") ?? ""}{End}");
            }
            else
            {
                formater.AddString(nowLevel, TextAlign.Top, $"{Start}\\cf3 {bxt.Text.Replace("\\", "\\\\").Replace("\"", "&quot;") ?? ""}");
                foreach (BinaryXmlTag sub in bxt.SubTags)
                {
                    sub.ApplyToRichText(formater, nowLevel + 1);
                }
                formater.AddString(nowLevel, TextAlign.Top, End);
            }

        }
    }
}
