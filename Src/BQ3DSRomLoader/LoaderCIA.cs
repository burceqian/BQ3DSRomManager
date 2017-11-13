using BQInterface;
using BQStructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQ3DSRomLoader
{
    public class LoaderCIA : IRomLoader
    {
        RomInfo IRomLoader.GetRomInfo(FileInfo pRomFile)
        {
            RomInfo lRomInfo = new RomInfo();

            lRomInfo.OriginalName = pRomFile.Name;

            byte[] tByteContent = null;
            string tStrContent = "";
            FileStream tFS;
            try
            {
                tFS = new FileStream(pRomFile.FullName, FileMode.Open);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

            // Serial
            tFS.Position = 0X3a90;
            tByteContent = new byte[10];
            tFS.Read(tByteContent, 0, tByteContent.Length);
            lRomInfo.Serial = System.Text.Encoding.Default.GetString(tByteContent);

            // Title_ID
            tFS.Position = 0X3a48;
            tByteContent = new byte[8];
            tFS.Read(tByteContent, 0, tByteContent.Length);
            tStrContent = "";
            for (int i = tByteContent.Length - 1; i >= 0; i--)
            {
                tStrContent += tByteContent[i].ToString("X2");
            }

            lRomInfo.Title_ID = tStrContent;

            tFS.Close();

            return lRomInfo;
        }
    }
}
