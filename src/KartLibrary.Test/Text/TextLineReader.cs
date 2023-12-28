using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eP.Text
{
    public class TextLineReader
    {
        // Members
        private string baseText;
        private int ln = 0;
        private int pos = 0;
        private int lineStartPos = 0;
        private HashSet<char> blankChars = new HashSet<char>();

        // Properies
        public string BaseText => baseText;
        public int Line => ln;
        public int Position => pos;
        public int PositionInLine => pos - lineStartPos;
        public bool IsBeginOfLine => PositionInLine == 0;
        public bool IsEnd => pos >= baseText.Length;
        public bool IsEndOfLine => IsEnd || newLineSymbolLen() > 0;
        public IReadOnlySet<char> BlankCharacters => blankChars;

        // Constructors
        public TextLineReader(string text)
        {
            baseText = text;
            blankChars = new HashSet<char>(new char[] { ' ', '　', '\t' }); // Standard blank, Chinese full-width blank, tab
        }

        // Public methods
        public char ReadChar()
        {
            if (IsEndOfLine) return '\0';
            else return baseText[pos++];
        }

        public string ReadString(int length)
        {
            if (IsEndOfLine) return "";
            int maxLen = (pos + length) > baseText.Length ? baseText.Length - pos : length;
            int startPos = pos;
            int outLen = 0;
            for(; outLen < maxLen && !IsEndOfLine; outLen++)
                pos++;
            return baseText.Substring(startPos, outLen);
        }

        public string ReadToLineEnd()
        {
            if (IsEnd)
                return "";
            int startPos = pos;
            while (!IsEndOfLine)
                pos++;
            return baseText.Substring(startPos, pos - startPos);
        }

        public bool AcceptString(string compareStr)
        {
            if((pos + compareStr.Length) > baseText.Length) 
                return false;
            if(baseText.Substring(pos, compareStr.Length) == compareStr)
            {
                pos += compareStr.Length;
                return true;
            }
            else 
                return false;
        }

        public bool AcceptChar(char compareCh)
        {
            if(IsEndOfLine)
                return false;
            if (baseText[pos] == compareCh)
            {
                pos++;
                return true; 
            }
            else
            {
                return false;
            }
        }

        public bool AcceptIf(IReadOnlySet<char> allowCharSet)
        {
            if (IsEndOfLine)
                return false;
            if (allowCharSet.Contains(baseText[pos]))
            {
                pos++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public string ReadUntil(IReadOnlySet<char> terminalCharSet)
        {
            int startPos = pos;
            while(!IsEndOfLine) 
            {
                if (terminalCharSet.Contains(baseText[pos]))
                    break;
                else
                    pos++;
            }
            return baseText.Substring(startPos, pos - startPos);
        }

        public string ReadIf(IReadOnlySet<char> allowCharSet)
        {
            int startPos = pos;
            while (!IsEndOfLine)
            {
                if (!allowCharSet.Contains(baseText[pos]))
                    break;
                else
                    pos++;
            }
            return baseText.Substring(startPos, pos - startPos);
        }
        
        public char PeekChar()
        {
            if (IsEndOfLine)
                return '\0';
            else
                return baseText[pos];
        }

        public int CountIndentLevel(string indentStr)
        {
            int count = 0;
            while(AcceptString(indentStr))
                count++;
            return count;
        }

        public int CountIndentLevel(string indentStr, int maxCount)
        {
            int count = 0;
            while (count < maxCount && AcceptString(indentStr))
                count++;
            return count;
        }

        public bool MoveNextLine()
        {
            int newLineLen = 0;
            while (((newLineLen = newLineSymbolLen()) == 0) && !IsEnd)
                pos++;
            pos += newLineLen;
            lineStartPos = pos;
            if (!IsEnd)
                ln++;
            return !IsEnd;
        }

        // private methods
        private int newLineSymbolLen()
        {
            // If returns value is zero, it means current character is not new line symbol.
            if(IsEnd)
                return 0;
            char prevCh = pos > 1 ? baseText[pos - 1] : '\0';
            char curCh = baseText[pos];
            char nextCh = (pos + 1) < baseText.Length ? baseText[pos + 1] : '\0';
            if (curCh == '\r')
            {
                if(nextCh == '\n')
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
            else if(curCh == '\n' && prevCh != '\r')
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
