﻿namespace NSGameDownloader
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.textBox_keyword = new System.Windows.Forms.TextBox();
            this.button_search = new System.Windows.Forms.Button();
            this.radioButton_nsp = new System.Windows.Forms.RadioButton();
            this.radioButton_xci = new System.Windows.Forms.RadioButton();
            this.pictureBox_gameicon = new System.Windows.Forms.PictureBox();
            this.label_info = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_update_game = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.发送反馈ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar_download = new System.Windows.Forms.ToolStripProgressBar();
            this.label_progress = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.label_count = new System.Windows.Forms.Label();
            this.radioButton_upd = new System.Windows.Forms.RadioButton();
            this.label_url = new System.Windows.Forms.Label();
            this.panWebBrowser = new System.Windows.Forms.WebBrowser();
            this.label_info_size = new System.Windows.Forms.Label();
            this.label_info_launch_date = new System.Windows.Forms.Label();
            this.label_info_support_lan = new System.Windows.Forms.Label();
            this.localDirLabel = new System.Windows.Forms.LinkLabel();
            this.label_info_type = new System.Windows.Forms.Label();
            this.checkbox_cn = new System.Windows.Forms.CheckBox();
            this.info_label_name = new System.Windows.Forms.Label();
            this.info_label_publisher = new System.Windows.Forms.Label();
            this.check_box_download = new System.Windows.Forms.CheckBox();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView1 = new System.Windows.Forms.ListView();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_gameicon)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_keyword
            // 
            this.textBox_keyword.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_keyword.Location = new System.Drawing.Point(13, 35);
            this.textBox_keyword.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_keyword.Name = "textBox_keyword";
            this.textBox_keyword.Size = new System.Drawing.Size(327, 27);
            this.textBox_keyword.TabIndex = 99;
            this.textBox_keyword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_keyword_KeyPress);
            // 
            // button_search
            // 
            this.button_search.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_search.Location = new System.Drawing.Point(497, 34);
            this.button_search.Margin = new System.Windows.Forms.Padding(4);
            this.button_search.Name = "button_search";
            this.button_search.Size = new System.Drawing.Size(84, 28);
            this.button_search.TabIndex = 1;
            this.button_search.Text = "搜索";
            this.button_search.UseVisualStyleBackColor = true;
            this.button_search.Click += new System.EventHandler(this.button_search_Click);
            // 
            // radioButton_nsp
            // 
            this.radioButton_nsp.AutoSize = true;
            this.radioButton_nsp.Checked = true;
            this.radioButton_nsp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton_nsp.Location = new System.Drawing.Point(379, 389);
            this.radioButton_nsp.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton_nsp.Name = "radioButton_nsp";
            this.radioButton_nsp.Size = new System.Drawing.Size(90, 24);
            this.radioButton_nsp.TabIndex = 3;
            this.radioButton_nsp.Text = "查看NSP";
            this.radioButton_nsp.UseVisualStyleBackColor = true;
            this.radioButton_nsp.Click += new System.EventHandler(this.radioButton_Click);
            // 
            // radioButton_xci
            // 
            this.radioButton_xci.AutoSize = true;
            this.radioButton_xci.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton_xci.Location = new System.Drawing.Point(477, 389);
            this.radioButton_xci.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton_xci.Name = "radioButton_xci";
            this.radioButton_xci.Size = new System.Drawing.Size(84, 24);
            this.radioButton_xci.TabIndex = 4;
            this.radioButton_xci.Text = "查看XCI";
            this.radioButton_xci.UseVisualStyleBackColor = true;
            this.radioButton_xci.Click += new System.EventHandler(this.radioButton_Click);
            // 
            // pictureBox_gameicon
            // 
            this.pictureBox_gameicon.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pictureBox_gameicon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox_gameicon.ImageLocation = "";
            this.pictureBox_gameicon.Location = new System.Drawing.Point(13, 421);
            this.pictureBox_gameicon.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox_gameicon.Name = "pictureBox_gameicon";
            this.pictureBox_gameicon.Size = new System.Drawing.Size(150, 150);
            this.pictureBox_gameicon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_gameicon.TabIndex = 7;
            this.pictureBox_gameicon.TabStop = false;
            this.pictureBox_gameicon.Click += new System.EventHandler(this.pictureBox_gameicon_Click);
            // 
            // label_info
            // 
            this.label_info.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_info.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_info.Location = new System.Drawing.Point(16, 575);
            this.label_info.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_info.Name = "label_info";
            this.label_info.Size = new System.Drawing.Size(684, 97);
            this.label_info.TabIndex = 8;
            this.label_info.Text = "简介";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1420, 28);
            this.menuStrip1.TabIndex = 100;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_update_game});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // menu_update_game
            // 
            this.menu_update_game.Name = "menu_update_game";
            this.menu_update_game.Size = new System.Drawing.Size(216, 26);
            this.menu_update_game.Text = "更新游戏库";
            this.menu_update_game.Click += new System.EventHandler(this.menu_update_game_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查看帮助ToolStripMenuItem,
            this.toolStripSeparator1,
            this.发送反馈ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.关于ToolStripMenuItem});
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.帮助ToolStripMenuItem.Text = "帮助";
            // 
            // 查看帮助ToolStripMenuItem
            // 
            this.查看帮助ToolStripMenuItem.Name = "查看帮助ToolStripMenuItem";
            this.查看帮助ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.查看帮助ToolStripMenuItem.Text = "查看帮助";
            this.查看帮助ToolStripMenuItem.Click += new System.EventHandler(this.查看帮助ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(141, 6);
            // 
            // 发送反馈ToolStripMenuItem
            // 
            this.发送反馈ToolStripMenuItem.Name = "发送反馈ToolStripMenuItem";
            this.发送反馈ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.发送反馈ToolStripMenuItem.Text = "发送反馈";
            this.发送反馈ToolStripMenuItem.Click += new System.EventHandler(this.发送反馈ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(141, 6);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.关于ToolStripMenuItem.Text = "关于";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar_download,
            this.label_progress,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 676);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1420, 25);
            this.statusStrip1.TabIndex = 101;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar_download
            // 
            this.toolStripProgressBar_download.Maximum = 5000;
            this.toolStripProgressBar_download.Name = "toolStripProgressBar_download";
            this.toolStripProgressBar_download.Size = new System.Drawing.Size(133, 20);
            this.toolStripProgressBar_download.Visible = false;
            // 
            // label_progress
            // 
            this.label_progress.ForeColor = System.Drawing.Color.DimGray;
            this.label_progress.LinkColor = System.Drawing.Color.DimGray;
            this.label_progress.Name = "label_progress";
            this.label_progress.Size = new System.Drawing.Size(93, 20);
            this.label_progress.Text = "正在下载xxx";
            this.label_progress.Visible = false;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AccessibleRole = System.Windows.Forms.AccessibleRole.Cursor;
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.DimGray;
            this.toolStripStatusLabel1.IsLink = true;
            this.toolStripStatusLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.toolStripStatusLabel1.LinkColor = System.Drawing.Color.DimGray;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(180, 20);
            this.toolStripStatusLabel1.Text = "特别感谢 @ 91wii.riggzh";
            this.toolStripStatusLabel1.Click += new System.EventHandler(this.ToolStripStatusLabel1_Click);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.AccessibleRole = System.Windows.Forms.AccessibleRole.Cursor;
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.DimGray;
            this.toolStripStatusLabel2.IsLink = true;
            this.toolStripStatusLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.toolStripStatusLabel2.LinkColor = System.Drawing.Color.DimGray;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(176, 20);
            this.toolStripStatusLabel2.Text = " @ github ningxiaoxiao";
            this.toolStripStatusLabel2.Click += new System.EventHandler(this.toolStripStatusLabel2_Click);
            // 
            // label_count
            // 
            this.label_count.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_count.Location = new System.Drawing.Point(592, 35);
            this.label_count.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_count.Name = "label_count";
            this.label_count.Size = new System.Drawing.Size(111, 26);
            this.label_count.TabIndex = 103;
            this.label_count.Text = "count: 计算中";
            this.label_count.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radioButton_upd
            // 
            this.radioButton_upd.AutoSize = true;
            this.radioButton_upd.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton_upd.Location = new System.Drawing.Point(569, 389);
            this.radioButton_upd.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton_upd.Name = "radioButton_upd";
            this.radioButton_upd.Size = new System.Drawing.Size(131, 24);
            this.radioButton_upd.TabIndex = 104;
            this.radioButton_upd.Text = "查看UPD+DLC";
            this.radioButton_upd.UseVisualStyleBackColor = true;
            this.radioButton_upd.Click += new System.EventHandler(this.radioButton_Click);
            // 
            // label_url
            // 
            this.label_url.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_url.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_url.Location = new System.Drawing.Point(704, 36);
            this.label_url.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_url.Name = "label_url";
            this.label_url.Size = new System.Drawing.Size(700, 26);
            this.label_url.TabIndex = 105;
            this.label_url.Text = "选择游戏查看下载地址";
            this.label_url.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label_url.UseMnemonic = false;
            this.label_url.Click += new System.EventHandler(this.label_url_Click);
            // 
            // panWebBrowser
            // 
            this.panWebBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panWebBrowser.Location = new System.Drawing.Point(708, 69);
            this.panWebBrowser.Margin = new System.Windows.Forms.Padding(4);
            this.panWebBrowser.MinimumSize = new System.Drawing.Size(27, 25);
            this.panWebBrowser.Name = "panWebBrowser";
            this.panWebBrowser.Size = new System.Drawing.Size(696, 603);
            this.panWebBrowser.TabIndex = 2;
            this.panWebBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
            this.panWebBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.panWebBrowser_Navigated);
            this.panWebBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.panWebBrowser_Navigating);
            // 
            // label_info_size
            // 
            this.label_info_size.AutoSize = true;
            this.label_info_size.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_info_size.Location = new System.Drawing.Point(394, 441);
            this.label_info_size.Name = "label_info_size";
            this.label_info_size.Size = new System.Drawing.Size(54, 20);
            this.label_info_size.TabIndex = 106;
            this.label_info_size.Text = "大小：";
            // 
            // label_info_launch_date
            // 
            this.label_info_launch_date.AutoSize = true;
            this.label_info_launch_date.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_info_launch_date.Location = new System.Drawing.Point(170, 444);
            this.label_info_launch_date.Name = "label_info_launch_date";
            this.label_info_launch_date.Size = new System.Drawing.Size(84, 20);
            this.label_info_launch_date.TabIndex = 107;
            this.label_info_launch_date.Text = "发布日期：";
            // 
            // label_info_support_lan
            // 
            this.label_info_support_lan.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_info_support_lan.Location = new System.Drawing.Point(170, 513);
            this.label_info_support_lan.Name = "label_info_support_lan";
            this.label_info_support_lan.Size = new System.Drawing.Size(530, 41);
            this.label_info_support_lan.TabIndex = 108;
            this.label_info_support_lan.Text = "支持语言：";
            // 
            // localDirLabel
            // 
            this.localDirLabel.ActiveLinkColor = System.Drawing.Color.Black;
            this.localDirLabel.AutoSize = true;
            this.localDirLabel.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.localDirLabel.LinkColor = System.Drawing.Color.Black;
            this.localDirLabel.Location = new System.Drawing.Point(12, 389);
            this.localDirLabel.Name = "localDirLabel";
            this.localDirLabel.Size = new System.Drawing.Size(69, 20);
            this.localDirLabel.TabIndex = 110;
            this.localDirLabel.TabStop = true;
            this.localDirLabel.Text = "本地目录";
            this.localDirLabel.Visible = false;
            this.localDirLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.localDirLabel_LinkClicked);
            // 
            // label_info_type
            // 
            this.label_info_type.AutoSize = true;
            this.label_info_type.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_info_type.Location = new System.Drawing.Point(170, 467);
            this.label_info_type.Name = "label_info_type";
            this.label_info_type.Size = new System.Drawing.Size(54, 20);
            this.label_info_type.TabIndex = 111;
            this.label_info_type.Text = "类型：";
            // 
            // checkbox_cn
            // 
            this.checkbox_cn.AutoSize = true;
            this.checkbox_cn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkbox_cn.Location = new System.Drawing.Point(347, 38);
            this.checkbox_cn.Name = "checkbox_cn";
            this.checkbox_cn.Size = new System.Drawing.Size(61, 24);
            this.checkbox_cn.TabIndex = 112;
            this.checkbox_cn.Text = "中文";
            this.checkbox_cn.UseVisualStyleBackColor = true;
            this.checkbox_cn.CheckedChanged += new System.EventHandler(this.checkbox_cn_CheckedChanged);
            // 
            // info_label_name
            // 
            this.info_label_name.AutoSize = true;
            this.info_label_name.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.info_label_name.Location = new System.Drawing.Point(170, 421);
            this.info_label_name.Name = "info_label_name";
            this.info_label_name.Size = new System.Drawing.Size(69, 20);
            this.info_label_name.TabIndex = 113;
            this.info_label_name.Text = "游戏名：";
            // 
            // info_label_publisher
            // 
            this.info_label_publisher.AutoSize = true;
            this.info_label_publisher.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.info_label_publisher.Location = new System.Drawing.Point(170, 490);
            this.info_label_publisher.Name = "info_label_publisher";
            this.info_label_publisher.Size = new System.Drawing.Size(69, 20);
            this.info_label_publisher.TabIndex = 114;
            this.info_label_publisher.Text = "发行商：";
            // 
            // check_box_download
            // 
            this.check_box_download.AutoSize = true;
            this.check_box_download.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.check_box_download.Location = new System.Drawing.Point(414, 38);
            this.check_box_download.Name = "check_box_download";
            this.check_box_download.Size = new System.Drawing.Size(76, 24);
            this.check_box_download.TabIndex = 115;
            this.check_box_download.Text = "已下载";
            this.check_box_download.UseVisualStyleBackColor = true;
            this.check_box_download.Visible = false;
            this.check_box_download.CheckedChanged += new System.EventHandler(this.check_box_download_CheckedChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "TID";
            this.columnHeader1.Width = 160;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "NAME";
            this.columnHeader2.Width = 260;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "NSP";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 44;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "XCI";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 44;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "UPD/DLC";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader6.Width = 44;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "中文";
            this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader7.Width = 44;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader6,
            this.columnHeader7});
            this.listView1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.LabelEdit = true;
            this.listView1.Location = new System.Drawing.Point(13, 69);
            this.listView1.Margin = new System.Windows.Forms.Padding(4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(687, 312);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1420, 701);
            this.Controls.Add(this.check_box_download);
            this.Controls.Add(this.info_label_publisher);
            this.Controls.Add(this.info_label_name);
            this.Controls.Add(this.checkbox_cn);
            this.Controls.Add(this.label_info_type);
            this.Controls.Add(this.localDirLabel);
            this.Controls.Add(this.label_info_support_lan);
            this.Controls.Add(this.label_info_launch_date);
            this.Controls.Add(this.label_info_size);
            this.Controls.Add(this.label_url);
            this.Controls.Add(this.radioButton_upd);
            this.Controls.Add(this.label_count);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.label_info);
            this.Controls.Add(this.pictureBox_gameicon);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.radioButton_xci);
            this.Controls.Add(this.radioButton_nsp);
            this.Controls.Add(this.panWebBrowser);
            this.Controls.Add(this.button_search);
            this.Controls.Add(this.textBox_keyword);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "NSGameDownloader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_gameicon)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_keyword;
        private System.Windows.Forms.Button button_search;
        private System.Windows.Forms.WebBrowser panWebBrowser;
        private System.Windows.Forms.RadioButton radioButton_nsp;
        private System.Windows.Forms.RadioButton radioButton_xci;
        private System.Windows.Forms.PictureBox pictureBox_gameicon;
        private System.Windows.Forms.Label label_info;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menu_update_game;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar_download;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 发送反馈ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.Label label_count;
        private System.Windows.Forms.RadioButton radioButton_upd;
        private System.Windows.Forms.Label label_url;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label label_info_size;
        private System.Windows.Forms.Label label_info_launch_date;
        private System.Windows.Forms.Label label_info_support_lan;
        private System.Windows.Forms.LinkLabel localDirLabel;
        private System.Windows.Forms.Label label_info_type;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.CheckBox checkbox_cn;
        private System.Windows.Forms.Label info_label_name;
        private System.Windows.Forms.Label info_label_publisher;
        private System.Windows.Forms.CheckBox check_box_download;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ToolStripStatusLabel label_progress;
    }
}

