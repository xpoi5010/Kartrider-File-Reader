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
    public partial class ExportOption : Form
    {
        public ExportOption()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        public void ExportNowFolder(string TargetPath, bool IncludeSubFolder, Rho file, RhoDirectory curDir)
        {
            if(this.ShowDialog() == DialogResult.OK) 
            {
                ExportFolder ef = new ExportFolder();
                ef.ExportNowFolder(TargetPath, IncludeSubFolder, file, curDir, conBml.Checked,conDds.Checked);
            }
        }

        public void ExportAllFolder(string TargetPath, bool IncludeSubFolder, Rho file)
        {
            if (this.ShowDialog() == DialogResult.OK)
            {
                ExportFolder ef = new ExportFolder();
                ef.ExportAllFolder(TargetPath, IncludeSubFolder, file, conBml.Checked, conDds.Checked);
            }
        }

        private void ExportOption_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing)
            {
                if(MessageBox.Show("Do you want to stop export?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.DialogResult = DialogResult.Cancel;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
