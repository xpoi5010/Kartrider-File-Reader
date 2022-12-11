using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace KartRider.Text
{
    public class RichTextFormater
    {
        public int LevelDelta { get; set; } = 1;

        private List<TextFormat> TextFormats = new List<TextFormat>();


        public void AddString(int Level, TextAlign align, string Text)
        {
            string[] Lines = Regex.Split(Text, "\\r\\n");
            foreach (string Line in Lines)
            {
                TextFormats.Add(new TextFormat()
                {
                    Level = Level,
                    Text = Line,
                    Align = align
                });
            }
        }

        public void StartFormat(RichTextBox rtb)
        {
            List<string> TopLine = new List<string>();
            List<string> BottomLine = new List<string>();
            foreach (TextFormat tf in TextFormats)
            {
                switch (tf.Align)
                {
                    case TextAlign.Top:
                        TopLine.Add($"{"".PadLeft(LevelDelta * tf.Level, ' ')}{tf.Text}");
                        break;
                    case TextAlign.Bottom:
                        BottomLine.Add($"{"".PadLeft(LevelDelta * tf.Level, ' ')}{tf.Text}");
                        break;
                }
            }
            List<string> output = new List<string>();
            foreach (string tl in TopLine)
            {
                output.Add(escapeNonAsciiChar($@"\cf0 {tl}\par "));
            }
            BottomLine.Reverse();
            foreach (string tl in BottomLine)
            {
                output.Add(escapeNonAsciiChar($@"\cf0 {tl}\par "));
            }
            const string rtfHead = @"{\rtf1\ansi\ansicpg65001\deff0\nouicompat\deflang1033\deflangfe1028{\fonttbl{\f0\fnil Consolas;}}
{\colortbl ;\red0\green0\blue255;\red165\green42\blue42;\red0\green0\blue0;\red255\green0\blue0;}
{\*\generator Riched20 10.0.19041}\viewkind4\uc1\fs18";
            rtb.Rtf = $"{rtfHead}\r\n{string.Join("\r\n", output)}\r\n}}";
        }
        private string escapeNonAsciiChar(string input)
        {
            StringBuilder sb = new StringBuilder();
            foreach(char c in input)
            {
                if (Char.IsAscii(c))
                {
                    sb.Append(c);
                }
                else
                {
                    byte[] utf8_converted = Encoding.UTF8.GetBytes(new char[] { c });
                    foreach(byte b in utf8_converted)
                    {
                        sb.Append($@"\'{Convert.ToString(b,16).PadLeft(2,'0')}");
                    }
                }
            }
            return sb.ToString();
        }
        public enum FormatTextCommand
        {
            EscapeText,EscapeArgument,NormalText,PaddingState
        }
    }
}
