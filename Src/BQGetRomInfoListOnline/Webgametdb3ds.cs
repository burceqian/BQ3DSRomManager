using BQ3DSCommonFunction;
using BQStructure;
using System;
using System.Collections.Generic;
using System.IO;
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
        string _3dsdbFilePath = Environment.CurrentDirectory + @"\Covers\";

        public bool GetGameConver(string pGameSerial)
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

        public Romgamedb3dsInfo GetGameInfo(string pGameSerial)
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
}
