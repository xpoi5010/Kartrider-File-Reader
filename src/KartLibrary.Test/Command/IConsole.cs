using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartLibrary.Tests.Command
{
    public interface IConsole
    {
        int Width { get; }
        
        int Height { get; }

        int CursorX { get; }

        int CursorY { get; }

        event Func<string, int, string[]> AutoComplete;

        void MoveCursorTo(int x, int y);
        
        void SetForegroundColor(Color color);

        void SetForegroundColor(ConsoleColor color);

        void SetBackgroundColor(Color color);

        void SetBackgroundColor(ConsoleColor color);

        void SetDefaultColor();

        void Write(string message);

        void Write(string formatString,  params object[] args);

        void WriteLine(string message);

        void WriteLine(string formatString, params object[] args);

        char ReadChar();

        string? ReadLine();

        void Clear();

        void SetLock();

        void ReleaseLock();
    }
}
