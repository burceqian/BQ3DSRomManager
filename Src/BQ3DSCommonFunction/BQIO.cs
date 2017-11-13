using BQStructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQ3DSCommonFunction
{
    public class BQIO
    {
        private static string RomFolder = Environment.CurrentDirectory + @"\Roms\";
        public static List<FileInfo> GetAllFilesInDirectory(DirectoryInfo directoryInfo, List<FileInfo> fileList)
        {
            //遍历文件
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                fileList.Add(file);
            }

            //遍历文件夹
            foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
            {
                fileList = GetAllFilesInDirectory(subfolder, fileList);
            }

            return fileList;
        }

        public static void CopyRomToRomFolder(RomInfo romInfo, FileInfo file)
        {
            string lromFolder = RomFolder + romInfo.SubSerial + @"\";
            string lromfile = lromFolder + romInfo.SubSerial + file.Extension;
            FileInfo ltarFile = new FileInfo(lromfile);
            if (Directory.Exists(lromFolder) == false)
            {
                Directory.CreateDirectory(lromFolder);
            }

            if (ltarFile.Exists == false)
            {
                File.Copy(file.FullName, ltarFile.FullName);
            }
        }

        public static void CopyToRomTemp(FileInfo unknownFile)
        {
            string lromFolder = RomFolder + @"\Unknow\";
            string lromfile = lromFolder + unknownFile.Name;
            FileInfo ltarFile = new FileInfo(lromfile);
            if (Directory.Exists(lromFolder) == false)
            {
                Directory.CreateDirectory(lromFolder);
            }

            if (ltarFile.Exists == false)
            {
                File.Copy(unknownFile.FullName, ltarFile.FullName);
            }
        }
    }
}
