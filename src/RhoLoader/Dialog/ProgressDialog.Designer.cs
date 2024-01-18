namespace RhoLoader.Dialog
{
    partial class ProgressDialog
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
            text_progress = new Label();
            text_extract_file = new Label();
            label_progress = new Label();
            label2 = new Label();
            label_extracting = new Label();
            label_extract = new Label();
            progress_main = new Controls.DarkProgressBar();
            statusText = new Label();
            label5 = new Label();
            btn_cancel = new Button();
            SuspendLayout();
            // 
            // text_progress
            // 
            text_progress.Font = new Font("Segoe UI Variable Display", 10F);
            text_progress.Location = new Point(114, 107);
            text_progress.Name = "text_progress";
            text_progress.Size = new Size(109, 23);
            text_progress.TabIndex = 25;
            text_progress.Text = "3 / 256";
            text_progress.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // text_extract_file
            // 
            text_extract_file.Font = new Font("Segoe UI Variable Display", 10F);
            text_extract_file.Location = new Point(114, 72);
            text_extract_file.Name = "text_extract_file";
            text_extract_file.Size = new Size(487, 23);
            text_extract_file.TabIndex = 24;
            text_extract_file.Text = "status";
            text_extract_file.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label_progress
            // 
            label_progress.Font = new Font("Segoe UI Variable Display Semib", 9.75F, FontStyle.Bold);
            label_progress.Location = new Point(27, 107);
            label_progress.Name = "label_progress";
            label_progress.Size = new Size(74, 23);
            label_progress.TabIndex = 23;
            label_progress.Text = "Progress:";
            label_progress.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 88);
            label2.Name = "label2";
            label2.Size = new Size(0, 15);
            label2.TabIndex = 22;
            // 
            // label_extracting
            // 
            label_extracting.Font = new Font("Segoe UI Variable Display Semib", 9.75F, FontStyle.Bold);
            label_extracting.Location = new Point(20, 72);
            label_extracting.Name = "label_extracting";
            label_extracting.Size = new Size(81, 23);
            label_extracting.TabIndex = 21;
            label_extracting.Text = "Label";
            label_extracting.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label_extract
            // 
            label_extract.Font = new Font("Segoe UI Variable Display", 24F);
            label_extract.Location = new Point(2, 2);
            label_extract.Name = "label_extract";
            label_extract.Padding = new Padding(25, 0, 0, 0);
            label_extract.Size = new Size(603, 70);
            label_extract.TabIndex = 20;
            label_extract.Text = "Progress Dialog Title";
            label_extract.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // progress_main
            // 
            progress_main.BackColor = Color.FromArgb(225, 225, 225);
            progress_main.Location = new Point(0, 99);
            progress_main.MaxValue = 100D;
            progress_main.Name = "progress_main";
            progress_main.Size = new Size(617, 5);
            progress_main.TabIndex = 19;
            progress_main.Value = 0D;
            // 
            // statusText
            // 
            statusText.AutoSize = true;
            statusText.Font = new Font("Consolas", 9F);
            statusText.Location = new Point(70, 20);
            statusText.Name = "statusText";
            statusText.Size = new Size(0, 14);
            statusText.TabIndex = 18;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(71, 20);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(0, 15);
            label5.TabIndex = 17;
            // 
            // btn_cancel
            // 
            btn_cancel.Font = new Font("Segoe UI Variable Display", 9F);
            btn_cancel.ForeColor = Color.Black;
            btn_cancel.Location = new Point(483, 117);
            btn_cancel.Margin = new Padding(4, 3, 4, 3);
            btn_cancel.Name = "btn_cancel";
            btn_cancel.Size = new Size(118, 34);
            btn_cancel.TabIndex = 16;
            btn_cancel.Text = "Cancel";
            btn_cancel.UseVisualStyleBackColor = true;
            // 
            // ProgressDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(32, 32, 32);
            ClientSize = new Size(617, 164);
            Controls.Add(text_progress);
            Controls.Add(text_extract_file);
            Controls.Add(label_progress);
            Controls.Add(label2);
            Controls.Add(label_extracting);
            Controls.Add(label_extract);
            Controls.Add(progress_main);
            Controls.Add(statusText);
            Controls.Add(label5);
            Controls.Add(btn_cancel);
            ForeColor = Color.FromArgb(235, 235, 235);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "ProgressDialog";
            Text = "ProgressDialog";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label text_progress;
        private Label text_extract_file;
        private Label label_progress;
        private Label label2;
        private Label label_extracting;
        private Label label_extract;
        private Controls.DarkProgressBar progress_main;
        private Label statusText;
        private Label label5;
        private Button btn_cancel;
    }
}