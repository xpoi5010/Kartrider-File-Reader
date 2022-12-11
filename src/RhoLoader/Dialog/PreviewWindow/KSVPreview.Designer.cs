namespace RhoLoader.PreviewWindow
{
    partial class KSVPreview
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
            this.contestName = new System.Windows.Forms.Label();
            this.infoBox = new System.Windows.Forms.ListView();
            this.key = new System.Windows.Forms.ColumnHeader();
            this.value = new System.Windows.Forms.ColumnHeader();
            this.players = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contestName
            // 
            this.contestName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.contestName.Dock = System.Windows.Forms.DockStyle.Top;
            this.contestName.Font = new System.Drawing.Font("Meiryo UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.contestName.ForeColor = System.Drawing.Color.White;
            this.contestName.Location = new System.Drawing.Point(0, 0);
            this.contestName.Margin = new System.Windows.Forms.Padding(0);
            this.contestName.Name = "contestName";
            this.contestName.Size = new System.Drawing.Size(934, 41);
            this.contestName.TabIndex = 0;
            this.contestName.Text = "這是個測試用文字 这是个测试用文字 이것은 테스트 텍스트입니다";
            this.contestName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // infoBox
            // 
            this.infoBox.BackColor = System.Drawing.SystemColors.HotTrack;
            this.infoBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.infoBox.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.key,
            this.value});
            this.infoBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoBox.Font = new System.Drawing.Font("Meiryo", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.infoBox.ForeColor = System.Drawing.Color.White;
            this.infoBox.FullRowSelect = true;
            this.infoBox.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.infoBox.Location = new System.Drawing.Point(0, 41);
            this.infoBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.infoBox.MultiSelect = false;
            this.infoBox.Name = "infoBox";
            this.infoBox.Size = new System.Drawing.Size(934, 478);
            this.infoBox.TabIndex = 1;
            this.infoBox.UseCompatibleStateImageBehavior = false;
            this.infoBox.View = System.Windows.Forms.View.Details;
            // 
            // key
            // 
            this.key.Width = 215;
            // 
            // value
            // 
            this.value.Width = 215;
            // 
            // players
            // 
            this.players.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.players.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.players.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.players.Dock = System.Windows.Forms.DockStyle.Fill;
            this.players.Font = new System.Drawing.Font("Meiryo", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.players.FullRowSelect = true;
            this.players.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.players.Location = new System.Drawing.Point(0, 30);
            this.players.Margin = new System.Windows.Forms.Padding(0);
            this.players.Name = "players";
            this.players.Size = new System.Drawing.Size(433, 448);
            this.players.TabIndex = 2;
            this.players.UseCompatibleStateImageBehavior = false;
            this.players.View = System.Windows.Forms.View.Details;
            this.players.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.players_ItemSelectionChanged);
            this.players.MouseHover += new System.EventHandler(this.players_MouseHover);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 430;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.IndianRed;
            this.panel1.Controls.Add(this.players);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(501, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(433, 478);
            this.panel1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Bahnschrift", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(433, 30);
            this.label2.TabIndex = 5;
            this.label2.Text = "Players";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KSVPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(934, 519);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.infoBox);
            this.Controls.Add(this.contestName);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "KSVPreview";
            this.Text = "KSVPreview";
            this.Load += new System.EventHandler(this.KSVPreview_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label contestName;
        private System.Windows.Forms.ListView infoBox;
        private System.Windows.Forms.ColumnHeader key;
        private System.Windows.Forms.ColumnHeader value;
        private System.Windows.Forms.ListView players;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private Panel panel1;
        private Label label2;
    }
}