﻿using BQInterface;
using BQStructure;
using System;
using System.IO;

namespace BQ3DSRomLoader
{
    public class Loader3DS: IRomLoader
    {
        RomInfo IRomLoader.GetRomInfo(string pRomPath)
        {
            RomInfo lRomInfo = new RomInfo();

            byte[] tByteContent = null;
            string tStrContent = "";
            FileStream tFS = new FileStream(pRomPath, FileMode.Open);

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
    }
}
