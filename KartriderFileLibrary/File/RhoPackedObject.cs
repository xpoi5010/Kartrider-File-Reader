using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartRider.File
{
    public interface IPackedObject
    {
        ObjectType Type { get; }
    }

    public enum ObjectType
    {
        Folder,File
    }
}
