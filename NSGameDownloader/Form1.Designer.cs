namespace NSGameDownloader
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
            this.panWebBrowser = new System.Windows.Forms.WebBrowser();
            this.radioButton_nsp = new System.Windows.Forms.RadioButton();
            this.radioButton_xci = new System.Windows.Forms.RadioButton();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pictureBox_gameicon = new System.Windows.Forms.PictureBox();
            this.label_info = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.更新TitleId文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.发送反馈ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar_download = new System.Windows.Forms.ToolStripProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label_count = new System.Windows.Forms.Label();
            this.button_lookUdpDlc = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_gameicon)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_keyword
            // 
            this.textBox_keyword.Location = new System.Drawing.Point(10, 28);
            this.textBox_keyword.Name = "textBox_keyword";
            this.textBox_keyword.Size = new System.Drawing.Size(342, 21);
            this.textBox_keyword.TabIndex = 99;
            this.textBox_keyword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_keyword_KeyPress);
            // 
            // button_search
            // 
            this.button_search.Location = new System.Drawing.Point(358, 28);
            this.button_search.Name = "button_search";
            this.button_search.Size = new System.Drawing.Size(63, 21);
            this.button_search.TabIndex = 1;
            this.button_search.Text = "搜索";
            this.button_search.UseVisualStyleBackColor = true;
            this.button_search.Click += new System.EventHandler(this.button_search_Click);
            // 
            // panWebBrowser
            // 
            this.panWebBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panWebBrowser.Location = new System.Drawing.Point(441, 28);
            this.panWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.panWebBrowser.Name = "panWebBrowser";
            this.panWebBrowser.Size = new System.Drawing.Size(589, 496);
            this.panWebBrowser.TabIndex = 2;
            this.panWebBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
            // 
            // radioButton_nsp
            // 
            this.radioButton_nsp.AutoSize = true;
            this.radioButton_nsp.Checked = true;
            this.radioButton_nsp.Location = new System.Drawing.Point(12, 55);
            this.radioButton_nsp.Name = "radioButton_nsp";
            this.radioButton_nsp.Size = new System.Drawing.Size(41, 16);
            this.radioButton_nsp.TabIndex = 3;
            this.radioButton_nsp.TabStop = true;
            this.radioButton_nsp.Text = "NSP";
            this.radioButton_nsp.UseVisualStyleBackColor = true;
            this.radioButton_nsp.CheckedChanged += new System.EventHandler(this.radioButton_nsp_CheckedChanged);
            // 
            // radioButton_xci
            // 
            this.radioButton_xci.AutoSize = true;
            this.radioButton_xci.Location = new System.Drawing.Point(59, 55);
            this.radioButton_xci.Name = "radioButton_xci";
            this.radioButton_xci.Size = new System.Drawing.Size(41, 16);
            this.radioButton_xci.TabIndex = 4;
            this.radioButton_xci.TabStop = true;
            this.radioButton_xci.Text = "XCI";
            this.radioButton_xci.UseVisualStyleBackColor = true;
            this.radioButton_xci.CheckedChanged += new System.EventHandler(this.radioButton_xci_CheckedChanged);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(12, 77);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(411, 228);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "TID";
            this.columnHeader1.Width = 59;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "NAME";
            this.columnHeader2.Width = 258;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "DLC";
            this.columnHeader3.Width = 33;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "UPD";
            this.columnHeader4.Width = 34;
            // 
            // pictureBox_gameicon
            // 
            this.pictureBox_gameicon.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pictureBox_gameicon.ImageLocation = "";
            this.pictureBox_gameicon.Location = new System.Drawing.Point(12, 311);
            this.pictureBox_gameicon.Name = "pictureBox_gameicon";
            this.pictureBox_gameicon.Size = new System.Drawing.Size(100, 100);
            this.pictureBox_gameicon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_gameicon.TabIndex = 7;
            this.pictureBox_gameicon.TabStop = false;
            // 
            // label_info
            // 
            this.label_info.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_info.Location = new System.Drawing.Point(118, 337);
            this.label_info.Name = "label_info";
            this.label_info.Size = new System.Drawing.Size(305, 149);
            this.label_info.TabIndex = 8;
            this.label_info.Text = "0GB";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1042, 25);
            this.menuStrip1.TabIndex = 100;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.更新TitleId文件ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 更新TitleId文件ToolStripMenuItem
            // 
            this.更新TitleId文件ToolStripMenuItem.Name = "更新TitleId文件ToolStripMenuItem";
            this.更新TitleId文件ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.更新TitleId文件ToolStripMenuItem.Text = "更新TitleId文件";
            this.更新TitleId文件ToolStripMenuItem.Click += new System.EventHandler(this.更新TitleId文件ToolStripMenuItem_Click);
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
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.帮助ToolStripMenuItem.Text = "帮助";
            // 
            // 查看帮助ToolStripMenuItem
            // 
            this.查看帮助ToolStripMenuItem.Name = "查看帮助ToolStripMenuItem";
            this.查看帮助ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.查看帮助ToolStripMenuItem.Text = "查看帮助";
            this.查看帮助ToolStripMenuItem.Click += new System.EventHandler(this.查看帮助ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(121, 6);
            // 
            // 发送反馈ToolStripMenuItem
            // 
            this.发送反馈ToolStripMenuItem.Name = "发送反馈ToolStripMenuItem";
            this.发送反馈ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.发送反馈ToolStripMenuItem.Text = "发送反馈";
            this.发送反馈ToolStripMenuItem.Click += new System.EventHandler(this.发送反馈ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(121, 6);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.关于ToolStripMenuItem.Text = "关于";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar_download});
            this.statusStrip1.Location = new System.Drawing.Point(0, 532);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1042, 22);
            this.statusStrip1.TabIndex = 101;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar_download
            // 
            this.toolStripProgressBar_download.Maximum = 5000;
            this.toolStripProgressBar_download.Name = "toolStripProgressBar_download";
            this.toolStripProgressBar_download.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar_download.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(12, 512);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 12);
            this.label1.TabIndex = 102;
            this.label1.Text = "特别感谢 @ 91wii.riggzh  github.bob";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label_count
            // 
            this.label_count.Location = new System.Drawing.Point(295, 55);
            this.label_count.Name = "label_count";
            this.label_count.Size = new System.Drawing.Size(128, 19);
            this.label_count.TabIndex = 103;
            this.label_count.Text = "count:11";
            this.label_count.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button_lookUdpDlc
            // 
            this.button_lookUdpDlc.Location = new System.Drawing.Point(335, 311);
            this.button_lookUdpDlc.Name = "button_lookUdpDlc";
            this.button_lookUdpDlc.Size = new System.Drawing.Size(88, 23);
            this.button_lookUdpDlc.TabIndex = 104;
            this.button_lookUdpDlc.Text = "查看UPD+DLC";
            this.button_lookUdpDlc.UseVisualStyleBackColor = true;
            this.button_lookUdpDlc.Visible = false;
            this.button_lookUdpDlc.Click += new System.EventHandler(this.button_lookUdpDlc_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1042, 554);
            this.Controls.Add(this.button_lookUdpDlc);
            this.Controls.Add(this.label_count);
            this.Controls.Add(this.label1);
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
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.PictureBox pictureBox_gameicon;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label_info;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 更新TitleId文件ToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar_download;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 发送反馈ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Label label_count;
        private System.Windows.Forms.Button button_lookUdpDlc;
    }
}

