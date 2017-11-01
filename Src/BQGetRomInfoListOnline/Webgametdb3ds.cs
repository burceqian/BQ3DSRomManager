using BQ3DSCommonFunction;
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
            List<string> lImageURLList = new List<string>();
            string lWebSoruce = BQWeb.DownloadWebHtml(URL3dsdb + pGameSerial);
            lWebSoruce = lWebSoruce.Replace("\n", "");
            lWebSoruce = lWebSoruce.Replace("\r", "");
            string lRegexImagePattern = @"href=""(?<imgLink>http:\/\/[-A-Za-z0-9+&@#\/%?=~_|!:,.;]+[-A-Za-z0-9+&@#\/%=~_|].jpg)""";
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
                if (System.IO.File.Exists(tImageFullName))
                {
                    continue;
                }

                FileInfo fileInfo = new System.IO.FileInfo(tImageFullName);

                if (Directory.Exists(fileInfo.DirectoryName) == false)
                {
                    Directory.CreateDirectory(fileInfo.DirectoryName);
                }

                BQWeb.DownloadWebFile(imageUrl, fileInfo.FullName);
            }

            return true;
        }

        public bool GetGameInfo(string pGameSerial)
        {
            string lWebSoruce = BQWeb.DownloadWebHtml(URL3dsdb + pGameSerial);
            lWebSoruce = lWebSoruce.Replace("\n", "");
            lWebSoruce = lWebSoruce.Replace("\r", "");
            Regex lRegexInfo = new Regex(@"(<)\s*(table)\s*(class)\s*(=)('DQedit')\s*(>)(?<body>.*)(<)(\/)\s*(table)(>)");
            // <tr>???</tr>
            Regex lRegextr = new Regex(@"(<)\s*(tr)\s(>)(?<tr>.*)(<)(\/)\s*(tr)\s*(>)");
            // <td>???</td>
            Regex lRegextd = new Regex(@"(?<td>(<)\s*(td).*(>).*(<)(\/)\s*(td)\s*(>))");
            Regex lRegextdValue = new Regex(@"(>)(?<value>.*)(<)");
            // <td class='head' align='right' width='110'  valign='top'>??</td>
            //Regex lRegextdHead = new Regex(@"(<)\s*(td)\s+(class)\s*(=)\s*('head')\s+(align)\s*(=)\s*('right')\s+.+(valign)\s*(=)\s*('top')\s*(>)(?<tdValue>.*)(<)(\/)\s*(td)\s*(>)");
            //Regex lRegextdValue = new Regex(@"(<)\s*(td)\s+(align)\s*(=)\s*('right')\s+.+(valign)\s*(=)\s*('top')\s*(>)(?<tdValue>.*)(<)(\/)\s*(td)\s*(>)");
            Match lResult = lRegexInfo.Match(lWebSoruce);
            if (lResult.Success == false)
            {
                return false;
            }

            string lRomInfo = lResult.Groups["body"].Value.ToString();

            lResult = lRegextr.Match(lRomInfo);
            if (lResult.Success == false)
            {
                return false;
            }
            foreach (var item in lResult.Groups)
            {
                string lRomtr = lResult.Groups["tr"].ToString();
            }

            return true;
        }
    }
}
