using RhoLoader.Model.Delegates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhoLoader.Model
{
    public class ProgressReporter
    {
        public event ProgressReportDelegate? ProgressReport;

        public ProgressReporter() 
        {
            
        }

        public void ReportProgress(int progress, int maxProgress, string statusText)
        {

        }
    }
}
