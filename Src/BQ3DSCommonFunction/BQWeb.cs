using System;
using System.IO;
using System.Net;

namespace BQ3DSCommonFunction
{
    public class BQWeb
    {
        public static bool DownloadWebFile(string pURL, string pFileFullName)
        {
            float lPercent = 0;
            try
            {
                HttpWebRequest lHWRT = (HttpWebRequest)HttpWebRequest.Create(pURL);
                HttpWebResponse lHWRS = (HttpWebResponse)lHWRT.GetResponse();
                long lTotal = lHWRS.ContentLength;
                Stream lWebStream = lHWRS.GetResponseStream();

                if (File.Exists(pFileFullName))
                {
                    File.Delete(pFileFullName);
                }

                Stream lFileStream = new FileStream(pFileFullName, FileMode.Create);
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
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static string DownloadWebHtml(string pURL)
        {
            string strHTML = "";
            WebClient myWebClient = new WebClient();
            Stream myStream = myWebClient.OpenRead(pURL);
            StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("utf-8"));
            strHTML = sr.ReadToEnd();
            myStream.Close();
            return strHTML;
        }
    }
}
