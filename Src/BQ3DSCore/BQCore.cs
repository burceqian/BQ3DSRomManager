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
                .Where(elementClass => typeof(IRomParser).IsAssignableFrom(elementClass)).Select(type => (IRomParser)Activator.CreateInstance(type)).ToList();
            _RomInfoNetCrawlerList = Assembly.Load("BQNetCrawlers").GetTypes()
                .Where(elementClass => typeof(IRomInfoNetCrawler).IsAssignableFrom(elementClass)).Select(type => (IRomInfoNetCrawler)Activator.CreateInstance(type)).ToList();

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
                BQDB.InsertRomInfoList(AllRomInfoList);
                //AllRomInfoList.ForEach(rominfo=> BQDB.InsertRomInfo(rominfo.BasicInfo));
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

            if (lPicList.Count > 0)
            {
                BQIO.DownloadRomPicToFolder(lPicList);
            }

            if (lRomInformation.ExpandInfo.LargeIcon != null)
            {
                BQIO.SaveLargeIco(lRomInformation.ExpandInfo.LargeIcon, lRomInformation);
            }

            if (lRomInformation.ExpandInfo.SmallIcon != null)
            {
                BQIO.SaveSmallIco(lRomInformation.ExpandInfo.SmallIcon, lRomInformation);
            }

            if (lRomInformation.BasicInfo.Serial != "")
            {
                RomInformation lDBRomInformation = BQDB.GetGameInfo(lRomInformation.BasicInfo.Serial);
                MergeRomInfo(lDBRomInformation, lRomInformation);
                BQDB.UpdateGameInfo(lDBRomInformation.BasicInfo);
                MergeRomInfo(lRomInformation, lDBRomInformation);
            }

            if (BQIO.CheckRomFileExist(lRomInformation)== false)
            {
                BQIO.CopyRomToRomFolder(lRomInformation, pRomFile);
            }

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
                    if (baseRomInfoProp.Name != "SubSerial" && baseRomInfoProp.Name == addromInfoProp.Name)
                    {
                        var baseValue = baseRomInfoProp.GetValue(pBaseRomInfo.BasicInfo);
                        var tarValue = addromInfoProp.GetValue(pAdditionRomInfo.BasicInfo);
                        if ((baseValue == null || baseValue.ToString().Trim() == "") &&
                            (tarValue != null && tarValue.ToString().Trim() != ""))
                        {
                            baseRomInfoProp.SetValue(pBaseRomInfo.BasicInfo, tarValue);
                        }
                    }
                }
            }

            if (pBaseRomInfo.ExpandInfo.LargeIcon == null && pAdditionRomInfo.ExpandInfo.LargeIcon != null)
            {
                pBaseRomInfo.ExpandInfo.LargeIcon = pAdditionRomInfo.ExpandInfo.LargeIcon;
            }

            if (pBaseRomInfo.ExpandInfo.SmallIcon == null && pAdditionRomInfo.ExpandInfo.SmallIcon != null)
            {
                pBaseRomInfo.ExpandInfo.SmallIcon = pAdditionRomInfo.ExpandInfo.SmallIcon;
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
