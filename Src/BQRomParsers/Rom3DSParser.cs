using BQInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BQStructure;

namespace BQRomParsers
{
    public class Rom3DSParser: IRomParser
    {
        RomInfo GetRomInfo(FileInfo pRomFile)
        {
            RomInfo lRomInfo = new RomInfo();

            lRomInfo.OriginalName = pRomFile.Name;

            byte[] tByteContent = null;
            string tStrContent = "";
            FileStream tFS = new FileStream(pRomFile.FullName, FileMode.Open);

            // Serial
            tFS.Position = 0X1150;
            tByteContent = new byte[10];
            tFS.Read(tByteContent, 0, tByteContent.Length);
            lRomInfo.Serial = System.Text.Encoding.Default.GetString(tByteContent).TrimEnd('\0');

            // Title_ID
            tFS.Position = 0X108;
            tByteContent = new byte[8];
            tFS.Read(tByteContent, 0, tByteContent.Length);
            tStrContent = "";
            for (int i = tByteContent.Length - 1; i >= 0; i--)
            {
                tStrContent += tByteContent[i].ToString("X2");
            }

            lRomInfo.Title_ID = tStrContent;

            // Card_Type
            tFS.Position = 0X18D;
            tByteContent = new byte[1];
            tFS.Read(tByteContent, 0, tByteContent.Length);

            if (tByteContent[0] == 0X1)
            {
                lRomInfo.Card_Type = CardType.Card1;
            }
            else
            {
                lRomInfo.Card_Type = CardType.Card2;
            }

            // Capacity
            tFS.Position = 0X104;
            tByteContent = new byte[4];
            tFS.Read(tByteContent, 0, tByteContent.Length);
            tStrContent = "";
            for (int i = 0; i < tByteContent.Length; i++)
            {
                tStrContent += tByteContent[i].ToString("X2");
            }

            if (tStrContent == "00000400")
            {
                lRomInfo.Capacity = "128 MB";
            }
            else if (tStrContent == "00000800")
            {
                lRomInfo.Capacity = "256 MB";
            }
            else if (tStrContent == "00001000")
            {
                lRomInfo.Capacity = "512 MB";
            }
            else if (tStrContent == "00002000")
            {
                lRomInfo.Capacity = "1 GB";
            }
            else if (tStrContent == "00004000")
            {
                lRomInfo.Capacity = "2 GB";
            }
            else if (tStrContent == "00008000")
            {
                lRomInfo.Capacity = "4 GB";
            }

            // Manufacturer
            tFS.Position = 0X1240;
            tByteContent = new byte[1];
            tFS.Read(tByteContent, 0, tByteContent.Length);
            tStrContent = tByteContent[0].ToString("X2");
            if (tStrContent == "C2")
            {
                lRomInfo.Manufacturer = "Macronix";
            }
            else if (tStrContent == "45")
            {
                lRomInfo.Manufacturer = "SanDisk";
            }
            else if (tStrContent == "AE")
            {
                lRomInfo.Manufacturer = "OKI Semiconductor";
            }
            else
            {
                lRomInfo.Manufacturer = "Un Know";
            }

            // ChipID
            lRomInfo.Chip_ID = tStrContent;
            if (lRomInfo.Capacity == "128 MB")
            {
                lRomInfo.Chip_ID += "7F";
            }
            else if (lRomInfo.Capacity == "256 MB")
            {
                lRomInfo.Chip_ID += "FF";
            }
            else if (lRomInfo.Capacity == "512 MB")
            {
                lRomInfo.Chip_ID += "FE";
            }
            else if (lRomInfo.Capacity == "1 GB")
            {
                lRomInfo.Chip_ID += "FA";
            }
            else if (lRomInfo.Capacity == "2 GB")
            {
                lRomInfo.Chip_ID += "F8";
            }
            else if (lRomInfo.Capacity == "4 GB")
            {
                lRomInfo.Chip_ID += "F0";
            }

            lRomInfo.Chip_ID += "00";

            if (lRomInfo.Card_Type == CardType.Card1)
            {
                lRomInfo.Chip_ID += "90";
            }
            else
            {
                lRomInfo.Chip_ID += "98";
            }

            // Card_ID
            tFS.Position = 0X1200;
            tByteContent = new byte[16];
            tFS.Read(tByteContent, 0, tByteContent.Length);
            if (tByteContent[0].ToString("X2") == "FF")
            {
                lRomInfo.Card_ID = "No ID in Rom";
            }
            else
            {
                tStrContent = "";
                for (int i = 0; i < tByteContent.Length; i++)
                {
                    tStrContent += tByteContent[i].ToString("X2");
                }
                lRomInfo.Card_ID = tStrContent;
            }

            tFS.Close();

            return lRomInfo;
        }

