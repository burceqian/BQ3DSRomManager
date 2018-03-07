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

            //MemoryStream ms = new MemoryStream();
            FileStream fileStream = new FileStream(pTarFile.FullName, FileMode.CreateNew);
            GZipStream compressedStream = new GZipStream(fileStream, CompressionMode.Compress, true);
            FileStream fs = new FileStream(pSrcFile.FullName, FileMode.Open);
            byte[] buf = new byte[10240];
            int count = 0;
            do
            {
                count = fs.Read(buf, 0, buf.Length);
                compressedStream.Write(buf, 0, count);
            }
            while (count > 0);

            fs.Close();
            compressedStream.Close();

            //FileStream fileStream = new FileStream(pTarFile.FullName, FileMode.CreateNew);
            //byte[] buffer = ms.ToArray();

            //fileStream.Write(buffer, 0, buffer.Length);
            fileStream.Close();

            //System.Diagnostics.Process p = new System.Diagnostics.Process();
            //p.StartInfo.FileName = "7z.exe";
            //p.StartInfo.Arguments = " a \"" + pTarFile.FullName + "\" \"" + pSrcFile.FullName + "\"";
            ////p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            ////p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            ////p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            ////p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            ////p.StartInfo.CreateNoWindow = false;//不显示程序窗口
            //p.Start();//启动程序

            ////向cmd窗口发送输入信息
            ////p.StandardInput.WriteLine("7z.exe a " + destinationZipFilePath + " " + sourceFilePath + @" >> C:\tasks\7z.log /s /q &exit");

            ////p.StandardInput.AutoFlush = true;

            ////获取cmd窗口的输出信息
            ////string output = p.StandardOutput.ReadToEnd();

            //p.WaitForExit();//等待程序执行完退出进程
            //p.Close();

            //ZipFile.CreateFromDirectory(pSrcFile.FullName, pTarFile.FullName);


            // 指定7z动态库文件路径，默认是"7z.dll"  
            if (IntPtr.Size == 4)
            {
                SevenZipCompressor.SetLibraryPath(@"x86\7z.dll");
            }
            else
            {
                SevenZipCompressor.SetLibraryPath(@"x64\7z.dll");
            }

            //SevenZipBase.SetLibraryPath(@"C:\Program Files\7-Zip\7z.dll");

            //var compressor = new SevenZipCompressor();
            // 可以在构造时指定临时文件夹  
            var compressor = new SevenZipCompressor(@"C:\D\ZipTemp");

            // 打印临时文件夹路径  
            Console.WriteLine(compressor.TempFolderPath);

            // 设置压缩等级  
            compressor.CompressionLevel = SevenZip.CompressionLevel.Normal;

            // 指定压缩包格式，默认为7z。  
            // 如果使用的7za.dll则只能使用7z格式。  
            compressor.ArchiveFormat = OutArchiveFormat.SevenZip;

            // 是否保持目录结构，默认为true。  
            compressor.DirectoryStructure = true;

            // 是否包含空目录，默认true。  
            compressor.IncludeEmptyDirectories = true;

            // 压缩目录时是否使用顶层目录，默认false  
            compressor.PreserveDirectoryRoot = false;

            // 加密7z头，默认false  
            compressor.EncryptHeaders = false;

            // 文件加密算法  
            compressor.ZipEncryptionMethod = ZipEncryptionMethod.ZipCrypto;

            // 尽快压缩（不会触发*Started事件，仅触发*Finished事件）  
            compressor.FastCompression = false;

            // 单个文件开始压缩  
            compressor.FileCompressionStarted += (sender, eventArgs) =>
            {
                Console.WriteLine($"正在压缩：{eventArgs.FileName}");
                Console.WriteLine($"进度:{eventArgs.PercentDone}%");
            };

            compressor.FileCompressionStarted += (sender, eventArgs) =>
            {
                Console.WriteLine($"正在压缩：{eventArgs.FileName}");
                Console.WriteLine($"进度:{eventArgs.PercentDone}%");
            };

            // 单个文件压缩完成时  
            compressor.FileCompressionFinished += (sender, eventArgs) =>
            {
                Console.WriteLine("FileCompressionFinished");
            };

            compressor.Compressing += (sender, eventArgs) =>
            {
                Console.WriteLine(eventArgs.PercentDelta);
                Console.WriteLine(eventArgs.PercentDone);
            };

            // 压缩完成  
            compressor.CompressionFinished += (sender, eventArgs) =>
            {
                Console.WriteLine("CompressionFinished");
            };

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

            MyDic.Add("VM.7z", @"C:\VM\Windows XP SP3.ova");
            compressor.CompressFileDictionary(MyDic, @"C:\VM\test.7z");//注意第二个参数为输出压缩包的文件
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

        public static void Compress(string filePath, string zipPath)
        {
            FileStream sourceFile = File.OpenRead(filePath);
            FileStream destinationFile = File.Create(zipPath);
            byte[] buffer = new byte[sourceFile.Length];
            GZipStream zip = null;
            try
            {
                sourceFile.Read(buffer, 0, buffer.Length);
                zip = new GZipStream(destinationFile, CompressionMode.Compress);
                zip.Write(buffer, 0, buffer.Length);
            }
            catch
            {
                throw;
            }
            finally
            {
                zip.Close();
                sourceFile.Close();
                destinationFile.Close();
            }
        }

        public static void Decompress(string zipPath, string filePath)
        {
            FileStream sourceFile = File.OpenRead(zipPath);

            string path = filePath.Replace(Path.GetFileName(filePath), "");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            FileStream destinationFile = File.Create(filePath);
            GZipStream unzip = null;
            byte[] buffer = new byte[sourceFile.Length];
            try
            {
                unzip = new GZipStream(sourceFile, CompressionMode.Decompress, true);
                int numberOfBytes = unzip.Read(buffer, 0, buffer.Length);

                destinationFile.Write(buffer, 0, numberOfBytes);
            }
            catch
            {
                throw;
            }
            finally
            {
                sourceFile.Close();
                destinationFile.Close();
                unzip.Close();
            }
        }
    }
}
