using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSGameDownloader
{
    public partial class Form2 : Form
    {

        public String gameId;
        public String gameName;
        public JObject _cookies;
        public JObject _config;


        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        public Form2(String gameId, String gameName, JObject _cookies, JObject _config)
        {
            this.gameId = gameId;
            this.gameName = gameName;
            this._cookies = _cookies;
            this._config = _config;

            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            WebRefresh();

            if (_config["localGameDir"].ToString().Length > 0 && Directory.Exists(_config["localGameDir"].ToString()))
            {
                btnLocalUrl.Visible = true;

                var dir = _config["localGameDir"].ToString() + "\\" + gameId.Substring(0, 5) + "\\" + gameId + "\\";
                if (Directory.Exists(dir))
                {
                    btnLocalUrl.Text = "打开本地目录";
                }
            }
            else
            {
                btnLocalUrl.Visible = false;
            }

              
        }

        private string GetPanUrl(string tid)
        {
            return (radioButton_xci.Checked ? _config["panUrlXci"].ToString() : (radioButton_upd.Checked ? _config["panUrlUpd"].ToString() : _config["panUrlNsp"].ToString()))
                + tid.Substring(0, 5) + "/"
                + tid + "&parentPath=/";

            //链接: https://pan.baidu.com/s/1uE06kZ2N-EHIBa5Q_X-K1w 
            //提取码: dupb 复制这段内容后打开百度网盘手机App，操作更方便哦
        }

        private void btnCopyUrl_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetDataObject(GetPanUrl(gameId), true);
                MessageBox.Show("已经复制到剪贴版上", "复制成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "复制失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLocalUrl_Click(object sender, EventArgs e)
        {
            var dir = _config["localGameDir"].ToString() + "\\" + gameId.Substring(0, 5) + "\\" + gameId + "\\";
            if (Directory.Exists(dir))
            {
                Process.Start("Explorer.exe", dir);
            }
            else
            {
                Directory.CreateDirectory(dir);
                Process.Start("Explorer.exe", dir);
                btnLocalUrl.Text = "打开本地目录";

                try
                {
                    Clipboard.SetDataObject(dir, true);
                    Console.WriteLine("copyed created dir path:" + dir);
                }
                catch
                {
                   
                }
            }
        }

        private void radioButton_Click(object sender, EventArgs e)
        {
            var r = sender as RadioButton;
            if (!r.Checked) return;
            WebRefresh();
        }

        private void WebRefresh()
        {
            var url = GetPanUrl(gameId);
            Console.WriteLine("打开:" + url);

            String cookie = "";
            JToken jt = _cookies[radioButton_nsp.Checked ? "nsp_cookie" : radioButton_xci.Checked ? "xci_cookie" : "upd_dlc_cookie"];
            if (jt != null)
            {
                cookie = jt.ToString();
            }
            //使用cookie方法免写密码,只有手动时 才更新cookie
            InternetSetCookie("https://pan.baidu.com/", "BDCLND", cookie);
            Console.WriteLine("set cookie BDCLND:" + cookie);

            //panWebBrowser.Navigate (url, "_self" , null, "User-Agent: Mozilla/5.0 (iPhone; CPU iPhone OS 12_1_4 like Mac OS X) AppleWebKit/605.1.15 (KHTMsL, like Gecko) Version/12.0 Mobile/15E148 Safari/604.1");
            panWebBrowser.Navigate(url); //点击刷新 只找本体
        }

        private void panWebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            Console.WriteLine("Navigating:" + e.Url);
        }

        private void panWebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            //GetCookies();
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var oUrl = WebUtility.UrlDecode(e.Url.ToString());
            Console.WriteLine("url:" + oUrl);

            if (panWebBrowser.Document.Body == null) return;
            UpdateCookies(radioButton_nsp.Checked ? "nsp_cookie" : radioButton_xci.Checked ? "xci_cookie" : "upd_dlc_cookie");
        }

        private void UpdateCookies(String cookieType)
        {
            var str = panWebBrowser.Document.Cookie.Replace(",", "%2C");
            var cookies = str.Split(';');
            foreach (var c in cookies)
            {
                var kv = c.Split('=');
                if (kv[0].Trim().Equals("BDCLND"))
                {
                    _cookies[cookieType] = kv.Length == 0 ? "" : kv[1].Trim();
                    Console.WriteLine("update cookie," + cookieType + ":" + (kv.Length == 0 ? "" : kv[1].Trim()));
                }
            }
        }
    }
}
