using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ExcelDataReader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSGameDownloader.Properties;

namespace NSGameDownloader
{
    //todo 增加在外部浏览器打开的功能
    //todo 尝试进行从百度云直接得到真实下载地址 
    public partial class Form1 : Form
    {

        private const string ExcelPath = "db.xlsx";
        private const int EM_SETCUEBANNER = 0x1501;
        private Config _config = new Config();
        /// <summary>
        ///     原始值
        /// </summary>
        private string _curTid;

        private JObject _titlekeys;
        private List<PictureBox> _pictureBoxs = new List<PictureBox>();
        public Form1()
        {
            InitializeComponent();
        }

        private void ReadExcel()
        {
            var fs = File.Open(ExcelPath, FileMode.Open, FileAccess.Read);
            var er = ExcelReaderFactory.CreateReader(fs);

            var titleDataSet = er.AsDataSet(new ExcelDataSetConfiguration
            {
                UseColumnDataType = true,
                ConfigureDataTable = r => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true
                }
            });

            //读出第一个表 
            //0         1       2       3       4       5       6
            //tid       iszh    cname   oname   ext     ver     havedlc
            var dt = titleDataSet.Tables[0];
            Invoke(new Action(() =>
            {
                toolStripProgressBar_download.Maximum = dt.Rows.Count;
                toolStripProgressBar_download.Value = 0;
            }));
            foreach (DataRow row in dt.Rows)
            {
                var tid = row[0].ToString();
                var iszh = row[1].ToString() != "x";
                var cname = row[2].ToString();
                var allnames = row[3].ToString();
                var haveXci = row[4].ToString().Contains("XCI");
                var haveNsp = row[4].ToString().Contains("NSP");
                var haveUpd = row[5].ToString() != "v0";
                var haveDlc = row[6].ToString() != "×";


                var jtemp = new JObject
                {
                    ["tid"] = tid,
                    ["iszh"] = iszh,
                    ["cname"] = cname,
                    ["allnames"] = allnames,

                    ["xci"] = haveXci,
                    ["nsp"] = haveNsp,
                    ["upd"] = haveUpd,
                    ["dlc"] = haveDlc,
                    ["ver"] = row[5].ToString(),
                    ["region"] = "AU",
                    ["ename"] = "",

                };
                _titlekeys[tid] = jtemp;
                Invoke(new Action(() => { toolStripProgressBar_download.Value++; }));
            }


            fs.Close();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            // 解决WebClient不能通过https下载内容问题
            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            pictureBox1.Visible = false;
            _pictureBoxs.Add(pictureBox1);
            for (int i = 0; i < 10; i++)
            {
                _pictureBoxs.Add(pictureBox1.Clone());
            }

            LoadConfigFormGithub();

            var tl = new Thread(ThreadLoad);
            tl.Start();
        }

        /// <summary>
        ///     线程初始,不会占用启动时间
        /// </summary>
        private void ThreadLoad()
        {
            if (!File.Exists(_config.TitleKeysPath))
            {
                var t = new Thread(UpdateTitleKey);
                t.Start();
            }
            else
            {
                _titlekeys = JObject.Parse(File.ReadAllText(_config.TitleKeysPath));
            }

            if (!Directory.Exists("image")) Directory.CreateDirectory("image");
            //使用winapi 做占位符
            Invoke(new Action(() => { SendMessage(textBox_keyword.Handle, EM_SETCUEBANNER, 0, "在这里输入 id,中文名,英文名 关键字..."); }));

            SearchGame();
        }

