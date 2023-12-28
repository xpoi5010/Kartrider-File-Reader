using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Runtime.CompilerServices;
using System.Net;
using RhoLoader.Update;
using KartLibrary.IO;
using KartLibrary.Game.Engine.Tontrollers;
using KartLibrary.Game.Engine.Relements;
using KartLibrary.Game.Engine.Properities;
using KartLibrary.Game.Engine.Render;
using System.Numerics;

namespace RhoLoader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            Application.Run(new MainWindow());
        }
    }
}
