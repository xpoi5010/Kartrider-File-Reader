using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartLibrary.File
{
    public interface IModifiableRhoFile: IDisposable
    {
        IModifiableRhoFolder? Parent { get; }

        string Name { get; set; }

        string FullName { get; }

        int Size { get; }

        IDataSource? DataSource { get; set; }

        Stream CreateStream();
    }
}
