using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartLibrary.Tests.Command
{
    public class StandardConsole : IConsole
    {
        private bool _locked = false;
        private int _lockThreadId = -1;

        public int Width => Console.WindowWidth;

        public int Height => Console.WindowHeight;

        public int CursorX => Console.CursorLeft;

        public int CursorY => Console.CursorTop;

        public event Func<string, int, string[]> AutoComplete;

        public char[] Separators {  get; set; } = new char[0];

        public StandardConsole()
        {
            System.ReadLine.HistoryEnabled = true;
            System.ReadLine.AutoCompletionHandler = new StandardConsoleAutoCompleteHandler(this);
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
        }
        
        public void Clear() => Console.Clear();

        public void MoveCursorTo(int x, int y) => Console.SetCursorPosition(x, y);

        public char ReadChar() => (char)Console.Read();

        public ConsoleKeyInfo ReadKey() => Console.ReadKey();
        
        public string? ReadLine()
        {
            return System.ReadLine.Read();
        }

        public void SetBackgroundColor(Color color)
        {
            Console.Write($"\u001b[48;2;{color.R};{color.G};{color.B}m");
        }

        public void SetBackgroundColor(ConsoleColor color)
        {
            Console.BackgroundColor = color;
        }

        public void SetForegroundColor(Color color)
        {
            Console.Write($"\u001b[38;2;{color.R};{color.G};{color.B}m");
        }

        public void SetForegroundColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        public void SetDefaultColor()
        {
            Console.ResetColor();
        }

        public void Write(string message) => Console.Write(message);

        public void Write(string formatString, params object[] args) => Console.Write(formatString, args);

        public void WriteLine(string message) => Console.WriteLine(message);
        
        public void WriteLine(string formatString, params object[] args) => Console.WriteLine(formatString, args);

        public void SetLock()
        {
            int currentThreadId = Thread.CurrentThread.ManagedThreadId;
            while (_locked && currentThreadId != _lockThreadId) ;
            _lockThreadId = currentThreadId;
            _locked = true;
        }

        public void ReleaseLock()
        {
            int currentThreadId = Thread.CurrentThread.ManagedThreadId;
            if (currentThreadId != _lockThreadId)
                return;
            _lockThreadId = -1;
            _locked = false ;
        }

        internal string[] onAutoComplete(string text, int index)
        {
            return AutoComplete?.Invoke(text, index) ?? new string[0];
        }

        private class StandardConsoleAutoCompleteHandler : IAutoCompleteHandler
        {
            public char[] Separators
            {
                get => _standardConsole.Separators;
                set => _standardConsole.Separators = value;
            }

            private StandardConsole _standardConsole;

            public StandardConsoleAutoCompleteHandler(StandardConsole standardConsole)
            {
                _standardConsole = standardConsole;
            }

            public string[] GetSuggestions(string text, int index)
            {
                return _standardConsole.onAutoComplete(text, index);
            }
        }

    }
}
