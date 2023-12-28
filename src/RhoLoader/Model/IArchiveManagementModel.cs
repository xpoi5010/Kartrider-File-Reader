using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhoLoader.Model
{
    public interface IArchiveManagementModel
    {
        Task OpenAsync(string path, ProgressReporter? progressReporter);
        Task SaveAsync(string path, ProgressReporter? progressReporter);

    }
}
