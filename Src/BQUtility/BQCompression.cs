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
                BQLog.UpdateProgress("Compressing" + eventArgs.FileName, 0, 100);
            };

            // 单个文件压缩完成时  
            lSevenZipCompressor.FileCompressionFinished += (sender, eventArgs) =>
            {
                BQLog.UpdateProgress("FileCompressionFinished", 100, 100);
            };

            lSevenZipCompressor.Compressing += (sender, eventArgs) =>
            {
                BQLog.UpdateProgress("Compressing", eventArgs.PercentDone, 100);
            };

            // 压缩完成  
            lSevenZipCompressor.CompressionFinished += (sender, eventArgs) =>
            {
                BQLog.UpdateProgress("CompressionFinished", 100, 100);
            };

            //lSevenZipCompressor.CompressFiles(pTarFile.FullName, pSrcFile.FullName + ".7z");

            // 添加文件  
            //var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            //var files = new string[] { $"{baseDir}123.txt", $"{baseDir}456.txt" };
            //compressor.CompressFiles("Files.7z", files);
            //var files = new string[] { @"C:\D\3DSTest\Test\Me & My Pets 3D (Europe) (En,Fr,De,Es,It,Nl).3ds\Me & My Pets 3D (Europe) (En,Fr,De,Es,It,Nl).3ds", @"C:\D\3DSTest\Test\Me+%26+My+Pets+3D+%28Europe%29+%28En%2CFr%2CDe%2CEs%2CIt%2CNl%29.cia\Me & My Pets 3D (Europe) (En,Fr,De,Es,It,Nl).cia" };
            //compressor.CompressFiles(@"C:\D\3DSTest\test.7z", files);

            //files = new string[] { @"C:\D\3DSTest\Test\Me+%26+My+Pets+3D+%28Europe%29+%28En%2CFr%2CDe%2CEs%2CIt%2CNl%29.cia\Me & My Pets 3D (Europe) (En,Fr,De,Es,It,Nl).cia" };
            //compressor.CompressFiles(@"C:\D\3DSTest\test.7z", files);

            // 添加目录下的所有文件  
            //compressor.CompressDirectory($"{baseDir}Dir", "Dir.7z");


            //普通压缩压缩的文件带有文件夹，有时候使用不太方便  
            //MyCompressor.CompressFiles(@"data\text\1.txt.7z", @"data\text\1.txt");  

            //使用字典压缩，自定义格式  
            Dictionary<string, string> MyDic = new Dictionary<string, string>();
            //key为压缩包中的文件名  
            //value为待压缩文件  
            //MyDic["1.txt"] = @"data\text\1.txt";
            //MyDic.Add("MEMy.3ds", @"C:\D\3DSTest\Test\Me & My Pets 3D (Europe) (En,Fr,De,Es,It,Nl).3ds\Me & My Pets 3D (Europe) (En,Fr,De,Es,It,Nl).3ds");
            //MyDic.Add(@"AA\MEMy.cia", @"C:\D\3DSTest\Test\Me+%26+My+Pets+3D+%28Europe%29+%28En%2CFr%2CDe%2CEs%2CIt%2CNl%29.cia\Me & My Pets 3D (Europe) (En,Fr,De,Es,It,Nl).cia");

            MyDic.Add(pTarFile.Name, pSrcFile.FullName);
            lSevenZipCompressor.CompressFileDictionary(MyDic, pTarFile.FullName + ".7z");//注意第二个参数为输出压缩包的文件
            //compressor.CompressFileDictionary(MyDic, @"C:\D\3DSTest\test.7z");//注意第二个参数为输出压缩包的文件
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
