using eP.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartLibrary.Tests.Command
{
    public static class CommandParser
    {
        private static HashSet<char> quotStringTerStrs = new HashSet<char> { '"', '\\' };
        
        public static ParsedCommand Parse(string commandText)
        {
            ParsedCommand parsedCommand = new ParsedCommand();
            Stack<ParseState> parseStateStack = new Stack<ParseState>();
            StringBuilder stringBuilder = new StringBuilder(300);

            TextLineReader textLineReader = new TextLineReader(commandText);

            parseStateStack.Push(ParseState.None);
            while(!textLineReader.IsEnd)
            {
                ParseState parseState = parseStateStack.Peek();
                if(parseState == ParseState.None)
                {
                    if (textLineReader.IsEndOfLine)
                        textLineReader.MoveNextLine();
                    textLineReader.ReadIf(textLineReader.BlankCharacters);
                    if (textLineReader.AcceptString("--"))
                    {
                        parseStateStack.Push(ParseState.StringOption);
                    }
                    else if (textLineReader.AcceptChar('-'))
                    {
                        parseStateStack.Push(ParseState.CharOption);
                    }
                    else if (textLineReader.AcceptChar('"'))
                    {
                         parseStateStack.Push(ParseState.QuotString);
                    }
                    else if(!textLineReader.IsEnd)
                    {
                        parseStateStack.Push(ParseState.String);
                    }
                }
                else if(parseState == ParseState.String)
                {
                    string readStr = textLineReader.ReadUntil(textLineReader.BlankCharacters);
                    if(readStr.Length > 0)
                        parsedCommand.ArgumentQueue.push(new CommandArgument(CommandArgumentType.ArgumentString, readStr));
                    parseStateStack.Pop();
                }
                else if(parseState == ParseState.QuotString)
                {
                    string readStr = textLineReader.ReadUntil(quotStringTerStrs);
                    stringBuilder.Append(readStr);
                    if (textLineReader.IsEndOfLine)
                    {
                        stringBuilder.Append(Environment.NewLine);
                        textLineReader.MoveNextLine();
                    }
                    else if (textLineReader.AcceptChar('"'))
                    {
                        readStr = textLineReader.ReadUntil(textLineReader.BlankCharacters);
                        stringBuilder.Append(readStr);
                        string argStr = stringBuilder.ToString();
                        parsedCommand.ArgumentQueue.push(new CommandArgument(CommandArgumentType.ArgumentString, argStr));
                        stringBuilder.Clear();
                        parseStateStack.Pop();
                    }
                    else if (textLineReader.AcceptChar('\\'))
                    {
                        char escapeCh = textLineReader.ReadChar();
                        switch (escapeCh)
                        {
                            case '"': stringBuilder.Append('"'); break;
                            case 'r': stringBuilder.Append('\r'); break;
                            case 'n': stringBuilder.Append('\n'); break;
                            case 't': stringBuilder.Append('\t'); break;
                            default: throw new Exception("");
                        }
                    }
                }
                else if(parseState == ParseState.CharOption)
                {
                    string charOptions = textLineReader.ReadUntil(textLineReader.BlankCharacters);
                    foreach (char ch in charOptions)
                        parsedCommand.ArgumentQueue.push(new CommandArgument(CommandArgumentType.Option, $"{ch}"));
                    parseStateStack.Pop();
                }
                else if(parseState == ParseState.StringOption)
                {
                    string optionName = textLineReader.ReadUntil(textLineReader.BlankCharacters);
                    parsedCommand.ArgumentQueue.push(new CommandArgument(CommandArgumentType.Option, optionName));
                    parseStateStack.Pop();
                }
            }
            if (parsedCommand.ArgumentQueue.Count < 1)
                throw new Exception("");
            else
            {
                parsedCommand.CommandName = parsedCommand.ArgumentQueue.PopArgumentString();
            }
            return parsedCommand;
        }

        private enum ParseState
        {
            None, 
            CharOption, 
            StringOption,
            String,
            QuotString,
        }
    }
}
