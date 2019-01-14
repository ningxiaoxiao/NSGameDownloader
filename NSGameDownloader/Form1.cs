using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using NSGameDownloader.Properties;

namespace NSGameDownloader
{
    public partial class Form1 : Form
    {
        private const string PanUrlHead = "https://pan.baidu.com/s/";
        private const string NspPanKey = "1tOFTvpJwikcdo2W12Z8dEw";
        private const string XciPanKey = "1cwIw1-qsNOKaq6xrK0VUqQ";
        private const string NspPw = "vb4v";
        private const string XciPw = "fi4r";
        private const string TitleKeysURL = "https://snip.li/nutdb";
        private const string TitleKeysPath = "keys.json";
        private string curTid;
        private JObject Titlekeys;

        public Form1()
        {
            InitializeComponent();
        }


        private string GetUrl(string tid)
        {
            return PanUrlHead + (radioButton_xci.Checked ? XciPanKey : NspPanKey)
                              + "#list/path=/"
                              + (radioButton_xci.Checked ? "XCI" : "Nintendo Switch Games")
                              + (radioButton_xci.Checked ? "" : radioButton_DLC.Checked ? "/UPD + DLC" : "/NSP")
                              + "/" + tid.Substring(0, 5)
                              + "/" + tid
                              + "&parentPath=/";
        }

        public void UpdateTitleKey()
        {
            Titlekeys = new JObject();
            var http = new WebClient { Encoding = Encoding.UTF8 };

            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var html = http.DownloadString(TitleKeysURL);

            var keys = new List<string>(html.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries));

            //前3行 不要
            keys.RemoveAt(0);
            keys.RemoveAt(0);
            keys.RemoveAt(0);

            foreach (var key in keys)
            {
                var kan = key.Split('|');
                //0100fbd00b91e0000000000000000005|2ef7a3ef88b25c3999cb82a489c526dd|Valkyria Chronicles 4 Demo

                ///000结尾是本体
                /// 800是upd
                /// 别的是dlc
                var jtemp = new JObject
                {
                    ["title"] = kan[0].Trim().Substring(0, 16),
                    ["key"] = kan[2].Trim(),
                    ["name"] = kan[6].Trim(),
                    ["type"] = kan[3] == "1" ? "UPD" : kan[4] == "1" ? "DLC" : kan[5] == "1" ? "DEMO" : "BASE"
                };

                Titlekeys[kan[0].Trim()] = jtemp;
            }


            File.WriteAllText(TitleKeysPath, Titlekeys.ToString());
        }




        private void SetPw(string pw)
        {
            var element = webBrowser1.Document.GetElementById("nwtz3z5E");
            if (element == null) return;
            element.SetAttribute("value", pw);

            var alla = webBrowser1.Document.GetElementsByTagName("a");
            foreach (HtmlElement a in alla)
                if (a.InnerText == "提取文件")
                {
                    a.InvokeMember("Click");
                    break;
                }

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var oUrl = WebUtility.UrlDecode(e.Url.ToString());


            Console.WriteLine("log:" + oUrl);

            if (webBrowser1.Document.Body.InnerText != null && webBrowser1.Document.Body.InnerText.Contains("请输入提取码"))
            {
                if (e.Url.ToString().Contains(NspPanKey.Substring(2))) SetPw(NspPw);
                else if (e.Url.ToString().Contains(XciPanKey.Substring(2))) SetPw(XciPw);

            }
            else
            {
                if (oUrl == "https://pan.baidu.com/s/1tOFTvpJwikcdo2W12Z8dEw#list/path=/" ||
                    oUrl == "https://pan.baidu.com/s/1cwIw1-qsNOKaq6xrK0VUqQ#list/path=/")
                {
                    WebRefresh();//输入密码后会再一次来到根目录,要再跳一次
                }

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists(TitleKeysPath))
                UpdateTitleKey();
            else
                Titlekeys = JObject.Parse(File.ReadAllText(TitleKeysPath));

            if (!Directory.Exists("image")) Directory.CreateDirectory("image");
        }


