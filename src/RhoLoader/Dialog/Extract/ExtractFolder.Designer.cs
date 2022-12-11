namespace RhoLoader
{
    partial class ExtractFolder
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
            this.btn_cancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.statusText = new System.Windows.Forms.Label();
            this.progress_main = new RhoLoader.Controls.DarkProgressBar();
            this.label_extract = new System.Windows.Forms.Label();
            this.label_extracting = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_progress = new System.Windows.Forms.Label();
            this.text_extract_file = new System.Windows.Forms.Label();
            this.text_progress = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_cancel
            // 
            this.btn_cancel.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_cancel.ForeColor = System.Drawing.Color.Black;
            this.btn_cancel.Location = new System.Drawing.Point(481, 115);
            this.btn_cancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(118, 34);
            this.btn_cancel.TabIndex = 4;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.action_cancel);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(69, 18);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 15);
            this.label5.TabIndex = 6;
            // 
            // statusText
            // 
            this.statusText.AutoSize = true;
            this.statusText.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.statusText.Location = new System.Drawing.Point(68, 18);
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(0, 14);
            this.statusText.TabIndex = 8;
            // 
            // progress_main
            // 
            this.progress_main.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.progress_main.Location = new System.Drawing.Point(-5, 97);
            this.progress_main.MaxValue = 100D;
            this.progress_main.Name = "progress_main";
            this.progress_main.Size = new System.Drawing.Size(617, 5);
            this.progress_main.TabIndex = 9;
            this.progress_main.Value = 0D;
            // 
            // label_extract
            // 
            this.label_extract.Font = new System.Drawing.Font("Segoe UI Variable Display", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label_extract.Location = new System.Drawing.Point(0, 0);
            this.label_extract.Name = "label_extract";
            this.label_extract.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.label_extract.Size = new System.Drawing.Size(330, 70);
            this.label_extract.TabIndex = 10;
            this.label_extract.Text = "Extracting...";
            this.label_extract.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_extracting
            // 
            this.label_extracting.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label_extracting.Location = new System.Drawing.Point(18, 70);
            this.label_extracting.Name = "label_extracting";
            this.label_extracting.Size = new System.Drawing.Size(81, 23);
            this.label_extracting.TabIndex = 11;
            this.label_extracting.Text = "Extracting:";
            this.label_extracting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 15);
            this.label2.TabIndex = 12;
            // 
            // label_progress
            // 
            this.label_progress.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label_progress.Location = new System.Drawing.Point(25, 105);
            this.label_progress.Name = "label_progress";
            this.label_progress.Size = new System.Drawing.Size(74, 23);
            this.label_progress.TabIndex = 13;
            this.label_progress.Text = "Progress:";
            this.label_progress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // text_extract_file
            // 
            this.text_extract_file.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.text_extract_file.Location = new System.Drawing.Point(112, 70);
            this.text_extract_file.Name = "text_extract_file";
            this.text_extract_file.Size = new System.Drawing.Size(487, 23);
            this.text_extract_file.TabIndex = 14;
            this.text_extract_file.Text = "etc_/test.1s";
            this.text_extract_file.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // text_progress
            // 
            this.text_progress.Font = new System.Drawing.Font("Segoe UI Variable Display", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.text_progress.Location = new System.Drawing.Point(112, 105);
            this.text_progress.Name = "text_progress";
            this.text_progress.Size = new System.Drawing.Size(109, 23);
            this.text_progress.TabIndex = 15;
            this.text_progress.Text = "3 / 256";
            this.text_progress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ExtractFolder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(612, 160);
            this.Controls.Add(this.text_progress);
            this.Controls.Add(this.text_extract_file);
            this.Controls.Add(this.label_progress);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_extracting);
            this.Controls.Add(this.label_extract);
            this.Controls.Add(this.progress_main);
            this.Controls.Add(this.statusText);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btn_cancel);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "ExtractFolder";
            this.Text = "dialog_extract_folder";
            this.Shown += new System.EventHandler(this.action_show);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Label label5;
        private Label statusText;
        private Controls.DarkProgressBar progress_main;
        private Label label_extract;
        private Label label_extracting;
        private Label label2;
        private Label label_progress;
        private Label text_extract_file;
        private Label text_progress;
    }
}