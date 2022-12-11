namespace RhoLoader.Controls
{
    partial class DarkProgressBar
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer_ani = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer_ani
            // 
            this.timer_ani.Interval = 13;
            this.timer_ani.Tick += new System.EventHandler(this.timer_ani_Tick);
            // 
            // DarkProgressBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "DarkProgressBar";
            this.Size = new System.Drawing.Size(622, 10);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer_ani;
    }
}
