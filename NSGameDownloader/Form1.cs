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

            btnDownload.Enabled = false;

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


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam,
            [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        

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
                check_box_download.Enabled = false;
                textBox_keyword.Enabled = false;
                btnDownload.Enabled = false;
                label_progress.Visible = true;
                label_progress.Text = "下载db.xlsx...";
            }));


            _titlekeys = new JObject();

            try
            {
                if (!DownloadExcel()) {
                    Invoke(new Action(() =>
                    {
                        toolStripProgressBar_download.Visible = false;
                        label_progress.Visible = false;
                    }));
                    return;
                }

                Invoke(new Action(() =>
                {
                    toolStripProgressBar_download.Value = 2;
                    label_progress.Text = "解析db.xlsx...";
                }));

                ReadExcel();
                Invoke(new Action(() =>
                {
                    toolStripProgressBar_download.Value = 3;
                    label_progress.Text = "下载tiledb...";
                }));

                //ReadNutDb();
                if (!ReadNutTileDb("US.en")) {
                    Invoke(new Action(() =>
                    {
                        toolStripProgressBar_download.Visible = false;
                        label_progress.Visible = false;
                    }));
                    return;
                }
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
                check_box_download.Enabled = true;
                textBox_keyword.Enabled = true;
                toolStripProgressBar_download.Visible = false;
                label_progress.Visible = false;
                SearchGameName();
            }));
        }

        private bool DownloadExcel()
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

                    return true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("无法访问github,无法更新最新网盘地址.请检查网络", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (File.Exists("ExcelPath")) {
                    File.Delete("ExcelPath");
                }
                return false;
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
                    ["version"] = row[5].ToString(),
                    ["region"] = cname.Contains("（美版）") ? "US" : (cname.Contains("（日版）") ? "JP" : "US"),
                    ["releaseDate"] = "",

                };
                _titlekeys[tid] = jtemp;
            }

            Console.WriteLine("read excel success!");
            fs.Close();
        }

        // https://github.com/blawar/nut/tree/master/titledb
        private bool ReadNutTileDb(string region)
        {
            var http = new WebClient { Encoding = Encoding.UTF8 };
            ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Console.WriteLine("start download nuttiledb ...");

            String htmlJson;
            try
            {
                htmlJson = http.DownloadString("https://raw.githubusercontent.com/blawar/nut/master/titledb/US.en.json");
                Console.WriteLine("download nuttiledb success ...");
            }
            catch
            {
                MessageBox.Show("无法访问加载tiledb.请检查网络", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            Invoke(new Action(() =>
            {
                toolStripProgressBar_download.Value = 4;
                label_progress.Text = "解析tiledb...";
            }));
        
            Dictionary<string, JObject> dics;
            try
            {
                Console.WriteLine("start parse nuttiledb ...");
                dics = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(htmlJson);
                Console.WriteLine("parse nuttiledb success!");
            }
            catch
            {
                MessageBox.Show("tiledb解析错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            

            foreach (KeyValuePair<string, JObject> kvp in dics) {
              
                var tid = kvp.Value["id"];
                if (tid == null) continue;
                var isDemo = kvp.Value["isDemo"] == null ? true : kvp.Value["isDemo"].ToObject<bool>();
                if (isDemo) continue;

                var tidStr = tid.ToString();
                if (_titlekeys.ContainsKey(tid.ToString())) //只会得到本体
                {
                    _titlekeys[tidStr]["name"] = kvp.Value["name"];
                    _titlekeys[tidStr]["releaseDate"] = kvp.Value["releaseDate"];
                    _titlekeys[tidStr]["iconUrl"] = kvp.Value["iconUrl"];
                    _titlekeys[tidStr]["bannerUrl"] = kvp.Value["bannerUrl"];
                    _titlekeys[tidStr]["intro"] = kvp.Value["intro"];
                    _titlekeys[tidStr]["description"] = kvp.Value["description"];
                    _titlekeys[tidStr]["languages"] = kvp.Value["languages"];
                    _titlekeys[tidStr]["categorys"] = kvp.Value["category"];
                    _titlekeys[tidStr]["size"] = kvp.Value["size"];
                    _titlekeys[tidStr]["publisher"] = kvp.Value["publisher"];
                    _titlekeys[tidStr]["region"] = kvp.Value["region"];
                    _titlekeys[tidStr]["version"] = kvp.Value["version"];
                }
            }

            Console.WriteLine("parse nuttiledb success! count:" + _titlekeys.Count);

            return true;
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
                    var allstr = titlekey.Value["tid"].ToString()
                    + "#" + titlekey.Value["cname"]
                    + (titlekey.Value["name"] != null ? ("#" + titlekey.Value["name"].ToString()) : "");

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
                            titlekey.Value["cname"].ToString().Trim() != "" ? titlekey.Value["cname"].ToString().Trim() : (titlekey.Value["name"] != null ? titlekey.Value["name"].ToString() : titlekey.Value["allnames"].ToString()), //防止没有中文名
                            titlekey.Value["nsp"].ToObject<bool>() ? "●" : "",
                            titlekey.Value["xci"].ToObject<bool>() ? "●" : "",
                            titlekey.Value["dlc"].ToObject<bool>() ? "●" : "",
                            titlekey.Value["iszh"].ToObject<bool>() ? "●" : "",
                            titlekey.Value["publisher"]!= null ? titlekey.Value["publisher"].ToString() : "",
                            titlekey.Value["releaseDate"]!= null ? titlekey.Value["releaseDate"].ToString() : ""
                        }));
                    }
                        
                }

                label_count.Text = "总数：" + listView1.Items.Count;

                // 补全主动搜索
                // 01006C900CC60000
                if (listView1.Items.Count == 0 && keywords.Length == 16 && keywords.EndsWith("000")) {
                    listView1.Items.Add(new ListViewItem(new[]
                        {
                            keywords,
                            keywords, //防止没有中文名
                            "●",
                            "●",
                            "●",
                            "●",
                            ""
                        }));
                }

                if (listView1.Items.Count > 0)
                {
                    listView1.SelectedItems.Clear();
                    listView1.Items[0].Selected = true;//设定选中
                    listView1.Items[0].Focused = true;
                    listView1.Select();
                }
            }));
        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                btnDownload.Enabled = false;
                return;
            }
            else
            {
                btnDownload.Enabled = true;
            }

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

         
            //刷新来自eshop的信息
            //var t = new Thread(GetGameInfoFromEShop);
            //t.Start();

            if (!_titlekeys.ContainsKey(_curTid)) {
                info_label_name.Text = _curTid;
                info_label_publisher.Text = "发行商：--";
                label_info_size.Text = $"大小：--";
                label_info_support_lan.Text = $"支持语言：--";
                label_info_type.Text = "类型：--";
                label_info.Text = "";
                pictureBox_gameicon.Image = Resources.error;

                return;
            }

            info_label_name.Text = _titlekeys[_curTid]["cname"].ToString().Trim() != "" ? _titlekeys[_curTid]["cname"].ToString().Trim() : (_titlekeys[_curTid]["name"] != null ? _titlekeys[_curTid]["name"].ToString() : _titlekeys[_curTid]["allnames"].ToString());
            info_label_publisher.Text = "发行商：" + (_titlekeys[_curTid]["publisher"] != null ? _titlekeys[_curTid]["publisher"].ToString() : "");
            label_info_size.Text = $"大小：{ConvertBytes(_titlekeys[_curTid]["size"] !=null ? _titlekeys[_curTid]["size"].ToObject<long>() : 0)}";
            
            var lan_str = "";
            if (_titlekeys[_curTid]["languages"] != null) {
                var lans = _titlekeys[_curTid]["languages"].ToArray();
                foreach (var lan in lans)
                {
                    lan_str += lan.ToString();
                    lan_str += " ";
                }

            }

            label_info_support_lan.Text = $"支持语言：{lan_str}";


            var categorys_str = "";
            if (_titlekeys[_curTid]["categorys"] != null) {
                var categorys = _titlekeys[_curTid]["categorys"].ToArray();
                foreach (var cat in categorys)
                {
                    categorys_str += cat.ToString();
                    categorys_str += " ";
                }
            }
            
            label_info_type.Text = "类型：" + categorys_str;

            label_info.Text = $"{(_titlekeys[_curTid]!= null ? _titlekeys[_curTid]["description"] : "")}";

            if (_titlekeys[_curTid]["iconUrl"] != null)
            {
                var bannerUrl = _titlekeys[_curTid]["bannerUrl"];
                var bannerUrlStr = "";
                if (bannerUrl != null) {
                    bannerUrlStr = bannerUrl.ToString();
                }
                GetGameImage(_curTid, _titlekeys[_curTid]["iconUrl"].ToString(), bannerUrlStr);
            }
            else
            {
                pictureBox_gameicon.Image = Resources.error;
            }
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

        private void GetGameImage(String tid, String url, String bannerUrl)
        {
            var filename = "image\\" + tid + ".jpg";
            if (File.Exists(filename)) {
                try
                {
                    pictureBox_gameicon.Image = Image.FromFile(filename);
                }
                catch
                {
                    File.Delete(filename);
                }
            }

            if (!File.Exists(filename)) {
                var web = new WebClient { Encoding = Encoding.UTF8 };
                // 解决WebClient不能通过https下载内容问题
                ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                try
                {
                    web.DownloadFileCompleted += Icon_DownloadFileCompleted;
                    web.DownloadFileAsync(new Uri(url), filename, tid);
                    //web.DownloadFile(url, filename);
                    Invoke(new Action(() => {
                        pictureBox_gameicon.Image = Resources.load;
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
        }

        void Icon_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.UserState != null)
            {
                string tid = e.UserState.ToString();
                if (tid == _curTid)
                {
                    var filename = "image\\" + tid + ".jpg";
                    pictureBox_gameicon.Image = Image.FromFile(filename);
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
            SearchGameName(textBox_keyword.Text);
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
            var region = g["region"];
            if (region == null || region.ToString().Equals("US") || region.ToString() == "") {
                region = "AU";
            }

            Process.Start($"https://ec.nintendo.com/apps/{_curTid}/{region}");
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(_curTid, info_label_name.Text, _cookies, _config);
            form2.Text = info_label_name.Text;
            form2.Show();
        }
    }
}