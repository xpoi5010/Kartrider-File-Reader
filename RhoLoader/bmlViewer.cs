using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kartrider.Xml;
using System.IO;

namespace RhoLoader
{
    public partial class bmlViewer : Form
    {
        string ConvertedXml="";
        string FileName = "";
        public bmlViewer(byte[] data,string FileName)
        {
            InitializeComponent();
            BinaryXmlDocument bxd = new BinaryXmlDocument();
            bxd.Read(Encoding.GetEncoding("UTF-16"), data);
            ConvertedXml = bxd.RootTag.ToString();
            ucXmlRichTextBox1.Xml = ConvertedXml;
            ucXmlRichTextBox1.Select(0, 0);
            this.FileName = FileName;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "*.xml|XML File";
            sfd.FileName = $"{FileName}.xml";
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(sfd.FileName, FileMode.Create);
                byte[] data = Encoding.GetEncoding("UTF-16").GetBytes(ConvertedXml);
                fs.Write(data, 0, data.Length);
                fs.Close();
            }
        }
    }
}
