
namespace RhoLoader
{
    partial class bmlViewer
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.poweredByMicrosoftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ucXmlRichTextBox1 = new CustomXmlViewer.ucXmlRichTextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAsToolStripMenuItem,
            this.poweredByMicrosoftToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(103, 20);
            this.saveAsToolStripMenuItem.Text = "Convert To XML";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // poweredByMicrosoftToolStripMenuItem
            // 
            this.poweredByMicrosoftToolStripMenuItem.Enabled = false;
            this.poweredByMicrosoftToolStripMenuItem.Name = "poweredByMicrosoftToolStripMenuItem";
            this.poweredByMicrosoftToolStripMenuItem.Size = new System.Drawing.Size(135, 20);
            this.poweredByMicrosoftToolStripMenuItem.Text = "Powered By Microsoft";
            // 
            // ucXmlRichTextBox1
            // 
            this.ucXmlRichTextBox1.BackColor = System.Drawing.Color.White;
            this.ucXmlRichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ucXmlRichTextBox1.DetectUrls = false;
            this.ucXmlRichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucXmlRichTextBox1.Font = new System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucXmlRichTextBox1.Location = new System.Drawing.Point(0, 24);
            this.ucXmlRichTextBox1.Name = "ucXmlRichTextBox1";
            this.ucXmlRichTextBox1.ReadOnly = true;
            this.ucXmlRichTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.ucXmlRichTextBox1.Size = new System.Drawing.Size(800, 426);
            this.ucXmlRichTextBox1.TabIndex = 1;
            this.ucXmlRichTextBox1.Text = "";
            this.ucXmlRichTextBox1.WordWrap = false;
            this.ucXmlRichTextBox1.Xml = "";
            // 
            // bmlViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ucXmlRichTextBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "bmlViewer";
            this.Text = "bmlViewer";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem poweredByMicrosoftToolStripMenuItem;
        private CustomXmlViewer.ucXmlRichTextBox ucXmlRichTextBox1;
    }
}