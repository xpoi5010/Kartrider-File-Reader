using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RhoLoader.Controls
{
    public partial class DarkProgressBar : UserControl
    {
        private double m_value = 0d;
        public double Value
        {
            get
            {
                return m_value;
            }
            set
            {
                if (m_value > m_max_value)
                    m_value = m_max_value;
                else
                    m_value = value;
                UpdateProgress(m_value);
            }
        }
        private double m_max_value = 100d;
        public double MaxValue
        {
            get
            {
                return m_max_value;
            }
            set
            {
                m_max_value = value;
            }
        }


        private DateTime LastUpdate_Time = DateTime.Now;
        private double LastUpdate_StartValue = 0d;
        private double LastUpdate_EndValue = 0d;
        private double LastUpdate_CurrentVal = 0d;

        private const double AnimationDuration = 500;// ms
        private const double MaxAnimationDuration = 800; // ms

        public DarkProgressBar()
        {
            InitializeComponent();
            base.DoubleBuffered = true;
            timer_ani.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graph = e.Graphics;
            graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            graph.PixelOffsetMode= System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            graph.FillRectangle(new SolidBrush(Color.FromArgb(0x00,0x7A,0xB5)), 0, 0, (float)(LastUpdate_CurrentVal / m_max_value) * this.Width, this.Height);
            base.OnPaint(e);
        }

        private void UpdateProgress(double val)
        {
            if (this.InvokeRequired)
            {
                Action<double> action = new Action<double>(UpdateProgress);
                this.Invoke(action, val);
            }
            else
            {
                LastUpdate_Time = DateTime.Now;
                LastUpdate_StartValue = LastUpdate_CurrentVal;
                LastUpdate_EndValue = val;
            }
        }

        private void timer_ani_Tick(object sender, EventArgs e)
        {
            double _animation_duration = (MaxAnimationDuration / this.m_max_value) * (LastUpdate_EndValue - LastUpdate_StartValue);
            TimeSpan dur = DateTime.Now - LastUpdate_Time;
            if (dur.TotalMilliseconds >= _animation_duration)
                LastUpdate_CurrentVal = LastUpdate_EndValue;
            else
            {
                LastUpdate_CurrentVal = LastUpdate_StartValue + ((LastUpdate_EndValue - LastUpdate_StartValue) * (dur.TotalMilliseconds / _animation_duration));
                this.Refresh();
            }
            this.Refresh();
        }
    }
}
