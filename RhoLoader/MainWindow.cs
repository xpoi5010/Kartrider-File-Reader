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
using KartRider.Xml;
using System.IO;
using System.Diagnostics;
using System.Resources;

namespace RhoLoader
{
    public partial class MainWindow : Form
    {
        public MainWindow()
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
        Rho BaseRhoFile;

        Stack<RhoDirectory> DirStack = new Stack<RhoDirectory>();

        RhoDirectory CurrentDirectory => DirStack.Peek();

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //RhoTesting
                if (BaseRhoFile is not null)
                    BaseRhoFile.Dispose();
                BaseRhoFile = new Rho(openFileDialog1.FileName);
                DirStack.Clear();
                DirStack.Push(BaseRhoFile.RootDirectory);
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
            this.filemenuconvertXMLToolStripMenuItem.Text = ((string)this.filemenuconvertXMLToolStripMenuItem.Tag).GetStringBag();
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
        Random rd = new Random();
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            if (listView1.SelectedItems.Count == 0)
                return;
            ListViewItem lvii = listView1.SelectedItems[0];
            if (listView1.SelectedItems[0].SubItems[1].Text == ("listview_item2_file").GetStringBag())
            {
                RhoFileInfo fileInfo = CurrentDirectory.Files.Find(x => x.FullFileName == lvii.Text);
                if(fileInfo.Extension == "dds" || fileInfo.Extension == "tga")
                {
                    TgaDDsViewer tdv = new TgaDDsViewer();
                    tdv.Data = fileInfo.GetData();
                    tdv.Type = fileInfo.Extension == "dds" ? TgaDDsViewer.FileType.dds : fileInfo.Extension == "tga" ? TgaDDsViewer.FileType.tga : throw new Exception();
                    tdv.ShowBox(); 
                    return;
                }
                else if(fileInfo.Extension == "bml")
                {
                    byte[] bmlData = fileInfo.GetData();
                    bmlViewer bv = new bmlViewer(bmlData,fileInfo.Name);
                    bv.Show();
                    return;
                }
                else if(fileInfo.Extension == "ksv")
                {
                    byte[] ksvData = fileInfo.GetData();
                    KSVPreview preview = new KSVPreview(ksvData);
                    preview.Show();
                    return;
                }
                string TempName = $"{Environment.GetEnvironmentVariable("TEMP")}\\{lvii.Text}_{rd.Next()}";
                FileStream fs = new FileStream(Environment.GetEnvironmentVariable("TEMP") + $"\\{lvii.Text}", FileMode.Create);
                byte[] a = fileInfo.GetData();
                fs.Write(a, 0, a.Length);
                fs.Close();
                a = null;
                Process ps = new Process();
                ps.StartInfo.FileName = "explorer.exe";
                ps.StartInfo.Arguments = Environment.GetEnvironmentVariable("TEMP") + $"\\{lvii.Text}";
                ps.Start();
                return;
            }

            string FolderName = lvii.SubItems[0].Text;
            RhoDirectory newDir = CurrentDirectory.Directories.Find(x=>x.DirectoryName == FolderName);
            DirStack.Push(newDir);
            UpdateFolders();
            this.toolStripButton1.Enabled = true;
        }

