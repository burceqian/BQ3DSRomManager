using BQ3DSCommonFunction;
using BQInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BQStructure;
using System.IO;

namespace BQNetCrawlers
{
    public class Crawlergametdb3ds : IRomInfoNetCrawler
    {
        const string URL3dsdb = "http://www.gametdb.com/3DS/";
        private string _3dsdbFilePath = Environment.CurrentDirectory + @"\Covers\";

        RomInformation IRomInfoNetCrawler.ScanRomInfo(RomInformation pRomInfo)
        {
            return new RomInformation();
        }

        List<string> IRomInfoNetCrawler.ScanRomPic(RomInformation pRomInfo)
        {
            return null;
        }

        private bool GetGameConver(string pGameSerial)
        {
            string lRegexImagePattern = @"href=""(?<imgLink>http:\/\/[-A-Za-z0-9+&@#\/%?=~_|!:,.;]+[-A-Za-z0-9+&@#\/%=~_|].jpg)""";

            List<string> lImageURLList = new List<string>();
            string lWebSoruce = BQWeb.DownloadWebHtml(URL3dsdb + pGameSerial);
            lWebSoruce = lWebSoruce.Replace("\n", "");
            lWebSoruce = lWebSoruce.Replace("\r", "");

            MatchCollection lResult = Regex.Matches(lWebSoruce, lRegexImagePattern);
            if (lResult.Count <= 0)
            {
                return false;
            }
            foreach (Match match in lResult)
            {
                GroupCollection gc = match.Groups;
                lImageURLList.Add(gc["imgLink"].ToString());
            }

            //"http://art.gametdb.com/3ds/coverM/JA/AVSJ.jpg"
            foreach (var imageUrl in lImageURLList)
            {
                string tImagePath = imageUrl.Replace(@"http://art.gametdb.com/3ds/", "");
                tImagePath = tImagePath.Replace(@"/", @"\");
                string tImageFullName = _3dsdbFilePath + tImagePath;
                if (File.Exists(tImageFullName))
                {
                    continue;
                }

                FileInfo fileInfo = new FileInfo(tImageFullName);

                if (Directory.Exists(fileInfo.DirectoryName) == false)
                {
                    Directory.CreateDirectory(fileInfo.DirectoryName);
                }

                BQWeb.DownloadWebFile(imageUrl, fileInfo.FullName);
            }

            return true;
        }

        private Romgamedb3dsInfo GetGameInfo(string pGameSerial)
        {
            string lWebSoruce = BQWeb.DownloadWebHtml(URL3dsdb + pGameSerial);
            lWebSoruce = lWebSoruce.Replace("\n", "");
            lWebSoruce = lWebSoruce.Replace("\r", "");
            lWebSoruce = lWebSoruce.Replace("<tr><td", "</td></tr><tr><td");
            Regex lRegexInfo = new Regex(@"(<\s*?table\s*?class\s*?=\s*?'DQedit'\s*?>)(?<body>.*)(<\/\s*table>)");
            // <tr>???</tr>
            //Regex lRegextr = new Regex(@"(<\s*?tr\s*?>)(?<tr>.*?)(<\/\s*?tr\s*?>)");
            // <td>???</td>
            Regex lRegextd = new Regex(@"<\s*?td.*?>(?<tdValue>.*?)<\/\s*?td\s*?>");
            Regex lRegextdValue = new Regex(@"(>)(?<value>.*)(<)");
            Regex lRegextdDummy = new Regex(@"(<).*(>)");
            // <td class='head' align='right' width='110'  valign='top'>??</td>
            //Regex lRegextdHead = new Regex(@"(<)\s*(td)\s+(class)\s*(=)\s*('head')\s+(align)\s*(=)\s*('right')\s+.+(valign)\s*(=)\s*('top')\s*(>)(?<tdValue>.*)(<)(\/)\s*(td)\s*(>)");
            //Regex lRegextdValue = new Regex(@"(<)\s*(td)\s+(align)\s*(=)\s*('right')\s+.+(valign)\s*(=)\s*('top')\s*(>)(?<tdValue>.*)(<)(\/)\s*(td)\s*(>)");

            Match lResult = lRegexInfo.Match(lWebSoruce);
            if (lResult.Success == false)
            {
                return null;
            }

            string lRomInfo = lResult.Groups["body"].Value.ToString();
            MatchCollection lMatchCollection = lRegextd.Matches(lRomInfo);
            if (lMatchCollection.Count <= 0)
            {
                return null;
            }
            List<string> lTrList = new List<string>();
            foreach (Match match in lMatchCollection)
            {
                GroupCollection gc = match.Groups;
                lTrList.Add(gc["tdValue"].ToString());
            }

            Romgamedb3dsInfo romgamedb3DsInfo = new Romgamedb3dsInfo();

            string tPropName = "";
            bool tFindProp = false;
            foreach (var gameInfo in lTrList)
            {
                if (gameInfo == "")
                {
                    continue;
                }
                tFindProp = false;
                foreach (var item in romgamedb3DsInfo.GetType().GetProperties())
                {
                    string prpname = item.Name;
                    prpname = prpname.Replace("_1", " (");
                    prpname = prpname.Replace("_2", ")");
                    prpname = prpname.Replace("_3", " ");
                    prpname = prpname.Replace("_4", ".");
                    prpname = prpname.Replace("__", "");

                    if (gameInfo.ToLower() == prpname.ToLower())
                    {
                        tFindProp = true;
                        tPropName = item.Name;
                        break;
                    }
                }

                if (tFindProp)
                {
                    continue;
                }

                if (tPropName != "")
                {
                    string value = lRegextdDummy.Replace(gameInfo, "");

                    romgamedb3DsInfo.GetType().GetProperty(tPropName).SetValue(romgamedb3DsInfo, value);
                    tPropName = "";
                    tFindProp = false;
                }
            }

            return romgamedb3DsInfo;
        }
    }

    internal class Romgamedb3dsInfo
    {
        public string serial { get; set; }
        public string id { get; set; }
        public string region { get; set; }
        public string type { get; set; }
        public string languages { get; set; }
        public string title_1EN_2 { get; set; }//_1 = ( _2 =)
        public string title_1JA_2 { get; set; }//_1 = ( _2 =)
        public string title_1ZHTW_2 { get; set; }//_1 = ( _2 =)
        public string developer { get; set; }
        public string publisher { get; set; }
        public string release_3date { get; set; } // _3 = " "
        public string genre { get; set; }
        public string rating { get; set; }
        public string players { get; set; }
        public string req_4accessories { get; set; } // _4 = .
        public string accessories { get; set; }
        public string online_3players { get; set; }
        public string save_3blocks { get; set; }
        public string __case { get; set; }// __ = ""
    }
}
