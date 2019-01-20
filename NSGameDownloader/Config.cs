using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NSGameDownloader
{
    /// <summary>
    /// 从config.json文件加载设置
    /// </summary>
    public class Config
    {
        public static DownloadType downloadType { get; set; }
        private const string filePath = "config.json";
        public static Config config;
        public void Create()
        {
            if (config != null) return;
            if (File.Exists(filePath))
                config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(filePath));

        }

        public void Updata()
        { 

        }

    }

    public enum DownloadType
    {
        BaiduClient,
        Copy,
        Aria2C,
    }

}
