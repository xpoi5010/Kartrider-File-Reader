using KartLibrary.Tests.Command;
using KartLibrary.Xml;
using Microsoft.Win32.SafeHandles;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Windows.Win32.System.Console;

namespace KartLibrary.Tests
{
    internal class Program
    {
        static unsafe void Main(string[] args)
        {
            TestProgram testProgram = new TestProgram();
            testProgram.Run();
        }
    }
}
