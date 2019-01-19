using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using mshtml;
using Newtonsoft.Json.Linq;

namespace NSGameDownloader
{
    public static class PanApi
    {
        private static WebBrowser _webbrowser;
        private static int pageno = 1;
        private static HttpClient _client;
        private static CookieContainer _cookies = new CookieContainer();

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetCookie(string lpszUrl, string lpszCookieName,
            StringBuilder lpszCookieData, ref int lpdwSize);
        /// <summary>
        /// 从_webbrowser中得到cookie给client使用
        /// </summary>
        private static void GetCookies()
        {
            var str = _webbrowser.Document.Cookie;
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


        public static void Download()
        {
            /*
            if (!isLogin())
            {
                Console.WriteLine("没有登录");
                return;
            }*/
            if (_webbrowser.Document == null) return;
            GetYunData();
            var list = GetList(_webbrowser.Url);
            Console.WriteLine(list);
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
            var sb = new StringBuilder();
            InternetGetCookie("https://www.baidu.com/", "BDUSS", sb, ref size);

            // get cookie
            var bduss = sb.ToString();

            return !string.IsNullOrEmpty(bduss);
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
                var html = c.SendAsync(req).Result.Content.ReadAsStringAsync().Result;

                return html;
            }

           // var r = _webbrowser.Document.InvokeScript("getList", new[] { listurl });

           // return r?.ToString();

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


    }
}