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
using System.Web;
using System.Windows.Forms;
using mshtml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NSGameDownloader
{
    public static class PanApi
    {
        private const int INTERNET_COOKIE_THIRD_PARTY = 0x10;
        private const int INTERNET_COOKIE_HTTPONLY = 0x00002000;
        private const int INTERNET_FLAG_RESTRICTED_ZONE = 0x00020000;
        private static WebBrowser _webbrowser;
        private static int _pageno = 1;
        private static HttpClient _client;
        private static string _extra = "";
        private static List<panFile> _fileList;
        private static string _vcode;
        private static JObject _yunData;
        private static int _getHlinkRetryLimit = 10;

        private static readonly CookieContainer Cookies = new CookieContainer();


        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetGetCookieEx(
            string url,
            string cookieName,
            StringBuilder cookieData,
            ref int size,
            int flags,
            IntPtr pReserved);

        /// <summary>
        ///     从_webbrowser中得到cookie给client使用
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
                if (kv.Length > 2) kv[1] += "=" + kv[2];
                var cookie = new Cookie(kv[0].Trim(), kv.Length == 0 ? "" : HttpUtility.UrlEncode(kv[1].Trim()));
                Cookies.Add(new Uri("https://pan.baidu.com/"), cookie);
            }
        }

        public static void Create(WebBrowser web)
        {
            _webbrowser = web;
            _pageno = 1;
            _webbrowser.DocumentCompleted += _webbrowser_DocumentCompleted;
        }

        private static void _webbrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            GetCookies();
            AddJs();
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


        public static void Download(string code = null)
        {
            if (!IsLogin())
            {
                Console.WriteLine("没有登录");
                return;
            }

            if (_webbrowser.Document == null) return;
            GetExtra();
            GetYunData();
            var list = GetList(_webbrowser.Url.ToString());
            _fileList = new List<panFile>();
            var j = JObject.Parse(list);

            foreach (var f in j["list"])
                _fileList.Add(new panFile
                {
                    path = f["path"].ToString(),
                    fid = f["fs_id"].ToObject<long>(),
                    isdir = f["isdir"].ToString()
                });

            var glinks = GetGlink(code);

            Console.WriteLine(list);
            Console.WriteLine(glinks);
            var glinksj = JObject.Parse(glinks);
            if (glinksj["errno"].ToString() == "-20")
                Form1.Inst.GetCode(GetVcode());
            else
                foreach (var jf in glinksj["list"])
                {
                    var file = _fileList.FirstOrDefault(f => f.fid == jf["fs_id"].ToObject<long>());
                    file.glink = jf["dlink"].ToString();
                    Console.WriteLine(file.glink);
                    _getHlinkRetryLimit = 10;
                    var m = "";
                    GetHlink(file.glink, ref m);
                    file.md5 = m;
                }
        }

        /// <summary>
        ///     得到http-only cookie 里面带了bduss
        /// </summary>
        /// <returns></returns>
        private static bool IsLogin()
        {
            var size = 256 * 1024;
            // create buffer of correct size
            var sb = new StringBuilder(size);
            if (!InternetGetCookieEx("https://pan.baidu.com/", null, sb, ref size, INTERNET_COOKIE_HTTPONLY,
                IntPtr.Zero))
            {
                if (size < 0) return false;
                sb = new StringBuilder(size);

                if (!InternetGetCookieEx("https://pan.baidu.com/", null, sb, ref size, INTERNET_COOKIE_HTTPONLY,
                    IntPtr.Zero))
                    return false;
            }

            // get cookie
            var bduss = sb.ToString();

            if (string.IsNullOrEmpty(bduss))
                return false;

            SetCookies(bduss);


            return bduss.Contains("BDUSS");
        }

        private static void GetExtra()
        {
            //todo 为什么要解2次?????
            var temp = HttpUtility.UrlDecode(Cookies.GetCookies(new Uri("https://pan.baidu.com/"))["BDCLND"]?.Value);
            temp = HttpUtility.UrlDecode(temp);

            var j = new JObject
            {
                ["sekey"] = temp
            };

            _extra = j.ToString(Formatting.None);
        }

        public static void GetYunData()
        {
            var html = _webbrowser.DocumentText;
            var m = Regex.Match(html, @"yunData.setData\((.*)\)");

            var jtext = m.Groups[1].Value;
            _yunData = JObject.Parse(jtext);
        }

        private static string GetList(string url)
        {
            var listurl =
                $"https://pan.baidu.com/share/list?uk={_yunData["uk"]}&shareid={_yunData["shareid"]}&dir={HttpUtility.UrlEncode(getURLParameter(url, "path"))}&num=100&order=time&desc=1&clienttype=0&showempty=0&web=1&page={_pageno}";

            return GetHtmlWithCookie(listurl);
        }

        /// <summary>
        ///     尝试得到高速地址
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string GetGlink(string code = null)
        {
            var url =
                $"https://pan.baidu.com/api/sharedownload?sign={_yunData["sign"]}&timestamp={_yunData["timestamp"]}&bdstoken={_yunData["bdstoken"]}&channel=chunlei&web=1&app_id=250528&clienttype=0";

            var fsidList = new JArray();

            foreach (var file in _fileList) fsidList.Add(file.fid);


            var data = $"encrypt=0&product=share&uk={_yunData["uk"]}&primaryid=" + _yunData["shareid"];
            data += "&fid_list=" + fsidList.ToString(Formatting.None);
            data += "&extra=" + HttpUtility.UrlEncode(_extra);
            data += "&type=nolimit";
            if (code != null) data += "&vcode_str=" + _vcode + "&vcode_input=" + code;

            var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");

            using (var c = new HttpClient(new HttpClientHandler {UseCookies = false}))
            {
                // req.Headers.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                // req.Headers.Add("Accept-Encoding", "gzip, deflate");//Accept-Language: zh-CN,zh;q=0.9
                //req.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9");//Accept-Language: 

                // req.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36");
                req.Headers.Add("Cookie", HttpUtility.UrlDecode(Cookies.GetCookieHeader(new Uri(url))));
                var html = c.SendAsync(req).Result.Content.ReadAsStringAsync().Result;

                return html;
            }
        }

        private static string GetHlink(string glink, ref string md5)
        {
            using (var c = new HttpClient(new HttpClientHandler {UseCookies = false}))
            {
                c.DefaultRequestHeaders.Add("Cookie",
                    HttpUtility.UrlDecode(Cookies.GetCookieHeader(new Uri("https://pan.baidu.com/"))));
                var req = new HttpRequestMessage(HttpMethod.Head, glink);

                var res = c.SendAsync(req, HttpCompletionOption.ResponseHeadersRead).Result;
                if (res.IsSuccessStatusCode)
                {
                    var url = res.RequestMessage.RequestUri.ToString();
                    Console.WriteLine(url);
                    md5 = Convert.ToBase64String(res.Content.Headers.ContentMD5);

                    return url;
                }

                //todo 线程
                if (_getHlinkRetryLimit <= 0)
                {
                    md5 = null;
                    return null;
                }

                Thread.Sleep(500);
                Console.WriteLine("重试:" + _getHlinkRetryLimit);
                _getHlinkRetryLimit--;

                GetHlink(glink, ref md5);

                return null;
            }
        }

        /// <summary>
        ///     验证码
        /// </summary>
        /// <returns></returns>
        public static string GetVcode()
        {
            var html = GetHtmlWithCookie("https://pan.baidu.com/api/getvcode?prod=pan");
            var j = JObject.Parse(html);
            _vcode = j["vcode"].ToString();
            return j["img"].ToString();
        }


        private static string GetHtmlWithCookie(string url)
        {
            HttpStatusCode c;
            return GetHtmlWithCookie(url, out c);
        }

        private static string GetHtmlWithCookie(string url, out HttpStatusCode code)
        {
            try
            {
                using (var c = new HttpClient(new HttpClientHandler {UseCookies = false}))
                {
                    //todo 怎么才能从CookieContainer 带上 urldecode后的cookie?
                    c.DefaultRequestHeaders.Add("Cookie",
                        HttpUtility.UrlDecode(Cookies.GetCookieHeader(new Uri("https://pan.baidu.com/"))));


                    var req = new HttpRequestMessage(HttpMethod.Get, url);
                    var res = c.SendAsync(req).Result;
                    code = res.StatusCode;
                    return res.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("gethtml err:" + e.Message);
                code = HttpStatusCode.NotFound;
                return null;
            }
        }

        private static string getURLParameter(string url, string name)
        {
            //https://pan.baidu.com/s/17mnLTOuwvpacOrDY4GJjCQ#list/path=%2F%E8%B6%85%E7%BA%A7%E9%A9%AC%E9%87%8C%E5%A5%A5&parentPath=%2F
            var u = HttpUtility.UrlDecode(url);
            var array = u.Split(new[] {"#list/"}, StringSplitOptions.RemoveEmptyEntries)[1];

            var ps = array.Split(new[] {"&"}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in ps)
            {
                var kv = s.Split(new[] {"="}, StringSplitOptions.RemoveEmptyEntries);
                if (kv[0] == name) return kv[1];
            }

            return null;
        }
    }

    public class panFile
    {
        public string path { get; set; }
        public long fid { get; set; }
        public string isdir { get; set; }
        public string md5 { get; set; }
        public string size { get; set; }
        public string glink { get; set; }
        public string hlinks { get; set; }

        public string name
        {
            get
            {
                var tmp = path.Split('/');
                return tmp[tmp.Length - 1];
            }
        }
    }
}