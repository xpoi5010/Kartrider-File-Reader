using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhoLoader.Model.Dialog
{
    public class ProgressDialogModel
    {
        public string Title { get; set; }

        public string HeaderText { get; set; }

        public string DialogTitle { get; set; }

        public string StatusLabel { get; set; }

        public void ReportProgress(int currentValue, int maxValue, string statusText)
        {

        }
    }

    
}
