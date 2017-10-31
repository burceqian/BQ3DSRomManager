using BQ3DSCommonFunction;
using BQStructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace BQGetRomInfoListOnline
{
    public class Web3dsdb
    {
        const string URL3dsdb = "http://3dsdb.com/xml.php";
        string _3dsreleasesFileFullName = System.Environment.CurrentDirectory + @"\3dsreleases\3dsreleases.xml";
        string _3dsreleasesFilePath = System.Environment.CurrentDirectory + @"\3dsreleases\";

        public void GetRomInfo(RomInfo pCurrentRomInfo)
        {
            if (Directory.Exists(_3dsreleasesFilePath) == false)
            {
                Directory.CreateDirectory(_3dsreleasesFilePath);
            }

            GetRomInfoFrom3dsreleaseXML();

            if (BQWeb.DownloadWebFile("http://3dsdb.com/xml.php", _3dsreleasesFileFullName))
            {
                GetRomInfoFrom3dsreleaseXML();
            }
        }

        public void Update3dsreleases()
        {
            if (Directory.Exists(_3dsreleasesFilePath) == false)
            {
                Directory.CreateDirectory(_3dsreleasesFilePath);
            }

            BQWeb.DownloadWebFile(URL3dsdb, _3dsreleasesFileFullName);
        }

        private List<Rom3dsdbInfo> GetRomInfoFrom3dsreleaseXML()
        {
            if (File.Exists(_3dsreleasesFileFullName) == false)
            {
                return null;
            }

            List<Rom3dsdbInfo> lResult = new List<Rom3dsdbInfo>();
            XmlDocument lXmlDoc = new XmlDocument();
            lXmlDoc.Load(_3dsreleasesFileFullName);

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
}
