namespace RhoLoader
{
    partial class ExportOption
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
            this.conBml = new System.Windows.Forms.CheckBox();
            this.conDds = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.conKsv = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // conBml
            // 
            this.conBml.AutoSize = true;
            this.conBml.Location = new System.Drawing.Point(12, 12);
            this.conBml.Name = "conBml";
            this.conBml.Size = new System.Drawing.Size(136, 19);
            this.conBml.TabIndex = 0;
            this.conBml.Text = "Convert BML to XML";
            this.conBml.UseVisualStyleBackColor = true;
            // 
            // conDds
            // 
            this.conDds.AutoSize = true;
            this.conDds.Location = new System.Drawing.Point(12, 37);
            this.conDds.Name = "conDds";
            this.conDds.Size = new System.Drawing.Size(134, 19);
            this.conDds.TabIndex = 1;
            this.conDds.Text = "Convert DDS to PNG";
            this.conDds.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 87);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Start Export";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // conKsv
            // 
            this.conKsv.AutoSize = true;
            this.conKsv.Enabled = false;
            this.conKsv.Location = new System.Drawing.Point(12, 62);
            this.conKsv.Name = "conKsv";
            this.conKsv.Size = new System.Drawing.Size(136, 19);
            this.conKsv.TabIndex = 3;
            this.conKsv.Text = "Convert KSV to JSON";
            this.conKsv.UseVisualStyleBackColor = true;
            // 
            // ExportOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(157, 115);
            this.Controls.Add(this.conKsv);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.conDds);
            this.Controls.Add(this.conBml);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ExportOption";
            this.Text = "Option";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExportOption_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CheckBox conBml;
        private CheckBox conDds;
        private Button button1;
        private CheckBox conKsv;
    }
}