using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQ3DSCommonFunction
{
    public class BQZip
    {
        private static string UnZipPath = Environment.CurrentDirectory + @"\UnZip\";

        public static void UnZipFile(FileInfo file)
        {
            if (Directory.Exists(UnZipPath) == false)
            {
                Directory.CreateDirectory(UnZipPath);
            }

            SevenZipExtractor.ArchiveFile archiveFile = new SevenZipExtractor.ArchiveFile(file.FullName);
            archiveFile.Extract(UnZipPath);
        }

        public static List<FileInfo> GetUnZipRom()
        {
            return GetAllFilesInDirectory(new DirectoryInfo(UnZipPath), new List<FileInfo>());
        }

        public static void ClearUnZipFolder()
        {
            DirectoryInfo dir = new DirectoryInfo(UnZipPath);
            if (dir.Exists)
            {
                dir.Delete(true);
            }
        }

        public static List<FileInfo> GetAllFilesInDirectory(DirectoryInfo directoryInfo, List<FileInfo> fileList)
        {
            //遍历文件
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (file.Extension.ToLower() == ".3ds" ||
                    file.Extension.ToLower() == ".cia")
                {
                    fileList.Add(file);
                }
            }

            //遍历文件夹
            foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
            {
                fileList = GetAllFilesInDirectory(subfolder, fileList);
            }

            return fileList;
        }
    }
}
