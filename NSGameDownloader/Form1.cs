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
using System.Xml;
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
        private const string XCITitleKeysURL = "http://nswdb.com/xml.php";
        private const string TitleKeysPath = "keys.json";
        private const string NSPTitleKeysPath = "NSPkeys.json";
        private const string XCITitleKeysPath = "XCIkeys.json";
        private const string UPDTitleKeysPath = "UPDkeys.json";
        private const string DLCTitleKeysPath = "DLCkeys.json";
        private const string DEMOTitleKeysPath = "DEMOkeys.json";
        private string curTid;
        private string curType;
        private JObject Titlekeys;
        private JObject NSPTitlekeys;
        private JObject XCITitlekeys;
        private JObject UPDTitlekeys;
        private JObject DLCTitlekeys;
        private JObject DEMOTitlekeys;

        public Form1()
        {
            InitializeComponent();
            //UpdateTitleKey();
        }


        private string GetUrl(string tid,string type)
        {
            /*
            return PanUrlHead + (radioButton_xci.Checked ? XciPanKey : NspPanKey)
                              + "#list/path=/"
                              + (radioButton_xci.Checked ? "XCI" : "Nintendo Switch Games")
                              + (radioButton_xci.Checked ? "" : radioButton_UPD.Checked ? "/UPD + DLC" : "/NSP")
                              + "/" + tid.Substring(0, 5)
                              + "/" + tid
                              + "&parentPath=/";
            */
            return PanUrlHead + (type == "XCI" ? XciPanKey : NspPanKey)
                  + "#list/path=/"
                  + (type == "XCI" ? "XCI" : "Nintendo Switch Games")
                  + (type == "XCI" ? "" : type == "NSP" ?  "/NSP":"/UPD + DLC")
                  + "/" + tid.Substring(0, 5)
                  + "/" + tid
                  + "&parentPath=/";
        }
        public void UpdateXciTitleKey()
        {
            XCITitlekeys = new JObject();
            var http = new WebClient { Encoding = Encoding.UTF8 };

            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var html = http.DownloadString(XCITitleKeysURL);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(html);
            //XmlNode rootNode = xmlDoc.SelectSingleNode("releases");
            XmlElement root = xmlDoc.DocumentElement;
            XmlNodeList nodeLists = root.GetElementsByTagName("release");
            foreach(XmlNode node in nodeLists)
            {
                var jtemp = new JObject
                {
                    ["title"] = ((XmlElement)node).GetElementsByTagName("titleid")[0].InnerText,
                    ["key"] = "00000000000000000000000000000000",
                    ["name"] = ((XmlElement)node).GetElementsByTagName("name")[0].InnerText,
                    ["type"] ="XCI"
                };
                XCITitlekeys[jtemp.Value<string>("title")] = jtemp;
            }
            File.WriteAllText(XCITitleKeysPath, XCITitlekeys.ToString());
        }

        public void UpdateTitleKey()
        {
            Titlekeys = new JObject();
            NSPTitlekeys = new JObject();
            XCITitlekeys = new JObject();
            UPDTitlekeys = new JObject();
            DLCTitlekeys = new JObject();
            DEMOTitlekeys = new JObject();
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
                    ["type"] = kan[3] == "1" ? "UPD" : kan[4] == "1" ? "DLC" : kan[5] == "1" ? "DEMO" : "NSP"
                };

                switch (jtemp.Value<string>("type"))
                {
                    case "NSP":
                        NSPTitlekeys[kan[0].Trim()] = jtemp;
                        break;
                    case "UPD":
                        UPDTitlekeys[kan[0].Trim()] = jtemp;
                        break;
                    case "DLC":
                        DLCTitlekeys[kan[0].Trim()] = jtemp;
                        break;
                    case "DEMO":
                        DEMOTitlekeys[kan[0].Trim()] = jtemp;
                        break;

                }
                 

                Titlekeys[kan[0].Trim()] = jtemp;
            }


            File.WriteAllText(TitleKeysPath, Titlekeys.ToString());
            File.WriteAllText(NSPTitleKeysPath, NSPTitlekeys.ToString());
            File.WriteAllText(UPDTitleKeysPath, UPDTitlekeys.ToString());
            File.WriteAllText(DLCTitleKeysPath, DLCTitlekeys.ToString());
            File.WriteAllText(DEMOTitleKeysPath, DEMOTitlekeys.ToString());
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

            if (File.Exists(TitleKeysPath) && File.Exists(NSPTitleKeysPath) && File.Exists(UPDTitleKeysPath) && File.Exists(DLCTitleKeysPath) && File.Exists(DEMOTitleKeysPath) && File.Exists(XCITitleKeysPath))
            {
                Titlekeys = JObject.Parse(File.ReadAllText(TitleKeysPath));
                NSPTitlekeys = JObject.Parse(File.ReadAllText(NSPTitleKeysPath));
                XCITitlekeys = JObject.Parse(File.ReadAllText(XCITitleKeysPath));
                UPDTitlekeys = JObject.Parse(File.ReadAllText(UPDTitleKeysPath));
                DLCTitlekeys = JObject.Parse(File.ReadAllText(DLCTitleKeysPath));
                DEMOTitlekeys = JObject.Parse(File.ReadAllText(DEMOTitleKeysPath));

            }
            else
            //Titlekeys = JObject.Parse(File.ReadAllText(TitleKeysPath));
            {
                UpdateTitleKey();
                UpdateXciTitleKey();
            }
                

            if (!Directory.Exists("image")) Directory.CreateDirectory("image");

            listView1.Items.Clear();
            foreach (var titlekey in NSPTitlekeys)
                //默认显示NSP
                if (titlekey.Value["name"].ToString().Contains(textBox_keyword.Text.Trim()))
                    listView1.Items.Add(new ListViewItem(new[]
                    {
                        titlekey.Value["title"].ToString(),
                        titlekey.Value["name"].ToString(),
                        titlekey.Value["type"].ToString()
                    }));

            

        }


        private void radioButton_nsp_CheckedChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            foreach (var titlekey in NSPTitlekeys)
            {
                if (titlekey.Value["name"].ToString().Contains(textBox_keyword.Text.Trim()))
                listView1.Items.Add(new ListViewItem(new[]
                    {
                        titlekey.Value["title"].ToString(),
                        titlekey.Value["name"].ToString(),
                        titlekey.Value["type"].ToString()
                    }));
            }
            WebRefresh();
        }

        private void WebRefresh()
        {
            if (curTid == null) return;
            Navigate(GetUrl(curTid,curType));
        }

        private void radioButton_UPD_CheckedChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            foreach (var titlekey in UPDTitlekeys)
            {
                if (titlekey.Value["name"].ToString().Contains(textBox_keyword.Text.Trim()))
                listView1.Items.Add(new ListViewItem(new[]
                    {
                        titlekey.Value["title"].ToString(),
                        titlekey.Value["name"].ToString(),
                        titlekey.Value["type"].ToString()
                    }));
            }            
                    

           // WebRefresh();
        }

        private void radioButton_xci_CheckedChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            foreach (var titlekey in XCITitlekeys)
            {
                listView1.Items.Add(new ListViewItem(new[]
                    {
                        titlekey.Value["title"].ToString(),
                        titlekey.Value["name"].ToString(),
                        titlekey.Value["type"].ToString()
                    }));
            }
            //WebRefresh();
        }
   
        private void Navigate(string url)
        {
            //todo 多线程

            webBrowser1.Url = new Uri(url);
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            //var keys = Titlekeys.Root.Where(x => x.Contains(textBox_keyword.Text.Trim()));
            
            listView1.Items.Clear();
            if (radioButton_nsp.Checked)
            {
                foreach (var titlekey in NSPTitlekeys)
                    //不显示demo
                    if (titlekey.Value["name"].ToString().Contains(textBox_keyword.Text.Trim()) && titlekey.Value["type"].ToString() != "DEMO")
                        listView1.Items.Add(new ListViewItem(new[]
                        {
                        titlekey.Value["title"].ToString(),
                        titlekey.Value["name"].ToString(),
                        titlekey.Value["type"].ToString()
                    }));
            }
            else if (radioButton_UPD.Checked)
            {
                foreach (var titlekey in UPDTitlekeys)
                    //不显示demo
                    if (titlekey.Value["name"].ToString().Contains(textBox_keyword.Text.Trim()) && titlekey.Value["type"].ToString() != "DEMO")
                        listView1.Items.Add(new ListViewItem(new[]
                        {
                        titlekey.Value["title"].ToString(),
                        titlekey.Value["name"].ToString(),
                        titlekey.Value["type"].ToString()
                    }));
            }
            else if (radioButton_DLC.Checked)
            {
                foreach (var titlekey in DLCTitlekeys)
                    //不显示demo
                    if (titlekey.Value["name"].ToString().Contains(textBox_keyword.Text.Trim()) && titlekey.Value["type"].ToString() != "DEMO")
                        listView1.Items.Add(new ListViewItem(new[]
                        {
                        titlekey.Value["title"].ToString(),
                        titlekey.Value["name"].ToString(),
                        titlekey.Value["type"].ToString()
                    }));
            }
            else if (radioButton_xci.Checked)
            {
                foreach (var titlekey in XCITitlekeys)
                    //不显示demo
                    if (titlekey.Value["name"].ToString().Contains(textBox_keyword.Text.Trim()) && titlekey.Value["type"].ToString() != "DEMO")
                        listView1.Items.Add(new ListViewItem(new[]
                        {
                        titlekey.Value["title"].ToString(),
                        titlekey.Value["name"].ToString(),
                        titlekey.Value["type"].ToString()
                    }));
            }
            else
            {
                foreach (var titlekey in Titlekeys)
                    //不显示demo
                    if (titlekey.Value["name"].ToString().Contains(textBox_keyword.Text.Trim()) && titlekey.Value["type"].ToString() != "DEMO")
                        listView1.Items.Add(new ListViewItem(new[]
                        {
                        titlekey.Value["title"].ToString(),
                        titlekey.Value["name"].ToString(),
                        titlekey.Value["type"].ToString()
                    }));
                foreach (var titlekey in XCITitlekeys)
                    //不显示demo
                    if (titlekey.Value["name"].ToString().Contains(textBox_keyword.Text.Trim()) && titlekey.Value["type"].ToString() != "DEMO")
                        listView1.Items.Add(new ListViewItem(new[]
                        {
                        titlekey.Value["title"].ToString(),
                        titlekey.Value["name"].ToString(),
                        titlekey.Value["type"].ToString()
                    }));
            }
            

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            curTid = listView1.SelectedItems[0].Text;
            var g = Titlekeys[curTid].ToObject<JObject>();
            curTid = curTid.Substring(0, 13) + "000";
            curType = listView1.SelectedItems[0].SubItems[2].Text;
            //radioButton_UPD.Checked = ty == "DLC" || ty == "UPD";

            WebRefresh();


            Console.WriteLine(curTid);

           
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
            //File.WriteAllText(TitleKeysPath, Titlekeys.ToString());
        }

        private void radioButton_DLC_CheckedChanged_1(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            foreach (var titlekey in DLCTitlekeys)
            {
                if (titlekey.Value["name"].ToString().Contains(textBox_keyword.Text.Trim()))
                listView1.Items.Add(new ListViewItem(new[]
                    {
                        titlekey.Value["title"].ToString(),
                        titlekey.Value["name"].ToString(),
                        titlekey.Value["type"].ToString()
                    }));
            }
            WebRefresh();

        }

        private void button_updata_Click(object sender, EventArgs e)
        {
            UpdateTitleKey();
        }

        private void radioButton_ALL_CheckedChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            foreach (var titlekey in Titlekeys)
            {
                if (titlekey.Value["name"].ToString().Contains(textBox_keyword.Text.Trim()) && titlekey.Value["type"].ToString() != "DEMO")
                    listView1.Items.Add(new ListViewItem(new[]
                        {
                        titlekey.Value["title"].ToString(),
                        titlekey.Value["name"].ToString(),
                        titlekey.Value["type"].ToString()
                    }));
            }
            foreach (var titlekey in XCITitlekeys)
            {
                if (titlekey.Value["name"].ToString().Contains(textBox_keyword.Text.Trim()) && titlekey.Value["type"].ToString() != "DEMO")
                    listView1.Items.Add(new ListViewItem(new[]
                        {
                        titlekey.Value["title"].ToString(),
                        titlekey.Value["name"].ToString(),
                        titlekey.Value["type"].ToString()
                    }));
            }
            WebRefresh();
        }
    }
}