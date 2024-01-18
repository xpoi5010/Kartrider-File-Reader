using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhoLoader.Controllers.FileManagement
{
    public interface IFileManagementController: IDisposable
    {
        Task OpenFile(string path);
        Task SaveFile(string path);
    }
}
