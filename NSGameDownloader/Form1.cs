using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
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
        private const string ConfigPath = "config.json";
        private const string TitleKeysPath = "keys.json";
        private const string CookiePath = "cookie.json";
        private const string ExcelPath = "db.xlsx";
        private const int EM_SETCUEBANNER = 0x1501;

        private string _curTid;

        // 只显示中文游戏
        private bool _onlyShowCn;

        // 显示已下载
        private bool _showDownloaded;

        /**
         * panUrl: 百度盘地址
         * nutDbUrl: nutDbUrl
         */
        private JObject _config;
        private JObject _titlekeys;
        private DataSet TitleDataSet;
        private GameListViewItemComparer listSorter;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(ConfigPath))
            {
                _config = JObject.Parse(File.ReadAllText(ConfigPath));
            } else {
                MessageBox.Show("无法访问config.json配置文件", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_config["localGameDir"].ToString().Length > 0 && Directory.Exists(_config["localGameDir"].ToString())) {
                check_box_download.Visible = true;
            }

           
            listSorter = new GameListViewItemComparer();

            var tl = new Thread(ThreadLoad);
            tl.Start();
        }

        /// <summary>
        ///     线程初始,不会占用启动时间
        /// </summary>
        private void ThreadLoad()
        {

            ReadCookieFile();

            if (!File.Exists(TitleKeysPath))
            {
                var t = new Thread(UpdateTitleKey);
                t.Start();
            }
            else
            {
                _titlekeys = JObject.Parse(File.ReadAllText(TitleKeysPath));
            }

            if (!Directory.Exists("image")) Directory.CreateDirectory("image");
            //使用winapi 做占位符
            Invoke(new Action(() => {
                SendMessage(textBox_keyword.Handle, EM_SETCUEBANNER, 0, "在这里输入 id,中文名,英文名 关键字...");
            }));

            SearchGameName();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(TitleKeysPath, _titlekeys.ToString());

            WriteCookieFile();
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
                toolStripProgressBar_download.Maximum = 5;
                toolStripProgressBar_download.Value = 1;
                button_search.Text = "下载中";
                button_search.Enabled = false;
                checkbox_cn.Enabled = false;
                textBox_keyword.Enabled = false;
                label_progress.Visible = true;
                label_progress.Text = "下载db.xlsx...";
            }));


            _titlekeys = new JObject();

            try
            {
                DownloadExcel();
                Invoke(new Action(() =>
                {
                    toolStripProgressBar_download.Value = 2;
                    label_progress.Text = "解析db.xlsx...";
                }));

                ReadExcel();
                Invoke(new Action(() =>
                {
                    toolStripProgressBar_download.Value = 3;
                    label_progress.Text = "加载nutdb...";
                }));

                ReadNutDb();
                Invoke(new Action(() =>
                {
                    toolStripProgressBar_download.Value = 5;
                }));

                File.WriteAllText(TitleKeysPath, _titlekeys.ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "更新titleid出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Invoke(new Action(() =>
            {
                button_search.Text = "搜索";
                button_search.Enabled = true;
                checkbox_cn.Enabled = true;
                textBox_keyword.Enabled = true;
                toolStripProgressBar_download.Visible = false;
                label_progress.Visible = false;
                SearchGameName();
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

                    Console.WriteLine("start download excel ...");
                    http.DownloadFile(_config["excelDbUrl"].ToString(), ExcelPath);
                    Console.WriteLine("download excel success!");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("无法访问github,无法更新最新网盘地址.请检查网络", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void ReadExcel()
        {
            if (!File.Exists(ExcelPath))
            {
                MessageBox.Show("无法访问db.xlsx,请将db.xlsx放在程序根目录", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Console.WriteLine("start read excel ...");
            var fs = File.Open(ExcelPath, FileMode.Open, FileAccess.Read);
            var er = ExcelReaderFactory.CreateReader(fs);

            TitleDataSet = er.AsDataSet(new ExcelDataSetConfiguration
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
            var dt = TitleDataSet.Tables[0];
            foreach (DataRow row in dt.Rows)
            {
                var tid = row[0].ToString();
                var iszh = row[1].ToString() != "×";
                var cname = row[2].ToString();
                var allnames = row[3].ToString();
                var haveXci = row[4].ToString().Contains("XCI");
                var haveNsp = row[4].ToString().Contains("NSP");
                var haveUpd = !row[5].ToString().StartsWith("v0");
                var haveDlc = row[6].ToString() != "×";


                var jtemp = new JObject
                {
                    ["tid"] = tid,
                    ["iszh"] = iszh,
                    ["cname"] = cname,
                    ["allnames"] = allnames,
                    ["xci"] = haveXci,
                    ["nsp"] = haveNsp,
                    ["dlc"] = haveDlc || haveUpd,
                    ["ver"] = row[5].ToString(),
                    ["region"] = cname.Contains("（美版）") ? "US" : (cname.Contains("（日版）") ? "JP" : "US"),
                    ["ename"] = "",

                };
                _titlekeys[tid] = jtemp;
            }

            Console.WriteLine("read excel success!");
            fs.Close();
        }

        private void ReadNutDb()
        {
            var http = new WebClient { Encoding = Encoding.UTF8 };

            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Console.WriteLine("start download nutdb ...");
            var html = http.DownloadString(_config["nutDbUrl"].ToString());
            var keys = new List<string>(html.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries));
            if (keys.Count == 0) throw new Exception("没有得到数据");

            Invoke(new Action(() =>
            {
                toolStripProgressBar_download.Value = 4;
            }));

            Console.WriteLine("download nutdb success!");
            //前3行 不要
            keys.RemoveAt(0);
            keys.RemoveAt(0);
            keys.RemoveAt(0);

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

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var oUrl = WebUtility.UrlDecode(e.Url.ToString());
            label_url.Text = oUrl;

            if (panWebBrowser.Document.Body == null) return;
            UpdateCookies(radioButton_nsp.Checked ? "nsp_cookie" : radioButton_xci.Checked ? "xci_cookie" : "upd_dlc_cookie");
        }


        private JObject _cookies = new JObject();

        private void ReadCookieFile()
        {
            if (File.Exists(CookiePath))
                _cookies = JObject.Parse(File.ReadAllText(CookiePath));
            else
                _cookies = new JObject();
        }

        private void WriteCookieFile()
        {
            Console.WriteLine("save cookie to file");
            File.WriteAllText(CookiePath, JsonConvert.SerializeObject(_cookies));
        }


        private void UpdateCookies(String cookieType)
        {
            var str = panWebBrowser.Document.Cookie.Replace(",", "%2C");
            var cookies = str.Split(';');
            foreach (var c in cookies)
            {
                var kv = c.Split('=');
                if (kv[0].Trim().Equals("BDCLND")) {
                    _cookies[cookieType] = kv.Length == 0 ? "" : kv[1].Trim();
                    Console.WriteLine("update cookie,"+ cookieType + ":" + (kv.Length == 0 ? "" : kv[1].Trim()));
                }
            }
        }

        private void WebRefresh()
        {
            if (_curTid == null) return;
            var url = GetPanUrl(_curTid);
            Console.WriteLine("打开:" + url);

            String cookie = "";
            JToken jt = _cookies[radioButton_nsp.Checked ? "nsp_cookie" : radioButton_xci.Checked ? "xci_cookie" : "upd_dlc_cookie"];
            if (jt != null) {
                cookie = jt.ToString();
            }
            //使用cookie方法免写密码,只有手动时 才更新cookie
            InternetSetCookie("https://pan.baidu.com/", "BDCLND", cookie);
            Console.WriteLine("set cookie BDCLND:" + cookie);

            //panWebBrowser.Navigate (url, "_self" , null, "User-Agent: Mozilla/5.0 (iPhone; CPU iPhone OS 12_1_4 like Mac OS X) AppleWebKit/605.1.15 (KHTMsL, like Gecko) Version/12.0 Mobile/15E148 Safari/604.1");
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
            //var keys = Titlekeys.Root.Where(x => x.Contains(textBox_keyword.Text.Trim()));
            Console.WriteLine("start serach:" + textBox_keyword.Text);
            SearchGameName(textBox_keyword.Text);
        }

        private void SearchGameName(string keywords = "")
        {
            List<String> localGames = new List<String>();
            if (!_showDownloaded)
            {
                if (_titlekeys == null) return;
                if (_titlekeys.Count == 0) return;
            }
            else
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(_config["localGameDir"].ToString());
                    DirectoryInfo[] directories = dir.GetDirectories();
                    foreach (DirectoryInfo d in directories) {
                        if (d.Name.Length == 5 && d.Name.StartsWith("01")) {
                            DirectoryInfo[] subDirectories = d.GetDirectories();
                            foreach (DirectoryInfo subD in subDirectories)
                            {
                                //01009A700A538000
                                if (subD.Name.Length == 16)
                                {
                                    localGames.Add(subD.Name);
                                }
                            }
                        }
                    }

                    Console.WriteLine("load local games count:" + localGames.Count);
                }
                catch(Exception e)
                {
                    Console.WriteLine("load local games error:" + e);
                    return;
                }
            }
            

            Invoke(new Action(() =>
            {
                listView1.Items.Clear();
                foreach (var titlekey in _titlekeys)
                {
                    //全文件查找
                    var allstr = titlekey.Value["tid"].ToString() + titlekey.Value["cname"] + titlekey.Value["ename"] + titlekey.Value["allnames"];

                    if (_onlyShowCn && !titlekey.Value["iszh"].ToObject<bool>()) {
                        continue;
                    }

                    if (allstr.ToLower().Contains(keywords.Trim().ToLower())) {
                        if (_showDownloaded && !localGames.Contains(titlekey.Value["tid"].ToString()))
                        {
                            continue;
                        }

                        listView1.Items.Add(new ListViewItem(new[]
                        {
                            titlekey.Value["tid"].ToString(),
                            titlekey.Value["cname"].ToString().Trim() == ""
                                ? titlekey.Value["allnames"].ToString()
                                : titlekey.Value["cname"].ToString(), //防止没有中文名
                            titlekey.Value["nsp"].ToObject<bool>() ? "●" : "",
                            titlekey.Value["xci"].ToObject<bool>() ? "●" : "",
                            titlekey.Value["dlc"].ToObject<bool>() ? "●" : "",
                            titlekey.Value["iszh"].ToObject<bool>() ? "●" : ""
                        }));
                    }
                        
                }

                label_count.Text = "count:" + listView1.Items.Count;

                if (listView1.Items.Count > 0)
                {
                    listView1.SelectedItems.Clear();
                    listView1.Items[0].Selected = true;//设定选中
                    listView1.Items[0].Focused = true;
                    listView1.Select();
                }
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

            if (_config["localGameDir"].ToString().Length > 0) {
                localDirLabel.Visible = true;
                var dir = _config["localGameDir"].ToString() + "\\" + _curTid.Substring(0, 5) + "\\" + _curTid + "\\";
                if (Directory.Exists(dir))
                {
                    localDirLabel.Text = dir;
                }
                else
                {
                    localDirLabel.Text = "创建目录";
                }
            }


            radioButton_upd.Enabled = _titlekeys[_curTid]["dlc"].ToObject<bool>();
            radioButton_nsp.Enabled = _titlekeys[_curTid]["nsp"].ToObject<bool>();
            radioButton_xci.Enabled = _titlekeys[_curTid]["xci"].ToObject<bool>();

            if (!radioButton_upd.Enabled && radioButton_upd.Checked) radioButton_nsp.Checked = radioButton_nsp.Enabled;
            if (!radioButton_nsp.Enabled && radioButton_nsp.Checked) radioButton_xci.Checked = radioButton_xci.Enabled;
            if (!radioButton_xci.Enabled && radioButton_xci.Checked) radioButton_nsp.Checked = radioButton_nsp.Enabled;

            if (!radioButton_nsp.Checked && !radioButton_xci.Checked && !radioButton_upd.Checked)
            {
                panWebBrowser.Url = null;
            }
            else
            {
                WebRefresh();
            }

            //刷新来自eshop的信息
            var t = new Thread(GetGameInfoFromEShop);
            t.Start();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView listview = (ListView)sender;
            ListViewItem lstrow = listview.GetItemAt(e.X, e.Y);
            ListViewItem.ListViewSubItem lstcol = lstrow.GetSubItemAt(e.X, e.Y);
            string strText = lstcol.Text;
            try
            {
                Clipboard.SetDataObject(strText);
                string info = string.Format("内容【{0}】已经复制到剪贴板", strText);
                Console.WriteLine(info);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            Console.WriteLine("ColumnClick:" + e.Column);
            // 检查点击的列是不是现在的排序列.
            if (e.Column == listSorter.sortedColumn)
            {
                // 重新设置此列的排序方法.
                if (listSorter.sortOrder == SortOrder.Ascending)
                {
                    listSorter.sortOrder = SortOrder.Descending;
                }
                else
                {
                    listSorter.sortOrder = SortOrder.Ascending;
                }
            }
            else
            {
                // 设置排序列，默认为正向排序
                listSorter.sortedColumn = e.Column;
                listSorter.sortOrder = SortOrder.Ascending;
            }

            listView1.ListViewItemSorter = listSorter;
            // 用新的排序方法对ListView排序
            listView1.Sort();
        }

        private void GetGameInfoFromEShop()
        {
            var backUpTid = _curTid;
            Invoke(new Action(() => {
                if (backUpTid == _curTid) {
                    pictureBox_gameicon.Image = Resources.load;
                    info_label_name.Text = "游戏名：...";
                    label_info.Text = "";
                    label_info_size.Text = "大小：...";
                    info_label_publisher.Text = "发行商：...";
                    label_info_launch_date.Text = "发布日期：...";
                    label_info_support_lan.Text = "支持语言：...";
                    label_info_type.Text = "类型：...";
                }
            }));

            //todo 从http://www.eshop-switch.com 拿数据
            var g = _titlekeys[backUpTid].ToObject<JObject>();
            if (!g.ContainsKey("info"))
                using (var web = new WebClient { Encoding = Encoding.UTF8 })
                {
                    try
                    {
                        var url = $"https://ec.nintendo.com/apps/{backUpTid}/{g["region"]}";
                        Console.WriteLine("load game info:" + url);
                        var html = web.DownloadString(url);

                        html = html.Split(new[] { "NXSTORE.titleDetail.jsonData = " }, StringSplitOptions.RemoveEmptyEntries)[1];
                        html = html.Split(new[] { "NXSTORE.titleDetail" }, StringSplitOptions.RemoveEmptyEntries)[0];
                        html = html.Replace(";", "");

                        _titlekeys[backUpTid]["info"] = JObject.Parse(html);
                    }
                    catch
                    {
                        Invoke(new Action(() => {
                            if (backUpTid == _curTid)
                            {
                                info_label_name.Text = "游戏名：--";
                                label_info.Text = "获取简介信息错误";
                                label_info_size.Text = "大小：--";
                                info_label_publisher.Text = "发行商：--";
                                label_info_launch_date.Text = "发布日期：--";
                                label_info_support_lan.Text = "支持语言：--";
                                label_info_type.Text = "类型：--";
                                pictureBox_gameicon.Image = Resources.error;
                            }
                        }));
                        return;
                    }
                }

            var info = _titlekeys[backUpTid]["info"];
            var size = info["total_rom_size"].ToObject<long>();
            var launch_date = info["release_date_on_eshop"].ToString();
            var description = info["description"].ToString();
            var lans = info["languages"].ToArray();
            var lan_str = "";
            foreach (var lan in lans) {
                lan_str += lan["name"].ToString();
                lan_str += " ";
            }

            Invoke(new Action(() => {
                if (backUpTid == _curTid)
                {
                    info_label_name.Text = "游戏名：" + info["formal_name"].ToString();
                    info_label_publisher.Text = "发行商：" + info["publisher"]["name"].ToString();
                    label_info_size.Text = $"大小：{ConvertBytes(size)}";
                    label_info_launch_date.Text = $"发布日期：{launch_date}";
                    label_info_support_lan.Text = $"支持语言：{lan_str}";
                    label_info.Text = $"{info["description"]}";
                    label_info_type.Text = "类型：" + info["genre"].ToString();
                }
            }));

            if (backUpTid == _curTid)
            {
                if (info["applications"][0]["image_url"] != null)
                {
                    GetGameImage(backUpTid, info["applications"][0]["image_url"].ToString());
                }
                else
                {
                    pictureBox_gameicon.Image = Resources.error;
                }
            }
        }

        private void GetGameImage(String tid, String url)
        {
            var filename = "image\\" + tid + ".jpg";
            if (File.Exists(filename))
                try
                {
                    Invoke(new Action(() => {
                        if (tid == _curTid)
                            pictureBox_gameicon.Image = Image.FromFile(filename);
                    }));
                    
                    return;
                } catch
                {
                    if (tid == _curTid)
                        File.Delete(filename);
                }

            var web = new WebClient { Encoding = Encoding.UTF8 };
            // 解决WebClient不能通过https下载内容问题
            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                web.DownloadFile(url, filename);
                Invoke(new Action(() => {
                    if (tid == _curTid)
                        pictureBox_gameicon.Image = Image.FromFile(filename);
                }));
                
            }
            catch
            {
                Invoke(new Action(() => {
                    if (tid == _curTid)
                        pictureBox_gameicon.Image = Resources.error;
                }));
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
            SearchGameName(textBox_keyword.Text);
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

        private void checkbox_cn_CheckedChanged(object sender, EventArgs e)
        {
            _onlyShowCn = ((CheckBox)sender).Checked;
            textBox_keyword.Text = "";
            SearchGameName();
        }

        private void check_box_download_CheckedChanged(object sender, EventArgs e)
        {
            _showDownloaded = ((CheckBox)sender).Checked;
            textBox_keyword.Text = "";
            SearchGameName();
        }

        private void menu_update_game_Click(object sender, EventArgs e)
        {
            var t = new Thread(UpdateTitleKey);
            t.Start();
        }

        private void 查看帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/freedom10086/NSGameDownloader/wiki");
        }

        private void 发送反馈ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/freedom10086/NSGameDownloader/issues");
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/freedom10086/NSGameDownloader");
        }

        private void localDirLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_curTid == null) return;
            var dir = _config["localGameDir"].ToString() + "\\" + _curTid.Substring(0, 5) + "\\" + _curTid + "\\";
            if (Directory.Exists(dir))
            {
                Process.Start("Explorer.exe", dir);
            }
            else
            {
                Directory.CreateDirectory(dir);
                Process.Start("Explorer.exe", dir);
                localDirLabel.Text = dir;
            }
        }

        private void ToolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.91wii.com/space-uid-2358313.html");
        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/ningxiaoxiao/NSGameDownloader");
        }

        private void pictureBox_gameicon_Click(object sender, EventArgs e)
        {
            if (_curTid == null) return;
            var g = _titlekeys[_curTid].ToObject<JObject>();
            if (!g.ContainsKey("info")) { return; }
            Process.Start($"https://ec.nintendo.com/apps/{_curTid}/{g["region"]}");
        }
    }
}