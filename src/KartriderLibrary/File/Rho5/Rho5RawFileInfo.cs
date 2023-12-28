using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartLibrary.File
{
    internal class Rho5RawFileInfo
    {
        public string FullPath { get; init; }

        public int Offset { get; init; }

        public int DecompressedSize { get; init; }

        public int CompressedSize { get; init; }

        public byte[] Key { get; init; }

        public Rho5RawFileInfo()
        {

        }
    }
}
