using System;
using System.Collections.Generic;
using System.IO;

namespace BQ3DSCommonFunction
{
    public class BQZip
    {
        private static string DECOMPRESSION_TEMPDIR = Path.Combine(Environment.CurrentDirectory,@"\Temp\UnZip\");

        public static void UnZipFile(FileInfo file)
        {
            if (Directory.Exists(DECOMPRESSION_TEMPDIR) == false)
            {
                Directory.CreateDirectory(DECOMPRESSION_TEMPDIR);
            }

            SevenZipExtractor.ArchiveFile archiveFile = new SevenZipExtractor.ArchiveFile(file.FullName);
            archiveFile.Extract(DECOMPRESSION_TEMPDIR);
        }

        public static List<FileInfo> GetUnZipRom()
        {
            return GetAllFilesInDirectory(new DirectoryInfo(DECOMPRESSION_TEMPDIR), new List<FileInfo>());
        }

        public static void ClearUnZipFolder()
        {
            DirectoryInfo dir = new DirectoryInfo(DECOMPRESSION_TEMPDIR);
            if (dir.Exists)
            {
                dir.Delete(true);
            }
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