        RomInformation IRomParser.ParseRom(FileInfo pRomFile)
        {
            if (pRomFile.Extension.ToLower() != ".3ds")
            {
                return new RomInformation();
            }

            RomInformation lRomInfo = new RomInformation();

            lRomInfo.BasicInfo.OriginalName = pRomFile.Name;

            byte[] tByteContent = null;
            string tStrContent = "";
            FileStream tFS = new FileStream(pRomFile.FullName, FileMode.Open);

            // Serial
            tFS.Position = 0X1150;
            tByteContent = new byte[10];
            tFS.Read(tByteContent, 0, tByteContent.Length);
            lRomInfo.BasicInfo.Serial = System.Text.Encoding.Default.GetString(tByteContent).TrimEnd('\0');

            // Title_ID
            tFS.Position = 0X108;
            tByteContent = new byte[8];
            tFS.Read(tByteContent, 0, tByteContent.Length);
            tStrContent = "";
            for (int i = tByteContent.Length - 1; i >= 0; i--)
            {
                tStrContent += tByteContent[i].ToString("X2");
            }

            lRomInfo.BasicInfo.Title_ID = tStrContent;

            // Card_Type
            tFS.Position = 0X18D;
            tByteContent = new byte[1];
            tFS.Read(tByteContent, 0, tByteContent.Length);

            if (tByteContent[0] == 0X1)
            {
                lRomInfo.BasicInfo.Card_Type = "1";
            }
            else
            {
                lRomInfo.BasicInfo.Card_Type = "2";
            }

            // Capacity
            tFS.Position = 0X104;
            tByteContent = new byte[4];
            tFS.Read(tByteContent, 0, tByteContent.Length);
            tStrContent = "";
            for (int i = 0; i < tByteContent.Length; i++)
            {
                tStrContent += tByteContent[i].ToString("X2");
            }

            if (tStrContent == "00000400")
            {
                lRomInfo.BasicInfo.Capacity = "128 MB";
            }
            else if (tStrContent == "00000800")
            {
                lRomInfo.BasicInfo.Capacity = "256 MB";
            }
            else if (tStrContent == "00001000")
            {
                lRomInfo.BasicInfo.Capacity = "512 MB";
            }
            else if (tStrContent == "00002000")
            {
                lRomInfo.BasicInfo.Capacity = "1 GB";
            }
            else if (tStrContent == "00004000")
            {
                lRomInfo.BasicInfo.Capacity = "2 GB";
            }
            else if (tStrContent == "00008000")
            {
                lRomInfo.BasicInfo.Capacity = "4 GB";
            }

            // Manufacturer
            tFS.Position = 0X1240;
            tByteContent = new byte[1];
            tFS.Read(tByteContent, 0, tByteContent.Length);
            tStrContent = tByteContent[0].ToString("X2");
            if (tStrContent == "C2")
            {
                lRomInfo.BasicInfo.Manufacturer = "Macronix";
            }
            else if (tStrContent == "45")
            {
                lRomInfo.BasicInfo.Manufacturer = "SanDisk";
            }
            else if (tStrContent == "AE")
            {
                lRomInfo.BasicInfo.Manufacturer = "OKI Semiconductor";
            }
            else
            {
                lRomInfo.BasicInfo.Manufacturer = "Un Know";
            }

            // ChipID
            lRomInfo.BasicInfo.Chip_ID = tStrContent;
            if (lRomInfo.BasicInfo.Capacity == "128 MB")
            {
                lRomInfo.BasicInfo.Chip_ID += "7F";
            }
            else if (lRomInfo.BasicInfo.Capacity == "256 MB")
            {
                lRomInfo.BasicInfo.Chip_ID += "FF";
            }
            else if (lRomInfo.BasicInfo.Capacity == "512 MB")
            {
                lRomInfo.BasicInfo.Chip_ID += "FE";
            }
            else if (lRomInfo.BasicInfo.Capacity == "1 GB")
            {
                lRomInfo.BasicInfo.Chip_ID += "FA";
            }
            else if (lRomInfo.BasicInfo.Capacity == "2 GB")
            {
                lRomInfo.BasicInfo.Chip_ID += "F8";
            }
            else if (lRomInfo.BasicInfo.Capacity == "4 GB")
            {
                lRomInfo.BasicInfo.Chip_ID += "F0";
            }

            lRomInfo.BasicInfo.Chip_ID += "00";

            if (lRomInfo.BasicInfo.Card_Type == "1")
            {
                lRomInfo.BasicInfo.Chip_ID += "90";
            }
            else
            {
                lRomInfo.BasicInfo.Chip_ID += "98";
            }

            // Card_ID
            tFS.Position = 0X1200;
            tByteContent = new byte[16];
            tFS.Read(tByteContent, 0, tByteContent.Length);
            if (tByteContent[0].ToString("X2") == "FF")
            {
                lRomInfo.BasicInfo.Card_ID = "No ID in Rom";
            }
            else
            {
                tStrContent = "";
                for (int i = 0; i < tByteContent.Length; i++)
                {
                    tStrContent += tByteContent[i].ToString("X2");
                }
                lRomInfo.BasicInfo.Card_ID = tStrContent;
            }

            tFS.Close();

            return lRomInfo;
        }
    }
}
