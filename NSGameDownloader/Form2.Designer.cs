namespace NSGameDownloader
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.panWebBrowser = new System.Windows.Forms.WebBrowser();
            this.radioButton_upd = new System.Windows.Forms.RadioButton();
            this.radioButton_xci = new System.Windows.Forms.RadioButton();
            this.radioButton_nsp = new System.Windows.Forms.RadioButton();
            this.btnCopyUrl = new System.Windows.Forms.Button();
            this.btnLocalUrl = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panWebBrowser
            // 
            this.panWebBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panWebBrowser.Location = new System.Drawing.Point(13, 44);
            this.panWebBrowser.Margin = new System.Windows.Forms.Padding(4);
            this.panWebBrowser.MinimumSize = new System.Drawing.Size(27, 25);
            this.panWebBrowser.Name = "panWebBrowser";
            this.panWebBrowser.Size = new System.Drawing.Size(885, 638);
            this.panWebBrowser.TabIndex = 3;
            this.panWebBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
            this.panWebBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.panWebBrowser_Navigated);
            this.panWebBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.panWebBrowser_Navigating);
            // 
            // radioButton_upd
            // 
            this.radioButton_upd.AutoSize = true;
            this.radioButton_upd.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton_upd.Location = new System.Drawing.Point(203, 13);
            this.radioButton_upd.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton_upd.Name = "radioButton_upd";
            this.radioButton_upd.Size = new System.Drawing.Size(131, 24);
            this.radioButton_upd.TabIndex = 107;
            this.radioButton_upd.Text = "查看UPD+DLC";
            this.radioButton_upd.UseVisualStyleBackColor = true;
            this.radioButton_upd.Click += new System.EventHandler(this.radioButton_Click);
            // 
            // radioButton_xci
            // 
            this.radioButton_xci.AutoSize = true;
            this.radioButton_xci.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton_xci.Location = new System.Drawing.Point(111, 13);
            this.radioButton_xci.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton_xci.Name = "radioButton_xci";
            this.radioButton_xci.Size = new System.Drawing.Size(84, 24);
            this.radioButton_xci.TabIndex = 106;
            this.radioButton_xci.Text = "查看XCI";
            this.radioButton_xci.UseVisualStyleBackColor = true;
            this.radioButton_xci.Click += new System.EventHandler(this.radioButton_Click);
            // 
            // radioButton_nsp
            // 
            this.radioButton_nsp.AutoSize = true;
            this.radioButton_nsp.Checked = true;
            this.radioButton_nsp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButton_nsp.Location = new System.Drawing.Point(13, 13);
            this.radioButton_nsp.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton_nsp.Name = "radioButton_nsp";
            this.radioButton_nsp.Size = new System.Drawing.Size(90, 24);
            this.radioButton_nsp.TabIndex = 105;
            this.radioButton_nsp.TabStop = true;
            this.radioButton_nsp.Text = "查看NSP";
            this.radioButton_nsp.UseVisualStyleBackColor = true;
            this.radioButton_nsp.Click += new System.EventHandler(this.radioButton_Click);
            // 
            // btnCopyUrl
            // 
            this.btnCopyUrl.Location = new System.Drawing.Point(823, 13);
            this.btnCopyUrl.Name = "btnCopyUrl";
            this.btnCopyUrl.Size = new System.Drawing.Size(75, 23);
            this.btnCopyUrl.TabIndex = 108;
            this.btnCopyUrl.Text = "复制地址";
            this.btnCopyUrl.UseVisualStyleBackColor = true;
            this.btnCopyUrl.Click += new System.EventHandler(this.btnCopyUrl_Click);
            // 
            // btnLocalUrl
            // 
            this.btnLocalUrl.Location = new System.Drawing.Point(711, 13);
            this.btnLocalUrl.Name = "btnLocalUrl";
            this.btnLocalUrl.Size = new System.Drawing.Size(106, 23);
            this.btnLocalUrl.TabIndex = 109;
            this.btnLocalUrl.Text = "创建本地目录";
            this.btnLocalUrl.UseVisualStyleBackColor = true;
            this.btnLocalUrl.Click += new System.EventHandler(this.btnLocalUrl_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 695);
            this.Controls.Add(this.btnLocalUrl);
            this.Controls.Add(this.btnCopyUrl);
            this.Controls.Add(this.radioButton_upd);
            this.Controls.Add(this.radioButton_xci);
            this.Controls.Add(this.radioButton_nsp);
            this.Controls.Add(this.panWebBrowser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form2";
            this.Text = "下载游戏";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser panWebBrowser;
        private System.Windows.Forms.RadioButton radioButton_upd;
        private System.Windows.Forms.RadioButton radioButton_xci;
        private System.Windows.Forms.RadioButton radioButton_nsp;
        private System.Windows.Forms.Button btnCopyUrl;
        private System.Windows.Forms.Button btnLocalUrl;
    }
}