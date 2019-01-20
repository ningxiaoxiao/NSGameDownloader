using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NSGameDownloader
{
    public class Aria2cManager
    {
        public static string rpcurl = "http://127.0.0.1:6808/jsonrpc";
        public static int maxThread = 16;
        public static string token = "pandownload";


        public static bool Download(PanFile file)
        {
            if (file.hlinks.Length == 0) return false;
            var jd = new JObject()
            {
                ["id"] = file.name,
                ["jsonrpc"] = "2.0",
                ["method"] = "aria2.addUri",
                ["params"] = new JArray
                {
                    "token:"+token,
                    new JArray(file.hlinks),
                    new JObject()
                    {
                        ["async-dns"] = false,
                        ["check-certificate"] = false,
                        ["check-integrity"] = true,
                        ["checksum"] = "md5=" + file.md5,
                        ["max-connection-per-server"] = 16,
                        ["min-split-size"] = "1m",
                        ["out"] = file.name,
                        ["split"] = 16,
                        ["summary-interal"] = 0,
                        ["user-agent"] = "netdisk;8.3.1;android-android",
                    }
                },

            };
            var postdata = jd.ToString(Formatting.None);

            var ret = send(postdata);
            Console.WriteLine(postdata);
            return true;
        }

        private static string send(string data)
        {
            using (var c = new HttpClient())
            {
                var res= c.PostAsync(rpcurl, new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded")).Result;
                return res.Content.ReadAsStringAsync().Result;
            }
        }
    }
}
