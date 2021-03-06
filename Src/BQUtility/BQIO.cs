﻿using BQStructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace BQUtility
{
    public class BQIO
    {
        private static string _LastDir = "";

        private static List<FileInfo> _LastFileList = null;

        public static List<string> GetAllRomFileFromLocal()
        {
            List<string> lRomSerialList = new List<string>();

            DirectoryInfo lDir = new DirectoryInfo(BQDirectory.RomDir);

            DirectoryInfo[] lDirList = lDir.GetDirectories();

            for (int i = 0; i < lDirList.Length; i++)
            {
                lRomSerialList.Add(lDirList[i].Name);
            }
            return lRomSerialList;
        }

        public static List<FileInfo> GetRomFile(DirectoryInfo pTarFolder)
        {
            List<FileInfo> lResult = new List<FileInfo>();
            List<string> lSupportExtension = new List<string>() { "*.3ds", "*.3dz", "*.cia" };

            foreach (var extension in lSupportExtension)
            {
                foreach (FileInfo file in pTarFolder.GetFiles(extension))
                {
                    lResult.Add(file);
                }
            }

            return lResult;
        }

        public static List<FileInfo> GetAllFiles(DirectoryInfo directoryInfo)
        {
            List<FileInfo> lFileInfoList = new List<FileInfo>();

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                lFileInfoList.Add(file);
            }

            foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
            {
                lFileInfoList.AddRange(GetAllFiles(subfolder));
            }

            return lFileInfoList;
        }

        public static bool CheckRomFileExist(RomInformation pRomInfo)
        {
            if (pRomInfo.BasicInfo.SubSerial == "")
            {
                return false;
            }
            string lromFolder = Path.Combine(BQDirectory.RomDir, pRomInfo.BasicInfo.Serial);
            if (pRomInfo.ExpandInfo.RomType == "")
            {
                DirectoryInfo ldirInfo = new DirectoryInfo(lromFolder);
                if (ldirInfo.GetFiles().Length > 0)
                {
                    return true;
                }
            }
            else
            {
                string lromfile = Path.Combine(lromFolder, pRomInfo.BasicInfo.Serial + "." + pRomInfo.ExpandInfo.RomType + ".7z");
                return File.Exists(lromfile);
            }

            return false;
        }

        public static void CopyRomToRomFolder(RomInformation pRomInfo, FileInfo pFile)
        {
            if (CheckRomFileExist(pRomInfo))
            {
                return;
            }
            string lromFolder = Path.Combine(BQDirectory.RomDir, pRomInfo.BasicInfo.Serial);
            string lromfile = Path.Combine(lromFolder, pRomInfo.BasicInfo.Serial + "." + pRomInfo.ExpandInfo.RomType);
            BQCompression.CompressionFile(pFile, new FileInfo(lromfile));
        }

        public static List<BitmapImage> GetRomConverPic(RomInformation pRomInfo)
        {
            List<BitmapImage> lRomPicList = new List<BitmapImage>();
            DirectoryInfo lDir = new DirectoryInfo(BQDirectory.ConverDir);
            List<FileInfo> lAllConverList = new List<FileInfo>();
            lAllConverList=  GetAllFiles(lDir);
            foreach (var converFile in lAllConverList)
            {
                if (converFile.Name == pRomInfo.BasicInfo.SubSerial)
                {
                    BitmapImage lRomPic = new BitmapImage(new Uri(converFile.FullName));
                    lRomPicList.Add(lRomPic);
                }
            }

            return lRomPicList;
        }

        public static List<FileInfo> GetAllRomFile(DirectoryInfo rootDir)
        {
            List<string> lRomExtension = new List<string> { ".3ds", ".3dz", ".cia" };
            List<FileInfo> lResult = new List<FileInfo>();
            lResult = GetAllFiles(rootDir);
            for (int i = lResult.Count - 1; i >= 0; i--)
            {
                if (!lRomExtension.Contains(lResult[i].Extension.ToLower()))
                {
                    lResult.RemoveAt(i);
                }
            }

            return lResult;
        }

        public static void SaveLargeIco(BitmapImage pImage, RomInformation pRomInfo)
        {
            SaveIco(pImage, pRomInfo, true);
        }

        public static void SaveSmallIco(BitmapImage pImage, RomInformation pRomInfo)
        {
            SaveIco(pImage, pRomInfo, false);
        }

        private static void SaveIco(BitmapImage pImage, RomInformation pRomInfo,bool pLargeIco)
        {
            if (pImage == null)
            {
                return;
            }

            FileInfo fi;
            if (pLargeIco)
            {
                fi = new FileInfo(Path.Combine(BQDirectory.IcoLargeDir, pRomInfo.BasicInfo.SubSerial + ".jpg"));
            }
            else
            {
                fi = new FileInfo(Path.Combine(BQDirectory.IcoSmallDir, pRomInfo.BasicInfo.SubSerial + ".jpg"));
            }

            
            if (fi.Exists)
            {
                return;
            }

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(pImage));

            using (var fileStream = new FileStream(fi.FullName, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        public static BitmapImage GetRomLargeIco(RomInformation pRomInfo)
        {
            return GetRomIco(pRomInfo, true);
        }

        public static BitmapImage GetRomSmallIco(RomInformation pRomInfo)
        {
            return GetRomIco(pRomInfo, false);
        }

        private static BitmapImage GetRomIco(RomInformation pRomInfo,bool pLargeIco)
        {
            DirectoryInfo lDir;
            if (pLargeIco)
            {
                lDir = new DirectoryInfo(BQDirectory.IcoLargeDir);
            }
            else
            {
                lDir = new DirectoryInfo(BQDirectory.IcoSmallDir);
            }
            
            List<FileInfo> lAllConverList = new List<FileInfo>();
            lAllConverList = GetAllFiles(lDir);
            foreach (var converFile in lAllConverList)
            {
                if (converFile.Name.Replace(converFile.Extension,"") == pRomInfo.BasicInfo.SubSerial)
                {
                    BitmapImage tRomPic = new BitmapImage(new Uri(converFile.FullName));
                    return tRomPic;
                }
            }

            BitmapImage lRomPic;

            if (pLargeIco)
            {
                lRomPic = BitmapToBitmapImage(ResourceDefault.Large);
            }
            else
            {
                lRomPic = BitmapToBitmapImage(ResourceDefault.Small);
            }

            return lRomPic;
        }

        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return null;
            }
            Bitmap bitmapSource = new Bitmap(bitmap.Width, bitmap.Height);
            int i, j;
            for (i = 0; i < bitmap.Width; i++)
                for (j = 0; j < bitmap.Height; j++)
                {
                    Color pixelColor = bitmap.GetPixel(i, j);
                    Color newColor = Color.FromArgb(pixelColor.R, pixelColor.G, pixelColor.B);
                    bitmapSource.SetPixel(i, j, newColor);
                }
            MemoryStream ms = new MemoryStream();
            bitmapSource.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(ms.ToArray());
            bitmapImage.EndInit();

            return bitmapImage;
        }

        public static List<FileInfo> GetRomConvers(RomInformation pRomInfo)
        {
            DirectoryInfo converDir = new DirectoryInfo(BQDirectory.ConverDir);
            List<FileInfo> fileList = GetAllRomFile(converDir);
            for (int i = fileList.Count - 1; i >= 0; i--)
            {
                if (fileList[i].Name != pRomInfo.BasicInfo.SubSerial + ".jpg")
                {
                    fileList.RemoveAt(i);
                }
            }

            return fileList;
        }

        public static void Initialize()
        {

        }

        public static string GetRomType(FileInfo romFile)
        {
            return romFile.Extension.TrimStart('.');
        }

        //public static void CopyToRomTemp(FileInfo unknownFile)
        //{
        //    string lromFolder = RomFolder + @"\Unknow\";
        //    string lromfile = lromFolder + unknownFile.Name;
        //    FileInfo ltarFile = new FileInfo(lromfile);
        //    if (Directory.Exists(lromFolder) == false)
        //    {
        //        Directory.CreateDirectory(lromFolder);
        //    }

        //    if (ltarFile.Exists == false)
        //    {
        //        File.Copy(unknownFile.FullName, ltarFile.FullName);
        //    }
        //}
    }
}
