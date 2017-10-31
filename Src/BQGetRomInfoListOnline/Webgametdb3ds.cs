using BQ3DSCommonFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace BQGetRomInfoListOnline
{
    public class Webgametdb3ds
    {
        const string URL3dsdb = "http://www.gametdb.com/3DS/";
        string _3dsreleasesFileFullName = Environment.CurrentDirectory + @"\3dsreleases\3dsreleases.xml";
        string _3dsreleasesFilePath = Environment.CurrentDirectory + @"\3dsreleases\";

        public bool GetGameConver(string pGameSerial)
        {

            string lWebSoruce = BQWeb.DownloadWebHtml(URL3dsdb + pGameSerial);

            Regex lRegexInfo = new Regex(@"(<)\s*(table)\s*(class)\s*(=)('DQedit')\s*(>)(?<body>.*)(<)(\/)\s*(table)(>)");
            Regex lRegexImage = new Regex(@"(<)\s*(a)\s*(href)\s*(=)("")\s*(?<picLink>.*)("")\s*(alt)\s*(=)\s*("")(?<PicName>.*)\s*("")\s*(>)");

            return true;
        }
    }
}
