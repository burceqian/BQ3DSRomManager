using SevenZip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace BQUtility
{
    public class BQCompression
    {
        public static void CompressionFile(FileInfo pSrcFile, FileInfo pTarFile)
        {
            if (pTarFile.Directory.Exists == false)
            {
                Directory.CreateDirectory(pTarFile.Directory.FullName);
            }

            if (IntPtr.Size == 4)
            {
                SevenZipCompressor.SetLibraryPath(@"x86\7z.dll");
            }
            else
            {
                SevenZipCompressor.SetLibraryPath(@"x64\7z.dll");
            }

            // Set Temp Folder
            string lCompressTempFolder = Environment.GetEnvironmentVariable("TEMP");
            var lSevenZipCompressor = new SevenZipCompressor(lCompressTempFolder);

            // Print Temp Folder
            Console.WriteLine(lSevenZipCompressor.TempFolderPath);

            // Set CompressionLevel
            lSevenZipCompressor.CompressionLevel = SevenZip.CompressionLevel.Normal;

            // Set Compress Type 
            lSevenZipCompressor.ArchiveFormat = OutArchiveFormat.SevenZip;

            // Save DirectoryStructure
            lSevenZipCompressor.DirectoryStructure = true;

            // Compress blank folder  
            lSevenZipCompressor.IncludeEmptyDirectories = true;

            // Use root folder
            lSevenZipCompressor.PreserveDirectoryRoot = false;

            // Encrypt Header
            lSevenZipCompressor.EncryptHeaders = false;

            // Zip Encryption Method  
            lSevenZipCompressor.ZipEncryptionMethod = ZipEncryptionMethod.ZipCrypto;

            // Fast Compression
            // No started event, only finish event
            lSevenZipCompressor.FastCompression = false;

            // start compression file
            lSevenZipCompressor.FileCompressionStarted += (sender, eventArgs) =>
            {
                BQLog.UpdateProgress("开始压缩：" + eventArgs.FileName, 0, 100);
            };

            //lSevenZipCompressor.FileCompressionFinished += (sender, eventArgs) =>
            //{
            //    BQLog.UpdateProgress("FileCompressionFinished", 100, 100);
            //};

            lSevenZipCompressor.Compressing += (sender, eventArgs) =>
            {
                BQLog.UpdateProgress("压缩中(" + eventArgs.PercentDone + "%)：" + pSrcFile.Name, eventArgs.PercentDone, 100);
            };

            lSevenZipCompressor.CompressionFinished += (sender, eventArgs) =>
            {
                BQLog.UpdateProgress("压缩完成", 100, 100);
            };

            //lSevenZipCompressor.CompressFiles(pTarFile.FullName, pSrcFile.FullName + ".7z");
            
            Dictionary<string, string> MyDic = new Dictionary<string, string>();
            MyDic.Add(pTarFile.Name, pSrcFile.FullName);
            lSevenZipCompressor.CompressFileDictionary(MyDic, pTarFile.FullName + ".7z");
        }

        public static void UnCompressionFile(FileInfo pFile, DirectoryInfo pTarFolder)
        {
            ZipFile.ExtractToDirectory(pFile.FullName, pTarFolder.FullName);
        }

        public static void ClearUnCompressionFolder()
        {
            DirectoryInfo dir = new DirectoryInfo(BQDirectory.TempDir);
            if (dir.Exists)
            {
                dir.Delete(true);
            }
            dir.Create();
        }

        public static List<FileInfo> GetAllFilesInDirectory(DirectoryInfo directoryInfo, List<FileInfo> fileList)
        {
            List<string> lSupportExtension = new List<string>() { "*.3ds", "*.3dz", "*.cia" };

            foreach (var extension in lSupportExtension)
            {
                foreach (FileInfo file in directoryInfo.GetFiles(extension))
                {
                    fileList.Add(file);
                }
            }

            foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
            {
                fileList = GetAllFilesInDirectory(subfolder, fileList);
            }

            return fileList;
        }
    }
}
