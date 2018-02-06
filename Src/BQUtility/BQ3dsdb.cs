using BQStructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BQUtility
{
    public class BQ3dsdb
    {
        const string URL3dsdb = "http://3dsdb.com/xml.php";
        //private static string _3dsreleasesFileFullName = System.Environment.CurrentDirectory + @"\3dsreleases\3dsreleases.xml";
        //private static string _3dsreleasesFilePath = System.Environment.CurrentDirectory + @"\3dsreleases\";

        private static string _3dsreleasesFileName = @"3dsreleases.xml";

        public static bool Check3dsdb()
        {
            FileInfo l3dsdb = new FileInfo(Path.Combine(BQDirectory.DBDir, _3dsreleasesFileName));

            return l3dsdb.Exists;
        }

        public static void Update3dsdb()
        {
            FileInfo l3dsdb = new FileInfo(Path.Combine(BQDirectory.DBDir, _3dsreleasesFileName));

            if (!l3dsdb.Exists)
            {
                BQWeb.DownloadWebFile(URL3dsdb, l3dsdb);
            }
            else
            {
                FileInfo l3dsdbNew = new FileInfo(l3dsdb.FullName + "new");
                if (l3dsdbNew.Exists)
                {
                    l3dsdbNew.Delete();
                }
                BQWeb.DownloadWebFile(URL3dsdb, l3dsdbNew);
                l3dsdb.Delete();
                l3dsdbNew.MoveTo(l3dsdb.FullName);
            }
        }

        public static FileInfo Get3dsdbFile()
        {
            FileInfo l3dsdb = new FileInfo(Path.Combine(BQDirectory.DBDir, _3dsreleasesFileName));
            if (l3dsdb.Exists)
            {
                return l3dsdb;
            }
            return null;
        }

        public static List<RomInformation> GetAllRomInfo()
        {
            List<RomInformation> lRomInfoList = new List<RomInformation>();
            List<Rom3dsdbInfo> l3dsdbGameInfoList = GetRomInfoFrom3dsreleaseXML();
            l3dsdbGameInfoList.ForEach(dsdbGameInfo => lRomInfoList.Add(Conver3dsdbToRomInfo(dsdbGameInfo)));
            return lRomInfoList;
        }

        public static RomInformation GetRomInfo(RomInformation pRomInfo)
        {
            List<Rom3dsdbInfo> l3dsdbGameInfoList = GetRomInfoFrom3dsreleaseXML();
            Rom3dsdbInfo l3dsdbGameInfo = l3dsdbGameInfoList.AsParallel().FirstOrDefault(gameinfo => gameinfo.serial == pRomInfo.BasicInfo.Serial);
            if (l3dsdbGameInfo != null)
            {
                RomInformation lresult = Conver3dsdbToRomInfo(l3dsdbGameInfo);
                return lresult;
            }
            else
            {
                return new RomInformation();
            }
        }

        private static RomInformation Conver3dsdbToRomInfo(Rom3dsdbInfo pRom3dsdbInfo)
        {
            RomInformation lresult = new RomInformation();
            lresult.BasicInfo.Serial = pRom3dsdbInfo.serial;
            lresult.BasicInfo.Title_ID = pRom3dsdbInfo.titleid;
            lresult.BasicInfo.English_Title = pRom3dsdbInfo.name;
            lresult.BasicInfo.Publisher = pRom3dsdbInfo.publisher;
            lresult.BasicInfo.Region = pRom3dsdbInfo.region;
            lresult.BasicInfo.Languages = pRom3dsdbInfo.languages;
            lresult.BasicInfo.Developer = pRom3dsdbInfo.group;
            lresult.BasicInfo.Imagesize = pRom3dsdbInfo.imagesize;
            lresult.BasicInfo.Firmware = pRom3dsdbInfo.firmware;
            lresult.BasicInfo.Card_Type = pRom3dsdbInfo.card;
            return lresult;
        }

        private static List<Rom3dsdbInfo> GetRomInfoFrom3dsreleaseXML()
        {
            FileInfo l3dsdbFile = BQ3dsdb.Get3dsdbFile();

            if (!l3dsdbFile.Exists)
            {
                return new List<Rom3dsdbInfo>();
            }

            List<Rom3dsdbInfo> lResult = new List<Rom3dsdbInfo>();
            XmlDocument lXmlDoc = new XmlDocument();
            lXmlDoc.Load(l3dsdbFile.FullName);

            XmlNode xn = lXmlDoc.SelectSingleNode("releases");
            XmlNodeList xnl = xn.ChildNodes;

            foreach (XmlNode xn1 in xnl)
            {
                XmlElement tXmlE = (XmlElement)xn1;
                XmlNodeList tXmlNL = tXmlE.ChildNodes;
                Rom3dsdbInfo lRom3dsdbInfo = new Rom3dsdbInfo();

                for (int i = 0; i < tXmlNL.Count; i++)
                {
                    foreach (var InfoProp in lRom3dsdbInfo.GetType().GetProperties())
                    {
                        if (tXmlNL.Item(i).Name == InfoProp.Name)
                        {
                            InfoProp.SetValue(lRom3dsdbInfo, tXmlNL.Item(i).InnerText);
                            break;
                        }
                    }
                }
                lResult.Add(lRom3dsdbInfo);
            }

            return lResult;
        }
    }

    internal class Rom3dsdbInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string publisher { get; set; }
        public string region { get; set; }
        public string languages { get; set; }
        public string group { get; set; }
        public string imagesize { get; set; }
        public string serial { get; set; }
        public string titleid { get; set; }
        public string imgcrc { get; set; }
        public string filename { get; set; }
        public string releasename { get; set; }
        public string trimmedsize { get; set; }
        public string firmware { get; set; }
        public string type { get; set; }
        public string card { get; set; }
    }
}
