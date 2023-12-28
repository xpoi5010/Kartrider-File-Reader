using RhoLoader.Controls;

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
            components = new System.ComponentModel.Container();
            menu = new MenuStrip();
            menu_file = new ToolStripMenuItem();
            menu_file_open = new ToolStripMenuItem();
            menu_file_openFiles = new ToolStripMenuItem();
            menu_file_openFolder = new ToolStripMenuItem();
            menuToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            menu_file_exit = new ToolStripMenuItem();
            menu_extract = new ToolStripMenuItem();
            menu_extract_all = new ToolStripMenuItem();
            menu_extract_current = new ToolStripMenuItem();
            menu_about = new ToolStripMenuItem();
            menu_lang = new ToolStripMenuItem();
            menumodeToolStripMenuItem = new ToolStripMenuItem();
            listview_main = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            dialog_singleFile = new OpenFileDialog();
            dialog_multiFile = new OpenFileDialog();
            imageList_listview = new ImageList(components);
            contextMenu_list = new ContextMenuStrip(components);
            filemenu_extractfile = new ToolStripMenuItem();
            filemenu_extract_selected = new ToolStripMenuItem();
            filemenu_convertPNG = new ToolStripMenuItem();
            filemenu_convertXML = new ToolStripMenuItem();
            treeview_explorer = new Controls.DarkTreeView();
            split_main = new SplitContainer();
            panel_topbar = new Panel();
            textbox_path = new TextBox();
            icon_back = new PictureBox();
            menu.SuspendLayout();
            contextMenu_list.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)split_main).BeginInit();
            split_main.Panel1.SuspendLayout();
            split_main.Panel2.SuspendLayout();
            split_main.SuspendLayout();
            panel_topbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)icon_back).BeginInit();
            SuspendLayout();
            // 
            // menu
            // 
            menu.BackColor = Color.White;
            menu.Font = new Font("Segoe UI", 9F);
            menu.Items.AddRange(new ToolStripItem[] { menu_file, menu_extract, menu_about, menu_lang, menumodeToolStripMenuItem });
            menu.Location = new Point(0, 0);
            menu.Name = "menu";
            menu.Padding = new Padding(7, 2, 0, 2);
            menu.RenderMode = ToolStripRenderMode.Professional;
            menu.Size = new Size(967, 24);
            menu.TabIndex = 2;
            menu.Tag = "menu";
            menu.Text = "menu";
            // 
            // menu_file
            // 
            menu_file.BackColor = Color.White;
            menu_file.DropDownItems.AddRange(new ToolStripItem[] { menu_file_open, menu_file_openFiles, menu_file_openFolder, menuToolStripMenuItem, toolStripSeparator1, menu_file_exit });
            menu_file.ForeColor = Color.Black;
            menu_file.Name = "menu_file";
            menu_file.Size = new Size(71, 20);
            menu_file.Tag = "menu_file";
            menu_file.Text = "menu_file";
            // 
            // menu_file_open
            // 
            menu_file_open.ForeColor = Color.Black;
            menu_file_open.Name = "menu_file_open";
            menu_file_open.Size = new Size(170, 22);
            menu_file_open.Tag = "menu_open";
            menu_file_open.Text = "menu_open";
            menu_file_open.Click += action_open;
            // 
            // menu_file_openFiles
            // 
            menu_file_openFiles.ForeColor = Color.Black;
            menu_file_openFiles.Name = "menu_file_openFiles";
            menu_file_openFiles.Size = new Size(170, 22);
            menu_file_openFiles.Tag = "menu_openFiles";
            menu_file_openFiles.Text = "menu_openFiles";
            menu_file_openFiles.Click += action_open_files;
            // 
            // menu_file_openFolder
            // 
            menu_file_openFolder.ForeColor = Color.Black;
            menu_file_openFolder.Name = "menu_file_openFolder";
            menu_file_openFolder.Size = new Size(170, 22);
            menu_file_openFolder.Tag = "menu_openFolder";
            menu_file_openFolder.Text = "menu_openFolder";
            menu_file_openFolder.Click += action_openFolder;
            // 
            // menuToolStripMenuItem
            // 
            menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            menuToolStripMenuItem.Size = new Size(170, 22);
            menuToolStripMenuItem.Text = "menu_save";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.ForeColor = Color.Black;
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(167, 6);
            // 
            // menu_file_exit
            // 
            menu_file_exit.ForeColor = Color.Black;
            menu_file_exit.Name = "menu_file_exit";
            menu_file_exit.Size = new Size(170, 22);
            menu_file_exit.Tag = "menu_exit";
            menu_file_exit.Text = "menu_exit";
            menu_file_exit.Click += action_exit;
            // 
            // menu_extract
            // 
            menu_extract.BackColor = Color.White;
            menu_extract.DropDownItems.AddRange(new ToolStripItem[] { menu_extract_all, menu_extract_current });
            menu_extract.ForeColor = Color.Black;
            menu_extract.Name = "menu_extract";
            menu_extract.Size = new Size(91, 20);
            menu_extract.Tag = "menu_extract";
            menu_extract.Text = "menu_extract";
            // 
            // menu_extract_all
            // 
            menu_extract_all.ForeColor = Color.Black;
            menu_extract_all.Name = "menu_extract_all";
            menu_extract_all.Size = new Size(189, 22);
            menu_extract_all.Tag = "menu_extract_all";
            menu_extract_all.Text = "menu_extract_all";
            menu_extract_all.Click += action_extract_all;
            // 
            // menu_extract_current
            // 
            menu_extract_current.ForeColor = Color.Black;
            menu_extract_current.Name = "menu_extract_current";
            menu_extract_current.Size = new Size(189, 22);
            menu_extract_current.Tag = "menu_extract_current";
            menu_extract_current.Text = "menu_extract_current";
            menu_extract_current.Click += action_extract_current;
            // 
            // menu_about
            // 
            menu_about.BackColor = Color.White;
            menu_about.ForeColor = Color.Black;
            menu_about.Name = "menu_about";
            menu_about.Size = new Size(86, 20);
            menu_about.Tag = "menu_about";
            menu_about.Text = "menu_about";
            menu_about.Click += action_aboutWindow;
            // 
            // menu_lang
            // 
            menu_lang.BackColor = Color.White;
            menu_lang.ForeColor = Color.Black;
            menu_lang.Name = "menu_lang";
            menu_lang.Size = new Size(112, 20);
            menu_lang.Tag = "menu_Languages";
            menu_lang.Text = "menu_Languages";
            // 
            // menumodeToolStripMenuItem
            // 
            menumodeToolStripMenuItem.BackColor = Color.DarkGray;
            menumodeToolStripMenuItem.Name = "menumodeToolStripMenuItem";
            menumodeToolStripMenuItem.Size = new Size(86, 20);
            menumodeToolStripMenuItem.Text = "menu_mode";
            // 
            // listview_main
            // 
            listview_main.AllowDrop = true;
            listview_main.BackColor = Color.White;
            listview_main.BorderStyle = BorderStyle.None;
            listview_main.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader3, columnHeader2 });
            listview_main.Dock = DockStyle.Fill;
            listview_main.Font = new Font("Microsoft YaHei", 8.25F);
            listview_main.FullRowSelect = true;
            listview_main.Location = new Point(0, 0);
            listview_main.Margin = new Padding(4, 3, 4, 3);
            listview_main.Name = "listview_main";
            listview_main.Size = new Size(715, 478);
            listview_main.TabIndex = 5;
            listview_main.UseCompatibleStateImageBehavior = false;
            listview_main.View = View.Details;
            listview_main.MouseClick += action_listview_click;
            listview_main.MouseDoubleClick += action_listview_doubleclick;
            // 
            // columnHeader1
            // 
            columnHeader1.Tag = "col_name";
            columnHeader1.Text = "col_name";
            columnHeader1.Width = 104;
            // 
            // columnHeader3
            // 
            columnHeader3.Tag = "col_type";
            columnHeader3.Text = "col_type";
            columnHeader3.Width = 125;
            // 
            // columnHeader2
            // 
            columnHeader2.Tag = "col_size";
            columnHeader2.Text = "col_size";
            columnHeader2.TextAlign = HorizontalAlignment.Right;
            columnHeader2.Width = 75;
            // 
            // dialog_singleFile
            // 
            dialog_singleFile.Filter = "RhoFile|*.Rho|JMDFile|*.jmd";
            // 
            // dialog_multiFile
            // 
            dialog_multiFile.Filter = "RhoFile|*.Rho";
            dialog_multiFile.Multiselect = true;
            // 
            // imageList_listview
            // 
            imageList_listview.ColorDepth = ColorDepth.Depth8Bit;
            imageList_listview.ImageSize = new Size(16, 16);
            imageList_listview.TransparentColor = Color.Transparent;
            // 
            // contextMenu_list
            // 
            contextMenu_list.Items.AddRange(new ToolStripItem[] { filemenu_extractfile, filemenu_extract_selected, filemenu_convertPNG, filemenu_convertXML });
            contextMenu_list.Name = "FileMenu";
            contextMenu_list.Size = new Size(211, 92);
            // 
            // filemenu_extractfile
            // 
            filemenu_extractfile.Name = "filemenu_extractfile";
            filemenu_extractfile.Size = new Size(210, 22);
            filemenu_extractfile.Tag = "filemenu_extractfile";
            filemenu_extractfile.Text = "filemenu_extractfile";
            filemenu_extractfile.Click += action_extractfile;
            // 
            // filemenu_extract_selected
            // 
            filemenu_extract_selected.Name = "filemenu_extract_selected";
            filemenu_extract_selected.Size = new Size(210, 22);
            filemenu_extract_selected.Tag = "filemenu_extract_selected";
            filemenu_extract_selected.Text = "filemenu_extract_selected";
            filemenu_extract_selected.Click += action_extract_selected;
            // 
            // filemenu_convertPNG
            // 
            filemenu_convertPNG.Name = "filemenu_convertPNG";
            filemenu_convertPNG.Size = new Size(210, 22);
            filemenu_convertPNG.Tag = "filemenu_convertPng";
            filemenu_convertPNG.Text = "filemenu_convertPng";
            filemenu_convertPNG.Click += action_convert_png;
            // 
            // filemenu_convertXML
            // 
            filemenu_convertXML.Name = "filemenu_convertXML";
            filemenu_convertXML.Size = new Size(210, 22);
            filemenu_convertXML.Tag = "filemenu_convertXML";
            filemenu_convertXML.Text = "filemenu_convertXML";
            filemenu_convertXML.Click += action_convert_xml;
            // 
            // treeview_explorer
            // 
            treeview_explorer.Dock = DockStyle.Fill;
            treeview_explorer.DrawMode = TreeViewDrawMode.OwnerDrawAll;
            treeview_explorer.FullRowSelect = true;
            treeview_explorer.ItemHeight = 20;
            treeview_explorer.Location = new Point(0, 0);
            treeview_explorer.Name = "treeview_explorer";
            treeview_explorer.ShowPlusMinus = false;
            treeview_explorer.Size = new Size(250, 478);
            treeview_explorer.TabIndex = 0;
            treeview_explorer.Tag = "treeview_explorer";
            treeview_explorer.AfterSelect += action_node_select;
            // 
            // split_main
            // 
            split_main.Dock = DockStyle.Fill;
            split_main.Location = new Point(0, 48);
            split_main.Name = "split_main";
            // 
            // split_main.Panel1
            // 
            split_main.Panel1.Controls.Add(treeview_explorer);
            split_main.Panel1MinSize = 250;
            // 
            // split_main.Panel2
            // 
            split_main.Panel2.Controls.Add(listview_main);
            split_main.Panel2MinSize = 315;
            split_main.Size = new Size(967, 478);
            split_main.SplitterDistance = 250;
            split_main.SplitterWidth = 2;
            split_main.TabIndex = 0;
            // 
            // panel_topbar
            // 
            panel_topbar.Controls.Add(textbox_path);
            panel_topbar.Controls.Add(icon_back);
            panel_topbar.Dock = DockStyle.Top;
            panel_topbar.Location = new Point(0, 24);
            panel_topbar.Name = "panel_topbar";
            panel_topbar.Size = new Size(967, 24);
            panel_topbar.TabIndex = 7;
            // 
            // textbox_path
            // 
            textbox_path.BackColor = Color.White;
            textbox_path.Dock = DockStyle.Fill;
            textbox_path.Font = new Font("Segoe UI Variable Display Semib", 9F, FontStyle.Bold);
            textbox_path.Location = new Point(24, 0);
            textbox_path.Margin = new Padding(0);
            textbox_path.Name = "textbox_path";
            textbox_path.ReadOnly = true;
            textbox_path.Size = new Size(943, 23);
            textbox_path.TabIndex = 0;
            // 
            // icon_back
            // 
            icon_back.Dock = DockStyle.Left;
            icon_back.Enabled = false;
            icon_back.Image = Properties.Resources.ic_fluent_arrow_hook_up_left_24_filled_disabled;
            icon_back.Location = new Point(0, 0);
            icon_back.Name = "icon_back";
            icon_back.Size = new Size(24, 24);
            icon_back.SizeMode = PictureBoxSizeMode.CenterImage;
            icon_back.TabIndex = 1;
            icon_back.TabStop = false;
            icon_back.EnabledChanged += action_icon_enable_changed;
            icon_back.Click += action_back;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(967, 526);
            Controls.Add(split_main);
            Controls.Add(panel_topbar);
            Controls.Add(menu);
            MainMenuStrip = menu;
            Margin = new Padding(4, 3, 4, 3);
            Name = "MainWindow";
            Tag = "title";
            Text = "title";
            menu.ResumeLayout(false);
            menu.PerformLayout();
            contextMenu_list.ResumeLayout(false);
            split_main.Panel1.ResumeLayout(false);
            split_main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)split_main).EndInit();
            split_main.ResumeLayout(false);
            panel_topbar.ResumeLayout(false);
            panel_topbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)icon_back).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem menu_file;
        private System.Windows.Forms.ToolStripMenuItem menu_file_open;
        private System.Windows.Forms.ToolStripMenuItem menu_file_openFiles;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menu_file_exit;
        private System.Windows.Forms.ListView listview_main;
        private System.Windows.Forms.OpenFileDialog dialog_singleFile;
        private System.Windows.Forms.OpenFileDialog dialog_multiFile;
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
        private DarkTreeView treeview_explorer;
        private ToolStripMenuItem menuToolStripMenuItem;
        private ToolStripMenuItem menumodeToolStripMenuItem;
    }
}

