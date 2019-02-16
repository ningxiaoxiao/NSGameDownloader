using System.Dynamic;

namespace NSGameDownloader
{
    //应该从json中修改这个值,防止无法访问github.
    public class Config
    {
        public string PanUrlHead { get; set; }
        public string NspPanKey { get; set; }
        public string XciPanKey { get; set; }
        public string NspCookie { get; set; }
        public string XciCookie { get; set; }
        public string NutdbUrl { get; set; }
        public string TitleKeysPath { get; set; }

    }
}