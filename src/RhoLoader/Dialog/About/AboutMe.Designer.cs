namespace RhoLoader
{
    partial class AboutMe
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
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.version = new System.Windows.Forms.Label();
            this.checkUpdateStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Candara", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(14, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 45);
            this.label1.TabIndex = 0;
            this.label1.Text = "Rho Loader";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button1.Location = new System.Drawing.Point(588, 22);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(131, 40);
            this.button1.TabIndex = 2;
            this.button1.Text = "Feedback";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(13, 93);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(605, 57);
            this.label2.TabIndex = 3;
            this.label2.Text = "Rho Loader is a tool which helps you to explore the files included in Rho Type Fi" +
    "le. \r\nAuthor: eP Game Studio\r\nLicense: GPL v3";
            // 
            // version
            // 
            this.version.AutoSize = true;
            this.version.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.version.Location = new System.Drawing.Point(255, 27);
            this.version.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.version.Name = "version";
            this.version.Size = new System.Drawing.Size(93, 19);
            this.version.TabIndex = 4;
            this.version.Text = "Version : 1.0";
            // 
            // checkUpdateStatus
            // 
            this.checkUpdateStatus.AutoSize = true;
            this.checkUpdateStatus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkUpdateStatus.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkUpdateStatus.ForeColor = System.Drawing.Color.OrangeRed;
            this.checkUpdateStatus.Location = new System.Drawing.Point(257, 54);
            this.checkUpdateStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.checkUpdateStatus.Name = "checkUpdateStatus";
            this.checkUpdateStatus.Size = new System.Drawing.Size(239, 13);
            this.checkUpdateStatus.TabIndex = 5;
            this.checkUpdateStatus.Text = "New Version Released, Clicking for updating.";
            this.checkUpdateStatus.Click += new System.EventHandler(this.checkUpdateStatus_Click);
            // 
            // AboutMe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(741, 200);
            this.Controls.Add(this.checkUpdateStatus);
            this.Controls.Add(this.version);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "AboutMe";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label version;
        private System.Windows.Forms.Label checkUpdateStatus;
    }
}