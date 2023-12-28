using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartLibrary.File
{
    public interface IRhoFile : IDisposable
    {
        IRhoFolder? Parent { get; }

        string Name { get;  }

        string FullName { get; }

        int Size { get; }

        IDataSource? DataSource { get; }

        Stream CreateStream();
    }
}
