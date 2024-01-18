using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhoLoader.Model.Dialog
{
    public interface IDialogModel
    {
        string DialogTitle { get; set; }

        Task ShowDialogAsync();

        Task CloseDialogAsync();
    }
}
