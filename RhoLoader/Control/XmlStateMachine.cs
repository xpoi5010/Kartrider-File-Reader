/*
 * Source Code From: https://archive.codeplex.com/?p=xmlrichtextbox
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace CustomXmlViewer
{
    public class XmlStateMachine
    {
        #region Public Fields
        public XmlTokenType CurrentState = XmlTokenType.Unknown;
        #endregion Public Fields

        #region Private Fields
        private string subString = "";
        private string token = string.Empty;
        private Stack<XmlTokenType> PreviousStates = new Stack<XmlTokenType>();
        #endregion Private Fields

        #region Public Methods
        public string GetNextToken(string s, int location, out XmlTokenType ttype)
        {
            ttype = XmlTokenType.Unknown;

            // skip past any whitespace (token added to it at the end of method)
            string whitespace = GetWhitespace(s, location);
            if (!String.IsNullOrEmpty(whitespace))
            {
                location += whitespace.Length;
            }

            subString = s.Substring(location, s.Length - location);
            token = string.Empty;

            if (CurrentState == XmlTokenType.CDataStart)
            {
                // check for empty CDATA
                if (subString.StartsWith("]]>"))
                {
                    CurrentState = XmlTokenType.CDataEnd;
                    token = "]]>";
                }
                else
                {
                    CurrentState = XmlTokenType.CDataValue;
                    int n = subString.IndexOf("]]>");
                    token = subString.Substring(0, n);
                }
            }
            else if (CurrentState == XmlTokenType.DocTypeStart)
            {
                CurrentState = XmlTokenType.DocTypeName;
                token = "DOCTYPE";
            }
            else if (CurrentState == XmlTokenType.DocTypeName)
            {
                CurrentState = XmlTokenType.DocTypeDeclaration;
                int n = subString.IndexOf("[");
                token = subString.Substring(0, n);
            }
            else if (CurrentState == XmlTokenType.DocTypeDeclaration)
            {
                CurrentState = XmlTokenType.DocTypeDefStart;
                token = "[";
            }
            else if (CurrentState == XmlTokenType.DocTypeDefStart)
            {
                if (subString.StartsWith("]>"))
                {
                    CurrentState = XmlTokenType.DocTypeDefEnd;
                    token = "]>";
                }
                else
                {
                    CurrentState = XmlTokenType.DocTypeDefValue;
                    int n = subString.IndexOf("]>");
                    token = subString.Substring(0, n);
                }
            }
            else if (CurrentState == XmlTokenType.DocTypeDefValue)
            {
                CurrentState = XmlTokenType.DocTypeDefEnd;
                token = "]>";
            }
            else if (CurrentState == XmlTokenType.DoubleQuotationMarkStart)
            {
                // check for empty attribute value
                if (subString[0] == '\"')
                {
                    CurrentState = XmlTokenType.DoubleQuotationMarkEnd;
                    token = "\"";
                }
                else
                {
                    CurrentState = XmlTokenType.AttributeValue;
                    int n = subString.IndexOf("\"");
                    token = subString.Substring(0, n);
                }
            }
            else if (CurrentState == XmlTokenType.SingleQuotationMarkStart)
            {
                // check for empty attribute value
                if (subString[0] == '\'')
                {
                    CurrentState = XmlTokenType.SingleQuotationMarkEnd;
                    token = "\'";
                }
                else
                {
                    CurrentState = XmlTokenType.AttributeValue;
                    int n = subString.IndexOf("'");
                    token = subString.Substring(0, n);
                }
            }
            else if (CurrentState == XmlTokenType.CommentStart)
            {
                // check for empty comment
                if (subString.StartsWith("-->"))
                {
                    CurrentState = XmlTokenType.CommentEnd;
                    token = "-->";
                }
                else
                {
                    CurrentState = XmlTokenType.CommentValue;
                    token = ReadCommentValue(subString, location);
                }
            }
            else if (CurrentState == XmlTokenType.NodeStart)
            {
                CurrentState = XmlTokenType.NodeName;
                token = ReadNodeName(subString, location);
            }
            else if (CurrentState == XmlTokenType.XmlDeclarationStart)
            {
                CurrentState = XmlTokenType.NodeName;
                token = ReadNodeName(subString, location);
            }
            else if (CurrentState == XmlTokenType.NodeName)
            {
                if (subString[0] != '/' &&
                    subString[0] != '>')
                {
                    CurrentState = XmlTokenType.AttributeName;
                    token = ReadAttributeName(subString, location);
                }
                else
                {
                    HandleReservedXmlToken();
                }
            }
            else if (CurrentState == XmlTokenType.NodeEndValueStart)
            {
                if (subString[0] == '<')
                {
                    HandleReservedXmlToken();
                }
                else
                {
                    CurrentState = XmlTokenType.NodeValue;
                    token = ReadNodeValue(subString, location);
                }
            }
            else if (CurrentState == XmlTokenType.DoubleQuotationMarkEnd)
            {
                HandleAttributeEnd(location);
            }
            else if (CurrentState == XmlTokenType.SingleQuotationMarkEnd)
            {
                HandleAttributeEnd(location);
            }
            else
            {
                HandleReservedXmlToken();
            }

            if (token != string.Empty)
            {
                ttype = CurrentState;
                return whitespace + token;
            }

            return string.Empty;

        }

        public Color GetTokenColor(XmlTokenType ttype)
        {
            Color brown = Color.FromArgb(238, 149, 68);

            switch (ttype)
            {
                case XmlTokenType.NodeValue:
                case XmlTokenType.EqualSignStart:
                case XmlTokenType.EqualSignEnd:
                case XmlTokenType.DoubleQuotationMarkStart:
                case XmlTokenType.DoubleQuotationMarkEnd:
                case XmlTokenType.SingleQuotationMarkStart:
                case XmlTokenType.SingleQuotationMarkEnd:
                    return Color.Black;

                case XmlTokenType.XmlDeclarationStart:
                case XmlTokenType.XmlDeclarationEnd:
                case XmlTokenType.NodeStart:
                case XmlTokenType.NodeEnd:
                case XmlTokenType.NodeEndValueStart:
                case XmlTokenType.CDataStart:
                case XmlTokenType.CDataEnd:
                case XmlTokenType.CommentStart:
                case XmlTokenType.CommentEnd:
                case XmlTokenType.AttributeValue:
                case XmlTokenType.DocTypeStart:
                case XmlTokenType.DocTypeEnd:
                case XmlTokenType.DocTypeDefStart:
                case XmlTokenType.DocTypeDefEnd:
                    return Color.Blue;

                case XmlTokenType.CDataValue:
                case XmlTokenType.DocTypeDefValue:
                    return Color.Gray;

                case XmlTokenType.CommentValue:
                    return Color.Green;

                case XmlTokenType.DocTypeName:
                case XmlTokenType.NodeName:
                    return Color.Brown;

                case XmlTokenType.AttributeName:
                case XmlTokenType.DocTypeDeclaration:
                    return Color.Red;

                default:
                    return Color.Orange;
            }
        }

        public string GetXmlDeclaration(string s)
        {
            int start = s.IndexOf("<?");
            int end = s.IndexOf("?>");
            if (start > -1 &&
                end > start)
            {
                return s.Substring(start, end - start + 2);
            }
            else return string.Empty;
        }
        #endregion Public Methods

        #region Private Methods
        private void HandleAttributeEnd(int location)
        {
            if (subString.StartsWith(">"))
            {
                HandleReservedXmlToken();
            }
            else if (subString.StartsWith("/>"))
            {
                HandleReservedXmlToken();
            }
            else if (subString.StartsWith("?>"))
            {
                HandleReservedXmlToken();
            }
            else
            {
                CurrentState = XmlTokenType.AttributeName;
                token = ReadAttributeName(subString, location);
            }
        }

        private void HandleReservedXmlToken()
        {
            // check if state changer
            // <, >, =, </, />, <![CDATA[, <!--, -->

            if (subString.StartsWith("<![CDATA["))
            {
                CurrentState = XmlTokenType.CDataStart;
                token = "<![CDATA[";
            }
            else if (subString.StartsWith("<!DOCTYPE"))
            {
                CurrentState = XmlTokenType.DocTypeStart;
                token = "<!";
            }
            else if (subString.StartsWith("</"))
            {
                CurrentState = XmlTokenType.NodeStart;
                token = "</";
            }
            else if (subString.StartsWith("<!--"))
            {
                CurrentState = XmlTokenType.CommentStart;
                token = "<!--";
            }
            else if (subString.StartsWith("<?"))
            {
                CurrentState = XmlTokenType.XmlDeclarationStart;
                token = "<?";
            }
            else if (subString.StartsWith("<"))
            {
                CurrentState = XmlTokenType.NodeStart;
                token = "<";
            }
            else if (subString.StartsWith("="))
            {
                CurrentState = XmlTokenType.EqualSignStart;
                if (CurrentState == XmlTokenType.AttributeValue) CurrentState = XmlTokenType.EqualSignEnd;
                token = "=";
            }
            else if (subString.StartsWith("?>"))
            {
                CurrentState = XmlTokenType.XmlDeclarationEnd;
                token = "?>";
            }
            else if (subString.StartsWith(">"))
            {
                CurrentState = XmlTokenType.NodeEndValueStart;
                token = ">";
            }
            else if (subString.StartsWith("-->"))
            {
                CurrentState = XmlTokenType.CommentEnd;
                token = "-->";
            }
            else if (subString.StartsWith("]>"))
            {
                CurrentState = XmlTokenType.DocTypeEnd;
                token = "]>";
            }
            else if (subString.StartsWith("]]>"))
            {
                CurrentState = XmlTokenType.CDataEnd;
                token = "]]>";
            }
            else if (subString.StartsWith("/>"))
            {
                CurrentState = XmlTokenType.NodeEnd;
                token = "/>";
            }
            else if (subString.StartsWith("\""))
            {
                if (CurrentState == XmlTokenType.AttributeValue)
                {
                    CurrentState = XmlTokenType.DoubleQuotationMarkEnd;
                }
                else
                {
                    CurrentState = XmlTokenType.DoubleQuotationMarkStart;
                }
                token = "\"";
            }
            else if (subString.StartsWith("'"))
            {
                CurrentState = XmlTokenType.SingleQuotationMarkStart;
                if (CurrentState == XmlTokenType.AttributeValue)
                {
                    CurrentState = XmlTokenType.SingleQuotationMarkEnd;
                }
                token = "'";
            }
        }

        private List<string> GetAttributeTokens(string s)
        {
            List<string> list = new List<string>();
            string[] arr = s.Split(' ');
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = arr[i].Trim();
                if (arr[i].Length > 0) list.Add(arr[i]);
            }
            return list;
        }

        private string ReadNodeName(string s, int location)
        {
            string nodeName = "";

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '/' ||
                    s[i] == ' ' ||
                    s[i] == '>')
                {
                    return nodeName;
                }
                else nodeName += s[i].ToString();
            }

            return nodeName;
        }

        private string ReadAttributeName(string s, int location)
        {
            string attName = "";

            int n = s.IndexOf('=');
            if(n != -1) attName = s.Substring(0, n);

            return attName;
        }

        private string ReadNodeValue(string s, int location)
        {
            string nodeValue = "";

            int n = s.IndexOf('<');
            if (n != -1) nodeValue = s.Substring(0, n);

            return nodeValue;
        }

        private string ReadCommentValue(string s, int location)
        {
            string commentValue = "";

            int n = s.IndexOf("-->");
            if (n != -1) commentValue = s.Substring(0, n);

            return commentValue;
        }



        private string GetWhitespace(string s, int location)
        {
            bool foundWhitespace = false;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; (location + i) < s.Length; i++)
            {
                char c = s[location + i];
                if (Char.IsWhiteSpace(c))
                {
                    foundWhitespace = true;
                    sb.Append(c);
                }
                else break;
            }
            if (foundWhitespace) return sb.ToString();
            return String.Empty;
        }
        #endregion Private Methods
    }

    public enum XmlTokenType
    {
        Whitespace, XmlDeclarationStart, XmlDeclarationEnd, NodeStart, NodeEnd, NodeEndValueStart, NodeName, NodeValue, 
        AttributeName, AttributeValue, EqualSignStart, EqualSignEnd, CommentStart, CommentValue, CommentEnd, CDataStart, 
        CDataValue, CDataEnd, DoubleQuotationMarkStart, DoubleQuotationMarkEnd, SingleQuotationMarkStart, SingleQuotationMarkEnd,
        DocTypeStart, DocTypeName, DocTypeDeclaration, DocTypeDefStart, DocTypeDefValue, DocTypeDefEnd, DocTypeEnd, 
        DocumentEnd, Unknown
    }
}
