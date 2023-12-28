using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartCityStudio.Game.Dialog
{
    public abstract class OpenFileDialog
    {
        public bool Multiselect { get; set; }

        public string FileName { get; set; }

        public string[] FileNames { get; set; }
    }
}
