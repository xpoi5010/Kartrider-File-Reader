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

using System.IO;
using System.Diagnostics;

namespace RhoLoader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LanguageManager.LoadLang();
            LanguageManager.LanguageName = "en-us";
            AddLanguages();
            LoadLang();
            listView1.SmallImageList = imageList1;
            listView1.SmallImageList.Images.Add("file", new Bitmap(global::RhoLoader.Properties.Resources.baseline_insert_drive_file_black_18dp));
            listView1.SmallImageList.Images.Add("folder", new Bitmap(global::RhoLoader.Properties.Resources.baseline_folder_black_18dp));
        }
        RhoFile BaseRhoFile;

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //RhoTesting
                FileInfo fi = new FileInfo(openFileDialog1.FileName);
                BaseRhoFile = new RhoFile(openFileDialog1.FileName);
                UpdateFolders();
            }
        }

        private void LoadLang()
        {
            this.menuStrip1.Text = ((string)this.menuStrip1.Tag).GetStringBag();
            this.fileToolStripMenuItem.Text = ((string)this.fileToolStripMenuItem.Tag).GetStringBag();
            this.openToolStripMenuItem.Text = ((string)this.openToolStripMenuItem.Tag).GetStringBag();
            this.saveToolStripMenuItem.Text = ((string)this.saveToolStripMenuItem.Tag).GetStringBag();
            this.saveAsToolStripMenuItem.Text = ((string)this.saveAsToolStripMenuItem.Tag).GetStringBag();
            this.exitToolStripMenuItem.Text = ((string)this.exitToolStripMenuItem.Tag).GetStringBag();
            this.exportToolStripMenuItem1.Text = ((string)this.exportToolStripMenuItem1.Tag).GetStringBag();
            this.exportAllToolStripMenuItem.Text = ((string)this.exportAllToolStripMenuItem.Tag).GetStringBag();
            this.aboutToolStripMenuItem.Text = ((string)this.aboutToolStripMenuItem.Tag).GetStringBag();
            this.columnHeader1.Text = ((string)this.columnHeader1.Tag).GetStringBag();
            this.columnHeader2.Text = ((string)this.columnHeader2.Tag).GetStringBag();
            this.columnHeader3.Text = ((string)this.columnHeader3.Tag).GetStringBag();
            this.exportToolStripMenuItem.Text = ((string)this.exportToolStripMenuItem.Tag).GetStringBag();
            this.Text = ((string)this.Tag).GetStringBag();
            this.menuLanguagesToolStripMenuItem.Text = ((string)this.menuLanguagesToolStripMenuItem.Tag).GetStringBag();
            this.convertToPngToolStripMenuItem.Text = ((string)this.convertToPngToolStripMenuItem.Tag).GetStringBag();
            this.menuexportnowToolStripMenuItem.Text = ((string)this.menuexportnowToolStripMenuItem.Tag).GetStringBag();
            if (!(BaseRhoFile is null))
                UpdateFolders();
        }
        
        private void AddLanguages()
        {
            List<ToolStripItem> temp = new List<ToolStripItem>();
            string[] langs = LanguageManager.ListLanguages();
            foreach(string lang in langs)
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem();
                tsmi.Text = lang;
                tsmi.AutoSize = true;
                tsmi.Click += Tsmi_Click;
                temp.Add(tsmi);
            }
            menuLanguagesToolStripMenuItem.DropDownItems.AddRange(temp.ToArray());
        }

        private void Tsmi_Click(object sender, EventArgs e)
        {
            ToolStripItem tsi = (ToolStripItem)sender;
            LanguageManager.SetLanguage(DisplayName: tsi.Text);
            LoadLang();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            if (listView1.SelectedItems.Count == 0)
                return;
            ListViewItem lvii = listView1.SelectedItems[0];
            if (listView1.SelectedItems[0].SubItems[2].Text == ("listview_item2_file").GetStringBag())
            {
                Random rm = new Random();
                RhoPackedFileInfo fileInfo = (RhoPackedFileInfo)Array.Find(BaseRhoFile.NowFolderContent, x => x.Type == ObjectType.File && (((RhoPackedFileInfo)x).FileName + $".{((RhoPackedFileInfo)x).Extension}") == lvii.Text);
                if(fileInfo.Extension == "dds" || fileInfo.Extension == "tga")
                {
                    TgaDDsViewer tdv = new TgaDDsViewer();
                    tdv.Data = BaseRhoFile.GetPackedFile(fileInfo);
                    tdv.Type = fileInfo.Extension == "dds" ? TgaDDsViewer.FileType.dds : fileInfo.Extension == "tga" ? TgaDDsViewer.FileType.tga : throw new Exception();
                    tdv.ShowBox(); 
                    return;
                }
                else if(fileInfo.Extension == "bml")
                {
                    byte[] bmlData = BaseRhoFile.GetPackedFile(fileInfo);
                    bmlViewer bv = new bmlViewer(bmlData,fileInfo.FileName);
                    bv.Show();
                    return;
                }
                FileStream fs = new FileStream(Environment.GetEnvironmentVariable("TEMP") + $"\\{lvii.Text}", FileMode.Create);
                byte[] a = BaseRhoFile.GetPackedFile(fileInfo);
                fs.Write(a, 0, a.Length);
                fs.Close();
                a = null;
                Process ps = new Process();
                ps.StartInfo.FileName = Environment.GetEnvironmentVariable("TEMP") + $"\\{lvii.Text}";
                ps.Start();
                return;
            }

            string FolderName = lvii.SubItems[0].Text;
            BaseRhoFile.EnterToFolder(FolderName);
            UpdateFolders();
            this.toolStripButton1.Enabled = true;
        }

        private void UpdateFolders()
        {
            this.listView1.Items.Clear();
            this.toolStripTextBox1.Text = BaseRhoFile.NowPath;
            List<ListViewItem> lists = new List<ListViewItem>();
            foreach (IPackedObject ipo in BaseRhoFile.NowFolderContent)
            {
                ListViewItem lvi;
                if (ipo.Type == ObjectType.File)
                {
                    RhoPackedFileInfo jpfi = (RhoPackedFileInfo)ipo;
                    lvi = new ListViewItem(new string[] { jpfi.FileName + '.' + jpfi.Extension, $"0x{Convert.ToString(jpfi.Index, 16).PadLeft(8, '0')}", ("listview_item2_file").GetStringBag() });
                    lvi.ImageKey = "file";
                    
                }
                else
                {
                    RhoPackedFolderInfo jpfi = (RhoPackedFolderInfo)ipo;
                    lvi = new ListViewItem(new string[] { jpfi.FolderName, $"0x{Convert.ToString(jpfi.Index, 16).PadLeft(8, '0')}", ("listview_item2_folder").GetStringBag() });
                    lvi.ImageKey = "folder";
                }
                lists.Add(lvi);
            }
            listView1.Items.AddRange(lists.ToArray());
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            BaseRhoFile.BackToParentFolder();
            if (BaseRhoFile.NowFolder.Index == 0xFFFFFFFF)
                this.toolStripButton1.Enabled = false;
            else
                this.toolStripButton1.Enabled = true;
            UpdateFolders();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutMe am = new AboutMe();
            am.Show();
            //
            
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button != MouseButtons.Right)
                return;
            if (listView1.SelectedItems.Count == 0)
                return;
            if(listView1.SelectedItems[0].SubItems[2].Text == ("listview_item2_file").GetStringBag()) 
            {
                 FileMenu.Show((Control)sender, e.X, e.Y);
                 convertToPngToolStripMenuItem.Enabled = listView1.SelectedItems[0].SubItems[0].Text.EndsWith(".tga") || listView1.SelectedItems[0].SubItems[0].Text.EndsWith(".dds");
            } 
        }

        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            //e.Graphics.DrawImage(global::RhoLoader.Properties.Resources.baseline_insert_drive_file_black_18dp,new Point(0,0));
        }
        FolderBrowserDialog fbd = new FolderBrowserDialog();
        private void exportAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckOpenStatus())
                MessageBox.Show(("msg_PlzOpenRhoFileFirst").GetStringBag(), ("title").GetStringBag(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            //Debug
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                ExportFolder ef = new ExportFolder();
                ef.ExportAllFolder(fbd.SelectedPath, true, BaseRhoFile);
                
            }
        }

        private bool CheckOpenStatus()
        {
            return !(BaseRhoFile is null);
        }



        private void Panel1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
        }
        SaveFileDialog ofd = new SaveFileDialog();
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RhoPackedFileInfo fileInfo = (RhoPackedFileInfo)Array.Find(BaseRhoFile.NowFolderContent, x => x.Type == ObjectType.File && (((RhoPackedFileInfo)x).FileName + $".{((RhoPackedFileInfo)x).Extension}") == listView1.SelectedItems[0].SubItems[0].Text);
            ofd.Filter = "AllFiles|*.*";
            ofd.FileName = $"{fileInfo.FileName}.{fileInfo.Extension}";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(ofd.FileName, FileMode.Create);
                byte[] a = BaseRhoFile.GetPackedFile(fileInfo);
                fs.Write(a, 0, a.Length);
                fs.Close();
                a = null;
            }
        }

        private void menuexportnowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckOpenStatus())
                MessageBox.Show(("msg_PlzOpenRhoFileFirst").GetStringBag(), ("title").GetStringBag(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            //Debug
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ExportFolder ef = new ExportFolder();
                ef.ExportNowFolder(fbd.SelectedPath, true, BaseRhoFile);

            }
        }

        private void convertToPngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RhoPackedFileInfo fileInfo = (RhoPackedFileInfo)Array.Find(BaseRhoFile.NowFolderContent, x => x.Type == ObjectType.File && (((RhoPackedFileInfo)x).FileName + $".{((RhoPackedFileInfo)x).Extension}") == listView1.SelectedItems[0].SubItems[0].Text);
            byte[] a = BaseRhoFile.GetPackedFile(fileInfo);
            TgaDDsViewer t = new TgaDDsViewer();
            t.Data = a;
            t.ConvertTGADDSToPng();
            a = null;
            
        }
    }
}
