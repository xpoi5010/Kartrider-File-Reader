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
using System.Diagnostics;

namespace RhoLoader
{
    public partial class ExtractFolder : Form
    {
        private struct ExtractInfo
        {
            public string RelativePath { get; set; }
            public string Out_filename { get; set; }
            public PackFileInfo FileInfo { get; set; }
            public FileConvertProcessor ConvertProcessor { get; set; }
        }

        private delegate byte[] FileConvertProcessor(byte[] input);

        private static class ExtractConverter
        {
            public static byte[] DDSConverter(byte[] inputData)
            {
                var stream = new MemoryStream(inputData);
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
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] output = ms.ToArray();

                handle.Free();
                image.Dispose();
                ms.Dispose();
                bmp.Dispose();

                return output;
            }

            public static byte[] BMLConverter(byte[] inputData)
            {
                KartRider.Xml.BinaryXmlDocument bxd = new KartRider.Xml.BinaryXmlDocument();
                bxd.Read(Encoding.GetEncoding("UTF-16"), inputData);
                KartRider.Xml.BinaryXmlTag bxt = bxd.RootTag;
                string xmlData = bxt.ToString();
                byte[] output = Encoding.UTF8.GetBytes(xmlData);
                return output;
            }

            public static byte[] KSVConverter(byte[] inputData)
            {
                return inputData;
            }

            public static byte[] NoneConvert(byte[] inputData)
            {
                return inputData;
            }
        }

        private PackFolderInfo _extract_folder;
        private ExtractOptionToken _extract_option;
        private Task _bg_worker;
        private string _extract_path;
        private int _totalFiles = 0;

        private bool _terminated = false;
        private bool _bg_worker_finished = false;
        

        public ExtractFolder(PackFolderInfo extract_folder, string extract_to_path, ExtractOptionToken extract_option)
        {
            InitializeComponent();
            _extract_folder = extract_folder;
            _extract_option = extract_option;
            _extract_path = extract_to_path;
        }

        private void BeginExtract()
        {
            //relative_path means the relative path of folder.
            Queue<(string relative_path, PackFolderInfo folder)> extend_queue = new Queue<(string relative_path, PackFolderInfo folder)>();
            Queue<ExtractInfo> file_queue = new Queue<ExtractInfo>();
            extend_queue.Enqueue(( "",_extract_folder));
            ReportProgress("( Preprocessing extract files )", 0);
            while (extend_queue.Count > 0) 
            {
                if (_terminated)
                {
                    _bg_worker_finished = true;
                    TerminateExtract();
                    return;
                }
                (string relative_path, PackFolderInfo folder) cur_proc_obj = extend_queue.Dequeue();
                string out_path = $"{_extract_path}{cur_proc_obj.relative_path}";
                if(!Directory.Exists(out_path))
                    Directory.CreateDirectory(out_path);
                foreach(PackFolderInfo sub_folder in cur_proc_obj.folder.GetFoldersInfo())
                {
                    extend_queue.Enqueue(($"{cur_proc_obj.relative_path}\\{sub_folder.FolderName}", sub_folder));
                }
                foreach(PackFileInfo sub_file in cur_proc_obj.folder.GetFilesInfo())
                {
                    ExtractInfo extractInfo = new ExtractInfo
                    {
                        RelativePath = cur_proc_obj.relative_path,
                        FileInfo = sub_file
                    };
                    if((_extract_option & ExtractOptionToken.ConvertDDS) != ExtractOptionToken.None && sub_file.FileName.EndsWith(".dds"))
                    {
                        extractInfo.ConvertProcessor = ExtractConverter.DDSConverter;
                        extractInfo.Out_filename = $"{sub_file.FileName[0..^4]}.png";
                    }
                    else if ((_extract_option & ExtractOptionToken.ConvertBML) != ExtractOptionToken.None && sub_file.FileName.EndsWith(".bml"))
                    {
                        extractInfo.ConvertProcessor = ExtractConverter.BMLConverter;
                        extractInfo.Out_filename = $"{sub_file.FileName[0..^4]}.xml";
                    }
                    else if ((_extract_option & ExtractOptionToken.ConvertKSV) != ExtractOptionToken.None && sub_file.FileName.EndsWith(".ksv"))
                    {
                        extractInfo.ConvertProcessor = ExtractConverter.KSVConverter;
                        extractInfo.Out_filename = $"{sub_file.FileName[0..^4]}.json";
                    }
                    else
                    {
                        extractInfo.Out_filename = $"{sub_file.FileName}";
                    }
                    file_queue.Enqueue(extractInfo);
                }
            }
            _totalFiles = file_queue.Count;
            if (this.InvokeRequired)
                this.Invoke(() =>
                {
                    this.progress_main.MaxValue = _totalFiles;
                });
            while(file_queue.Count > 0)
            {
                if (_terminated)
                {
                    _bg_worker_finished = true;
                    TerminateExtract();
                    return;
                }
                ExtractInfo extract_info = file_queue.Dequeue();
                ReportProgress(extract_info.FileInfo.FullName, _totalFiles - file_queue.Count);
                FileStream out_fs = new FileStream($"{_extract_path}{extract_info.RelativePath}\\{extract_info.Out_filename}", FileMode.Create);
                byte[] file_data = extract_info.FileInfo.GetData();
                try
                {
                    byte[] proc_file_data = extract_info.ConvertProcessor?.Invoke(file_data) ?? file_data;
                    if (proc_file_data is null || proc_file_data.Length == 0)
                        throw new Exception("zero!");

                    out_fs.Write(proc_file_data, 0, proc_file_data.Length);
                    out_fs.Close();
                    file_data = null;
                    proc_file_data = null;
                }
                catch (Exception ex) 
                {
                    Debug.Print($"Error: {ex.Message}");
                    this._bg_worker_finished = true;
                    TerminateExtract();
                }
            }
            ReportProgress("Finished", _totalFiles);
            _bg_worker_finished = true;
            FinishExtract();
        }

        private void FinishExtract()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(FinishExtract);
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void TerminateExtract()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(TerminateExtract);
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void ReportProgress(string current_output_file, int file_no)
        {
            if (this.InvokeRequired)
            {
                Action<string, int> action = ReportProgress;
                this.Invoke(action, current_output_file, file_no);
            }
            else
            {
                text_extract_file.Text = current_output_file;
                text_progress.Text = $"{file_no}/{_totalFiles}";
                progress_main.Value = file_no - 1;

            }
        }

        // actions
        private void action_show(object sender, EventArgs e)
        {
            _bg_worker = new Task(BeginExtract);
            _bg_worker.Start();
        }

        private void action_cancel(object sender, EventArgs e)
        {
            if(MessageBox.Show("msg_cancelExtract".GetStringBag(), "msg_level_question".GetStringBag(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _terminated = true;
            }
        }
    }
}
