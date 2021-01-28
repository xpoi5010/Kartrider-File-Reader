using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace RhoLoader
{
    public partial class AboutMe : Form
    {
        public AboutMe()
        {
            InitializeComponent();
            Version ver = Assembly.GetExecutingAssembly().GetName().Version;
            this.version.Text = $"Version : {(ver.Revision == 0?"":ver.Revision == 1?"Beta ":ver.Revision == 2?"Dev ":"Custom ")}{ver.Major}.{ver.Minor}.{ver.Build}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If this program is running incorrectly,\r\nYou can report to RhoReader github page.\r\nThank you for reporting!");
        }
    }
}
