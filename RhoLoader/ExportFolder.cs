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

namespace RhoLoader
{
    public partial class ExportFolder : Form
    {
        public ExportFolder()
        {
            InitializeComponent();
        }


        public void ExportNowFolder(string TargetPath, bool IncludeSubFolder, RhoFile file)
        {
            this.Show();
            Task task = new Task(() =>
            {
                ExportNowFolder_bg(TargetPath,IncludeSubFolder, file);
            });
            task.Start();
        }

        public void ExportAllFolder(string TargetPath, bool IncludeSubFolder, RhoFile file)
        {
            this.Show();
            Task task = new Task(() =>
            {
                ExportAllFolder_bg(TargetPath, IncludeSubFolder, file);
            });
            task.Start();
        }

        bool Canceled = false;

        private void ExportNowFolder_bg(string outputPath, bool IncludeSubFolder,RhoFile file)
        {
            IPackedObject[] ipos = file.NowFolderContent;
            Stack<ExportProcessor> Stack = new Stack<ExportProcessor>();
            if (!Directory.Exists(outputPath))
                throw new Exception("");
            string StartPath = file.NowPath=="/"?"": file.NowPath;
            Stack.Push(new ExportProcessor
            {
                Path = StartPath,
                ipos = ipos
            }) ;
            while(Stack.Count > 0 && !Canceled)
            {
                ExportProcessor ep = Stack.Pop();
                string outPath = outputPath + (StartPath=="" ? ep.Path : ep.Path.Replace(StartPath, ""));
                foreach (IPackedObject ipo in ep.ipos)
                {
                    
                    if(ipo.Type == ObjectType.Folder && IncludeSubFolder)
                    {
                        RhoPackedFolderInfo jpfi = (RhoPackedFolderInfo)ipo;
                        ExportProcessor nep = new ExportProcessor
                        {
                            Path = ep.Path + $"\\{jpfi.FolderName}",
                            ipos = RhoPackedFilesInfoDecoder.GetRhoPackedFileInfos(file.GetStreamData(jpfi.Index), file.HeaderKey, jpfi.Index)
                        };
                        Stack.Push(nep);
                        if (!Directory.Exists($"{outPath}\\{((RhoPackedFolderInfo)ipo).FolderName}"))
                        {
                            Directory.CreateDirectory($"{outPath}\\{((RhoPackedFolderInfo)ipo).FolderName}");
                        }
                        continue;
                    }
                    if(ipo.Type == ObjectType.File)
                    {
                        RhoPackedFileInfo jfi = (RhoPackedFileInfo)ipo;
                        ChangeText(label5, $"Outputing: {ep.Path}\\{jfi.FileName}.{jfi.Extension}");
                        FileStream fs = new FileStream($"{outPath}\\{jfi.FileName}.{jfi.Extension}", FileMode.Create);
                        byte[] data = file.GetPackedFile(jfi);
                        fs.Write(data, 0, data.Length);
                        fs.Close();
                        data = null;
                    }
                }
                
            }
            CloseWindow();


        }

        private void ExportAllFolder_bg(string outputPath, bool IncludeSubFolder, RhoFile file)
        {
            IPackedObject[] ipos = RhoPackedFilesInfoDecoder.GetRhoPackedFileInfos(file.GetStreamData(0xFFFFFFFF), file.HeaderKey, 0xFFFFFFFF);
            Stack<ExportProcessor> Stack = new Stack<ExportProcessor>();
            if (!Directory.Exists(outputPath))
                throw new Exception("");
            string StartPath = "";
            Stack.Push(new ExportProcessor
            {
                Path = StartPath,
                ipos = ipos
            });
            while (Stack.Count > 0 && !Canceled)
            {
                ExportProcessor ep = Stack.Pop();
                string outPath = outputPath + (StartPath == "" ? ep.Path : ep.Path.Replace(StartPath, ""));
                foreach (IPackedObject ipo in ep.ipos)
                {

                    if (ipo.Type == ObjectType.Folder && IncludeSubFolder)
                    {
                        RhoPackedFolderInfo jpfi = (RhoPackedFolderInfo)ipo;
                        ExportProcessor nep = new ExportProcessor
                        {
                            Path = ep.Path + $"\\{jpfi.FolderName}",
                            ipos = RhoPackedFilesInfoDecoder.GetRhoPackedFileInfos(file.GetStreamData(jpfi.Index), file.HeaderKey, jpfi.Index)
                        };
                        Stack.Push(nep);
                        if (!Directory.Exists($"{outPath}\\{((RhoPackedFolderInfo)ipo).FolderName}"))
                        {
                            Directory.CreateDirectory($"{outPath}\\{((RhoPackedFolderInfo)ipo).FolderName}");
                        }
                        continue;
                    }
                    if (ipo.Type == ObjectType.File)
                    {
                        RhoPackedFileInfo jfi = (RhoPackedFileInfo)ipo;
                        ChangeText(label5, $"Outputing: {ep.Path}\\{jfi.FileName}.{jfi.Extension}");
                        FileStream fs = new FileStream($"{outPath}\\{jfi.FileName}.{jfi.Extension}", FileMode.Create);
                        byte[] data = file.GetPackedFile(jfi);
                        fs.Write(data, 0, data.Length);
                        fs.Close();
                        data = null;
                    }
                    if (Canceled)
                        break;
                }

            }
            CloseWindow();


        }

        public void CloseWindow()
        {
            if (this.InvokeRequired)
            {
                Action action = new Action(this.CloseWindow);
                this.Invoke(action);
            }
            else
            {
                this.Close();
            }
        }

        public void Cancel()
        {

        }

       private delegate void changeText(Label label, string Text);

       private void ChangeText(Label label, string Text)
       {
            if (this.InvokeRequired)
            {
                changeText ct = new changeText(ChangeText);
                this.Invoke(ct, label, Text);
            }
            else
            {
                label.Text = Text;
            }
        }
        private class ExportProcessor
        {
            public string Path;

            public IPackedObject[] ipos;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(("msg_cancelExport").GetStringBag(), ("title").GetStringBag(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Canceled = true;
            }
                
        }
    }
}
