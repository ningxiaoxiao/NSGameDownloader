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
            this.textBox_keyword = new System.Windows.Forms.TextBox();
            this.button_search = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.radioButton_nsp = new System.Windows.Forms.RadioButton();
            this.radioButton_xci = new System.Windows.Forms.RadioButton();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButton_UPD = new System.Windows.Forms.RadioButton();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.radioButton_DLC = new System.Windows.Forms.RadioButton();
            this.radioButton_ALL = new System.Windows.Forms.RadioButton();
            this.button_updata = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox_keyword
            // 
            this.textBox_keyword.Location = new System.Drawing.Point(12, 12);
            this.textBox_keyword.Name = "textBox_keyword";
            this.textBox_keyword.Size = new System.Drawing.Size(342, 21);
            this.textBox_keyword.TabIndex = 0;
            this.textBox_keyword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_keyword_KeyPress);
            // 
            // button_search
            // 
            this.button_search.Location = new System.Drawing.Point(360, 12);
            this.button_search.Name = "button_search";
            this.button_search.Size = new System.Drawing.Size(63, 23);
            this.button_search.TabIndex = 1;
            this.button_search.Text = "搜索";
            this.button_search.UseVisualStyleBackColor = true;
            this.button_search.Click += new System.EventHandler(this.button_search_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(441, 12);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(589, 530);
            this.webBrowser1.TabIndex = 2;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // radioButton_nsp
            // 
            this.radioButton_nsp.AutoSize = true;
            this.radioButton_nsp.Checked = true;
            this.radioButton_nsp.Location = new System.Drawing.Point(12, 39);
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
            this.radioButton_xci.Location = new System.Drawing.Point(59, 39);
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
            this.columnHeader3});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 61);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(411, 244);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "key";
            this.columnHeader1.Width = 111;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "name";
            this.columnHeader2.Width = 190;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "type";
            this.columnHeader3.Width = 70;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pictureBox1.ImageLocation = "";
            this.pictureBox1.Location = new System.Drawing.Point(12, 311);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(118, 311);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(305, 217);
            this.label2.TabIndex = 8;
            this.label2.Text = "label2";
            // 
            // radioButton_UPD
            // 
            this.radioButton_UPD.AutoSize = true;
            this.radioButton_UPD.Location = new System.Drawing.Point(106, 39);
            this.radioButton_UPD.Name = "radioButton_UPD";
            this.radioButton_UPD.Size = new System.Drawing.Size(41, 16);
            this.radioButton_UPD.TabIndex = 9;
            this.radioButton_UPD.TabStop = true;
            this.radioButton_UPD.Text = "UPD";
            this.radioButton_UPD.UseVisualStyleBackColor = true;
            this.radioButton_UPD.CheckedChanged += new System.EventHandler(this.radioButton_UPD_CheckedChanged);
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabel1.LinkColor = System.Drawing.Color.Silver;
            this.linkLabel1.Location = new System.Drawing.Point(9, 528);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(217, 17);
            this.linkLabel1.TabIndex = 10;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "特别感谢 @ 91wii.riggzh  github.bob";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // radioButton_DLC
            // 
            this.radioButton_DLC.AutoSize = true;
            this.radioButton_DLC.Location = new System.Drawing.Point(153, 39);
            this.radioButton_DLC.Name = "radioButton_DLC";
            this.radioButton_DLC.Size = new System.Drawing.Size(41, 16);
            this.radioButton_DLC.TabIndex = 11;
            this.radioButton_DLC.TabStop = true;
            this.radioButton_DLC.Text = "DLC";
            this.radioButton_DLC.UseVisualStyleBackColor = true;
            this.radioButton_DLC.CheckedChanged += new System.EventHandler(this.radioButton_DLC_CheckedChanged_1);
            // 
            // radioButton_ALL
            // 
            this.radioButton_ALL.AutoSize = true;
            this.radioButton_ALL.Location = new System.Drawing.Point(200, 39);
            this.radioButton_ALL.Name = "radioButton_ALL";
            this.radioButton_ALL.Size = new System.Drawing.Size(41, 16);
            this.radioButton_ALL.TabIndex = 12;
            this.radioButton_ALL.TabStop = true;
            this.radioButton_ALL.Text = "ALL";
            this.radioButton_ALL.UseVisualStyleBackColor = true;
            this.radioButton_ALL.CheckedChanged += new System.EventHandler(this.radioButton_ALL_CheckedChanged);
            // 
            // button_updata
            // 
            this.button_updata.Location = new System.Drawing.Point(332, 36);
            this.button_updata.Name = "button_updata";
            this.button_updata.Size = new System.Drawing.Size(91, 23);
            this.button_updata.TabIndex = 13;
            this.button_updata.Text = "更新本地数据";
            this.button_updata.UseVisualStyleBackColor = true;
            this.button_updata.Click += new System.EventHandler(this.button_updata_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1042, 554);
            this.Controls.Add(this.button_updata);
            this.Controls.Add(this.radioButton_ALL);
            this.Controls.Add(this.radioButton_DLC);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.radioButton_UPD);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.radioButton_xci);
            this.Controls.Add(this.radioButton_nsp);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.button_search);
            this.Controls.Add(this.textBox_keyword);
            this.Name = "Form1";
            this.Text = "NSGameDownloader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_keyword;
        private System.Windows.Forms.Button button_search;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.RadioButton radioButton_nsp;
        private System.Windows.Forms.RadioButton radioButton_xci;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButton_UPD;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.RadioButton radioButton_DLC;
        private System.Windows.Forms.RadioButton radioButton_ALL;
        private System.Windows.Forms.Button button_updata;
    }
}