        private void UpdateFolders()
        {
            this.listView1.Items.Clear();
            this.toolStripTextBox1.Text = GetCurrentPath();
            List<ListViewItem> lists = new List<ListViewItem>();
            RhoDirectory curDir = CurrentDirectory;
            foreach (RhoDirectory dir in curDir.Directories)
            {
                ListViewItem lvi = new ListViewItem(new string[] { dir.DirectoryName, ("listview_item2_folder").GetStringBag() , $"" });
                lvi.ImageKey = "folder";
                lists.Add(lvi);
            }
            foreach (RhoFileInfo file in curDir.Files)
            {
                ListViewItem lvi = new ListViewItem(new string[] { file.FullFileName,  ("listview_item2_file").GetStringBag(), $"{FormatDataLength(file.FileSize)}"});
                lvi.ImageKey = "file";
                lists.Add(lvi);
            }
            listView1.Items.AddRange(lists.ToArray());
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            BackToPrevDir();
            if (CurrentDirectory.DirIndex == 0xFFFFFFFF)
                this.toolStripButton1.Enabled = false;
            else
                this.toolStripButton1.Enabled = true;
            UpdateFolders();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutMe am = new AboutMe();
            am.Show();
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button != MouseButtons.Right)
                return;
            if (listView1.SelectedItems.Count == 0)
                return;
            if(listView1.SelectedItems[0].SubItems[1].Text == ("listview_item2_file").GetStringBag()) 
            {
                 FileMenu.Show((Control)sender, e.X, e.Y);
                 convertToPngToolStripMenuItem.Enabled = listView1.SelectedItems[0].SubItems[0].Text.EndsWith(".tga") || listView1.SelectedItems[0].SubItems[0].Text.EndsWith(".dds");
                filemenuconvertXMLToolStripMenuItem.Enabled = listView1.SelectedItems[0].SubItems[0].Text.EndsWith(".bml");
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
                ExportOption eo = new ExportOption();
                eo.ExportAllFolder(fbd.SelectedPath, true, BaseRhoFile);
            }
        }

        private bool CheckOpenStatus()
        {
            return !(BaseRhoFile is null);
        }

        private bool BackToPrevDir()
        {
            if (DirStack.Count <= 1)
                return false;
            DirStack.Pop();
            return true;
        }

        private void Panel1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
        }

        SaveFileDialog ofd = new SaveFileDialog();

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string FileName = listView1.SelectedItems[0].SubItems[0].Text;
            RhoFileInfo fileInfo = CurrentDirectory.Files.Find(x => x.FullFileName == FileName);
            ofd.Filter = "AllFiles|*.*";
            ofd.FileName = fileInfo.FullFileName;
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(ofd.FileName, FileMode.Create);
                byte[] a = fileInfo.GetData();
                fs.Write(a, 0, a.Length);
                fs.Close();
                a = null;
            }
        }

        private void menuexportnowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckOpenStatus())
                MessageBox.Show(("msg_PlzOpenRhoFileFirst").GetStringBag(), ("title").GetStringBag(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (fbd.ShowDialog() == DialogResult.OK)
            {
                ExportOption eo = new ExportOption();
                eo.ExportNowFolder(fbd.SelectedPath, true, BaseRhoFile,CurrentDirectory);
            }
        }

        private void convertToPngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string FileName = listView1.SelectedItems[0].SubItems[0].Text;
            RhoFileInfo fileInfo = CurrentDirectory.Files.Find(x => x.FullFileName == FileName); 
            byte[] a = fileInfo.GetData();
            TgaDDsViewer t = new TgaDDsViewer();
            t.Data = a;
            t.ConvertTGADDSToPng();
            a = null;
            
        }

        private void filemenuconvertXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string FileName = listView1.SelectedItems[0].SubItems[0].Text;
            RhoFileInfo fileInfo = CurrentDirectory.Files.Find(x => x.FullFileName == FileName);
            byte[] a = fileInfo.GetData();
            BinaryXmlDocument bxd = new BinaryXmlDocument();
            bxd.Read(Encoding.GetEncoding("UTF-16"), a);
            string output = bxd.RootTag.ToString();
            byte[] output_data = Encoding.GetEncoding("UTF-16").GetBytes(output);
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = $"{fileInfo.Name}.xml";
            sfd.Filter = "XML File|*.xml";
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(sfd.FileName, FileMode.Create);
                fs.Write(output_data, 0, output_data.Length);
                fs.Close();
            }
        }

        private string GetCurrentPath()
        {
            string[] ConvertedArr = Array.ConvertAll(DirStack.ToArray(), x => x.DirectoryName);
            Array.Reverse(ConvertedArr);
            if (ConvertedArr.Length == 1)
                return "/";
            return string.Join("/",ConvertedArr);
        }

        private string FormatDataLength(int length)
        {
            string[] units = { "Bytes" , "KiB" ,"MiB"};
            double dlen = length;
            foreach(string unit in units)
            {
                if (dlen > 1024)
                    dlen /= 1024;
                else
                    return $"{dlen: 0.00} {unit}";
            }
            return $"{dlen} {units[units.Length-1]}";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public enum OpenMode
    {
        SingleRhoFile,
        DataFolder,
        None
    }
}
