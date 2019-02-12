using System.Dynamic;

namespace NSGameDownloader
{
    //应该从json中修改这个值,防止无法访问github.
    public  class Config
    {
        public  string PanUrlHead = "https://pan.baidu.com/s/";
        public  string NspPanKey = "1YSo7_H28r2Q_xYtB1vI2QA";
        public  string XciPanKey = "1cwIw1-qsNOKaq6xrK0VUqQ";
        public  string NspCookie = @"jbJABRU1bnEilqgFANDScp7THxrIkl57hfDd/wzv29o=";
        public  string XciCookie = "83YnXyahT%2BktyGqrzphpP87nP1jVU3HIj0Jj2VXPmV4=";
        public  string NutdbUrl = "https://snip.li/nutdb";
        public  string TitleKeysPath = "keys.json";

    }
}