        private void LoadConfigFormGithub()
        {
            using (var http = new WebClient())
            {
                try
                {
                    var jstr = http.DownloadString("https://raw.githubusercontent.com/riggzh/ns/master/config.json");
                    _config = JsonConvert.DeserializeObject<Config>(jstr);
                }
                catch
                {
                    MessageBox.Show("无法访问github,无法更新最新网盘地址.请检查网络", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(_config.TitleKeysPath, _titlekeys.ToString());
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam,
            [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        public void UpdateTitleKey()
        {
            Invoke(new Action(() =>
            {
                toolStripProgressBar_download.Visible = true;
                toolStripProgressBar_download.Maximum = 2;
                toolStripProgressBar_download.Value = 1;
                button_search.Text = "下载中";
                button_search.Enabled = false;
                textBox_keyword.Enabled = false;
            }));

            _titlekeys = new JObject();

            try
            {
                //从github下载新的文件,
                DownloadExcel();
                ReadExcel();
                Invoke(new Action(() =>
                {
                    toolStripProgressBar_download.Maximum = 2;
                    toolStripProgressBar_download.Value = 1;
                }));
                ReadNutDb();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "更新titleid出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            File.WriteAllText(_config.TitleKeysPath, _titlekeys.ToString());

            Invoke(new Action(() =>
            {
                button_search.Text = "搜索";
                button_search.Enabled = true;
                textBox_keyword.Enabled = true;
                toolStripProgressBar_download.Visible = false;
                SearchGame();
            }));
        }

        private void DownloadExcel()
        {
            try
            {

                using (var http = new GZipWebClient())
                {
                    http.Headers.Add("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                    http.Headers.Add("User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36");
                    http.Headers.Add("Accept-Encoding: gzip, deflate, br");
                    http.Headers.Add("Upgrade-Insecure-Requests: 1");

                    http.DownloadFile("https://raw.githubusercontent.com/riggzh/ns/master/db.xlsx", ExcelPath);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("无法访问github,无法更新最新网盘地址.请检查网络", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void ReadNutDb()
        {
            using (var http = new GZipWebClient { Encoding = Encoding.UTF8 })
            {

                http.Headers.Add("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                http.Headers.Add("User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36");
                http.Headers.Add("Accept-Encoding: gzip, deflate, br");
                http.Headers.Add("Upgrade-Insecure-Requests: 1");

                var html = http.DownloadString(_config.NutdbUrl);
                var keys = new List<string>(html.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries));
                if (keys.Count == 0) throw new Exception("没有得到数据");


                //前3行 不要
                keys.RemoveAt(0);
                keys.RemoveAt(0);
                keys.RemoveAt(0);
                Invoke(new Action(() => { toolStripProgressBar_download.Maximum = keys.Count; }));
                var count = 0;
                foreach (var key in keys)
                {
                    var kan = key.Split('|');
                    //0                 |1                                  |2                                  |3          |4      |5      |6              |7              |8      |9
                    //id                |rightsId                           |key                                |isUpdate   |isDLC  |isDemo |baseName       |name           |version|region
                    //01000320000CC000  |01000320000CC0000000000000000000   |F64FBE562E753B662F7CC8D6C8B4EE79   |0          |0      |0      |1-2-Switch™    |1-2-Switch™    |0      |US

                    var tid = kan[0];
                    var name = kan[7];
                    var ver = kan[8];
                    var region = kan[9] == "US" ? "AU" : kan[9]; //美区用不了.换掉

                    if (_titlekeys.ContainsKey(tid)) //只会得到本体
                    {
                        _titlekeys[tid]["ver"] = ver;
                        _titlekeys[tid]["region"] = region;
                        _titlekeys[tid]["ename"] = name;
                    }

                    Invoke(new Action(() => { toolStripProgressBar_download.Value = ++count; }));
                }
            }


        }

        private string GetPanUrl(string tid)
        {
            return _config.PanUrlHead + (radioButton_xci.Checked ? _config.XciPanKey : _config.NspPanKey)
                              + "#list/path=/"
                              + (radioButton_xci.Checked ? "XCI" : "Nintendo Switch Games")
                              + (radioButton_xci.Checked ? "" : radioButton_upd.Checked ? "/UPD + DLC" : "/NSP")
                              + "/" + tid.Substring(0, 5)
                              + "/" + tid
                              + "&parentPath=/";
        }



        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            label_url.Text = e.Url.ToString();
            if (panWebBrowser.Document.Body == null) return;
            GetCookies();
            //缩放页面
            ((SHDocVw.WebBrowser)panWebBrowser.ActiveXInstance).ExecWB(SHDocVw.OLECMDID.OLECMDID_OPTICAL_ZOOM,
                SHDocVw.OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER, 70, IntPtr.Zero);

        }

        private CookieContainer _cookie = new CookieContainer();
        private void GetCookies()
        {
            var str = panWebBrowser.Document.Cookie.Replace(",", "%2C");
            var cookies = str.Split(';');
            foreach (var c in cookies)
            {
                var kv = c.Split('=');
                var cookie = new Cookie(kv[0].Trim(), kv.Length == 0 ? "" : kv[1].Trim());
                _cookie.Add(new Uri("https://pan.baidu.com"), cookie);
            }
        }

        private void WebRefresh()
        {
            if (_curTid == null) return;
            var url = GetPanUrl(_curTid);
            Console.WriteLine("打开:" + url);

            //使用cookie方法免写密码,只有手动时 才更新cookie
            InternetSetCookie("https://pan.baidu.com/", "BDCLND",
                radioButton_xci.Checked ? _config.XciCookie : _config.NspCookie);

            panWebBrowser.Navigate(url); //点击刷新 只找本体
        }


        private void radioButton_Click(object sender, EventArgs e)
        {
            var r = sender as RadioButton;
            if (!r.Checked) return;
            WebRefresh();
        }



        private void button_search_Click(object sender, EventArgs e)
        {
            SearchGame(textBox_keyword.Text);
        }

        private void SearchGame(string keywords = "")
        {
            if (_titlekeys == null) return;
            if (_titlekeys.Count == 0) return;

            Invoke(new Action(() =>
            {
                //todo 多关键字处理
                listView1.Items.Clear();
                foreach (var titlekey in _titlekeys)
                {
                    //全文查找
                    var allstr = titlekey.Value["tid"].ToString() + titlekey.Value["cname"] + titlekey.Value["ename"] + titlekey.Value["allnames"];

                    if (allstr.ToLower().Contains(keywords.Trim().ToLower()))
                        listView1.Items.Add(new ListViewItem(new[]
                        {
                            titlekey.Value["tid"].ToString(),
                            titlekey.Value["cname"].ToString().Trim() == ""
                                ? titlekey.Value["allnames"].ToString()
                                : titlekey.Value["cname"].ToString(), //防止没有中文名
                            titlekey.Value["nsp"].ToObject<bool>() ? "●" : "",
                            titlekey.Value["xci"].ToObject<bool>() ? "●" : "",
                            titlekey.Value["upd"].ToObject<bool>() ? "●" : "",
                            titlekey.Value["dlc"].ToObject<bool>() ? "●" : ""
                        }));
                }

                label_count.Text = "count:" + listView1.Items.Count;
            }));
        }


        private string GetBaseTidFormDlcTid(string tid)
        {
            //后3位置改000
            //13位退1
            var t16 = Convert.ToInt64("0x" + tid.Substring(0, 13) + "000", 16) - 4096;
            return t16.ToString("x16").ToUpper();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            _curTid = listView1.SelectedItems[0].Text;


            //如果点击的是dlc 或者是 upd 那要跳到upd+dlc的目录
            radioButton_upd.Enabled = _titlekeys[_curTid]["dlc"].ToObject<bool>() ||
                                   _titlekeys[_curTid]["upd"].ToObject<bool>();
            radioButton_nsp.Enabled = _titlekeys[_curTid]["nsp"].ToObject<bool>();
            radioButton_xci.Enabled = _titlekeys[_curTid]["xci"].ToObject<bool>();

            if (!radioButton_nsp.Enabled && radioButton_nsp.Checked) radioButton_xci.Checked = true;
            if (!radioButton_xci.Enabled && radioButton_xci.Checked) radioButton_nsp.Checked = true;


            WebRefresh();


            Console.WriteLine(_curTid);

            //刷新来自eshop的信息
            var t = new Thread(GetGameInfoFromEShop);
            t.Start();

        }

        private void GetGameInfoFromEShop()
        {
            pictureBox_gameicon.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox_gameicon.Image = Resources.load;

            //todo 从http://www.eshop-switch.com 拿数据
            var g = _titlekeys[_curTid].ToObject<JObject>();
            if (!g.ContainsKey("info"))
                using (var web = new WebClient { Encoding = Encoding.UTF8 })
                {
                    try
                    {
                        var url = $"https://ec.nintendo.com/apps/{_curTid}/{g["region"]}";
                        Console.WriteLine(url);
                        var ohtml = web.DownloadString(url);

                        var html = ohtml.Split(new[] { "NXSTORE.titleDetail.jsonData = " }, StringSplitOptions.RemoveEmptyEntries)[1];
                        html = html.Split(new[] { "NXSTORE.titleDetail" }, StringSplitOptions.RemoveEmptyEntries)[0];
                        html = html.Replace(";", "");

                        _titlekeys[_curTid]["info"] = JObject.Parse(html);
                        var id = _titlekeys[_curTid]["info"]["id"];
                        //
                        var pricesjstr = web.DownloadString($"https://ec.nintendo.com/api/{g["region"]}/en/guest_prices?ns_uids={id}");

                        var p = JArray.Parse(pricesjstr);
                        _titlekeys[_curTid]["info"]["price"] =
                            p[0]["price"]["regular_price"]["currency"] + ":" +
                            p[0]["price"]["regular_price"]["formatted_value"];

                    }
                    catch
                    {
                        Invoke(new Action(() => { label_info.Text = "获取信息错误"; }));
                        return;
                    }
                }

            var info = _titlekeys[_curTid]["info"];
            var size = info["total_rom_size"].ToObject<long>();

            if (!GetGameImage())
            {

                pictureBox_gameicon.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox_gameicon.Image = Resources.error;
            }

            Invoke(new Action(() =>
            {
                var lans = "";
                foreach (var v in info["languages"])
                {
                    lans += v["name"] + " ";
                }
                label_info.Text = $"{info["price"]}\n{lans}";

                foreach (var pictureBox in _pictureBoxs)
                {
                    pictureBox.Visible = false;
                }


                var screenshots = _titlekeys[_curTid]["info"]["screenshots"].ToObject<JArray>();
                //算出长度


                flowLayoutPanel1.AutoScrollMinSize = new Size((pictureBox1.Width + 10) * screenshots.Count, 0);
                var index = 0;
                foreach (var screenshot in screenshots)
                {
                    // pictureBox1.ImageLocation = 
                    _pictureBoxs[index].ImageLocation = screenshot["images"][0]["url"].ToString();
                    _pictureBoxs[index].Visible = true;
                    index++;
                }

            }));


        }

        private bool GetGameImage()
        {
            var filename = "image\\" + _curTid + ".jpg";

            if (File.Exists(filename))
                try
                {//有时会有0kb的图片
                    pictureBox_gameicon.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox_gameicon.Image = Image.FromFile(filename);
                    return true;
                }
                catch
                {
                    // ignored
                }

            using (var web = new WebClient { Encoding = Encoding.UTF8 })
            {

                try
                {
                    var imageurl = _titlekeys[_curTid]["info"]["applications"][0]["image_url"].ToString();
                    web.DownloadFile(imageurl, filename);
                    pictureBox_gameicon.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox_gameicon.Image = Image.FromFile(filename);
                    return true;
                }
                catch
                {
                    if (File.Exists(filename))
                        File.Delete(filename);
                    return false;
                }


            }

        }

        /// <summary>
        ///     对文件大小进行转换
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string ConvertBytes(long len)
        {
            if (len > 1073741824)
                return (len / 1073741824.0).ToString("F") + "GB";
            if (len > 1048576)
                return (len / 1048576.0).ToString("F") + "MB";
            return (len / 1024.0).ToString("F") + "KB";
        }

        private void textBox_keyword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter) return;
            e.Handled = true; //防止向上冒泡
            SearchGame(textBox_keyword.Text);
        }

        private void 更新TitleId文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var t = new Thread(UpdateTitleKey);
            t.Start();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.91wii.com/thread-104797-1-1.html");
        }

        private void 查看帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/ningxiaoxiao/NSGameDownloader/wiki");
        }

        private void 发送反馈ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/ningxiaoxiao/NSGameDownloader/issues");
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/ningxiaoxiao/NSGameDownloader");
        }

        private void label_url_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetDataObject(label_url.Text, true);
                MessageBox.Show("已经复制到剪贴版上", "复制成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "复制失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void panWebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {


            Console.WriteLine("Navigating:" + e.Url);
        }

        private void panWebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            //GetCookies();
        }
    }
    public static class ControlExtensions
    {
        public static T Clone<T>(this T controlToClone)
            where T : Control
        {
            PropertyInfo[] controlProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            T instance = Activator.CreateInstance<T>();

            foreach (PropertyInfo propInfo in controlProperties)
            {
                if (propInfo.CanWrite)
                {
                    if (propInfo.Name != "WindowTarget")
                        propInfo.SetValue(instance, propInfo.GetValue(controlToClone, null), null);
                }
            }

            return instance;
        }
    }
}
