using BQStructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BQUtility
{
    public static class BQ3dsGameInfo
    {
        const string _RegexImagePattern = @"href=""(?<imgLink>http:\/\/[-A-Za-z0-9+&@#\/%?=~_|!:,.;]+[-A-Za-z0-9+&@#\/%=~_|].jpg)""";
        const string URL3dsdb = "http://www.gametdb.com/3DS/";

        public static void DownLoadRomConver(RomInformation pRomInfo)
        {
            string lWebSoruce = "";
            lWebSoruce = BQWeb.DownloadWebHtml(URL3dsdb + pRomInfo.BasicInfo.SubSerial);
            lWebSoruce = lWebSoruce.Replace("\n", "");
            lWebSoruce = lWebSoruce.Replace("\r", "");

            MatchCollection lResult = Regex.Matches(lWebSoruce, _RegexImagePattern);
            if (lResult.Count <= 0)
            {
                return;
            }

            foreach (Match match in lResult)
            {
                GroupCollection gc = match.Groups;
                FileInfo tRomConverFile = SavePic(gc["imgLink"].ToString());
            }
        }

        private static FileInfo SavePic(string pUrl)
        {
            // http://art.gametdb.com/3ds/coverM/JA/AVSJ.jpg
            string[] lInfoList = pUrl.Split('/');
            string lFileName = lInfoList[lInfoList.Length - 1];
            string lFolderName = lInfoList[lInfoList.Length - 3] + "\\" + lInfoList[lInfoList.Length - 2];

            FileInfo fileInfo = new FileInfo(Path.Combine(BQDirectory.ConverDir, lFolderName, lFileName));

            if (fileInfo.Exists)
            {
                return fileInfo;
            }

            if (Directory.Exists(fileInfo.DirectoryName) == false)
            {
                Directory.CreateDirectory(fileInfo.DirectoryName);
            }

            BQWeb.DownloadWebFile(pUrl, fileInfo);

            return fileInfo;
        }
    }
}
