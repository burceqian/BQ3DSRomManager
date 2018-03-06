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



            ZipFile.CreateFromDirectory(pSrcFile.FullName, pTarFile.FullName);
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
