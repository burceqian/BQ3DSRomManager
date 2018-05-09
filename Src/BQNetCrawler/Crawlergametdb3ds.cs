using BQInterface;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BQStructure;
using System.IO;
using BQUtility;

namespace BQNetCrawlers
{
    public class Crawlergametdb3ds : IRomInfoNetCrawler
    {
        private static string _BufferCrawleredSubSerial = "";
        private static string _BufferCrawleredUrl = "";
        private Regex lRegexInfo = new Regex(@"(<\s*?table\s*?class\s*?=\s*?'DQedit'\s*?>)(?<body>.*)(<\/\s*table>)");
        // <tr>???</tr>
        //Regex lRegextr = new Regex(@"(<\s*?tr\s*?>)(?<tr>.*?)(<\/\s*?tr\s*?>)");
        // <td>???</td>
        private Regex lRegextd = new Regex(@"<\s*?td.*?>(?<tdValue>.*?)<\/\s*?td\s*?>");
        private Regex lRegextdValue = new Regex(@"(>)(?<value>.*)(<)");
        private Regex lRegextdDummy = new Regex(@"(<).*(>)");
        // <td class='head' align='right' width='110'  valign='top'>??</td>
        const string URL3dsdb = "http://www.gametdb.com/3DS/";

        RomInformation IRomInfoNetCrawler.ScanRomInfo(RomInformation pRomInfo)
        {
            RomInformation lreslut = new RomInformation();
            string lWebSoruce = "";
            if (_BufferCrawleredSubSerial == pRomInfo.BasicInfo.SubSerial)
            {
                lWebSoruce = _BufferCrawleredUrl;
            }
            else
            {
                lWebSoruce = BQWeb.DownloadWebHtml(URL3dsdb + pRomInfo.BasicInfo.SubSerial);
                lWebSoruce = lWebSoruce.Replace("\n", "");
                lWebSoruce = lWebSoruce.Replace("\r", "");
                _BufferCrawleredUrl = lWebSoruce;
            }
            lWebSoruce = lWebSoruce.Replace("<tr><td", "</td></tr><tr><td");

            Match lResult = lRegexInfo.Match(lWebSoruce);
            if (lResult.Success == false)
            {
                return lreslut;
            }

            string lRomInfo = lResult.Groups["body"].Value.ToString();
            MatchCollection lMatchCollection = lRegextd.Matches(lRomInfo);
            if (lMatchCollection.Count <= 0)
            {
                return lreslut;
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

            lreslut.BasicInfo.English_Title = romgamedb3DsInfo.title_1EN_2;
            lreslut.BasicInfo.Japanese_Title = romgamedb3DsInfo.title_1JA_2;
            lreslut.BasicInfo.Developer = romgamedb3DsInfo.developer;
            lreslut.BasicInfo.Players = romgamedb3DsInfo.players;
            lreslut.BasicInfo.ReleaseDate = romgamedb3DsInfo.release_3date;
            lreslut.BasicInfo.Publisher = romgamedb3DsInfo.publisher;
            lreslut.BasicInfo.Languages = romgamedb3DsInfo.languages;
            lreslut.BasicInfo.Genre = romgamedb3DsInfo.genre;

            return lreslut;
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
