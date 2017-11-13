using System;
using System.IO;
using System.Net;
using System.Threading;

namespace BQ3DSCommonFunction
{
    public class BQWeb
    {
        public static bool DownloadWebFile(string pURL, string pFileFullName)
        {
            float lPercent = 0;
            HttpWebRequest lHWRT = null;
            HttpWebResponse lHWRS = null;
            Stream lWebStream = null;
            Stream lFileStream = null;
            long lTotal;
            try
            {
                lHWRT = (HttpWebRequest)HttpWebRequest.Create(pURL);
                lHWRS = (HttpWebResponse)lHWRT.GetResponse();
                lTotal = lHWRS.ContentLength;
                lWebStream = lHWRS.GetResponseStream();

                if (File.Exists(pFileFullName))
                {
                    File.Delete(pFileFullName);
                }

                lFileStream = new FileStream(pFileFullName, FileMode.Create);
                long totalDownloadedByte = 0;
                byte[] lByteListContent = new byte[128];
                int lDataSize = lWebStream.Read(lByteListContent, 0, (int)lByteListContent.Length);
                while (lDataSize > 0)
                {
                    totalDownloadedByte = lDataSize + totalDownloadedByte;
                    lFileStream.Write(lByteListContent, 0, lDataSize);
                    lFileStream.Flush();
                    lDataSize = lWebStream.Read(lByteListContent, 0, (int)lByteListContent.Length);
                    lPercent = (float)totalDownloadedByte / (float)lTotal * 100;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (lFileStream != null)
                {
                    lFileStream.Close();
                }

                if (lWebStream != null)
                {
                    lWebStream.Close();
                }
            }
            return true;
        }

        public static string DownloadWebHtml(string pURL)
        {
            string strHTML = "";
            WebClient myWebClient = new WebClient();
            Stream myStream = null;
            try
            {
                myStream = myWebClient.OpenRead(pURL);
                StreamReader sr = new StreamReader(myStream, System.Text.Encoding.GetEncoding("utf-8"));
                strHTML = sr.ReadToEnd();
                myStream.Close();
            }
            catch (Exception ex)
            {
                string xx = ex.ToString();
            }

            return strHTML;
        }
    }
}
