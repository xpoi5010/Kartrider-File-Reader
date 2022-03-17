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
using Pfim;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace RhoLoader
{
    public partial class ExportFolder : Form
    {
        public ExportFolder()
        {
            InitializeComponent();
        }


        public void ExportNowFolder(string TargetPath, bool IncludeSubFolder, Rho file, RhoDirectory curDir, bool ConvertBML = false, bool ConvertDDS = false)
        {
            this.Show();
            Task task = new Task(() =>
            {
                ExportFolder_bg(TargetPath,IncludeSubFolder, file, curDir, ConvertBML, ConvertDDS);
            });
            task.Start();
        }

        public void ExportAllFolder(string TargetPath, bool IncludeSubFolder, Rho file, bool ConvertBML = false, bool ConvertDDS = false)
        {
            this.Show();
            Task task = new Task(() =>
            {
                ExportFolder_bg(TargetPath, IncludeSubFolder, file, file.RootDirectory, ConvertBML, ConvertDDS);
            });
            task.Start();
        }

        bool Canceled = false;

        private void ExportFolder_bg(string outputPath, bool IncludeSubFolder,Rho file,RhoDirectory curDir, bool ConvertBML = false, bool ConvertDDS = false, bool ConvertKSV = false)
        {
            Queue<ExportProcessor> processQueue = new Queue<ExportProcessor>();
            if (!Directory.Exists(outputPath))
                throw new Exception("");
            int _outputCounter = 0;
            processQueue.Enqueue(new ExportProcessor()
            {
                Directories = curDir.Directories,
                Path = "\\",
                Files = curDir.Files 
            });
            while(processQueue.Count > 0 && !Canceled)
            {
                ExportProcessor curProc = processQueue.Dequeue();
                _outputCounter = 0;
                foreach (RhoDirectory dir in curProc.Directories)
                {
                    string fullOutPath = $"{curProc.Path}\\{dir.DirectoryName}";
                    if (!Directory.Exists($"{outputPath}{fullOutPath}"))
                        Directory.CreateDirectory($"{outputPath}{fullOutPath}");
                    processQueue.Enqueue(new ExportProcessor()
                    {
                        Directories = dir.Directories,
                        Path = fullOutPath,
                        Files = dir.Files
                    });
                }
                foreach(RhoFileInfo fileInfo in curProc.Files)
                {
                    byte[] data = fileInfo.GetData();
                    string fileName = fileInfo.FullFileName;
                    if (ConvertBML && fileInfo.Extension == "bml")
                    {
                        KartRider.Xml.BinaryXmlDocument bxd = new KartRider.Xml.BinaryXmlDocument();
                        bxd.Read(Encoding.GetEncoding("UTF-16"), data);
                        KartRider.Xml.BinaryXmlTag bxt = bxd.RootTag;
                        string xmlData = bxt.ToString();
                        data = Encoding.UTF8.GetBytes(xmlData);
                        fileName = fileName.Replace(".bml", ".xml");
                    }
                    else if (ConvertDDS && fileInfo.Extension == "dds")
                    {
                        var stream = new MemoryStream(data);
                        IImage image = Pfim.Pfim.FromStream(stream);
                        var handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
                        var d = Marshal.UnsafeAddrOfPinnedArrayElement(image.Data, 0);
                        PixelFormat pf;
                        switch (image.Format)
                        {
                            case Pfim.ImageFormat.Rgba32:
                                pf = PixelFormat.Format32bppArgb;
                                break;
                            case Pfim.ImageFormat.Rgba16:
                                pf = PixelFormat.Format16bppArgb1555;
                                break;
                            case Pfim.ImageFormat.Rgb8:
                                pf = PixelFormat.Format8bppIndexed;
                                break;
                            case Pfim.ImageFormat.Rgb24:
                                pf = PixelFormat.Format24bppRgb;
                                break;
                            default:
                                throw new Exception("");
                        }
                        stream.Dispose();
                        stream = new MemoryStream();
                        Bitmap bmp = new Bitmap(image.Width, image.Height, image.Stride, pf, d);
                        bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        handle.Free();
                        fileName = fileName.Replace(".dds", ".png");
                        data = stream.ToArray();
                        stream.Dispose();
                    }
                    if ((_outputCounter % 5) == 0)
                        ChangeText(statusText, $"{curProc.Path}\\{fileName}");
                    FileStream fs = new FileStream($"{outputPath}{curProc.Path}\\{fileName}",FileMode.Create);
                    fs.Write(data,0, data.Length);
                    fs.Close();
                    _outputCounter++;
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

            public IEnumerable<RhoFileInfo> Files;

            public IEnumerable<RhoDirectory> Directories;
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
