using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhoLoader.Model.EventArgs
{
    public class ProgressEventArg
    {
        public int Progress { get; }

        public int MaxValue { get; }

        public string StatusText { get; }

        public ProgressEventArg(int progress, int maxProgressValue, string statusText)
        {
            Progress = progress;
            MaxValue = maxProgressValue;
            StatusText = statusText;
        }
    }
}