        private void radioButton_nsp_CheckedChanged(object sender, EventArgs e)
        {
            WebRefresh();
        }

        private void WebRefresh()
        {
            if (curTid == null) return;
            Navigate(GetUrl(curTid));
        }

        private void radioButton_DLC_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton_xci_CheckedChanged(object sender, EventArgs e)
        {
            WebRefresh();
        }
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);


        private void Navigate(string url)
        {
            //todo 多线程

            webBrowser1.Url = new Uri(url);
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            //var keys = Titlekeys.Root.Where(x => x.Contains(textBox_keyword.Text.Trim()));

            listView1.Items.Clear();
            foreach (var titlekey in Titlekeys)
                //不显示demo
                if (titlekey.Value["name"].ToString().Contains(textBox_keyword.Text.Trim()) && titlekey.Value["type"].ToString() != "DEMO")
                    listView1.Items.Add(new ListViewItem(new[]
                    {
                        titlekey.Value["title"].ToString(),
                        titlekey.Value["name"].ToString(),
                        titlekey.Value["type"].ToString()
                    }));
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            curTid = listView1.SelectedItems[0].Text;
            curTid = curTid.Substring(0, 13) + "000";
            var ty = listView1.SelectedItems[0].SubItems[2].Text;
            radioButton_DLC.Checked = ty == "DLC" || ty == "UPD";

            WebRefresh();


            Console.WriteLine(curTid);

            var g = Titlekeys[curTid].ToObject<JObject>();
            if (!g.ContainsKey("info"))
            {
                var i = GetGameInfo(curTid);
                if (i == null) return;
                Titlekeys[curTid]["info"] = i;
            }


            var info = Titlekeys[curTid]["info"];


            var size = info["total_rom_size"].ToObject<long>();

            label2.Text = ConvertBytes(size) + "\n";
            label2.Text += info["description"] + "\n";


            var t = new Thread(GetGameImage);
            t.Start(curTid);




        }

        public static string ConvertBytes(long len)
        {
            if (len > 1073741824)
                return (len / 1073741824.0).ToString("F") + "GB";
            if (len > 1048576)
                return (len / 1048576.0).ToString("F") + "MB";
            return (len / 1024.0).ToString("F") + "KB";
        }

        private void GetGameImage(object otid)
        {
            //todo 多线程
            pictureBox1.Image = Resources.load;



            var tid = otid as string;
            var filename = "image\\" + tid + ".jpg";

            if (File.Exists(filename))
            {
                pictureBox1.Image = Image.FromFile(filename);
                return;
            }

            var web = new WebClient { Encoding = Encoding.UTF8 };
            // 解决WebClient不能通过https下载内容问题
            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                web.DownloadFile("https://terannet.sirv.com/CDNSP/" + tid.ToLower() + ".jpg", filename);
                pictureBox1.Image = Image.FromFile(filename);

            }
            catch
            {
                if (File.Exists(filename))
                    File.Delete(filename);


                pictureBox1.Image = Resources.error;
                return;
            }
        }

        private JObject GetGameInfo(string tid)
        {
            //todo 多线程
            //https://ec.nintendo.com/apps/%s/AU
            //todo 从http://www.eshop-switch.com 拿数据
            using (var web = new WebClient() { Encoding = Encoding.UTF8 })
            {
                string html;
                try
                {
                    html = web.DownloadString($"https://ec.nintendo.com/apps/{tid}/AU");
                }
                catch
                {
                    return null;
                }

                html = html.Split(new[] { "NXSTORE.titleDetail.jsonData = " }, StringSplitOptions.RemoveEmptyEntries)[1]
                    .Split(new[] { "NXSTORE.titleDetail" }, StringSplitOptions.RemoveEmptyEntries)[0].Replace(";", "");

                return JObject.Parse(html);
            }
        }

        private void textBox_keyword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter) return;
            e.Handled = true;
            button_search_Click(null, null);

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.91wii.com/thread-104797-1-1.html");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(TitleKeysPath, Titlekeys.ToString());
        }
    }
}