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
        string _3dsreleasesFilePath = System.Environment.CurrentDirectory + @"\3dsreleases\3dsreleases.xml";

        public void GetRomInfo(RomInfo pCurrentRomInfo)
        {
            if (Directory.Exists(System.Environment.CurrentDirectory + @"\3dsreleases\") == false)
            {
                Directory.CreateDirectory(System.Environment.CurrentDirectory + @"\3dsreleases\");
            }

            GetRomInfoFrom3dsreleaseXML();

            if (Download3dsdbFile("http://3dsdb.com/xml.php", _3dsreleasesFilePath))
            {
                GetRomInfoFrom3dsreleaseXML();
            }
        }

        public void Update3dsreleases()
        {
            if (Directory.Exists(System.Environment.CurrentDirectory + @"\3dsreleases\") == false)
            {
                Directory.CreateDirectory(System.Environment.CurrentDirectory + @"\3dsreleases\");
            }

            Download3dsdbFile("http://3dsdb.com/xml.php", _3dsreleasesFilePath);
        }

        private bool Download3dsdbFile(string pURL, string pFileName)
        {
            float lPercent = 0;
            try
            {
                HttpWebRequest lHWRT = (HttpWebRequest)HttpWebRequest.Create(pURL);
                HttpWebResponse lHWRS = (HttpWebResponse)lHWRT.GetResponse();
                long lTotal = lHWRS.ContentLength;
                Stream lWebStream = lHWRS.GetResponseStream();

                if (File.Exists(_3dsreleasesFilePath))
                {
                    File.Delete(_3dsreleasesFilePath);
                }

                Stream lFileStream = new FileStream(pFileName, FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] lByteListContent = new byte[1024];
                int lDataSize = lWebStream.Read(lByteListContent, 0, (int)lByteListContent.Length);
                while (lDataSize > 0)
                {
                    totalDownloadedByte = lDataSize + totalDownloadedByte;
                    lFileStream.Write(lByteListContent, 0, lDataSize);
                    lDataSize = lWebStream.Read(lByteListContent, 0, (int)lByteListContent.Length);
                    lPercent = (float)totalDownloadedByte / (float)lTotal * 100;
                }
                lFileStream.Close();
                lWebStream.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private List<Rom3dsdbInfo> GetRomInfoFrom3dsreleaseXML()
        {
            if (File.Exists(_3dsreleasesFilePath) == false)
            {
                return null;
            }

            List<Rom3dsdbInfo> lResult = new List<Rom3dsdbInfo>();
            XmlDocument lXmlDoc = new XmlDocument();
            lXmlDoc.Load(_3dsreleasesFilePath);

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
