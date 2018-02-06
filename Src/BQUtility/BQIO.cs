using BQStructure;
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

        private static List<FileInfo> GetAllFiles(DirectoryInfo directoryInfo, List<FileInfo> fileList)
        {
            if (_LastDir == directoryInfo.FullName)
            {
                return _LastFileList;
            }

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                fileList.Add(file);
            }

            foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
            {
                fileList = GetAllFiles(subfolder, fileList);
            }

            _LastFileList = fileList;

            return _LastFileList;
        }

        public static void CopyRomToRomFolder(RomInformation pRomInfo, FileInfo pFile)
        {
            string lromFolder = Path.Combine(BQDirectory.RomDir, pRomInfo.BasicInfo.SubSerial);
            string lromfile = Path.Combine(lromFolder, pRomInfo.BasicInfo.SubSerial + ".zip");
            BQCompression.CompressionFile(pFile, new FileInfo(lromfile));
        }

        public static void DownloadRomPicToFolder(List<string> pRomPicList)
        {
            foreach (var romPic in pRomPicList)
            {
                if (romPic.Length > 5 && romPic.Substring(0,5).ToLower() == "http:")
                {
                    SavePic(romPic);
                }
            }
        }

        public static List<BitmapImage> GetRomConverPic(RomInformation pRomInfo)
        {
            List<BitmapImage> lRomPicList = new List<BitmapImage>();
            DirectoryInfo lDir = new DirectoryInfo(BQDirectory.ConverDir);
            List<FileInfo> lAllConverList = new List<FileInfo>();
            lAllConverList=  GetAllFiles(lDir, lAllConverList);
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
            lAllConverList = GetAllFiles(lDir, lAllConverList);
            foreach (var converFile in lAllConverList)
            {
                if (converFile.Name == pRomInfo.BasicInfo.SubSerial)
                {
                    BitmapImage lRomPic = new BitmapImage(new Uri(converFile.FullName));
                    return lRomPic;
                }
            }

            return null;
        }

        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
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

        private static void SavePic(string pUrl)
        {
            // http://art.gametdb.com/3ds/coverM/JA/AVSJ.jpg
            string[] lInfoList = pUrl.Split('/');
            string lFileName = lInfoList[lInfoList.Length - 1];
            string lFolderName = lInfoList[lInfoList.Length - 3] + "\\" + lInfoList[lInfoList.Length - 2];

            FileInfo fileInfo = new FileInfo(Path.Combine(BQDirectory.ConverDir, lFolderName, lFileName));

            if (Directory.Exists(fileInfo.DirectoryName) == false)
            {
                Directory.CreateDirectory(fileInfo.DirectoryName);
            }

            BQWeb.DownloadWebFile(pUrl, fileInfo);
        }

        public static void Initialize()
        {

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
