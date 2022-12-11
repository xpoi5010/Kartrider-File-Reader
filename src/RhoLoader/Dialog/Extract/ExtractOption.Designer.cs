namespace RhoLoader
{
    partial class ExtractOption
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
            this.btn_extract = new System.Windows.Forms.Button();
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
            // btn_extract
            // 
            this.btn_extract.Location = new System.Drawing.Point(12, 87);
            this.btn_extract.Name = "btn_extract";
            this.btn_extract.Size = new System.Drawing.Size(133, 23);
            this.btn_extract.TabIndex = 2;
            this.btn_extract.Text = "Start Export";
            this.btn_extract.UseVisualStyleBackColor = true;
            this.btn_extract.Click += new System.EventHandler(this.action_submit);
            // 
            // ExtractOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(157, 115);
            this.Controls.Add(this.btn_extract);
            this.Controls.Add(this.conDds);
            this.Controls.Add(this.conBml);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ExtractOption";
            this.Text = "Option";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExportOption_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CheckBox conBml;
        private CheckBox conDds;
        private Button btn_extract;
    }
}