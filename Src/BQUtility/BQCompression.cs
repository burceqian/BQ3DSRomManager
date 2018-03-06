using System;
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
