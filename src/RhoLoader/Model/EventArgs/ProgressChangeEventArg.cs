using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhoLoader.Model.EventArgs
{
    public class ProgressChangeEventArg
    {
        public int Progress { get; init; }
        public string StatusText { get; init; }

        public ProgressChangeEventArg()
        {
            Progress = 0;
            StatusText = "";
        }

        public ProgressChangeEventArg(int progress, string statusText)
        {
            Progress = progress;
            StatusText = statusText;
        }
    }
}
