using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pfim;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace RhoLoader
{
    public partial class TgaDDsViewer : Form
    {
        public TgaDDsViewer()
        {
            InitializeComponent();
        }

        public byte[] Data { get; set; }

        public FileType Type { get; set; }

        private double scale_N = 1.0d;

        public enum FileType
        {
            dds,tga
        }
        GCHandle handle;
        public void ShowBox()
        {
            this.Show();
            using(MemoryStream ms = new MemoryStream(Data))
            {
                IImage image = Pfim.Pfim.FromStream(ms);
                handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
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
                Bitmap bmp = new Bitmap(image.Width,image.Height,image.Stride,pf,d);
                pictureBox1.Image = bmp;
                pictureBox1.Width = image.Width;
                pictureBox1.Height = image.Height;
                pictureBox1.Location = new Point(0, 24);
                scale_N = 1;
                scale.Text = $"{scale_N:00.00x}";
            }
        }

        ~TgaDDsViewer()
        {
            handle.Free();

        }
        bool Dark = false;
        private void turnToDarkBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Dark)
            {
                pictureBox1.BackColor = Color.FromArgb(240, 240, 240);
                Dark = false;
                panel1.BackColor = pictureBox1.BackColor;
                turnToDarkBackgroundToolStripMenuItem.Text = "Turn to Dark Background";
                return;
            }
            pictureBox1.BackColor = Color.FromArgb(31, 31, 31);
            Dark = true;
            turnToDarkBackgroundToolStripMenuItem.Text = "Turn to Light Background";
            panel1.BackColor = pictureBox1.BackColor;
        }

        private void saveToPngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog savePng = new SaveFileDialog
            {
                Filter = "PNGFile|*.png",
                Title = "Select the location you want to save."
            };
            if(savePng.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(savePng.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        public void ConvertTGADDSToPng()
        {
            using (MemoryStream ms = new MemoryStream(Data))
            {
                IImage image = Pfim.Pfim.FromStream(ms);
                handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
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
                Bitmap bmp = new Bitmap(image.Width, image.Height, image.Stride, pf, d);
                SaveFileDialog savePng = new SaveFileDialog
                {
                    Filter = "PNGFile|*.png",
                    Title = "Select the location you want to save."
                };
                if (savePng.ShowDialog() == DialogResult.OK)
                {
                    bmp.Save(savePng.FileName, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            lastEventX = e.X;
            lastEventY =e.Y;
        }

        private void panel1_MouseWheel(object sender,MouseEventArgs e)
        {
            if(e.Delta > 0)
            {
                scale_N += 0.27d;
                if (scale_N >= 5.05d)
                    scale_N = 5.05d;

                scale.Text = $"{scale_N:00.00x}";
                UpdatePictureBox();
            }
            else if(e.Delta < 0)
            {

                scale_N -= 0.27d;

                if (scale_N < 0.19d)
                    scale_N = 0.19d;

                scale.Text = $"{scale_N:00.00x}";
                UpdatePictureBox();

            }
        }

        private void UpdatePictureBox()
        {
            pictureBox1.Size = new Size((int)(pictureBox1.Image.Size.Width * scale_N), (int)(pictureBox1.Image.Size.Height * scale_N));
        }

        int lastEventX = 0;
        int lastEventY = 0;

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            int deltaX = e.X - lastEventX;
            int deltaY = e.Y - lastEventY;
            pictureBox1.Location = Point.Add(pictureBox1.Location, new Size(deltaX,deltaY));
            lastEventX = e.X;
            lastEventY = e.Y;
            //Debug.Print($"x:{deltaX} y:{deltaY}");
        }
    }
}
