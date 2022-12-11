using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KartRider.File;

namespace RhoLoader
{
    public partial class ExtractOption : Form
    {
        public ExtractOptionToken SelectOption { get; set; } = ExtractOptionToken.None;
        public ExtractOption()
        {
            InitializeComponent();
        }
        
        private void ExportOption_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing)
                this.DialogResult = DialogResult.Cancel;
        }

        private void action_submit(object sender, EventArgs e)
        {
            if (conDds.Checked)
                SelectOption |= ExtractOptionToken.ConvertDDS;
            if (conBml.Checked)
                SelectOption |= ExtractOptionToken.ConvertBML;
            this.DialogResult = DialogResult.OK;
        }
    }

    public enum ExtractOptionToken
    {
        None = 0,
        ConvertDDS = 1,
        ConvertBML = 2,
        ConvertKSV = 4
    }
}
