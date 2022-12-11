using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartRider.File
{
    public interface IPackedFile
    {
        PackFileType FileType { get; }
        Stream GetStream();
    }
}
