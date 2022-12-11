namespace RhoLoader
{
    partial class MainWindow
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
        /// Because Designer have some bugs which cause the custom controls will be disappeared, 
        /// it is recommend to disable ".NET Core Win Forms Designer".
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menu = new System.Windows.Forms.MenuStrip();
            this.menu_file = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_file_open = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_file_openFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menu_file_exit = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_extract = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_extract_all = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_extract_current = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_about = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_lang = new System.Windows.Forms.ToolStripMenuItem();
            this.listview_main = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.dialog_singleFile = new System.Windows.Forms.OpenFileDialog();
            this.imageList_listview = new System.Windows.Forms.ImageList(this.components);
            this.contextMenu_list = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.filemenu_extractfile = new System.Windows.Forms.ToolStripMenuItem();
            this.filemenu_convertPNG = new System.Windows.Forms.ToolStripMenuItem();
            this.filemenu_convertXML = new System.Windows.Forms.ToolStripMenuItem();
            this.filemenu_extract_selected = new System.Windows.Forms.ToolStripMenuItem();
            this.treeview_explorer = new Controls.DarkTreeView();
            this.split_main = new System.Windows.Forms.SplitContainer();
            this.panel_topbar = new System.Windows.Forms.Panel();
            this.textbox_path = new System.Windows.Forms.TextBox();
            this.icon_back = new System.Windows.Forms.PictureBox();
            this.menu.SuspendLayout();
            this.contextMenu_list.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split_main)).BeginInit();
            this.split_main.Panel2.SuspendLayout();
            this.split_main.SuspendLayout();
            this.panel_topbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.icon_back)).BeginInit();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.BackColor = System.Drawing.Color.White;
            this.menu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_file,
            this.menu_extract,
            this.menu_about,
            this.menu_lang});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menu.Size = new System.Drawing.Size(967, 24);
            this.menu.TabIndex = 2;
            this.menu.Tag = "menu";
            this.menu.Text = "menu";
            // 
            // menu_file
            // 
            this.menu_file.BackColor = System.Drawing.Color.White;
            this.menu_file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_file_open,
            this.menu_file_openFolder,
            this.toolStripSeparator1,
            this.menu_file_exit});
            this.menu_file.ForeColor = System.Drawing.Color.Black;
            this.menu_file.Name = "menu_file";
            this.menu_file.Size = new System.Drawing.Size(71, 20);
            this.menu_file.Tag = "menu_file";
            this.menu_file.Text = "menu_file";
            // 
            // menu_file_open
            // 
            this.menu_file_open.ForeColor = System.Drawing.Color.Black;
            this.menu_file_open.Name = "menu_file_open";
            this.menu_file_open.Size = new System.Drawing.Size(170, 22);
            this.menu_file_open.Tag = "menu_open";
            this.menu_file_open.Text = "menu_open";
            this.menu_file_open.Click += new System.EventHandler(this.action_open);
            // 
            // menu_file_openFolder
            // 
            this.menu_file_openFolder.ForeColor = System.Drawing.Color.Black;
            this.menu_file_openFolder.Name = "menu_file_openFolder";
            this.menu_file_openFolder.Size = new System.Drawing.Size(170, 22);
            this.menu_file_openFolder.Tag = "menu_openFolder";
            this.menu_file_openFolder.Text = "menu_openFolder";
            this.menu_file_openFolder.Click += new System.EventHandler(this.action_openFolder);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.ForeColor = System.Drawing.Color.Black;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(167, 6);
            // 
            // menu_file_exit
            // 
            this.menu_file_exit.ForeColor = System.Drawing.Color.Black;
            this.menu_file_exit.Name = "menu_file_exit";
            this.menu_file_exit.Size = new System.Drawing.Size(170, 22);
            this.menu_file_exit.Tag = "menu_exit";
            this.menu_file_exit.Text = "menu_exit";
            this.menu_file_exit.Click += new System.EventHandler(this.action_exit);
            // 
            // menu_extract
            // 
            this.menu_extract.BackColor = System.Drawing.Color.White;
            this.menu_extract.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_extract_all,
            this.menu_extract_current});
            this.menu_extract.ForeColor = System.Drawing.Color.Black;
            this.menu_extract.Name = "menu_extract";
            this.menu_extract.Size = new System.Drawing.Size(89, 20);
            this.menu_extract.Tag = "menu_extract";
            this.menu_extract.Text = "menu_extract";
            // 
            // menu_extract_all
            // 
            this.menu_extract_all.ForeColor = System.Drawing.Color.Black;
            this.menu_extract_all.Name = "menu_extract_all";
            this.menu_extract_all.Size = new System.Drawing.Size(167, 22);
            this.menu_extract_all.Tag = "menu_extract_all";
            this.menu_extract_all.Text = "menu_extract_all";
            this.menu_extract_all.Click += new System.EventHandler(this.action_extract_all);
            // 
            // menu_extract_current
            // 
            this.menu_extract_current.ForeColor = System.Drawing.Color.Black;
            this.menu_extract_current.Name = "menu_extract_current";
            this.menu_extract_current.Size = new System.Drawing.Size(167, 22);
            this.menu_extract_current.Tag = "menu_extract_current";
            this.menu_extract_current.Text = "menu_extract_current";
            this.menu_extract_current.Click += new System.EventHandler(this.action_extract_current);
            // 
            // menu_about
            // 
            this.menu_about.BackColor = System.Drawing.Color.White;
            this.menu_about.ForeColor = System.Drawing.Color.Black;
            this.menu_about.Name = "menu_about";
            this.menu_about.Size = new System.Drawing.Size(86, 20);
            this.menu_about.Tag = "menu_about";
            this.menu_about.Text = "menu_about";
            this.menu_about.Click += new System.EventHandler(this.action_aboutWindow);
            // 
            // menu_lang
            // 
            this.menu_lang.BackColor = System.Drawing.Color.White;
            this.menu_lang.ForeColor = System.Drawing.Color.Black;
            this.menu_lang.Name = "menu_lang";
            this.menu_lang.Size = new System.Drawing.Size(112, 20);
            this.menu_lang.Tag = "menu_Languages";
            this.menu_lang.Text = "menu_Languages";
            // 
            // listview_main
            // 
            this.listview_main.AllowDrop = true;
            this.listview_main.BackColor = System.Drawing.Color.White;
            this.listview_main.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listview_main.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3,
            this.columnHeader2});
            this.listview_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listview_main.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.listview_main.FullRowSelect = true;
            this.listview_main.Location = new System.Drawing.Point(0, 0);
            this.listview_main.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.listview_main.MultiSelect = true;
            this.listview_main.Name = "listview_main";
            this.listview_main.Size = new System.Drawing.Size(715, 478);
            this.listview_main.TabIndex = 5;
            this.listview_main.UseCompatibleStateImageBehavior = false;
            this.listview_main.View = System.Windows.Forms.View.Details;
            this.listview_main.MouseClick += new System.Windows.Forms.MouseEventHandler(this.action_listview_click);
            this.listview_main.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.action_listview_doubleclick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Tag = "col_name";
            this.columnHeader1.Text = "col_name";
            this.columnHeader1.Width = 104;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Tag = "col_type";
            this.columnHeader3.Text = "col_type";
            this.columnHeader3.Width = 125;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Tag = "col_size";
            this.columnHeader2.Text = "col_size";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader2.Width = 75;
            // 
            // dialog_singleFile
            // 
            this.dialog_singleFile.Filter = "RhoFile|*.Rho";
            // 
            // imageList_listview
            // 
            this.imageList_listview.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList_listview.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList_listview.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // contextMenu_list
            // 
            this.contextMenu_list.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filemenu_extractfile,
            this.filemenu_extract_selected,
            this.filemenu_convertPNG,
            this.filemenu_convertXML,});
            this.contextMenu_list.Name = "FileMenu";
            this.contextMenu_list.Size = new System.Drawing.Size(191, 70);
            // 
            // filemenu_extractfile
            // 
            this.filemenu_extractfile.Name = "filemenu_extractfile";
            this.filemenu_extractfile.Size = new System.Drawing.Size(190, 22);
            this.filemenu_extractfile.Tag = "filemenu_extractfile";
            this.filemenu_extractfile.Text = "filemenu_extractfile";
            this.filemenu_extractfile.Click += new System.EventHandler(this.action_extractfile);
            // 
            // filemenu_extract_selected
            // 
            this.filemenu_extract_selected.Name = "filemenu_extract_selected";
            this.filemenu_extract_selected.Size = new System.Drawing.Size(190, 22);
            this.filemenu_extract_selected.Tag = "filemenu_extract_selected";
            this.filemenu_extract_selected.Text = "filemenu_extract_selected";
            this.filemenu_extract_selected.Click += new System.EventHandler(this.action_extract_selected);
            // 
            // filemenu_convertPNG
            // 
            this.filemenu_convertPNG.Name = "fileMenu_convertPNG";
            this.filemenu_convertPNG.Size = new System.Drawing.Size(190, 22);
            this.filemenu_convertPNG.Tag = "filemenu_convertPng";
            this.filemenu_convertPNG.Text = "filemenu_convertPng";
            this.filemenu_convertPNG.Click += new System.EventHandler(this.action_convert_png);
            // 
            // filemenu_convertXML
            // 
            this.filemenu_convertXML.Name = "fileMenu_convertXML";
            this.filemenu_convertXML.Size = new System.Drawing.Size(190, 22);
            this.filemenu_convertXML.Tag = "filemenu_convertXML";
            this.filemenu_convertXML.Text = "filemenu_convertXML";
            this.filemenu_convertXML.Click += new System.EventHandler(this.action_convert_xml);
            //
            // treeview_explorer
            //
            this.treeview_explorer.Name = "treeview_explorer";
            this.treeview_explorer.Dock = DockStyle.Fill;
            this.treeview_explorer.AfterSelect += new TreeViewEventHandler(action_node_select);
            this.treeview_explorer.Tag = "treeview_explorer";
            // 
            // split_main
            // 
            this.split_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split_main.Location = new System.Drawing.Point(0, 48);
            this.split_main.Name = "split_main";
            //
            // split_main.Panel1
            //
            this.split_main.Panel1.Controls.Add(this.treeview_explorer);
            this.split_main.Panel1MinSize =250;
            this.split_main.SplitterDistance = 250;
            this.split_main.SplitterWidth = 2;
            this.split_main.TabIndex = 0;
            // 
            // split_main.Panel2
            // 
            this.split_main.Panel2.Controls.Add(this.listview_main);
            this.split_main.Panel2MinSize = 315;
            this.split_main.Size = new System.Drawing.Size(967, 478);
            this.split_main.SplitterDistance = 250;
            this.split_main.SplitterWidth = 2;
            this.split_main.TabIndex = 0;
            // 
            // panel_topbar
            // 
            this.panel_topbar.Controls.Add(this.textbox_path);
            this.panel_topbar.Controls.Add(this.icon_back);
            this.panel_topbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_topbar.Location = new System.Drawing.Point(0, 24);
            this.panel_topbar.Name = "panel_topbar";
            this.panel_topbar.Size = new System.Drawing.Size(967, 24);
            this.panel_topbar.TabIndex = 7;
            // 
            // textbox_path
            // 
            this.textbox_path.BackColor = System.Drawing.Color.White;
            this.textbox_path.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textbox_path.Font = new System.Drawing.Font("Segoe UI Variable Display Semib", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.textbox_path.Location = new System.Drawing.Point(24, 0);
            this.textbox_path.Margin = new System.Windows.Forms.Padding(0);
            this.textbox_path.Name = "textbox_path";
            this.textbox_path.ReadOnly = true;
            this.textbox_path.Size = new System.Drawing.Size(943, 23);
            this.textbox_path.TabIndex = 0;
            // 
            // icon_back
            // 
            this.icon_back.Dock = System.Windows.Forms.DockStyle.Left;
            this.icon_back.Enabled = false;
            this.icon_back.Image = global::RhoLoader.Properties.Resources.ic_fluent_arrow_hook_up_left_24_filled_disabled;
            this.icon_back.Location = new System.Drawing.Point(0, 0);
            this.icon_back.Name = "icon_back";
            this.icon_back.Size = new System.Drawing.Size(24, 24);
            this.icon_back.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.icon_back.TabIndex = 1;
            this.icon_back.TabStop = false;
            this.icon_back.EnabledChanged += new System.EventHandler(this.action_icon_enable_changed);
            this.icon_back.Click += new System.EventHandler(this.action_back);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(967, 526);
            this.Controls.Add(this.split_main);
            this.Controls.Add(this.panel_topbar);
            this.Controls.Add(this.menu);
            this.MainMenuStrip = this.menu;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MainWindow";
            this.Tag = "title";
            this.Text = "title";
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.contextMenu_list.ResumeLayout(false);
            this.split_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split_main)).EndInit();
            this.split_main.ResumeLayout(false);
            this.panel_topbar.ResumeLayout(false);
            this.panel_topbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.icon_back)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem menu_file;
        private System.Windows.Forms.ToolStripMenuItem menu_file_open;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menu_file_exit;
        private System.Windows.Forms.ListView listview_main;
        private System.Windows.Forms.OpenFileDialog dialog_singleFile;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ToolStripMenuItem menu_about;
        private System.Windows.Forms.ImageList imageList_listview;
        private System.Windows.Forms.ToolStripMenuItem menu_extract;
        private System.Windows.Forms.ToolStripMenuItem menu_extract_all;
        private System.Windows.Forms.ToolStripMenuItem menu_lang;
        private System.Windows.Forms.ToolStripMenuItem menu_extract_current;
        private System.Windows.Forms.ContextMenuStrip contextMenu_list;
        private System.Windows.Forms.ToolStripMenuItem filemenu_extractfile;
        private System.Windows.Forms.ToolStripMenuItem filemenu_convertPNG;
        private System.Windows.Forms.ToolStripMenuItem filemenu_convertXML;
        private System.Windows.Forms.ToolStripMenuItem filemenu_extract_selected;
        private ToolStripMenuItem menu_file_openFolder;
        private SplitContainer split_main;
        private Panel panel_topbar;
        private TextBox textbox_path;
        private PictureBox icon_back;
        private Controls.DarkTreeView treeview_explorer;
    }
}

