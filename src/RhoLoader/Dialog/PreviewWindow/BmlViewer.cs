using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KartLibrary.Xml;
using System.IO;
using System.Diagnostics;
using RhoLoader.XML;

namespace RhoLoader.PreviewWindow
{
    public partial class bmlViewer : Form
    {
        string ConvertedXml="";
        string FileName = "";
        byte[] data;
        public bool Initized { get; set; } = false;
        public bmlViewer(byte[] data,string FileName)
        {
            InitializeComponent();
            this.FileName = FileName;
            this.data = data;
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

        private void bmlViewer_Load(object sender, EventArgs e)
        {
            BinaryXmlDocument bxd = new BinaryXmlDocument();
            bxd.Read(Encoding.GetEncoding("UTF-16"), data);
            DateTime dt = DateTime.Now;
            ConvertedXml = bxd.RootTag.ToString();
            TimeSpan time1 = DateTime.Now - dt;
            dt = DateTime.Now;
            bxd.RootTag.ApplyToRichTextBox(richTextBox1);
            richTextBox1.Select(0, 0);
            TimeSpan time2 = DateTime.Now - dt;
            Initized = true;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
