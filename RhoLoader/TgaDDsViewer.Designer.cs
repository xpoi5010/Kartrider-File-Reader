namespace RhoLoader
{
    partial class TgaDDsViewer
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.turnToDarkBackgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToPngToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scale = new System.Windows.Forms.ToolStripMenuItem();
            this.poweredByPfiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Enabled = false;
            this.pictureBox1.Location = new System.Drawing.Point(0, 31);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(354, 277);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.turnToDarkBackgroundToolStripMenuItem,
            this.saveToPngToolStripMenuItem,
            this.scale,
            this.poweredByPfiToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(933, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // turnToDarkBackgroundToolStripMenuItem
            // 
            this.turnToDarkBackgroundToolStripMenuItem.Name = "turnToDarkBackgroundToolStripMenuItem";
            this.turnToDarkBackgroundToolStripMenuItem.Size = new System.Drawing.Size(151, 20);
            this.turnToDarkBackgroundToolStripMenuItem.Text = "Turn to Dark Background";
            this.turnToDarkBackgroundToolStripMenuItem.Click += new System.EventHandler(this.turnToDarkBackgroundToolStripMenuItem_Click);
            // 
            // saveToPngToolStripMenuItem
            // 
            this.saveToPngToolStripMenuItem.Name = "saveToPngToolStripMenuItem";
            this.saveToPngToolStripMenuItem.Size = new System.Drawing.Size(82, 20);
            this.saveToPngToolStripMenuItem.Text = "Save To Png";
            this.saveToPngToolStripMenuItem.Click += new System.EventHandler(this.saveToPngToolStripMenuItem_Click);
            // 
            // scale
            // 
            this.scale.Enabled = false;
            this.scale.Name = "scale";
            this.scale.Size = new System.Drawing.Size(49, 20);
            this.scale.Text = "Scale:";
            // 
            // poweredByPfiToolStripMenuItem
            // 
            this.poweredByPfiToolStripMenuItem.Enabled = false;
            this.poweredByPfiToolStripMenuItem.Name = "poweredByPfiToolStripMenuItem";
            this.poweredByPfiToolStripMenuItem.Size = new System.Drawing.Size(109, 20);
            this.poweredByPfiToolStripMenuItem.Text = "Powered By Pfim";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(933, 519);
            this.panel1.TabIndex = 2;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.panel1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseWheel);
            // 
            // TgaDDsViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "TgaDDsViewer";
            this.Text = "Image Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem turnToDarkBackgroundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToPngToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scale;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem poweredByPfiToolStripMenuItem;
    }
}