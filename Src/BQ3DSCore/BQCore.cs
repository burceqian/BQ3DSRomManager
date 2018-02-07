using BQInterface;
using BQStructure;
using BQUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace BQ3DSCore
{
    public class BQCore
    {
        private static List<IRomParser> _RomParserList = new List<IRomParser>();
        private static List<IRomInfoNetCrawler> _RomInfoNetCrawlerList = new List<IRomInfoNetCrawler>();

        public static void Initialize()
        {
            // initialize components
            //_RomParserList = Assembly.GetExecutingAssembly().GetTypes()
            _RomParserList = Assembly.Load("BQRomParsers").GetTypes()
                .Where(elementClass => elementClass.IsAssignableFrom(typeof(IRomParser))).Select(type => (IRomParser)Activator.CreateInstance(type)).ToList();
            _RomInfoNetCrawlerList = Assembly.Load("BQNetCrawlers").GetTypes()
                .Where(elementClass => elementClass.IsAssignableFrom(typeof(IRomInfoNetCrawler))).Select(type => (IRomInfoNetCrawler)Activator.CreateInstance(type)).ToList();

            // initialize work folder
            BQDirectory.Initilize();

            // Check3dsdb
            if (!BQ3dsdb.Check3dsdb())
            {
                BQ3dsdb.Update3dsdb();
            }

            // CheckDB
            if (!BQDB.CheckDBExist())
            {
                BQDB.CreateNewDB();
                List<RomInformation> AllRomInfoList = BQ3dsdb.GetAllRomInfo();
                AllRomInfoList.ForEach(rominfo=> BQDB.InsertRomInfo(rominfo.BasicInfo));
            }
        }

        public static List<RomInformation> InitializeFirstRomList()
        {
            List < RomInformation > lAllRomInfo = BQDB.GetAllGameInfo();

            foreach (var romInfo in lAllRomInfo)
            {
                romInfo.ExpandInfo.LargeIcon = BQIO.GetRomLargeIco(romInfo);
                romInfo.ExpandInfo.SmallIcon = BQIO.GetRomSmallIco(romInfo);
            }

            return lAllRomInfo;
        }

        private static RomInformation ParseRom(FileInfo pRomFile)
        {
            RomInformation lRomInformation = new RomInformation();
            foreach (var romParser in _RomParserList)
            {
                MergeRomInfo(lRomInformation, romParser.ParseRom(pRomFile));
            }

            List<string> lPicList = new List<string>();

            foreach (var item in _RomInfoNetCrawlerList)
            {
                lPicList.AddRange(item.ScanRomPic(lRomInformation));
            }

            BQIO.DownloadRomPicToFolder(lPicList);

            BQIO.SaveLargeIco(lRomInformation.ExpandInfo.LargeIcon, lRomInformation);

            BQIO.SaveLargeIco(lRomInformation.ExpandInfo.SmallIcon, lRomInformation);

            return lRomInformation;
        }

        public static List<RomInformation> LoadRom(FileInfo pRomFile)
        {
            List<RomInformation> lRomInformationList = new List<RomInformation>();

            if (BQSpecs.CompressionFileExtension.Contains(pRomFile.Extension))
            {

                BQCompression.UnCompressionFile(pRomFile, new DirectoryInfo(BQDirectory.TempDir));
                List<FileInfo> lFileList = BQIO.GetRomFile(new DirectoryInfo(BQDirectory.TempDir));
                foreach (var file in lFileList)
                {
                    RomInformation tRomInformation = ParseRom(pRomFile);
                    lRomInformationList.Add(tRomInformation);
                }
            }
            else
            {
                RomInformation tRomInformation = ParseRom(pRomFile);
                lRomInformationList.Add(tRomInformation);
            }

            return lRomInformationList;

            //ParseRom()

            //List<FileInfo> lRomFileList = FiledRomFile(pRomFile);



            //foreach (var romFile in lRomFileList)
            //{
            //    RomInformation tRomInformation = new RomInformation();
            //    foreach (var romParser in _RomParserList)
            //    {
            //        MergeRomInfo(tRomInformation, romParser.ParseRom(pRomFile));
            //    }

            //    List<string> lPicList = new List<string>();

            //    foreach (var item in _RomInfoNetCrawlerList)
            //    {
            //        lPicList.AddRange(item.ScanRomPic(tRomInformation));
            //    }

            //    BQIO.GetRomConverPic DownloadRomPic(lPicList);

            //    lRomInformationList.Add(tRomInformation);

            //    BQIO.CopyRomToRomFolder(tRomInformation, romFile);
            //}

            return lRomInformationList;
        }

        public static List<RomInformation> LoadRom(DirectoryInfo pDir)
        {
            List<RomInformation> lResult = new List<RomInformation>();
            pDir.GetFiles().ToList().ForEach(p => { lResult.AddRange(LoadRom(p)); });
            return lResult;
        }

        public static void MergeRomInfo(RomInformation pBaseRomInfo, RomInformation pAdditionRomInfo)
        {
            foreach (var addromInfoProp in pAdditionRomInfo.BasicInfo.GetType().GetProperties())
            {
                foreach (var baseRomInfoProp in pBaseRomInfo.BasicInfo.GetType().GetProperties())
                {
                    if (baseRomInfoProp.Name == addromInfoProp.Name)
                    {
                        var baseValue = baseRomInfoProp.GetValue(pBaseRomInfo);
                        if (baseValue.ToString() == "")
                        {
                            baseRomInfoProp.SetValue(pBaseRomInfo, addromInfoProp.GetValue(pAdditionRomInfo));
                        }
                    }
                }
            }
        }

        public static List<FileInfo> FiledRomFile(FileInfo pRomFile)
        {
            List<FileInfo> lResult = new List<FileInfo>();

            if (BQSpecs.RomFileExtension.Contains(pRomFile.Extension.ToLower()))
            {
                BQCompression.UnCompressionFile(pRomFile, new DirectoryInfo(BQDirectory.TempDir));
                List<FileInfo> lUnCompressionFiles = BQIO.GetRomFile(new DirectoryInfo(BQDirectory.TempDir));
                if (lUnCompressionFiles.Count == 0)
                {
                    lResult = lUnCompressionFiles;
                }
            }
            else
            {
                lResult.Add(pRomFile);
            }

            return lResult;
        }
    }
}
