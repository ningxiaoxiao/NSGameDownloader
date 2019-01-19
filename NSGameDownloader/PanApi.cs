using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using mshtml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NSGameDownloader
{
    public static class PanApi
    {
        private static WebBrowser _webbrowser;
        private static int pageno = 1;
        private static HttpClient _client;
        private static CookieContainer _cookies = new CookieContainer();
        private const int INTERNET_COOKIE_THIRD_PARTY = 0x10;
        private const int INTERNET_COOKIE_HTTPONLY = 0x00002000;
        private const int INTERNET_FLAG_RESTRICTED_ZONE = 0x00020000;


        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetGetCookieEx(
            string url,
            string cookieName,
            StringBuilder cookieData,
            ref int size,
            int flags,
            IntPtr pReserved);

        /// <summary>
        /// 从_webbrowser中得到cookie给client使用
        /// </summary>
        private static void GetCookies()
        {
            var str = _webbrowser.Document.Cookie;
            Console.WriteLine(str);
            SetCookies(str);
        }

        private static void SetCookies(string str)
        {
            var cookies = str.Split(';');
            foreach (var c in cookies)
            {
                var kv = c.Split('=');
                if (kv.Length > 2)
                {
                    kv[1] += "=" + kv[2];
                }
                var cookie = new Cookie(kv[0].Trim(), kv.Length == 0 ? "" : HttpUtility.UrlEncode(kv[1].Trim()));
                _cookies.Add(new Uri("https://pan.baidu.com/"), cookie);
            }
        }
        public static void Create(WebBrowser web)
        {

            _webbrowser = web;
            pageno = 1;
            _webbrowser.DocumentCompleted += _webbrowser_DocumentCompleted;
        }

        private static void _webbrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            GetCookies();
            AddJs();
        }


        public static void Download(string code = null)
        {

            if (!isLogin())
            {
                Console.WriteLine("没有登录");
                return;
            }
            if (_webbrowser.Document == null) return;
            getExtra();
            GetYunData();
            var list = GetList(_webbrowser.Url);
            FileList = new List<panFile>();
            var j = JObject.Parse(list);

            foreach (var f in j["list"])
            {
                FileList.Add(new panFile()
                {
                    path = f["path"].ToString(),
                    fid = f["fs_id"].ToObject<Int64>(),
                    isdir = f["isdir"].ToString(),

                });
            }

            var glinks = GetGlink(code);

            Console.WriteLine(list);
            Console.WriteLine(glinks);
            var glinksj = JObject.Parse(glinks);
            if (glinksj["errno"].ToString() == "-20")
            {
                Form1.Inst.GetCode(getCode());
            }
            else
            {
                foreach (var jf in glinksj["list"])
                {
                    var file = FileList.FirstOrDefault(f => f.fid == jf["fs_id"].ToObject<Int64>());
                    file.glink = jf["dlink"].ToString();
                    Console.WriteLine(file.glink);
                    retryLimit = 10;
                    GetHlink(file.glink);
                }
            }

        }

        private static int retryLimit = 10;
        private static string GetHlink(string glink)
        {


            using (var c = new HttpClient(new HttpClientHandler() { UseCookies = false, }))
            {



                c.DefaultRequestHeaders.Add("Cookie", HttpUtility.UrlDecode(_cookies.GetCookieHeader(new Uri("https://pan.baidu.com/"))));
                var req = new HttpRequestMessage(HttpMethod.Head, glink);

                var res = c.SendAsync(req, HttpCompletionOption.ResponseHeadersRead).Result;
                if (res.IsSuccessStatusCode)
                {
                    var url = res.RequestMessage.RequestUri.ToString();
                    Console.WriteLine(url);
                    var md5 = Convert.ToBase64String(res.Content.Headers.ContentMD5);

                    return url;
                }
                else
                {
                    //todo 线程
                    if (retryLimit <= 0)
                        return null;
                    Thread.Sleep(500);
                    Console.WriteLine("重试:" + retryLimit);
                    retryLimit--;
                    GetHlink(glink);


                }

                return null;
            }
        }


        private static string getCode()
        {


            using (var c = new HttpClient(new HttpClientHandler() { UseCookies = false }))
            {

                var url = "https://pan.baidu.com/api/getvcode?prod=pan";

                c.DefaultRequestHeaders.Add("Cookie", HttpUtility.UrlDecode(_cookies.GetCookieHeader(new Uri(url))));


                var html = c.GetStringAsync(url).Result;
                var j = JObject.Parse(html);
                vcode = j["vcode"].ToString();
                return j["img"].ToString();
            }
        }

        private static void AddJs()
        {
            if (_webbrowser.Document == null) return;

            var head = _webbrowser.Document.GetElementsByTagName("head")[0];
            var scriptEl = _webbrowser.Document.CreateElement("script");
            var element = scriptEl.DomElement as IHTMLScriptElement;
            element.text = File.ReadAllText("pan.js");

            head.AppendChild(scriptEl);
            _webbrowser.Document.InvokeScript("init");
        }

        private static bool isLogin()
        {

            var size = 256 * 1024;
            // create buffer of correct size
            var sb = new StringBuilder(size);
            if (!InternetGetCookieEx("https://pan.baidu.com/", null, sb, ref size, INTERNET_COOKIE_HTTPONLY,
                IntPtr.Zero))
            {
                if (size < 0)
                {
                    return false;
                }
                sb = new StringBuilder(size);

                if (!InternetGetCookieEx("https://pan.baidu.com/", null, sb, ref size, INTERNET_COOKIE_HTTPONLY,
                    IntPtr.Zero))
                {
                    return false;
                }
            }

            // get cookie
            var bduss = sb.ToString();

            if (string.IsNullOrEmpty(bduss))
                return false;

            SetCookies(bduss);



            return bduss.Contains("BDUSS");
        }

        private static string GetList(Uri uri)
        {

            //     //
            // var weburl =
            //    $"https://pan.baidu.com/share/list?uk={_yunData["uk"]}&shareid={_yunData["shareid"]}&order=other&desc=1&showempty=0&web=1&page=1&num=100&dir={HttpUtility.UrlEncode(getURLParameter("path"))}&t=0.6157144403518271&channel=chunlei&web=1&app_id=250528&clienttype=0";

            var listurl =
                $"https://pan.baidu.com/share/list?uk={_yunData["uk"]}&shareid={_yunData["shareid"]}&dir={HttpUtility.UrlEncode(getURLParameter("path"))}&num=100&order=time&desc=1&clienttype=0&showempty=0&web=1&page={pageno}";

            //url: 'https://pan.baidu.com/share/list?uk='+self.yunData.uk+"&shareid="+self.yunData.shareid+'&dir='+getURLParameter(self.url, 'path')+"&bdstoken="+self.yunData.bdstoken+"&num=100&order=time&desc=1&clienttype=0&showempty=0&web=1&page="+self.pageno,

            using (var c = new HttpClient(new HttpClientHandler() { UseCookies = false }))
            {


                var req = new HttpRequestMessage(HttpMethod.Get, listurl);
                req.Headers.Add("Cookie", HttpUtility.UrlDecode(_cookies.GetCookieHeader(new Uri(listurl))));
                var res = c.SendAsync(req).Result;
                var html = res.Content.ReadAsStringAsync().Result;
                Console.WriteLine(res.Headers);

                return html;
            }

            // var r = _webbrowser.Document.InvokeScript("getList", new[] { listurl });

            // return r?.ToString();

        }

        private static string extra = "";

        private static void getExtra()
        {
            var temp = HttpUtility.UrlDecode(_cookies.GetCookies(new Uri("https://pan.baidu.com/"))["BDCLND"]?.Value);
            temp = HttpUtility.UrlDecode(temp);

            var j = new JObject()
            {
                ["sekey"] = temp,
            };

            extra = j.ToString(Formatting.None);
        }
        private static string GetGlink(string code = null)
        {
            // var url = "http://pan.baidu.com/api/sharedownload?sign=" + self.yunData.sign + "&timestamp=" + self.yunData.timestamp + "&bdstoken=" + self.yunData.bdstoken + "&channel=chunlei&web=1&app_id=250528&clienttype=0";
            var url = $"https://pan.baidu.com/api/sharedownload?sign={_yunData["sign"]}&timestamp={_yunData["timestamp"]}&bdstoken={_yunData["bdstoken"]}&channel=chunlei&web=1&app_id=250528&clienttype=0";
            //

            var fsidList = new JArray();

            foreach (var file in FileList)
            {
                fsidList.Add(file.fid);
            }


            var data = $"encrypt=0&product=share&uk={_yunData["uk"]}&primaryid=" + _yunData["shareid"];
            data += "&fid_list=" + fsidList.ToString(Formatting.None);
            data += "&extra=" + HttpUtility.UrlEncode(extra);
            data += "&type=nolimit";
            if (code != null)
            {
                data += "&vcode_str=" + vcode + "&vcode_input=" + code;
            }

            var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");

            using (var c = new HttpClient(new HttpClientHandler() { UseCookies = false }))
            {

                // req.Headers.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                // req.Headers.Add("Accept-Encoding", "gzip, deflate");//Accept-Language: zh-CN,zh;q=0.9
                //req.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9");//Accept-Language: 

                req.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36");
                req.Headers.Add("Cookie", HttpUtility.UrlDecode(_cookies.GetCookieHeader(new Uri(url))));
                var html = c.SendAsync(req).Result.Content.ReadAsStringAsync().Result;

                return html;
            }

        }


        private static JObject _yunData;
        public static void GetYunData()
        {
            var html = _webbrowser.DocumentText;
            var m = Regex.Match(html, @"yunData.setData\((.*)\)");

            var jtext = m.Groups[1].Value;
            _yunData = JObject.Parse(jtext);


        }

        private static string getURLParameter(string name)
        {
            //https://pan.baidu.com/s/17mnLTOuwvpacOrDY4GJjCQ#list/path=%2F%E8%B6%85%E7%BA%A7%E9%A9%AC%E9%87%8C%E5%A5%A5&parentPath=%2F
            var url = HttpUtility.UrlDecode(_webbrowser.Url.AbsoluteUri);
            var array = url.Split(new[] { "#list/" }, StringSplitOptions.RemoveEmptyEntries)[1];

            var ps = array.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in ps)
            {
                var kv = s.Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (kv[0] == name) return kv[1];
            }

            return null;
        }

        private static List<panFile> FileList;
        private static string vcode;
    }

    public class panFile
    {
        public string path { get; set; }
        public Int64 fid { get; set; }
        public string isdir { get; set; }
        public string md5 { get; set; }
        public string size { get; set; }
        public string glink { get; set; }
        public string hlinks { get; set; }

        public string name
        {
            get
            {
                string[] tmp = path.Split('/');
                return tmp[tmp.Length - 1];
            }
        }
    }
}