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
            _RomParserList = Assembly.Load("BQRomParsers").GetTypes()
                .Where(elementClass => typeof(IRomParser).IsAssignableFrom(elementClass)).Select(type => (IRomParser)Activator.CreateInstance(type)).ToList();
            _RomInfoNetCrawlerList = Assembly.Load("BQNetCrawlers").GetTypes()
                .Where(elementClass => typeof(IRomInfoNetCrawler).IsAssignableFrom(elementClass)).Select(type => (IRomInfoNetCrawler)Activator.CreateInstance(type)).ToList();

            // initialize work folder
            BQDirectory.Initilize();

            // Check3dsdb
            try
            {
                BQLog.UpdateProgress("初始化Rom信息", 2, 5);
                if (!BQ3dsdbXML.Check3dsdb())
                {
                    BQ3dsdbXML.Update3dsdbXMLFile();
                }
            }
            catch (Exception ex)
            {
                throw new BQException() { Source = ex.Source, BQErrorMessage = "Update DB Error" };
            }

            try
            {
                BQLog.UpdateProgress("初始化Rom数据库", 3, 5);
                // CheckDB
                if (!BQDB.CheckDBExist())
                {
                    BQDB.CreateNewDB();
                    List<RomInformation> AllRomInfoList = BQ3dsdbXML.GetAllRomInfo();
                    BQDB.InsertRomInfoList(AllRomInfoList);
                }
            }
            catch (Exception ex)
            {
                throw new BQException() {  Source=ex.Source, BQErrorMessage="Update DB Error"};
            }
        }

        public static List<RomInformation> InitializeFirstRomList()
        {
            List<RomInformation> lAllRomInfo = new List<RomInformation>();
            List<string> lRomSerialList = BQIO.GetAllRomFileFromLocal();

            for (int i = 0; i < lRomSerialList.Count; i++)
            {
                lAllRomInfo.Add(BQDB.GetGameInfo(lRomSerialList[i]));
            }

            foreach (var romInfo in lAllRomInfo)
            {
                romInfo.ExpandInfo.LargeIcon = BQIO.GetRomLargeIco(romInfo);
                romInfo.ExpandInfo.SmallIcon = BQIO.GetRomSmallIco(romInfo);
            }

            return lAllRomInfo;
        }

        public static List<RomInformation> GetAllRomList()
        {
            List<RomInformation> lAllRomInfo = BQDB.GetAllGameInfo();

            foreach (var romInfo in lAllRomInfo)
            {
                romInfo.ExpandInfo.LargeIcon = BQIO.GetRomLargeIco(romInfo);
                romInfo.ExpandInfo.SmallIcon = BQIO.GetRomSmallIco(romInfo);
            }

            return lAllRomInfo;
        }

        private static RomInformation ParseRom(FileInfo pRomFile,bool isCNRom = false)
        {
            RomInformation lRomInformation = new RomInformation();

            lRomInformation.ExpandInfo.RomType = pRomFile.Extension.TrimStart('.');

            foreach (var romParser in _RomParserList)
            {
                MergeRomInfo(lRomInformation, romParser.ParseRom(pRomFile));
            }

            if (lRomInformation.BasicInfo.Serial != "")
            {
                lRomInformation.BasicInfo.SourceSerial = lRomInformation.BasicInfo.Serial;
                UpdateRomInfoToDB(lRomInformation);
                if (isCNRom)
                {
                    lRomInformation.BasicInfo.SourceSerial = lRomInformation.BasicInfo.Serial;
                    lRomInformation.BasicInfo.Serial = CreateCNSerial(lRomInformation.BasicInfo.SourceSerial);
                    UpdateRomInfoToDB(lRomInformation);
                }
            }

            if (lRomInformation.ExpandInfo.LargeIcon != null)
            {
                BQIO.SaveLargeIco(lRomInformation.ExpandInfo.LargeIcon, lRomInformation);
            }

            if (lRomInformation.ExpandInfo.SmallIcon != null)
            {
                BQIO.SaveSmallIco(lRomInformation.ExpandInfo.SmallIcon, lRomInformation);
            }

            if (BQIO.CheckRomFileExist(lRomInformation)== false)
            {
                BQIO.CopyRomToRomFolder(lRomInformation, pRomFile);
            }

            return lRomInformation;
        }

        private static void UpdateRomInfoToDB(RomInformation pRomInfo)
        {
            RomInformation lTempRomInformation = BQDB.GetGameInfo(pRomInfo.BasicInfo.Serial);

            if (lTempRomInformation != null)
            {
                if (MergeRomInfo(pRomInfo, lTempRomInformation))
                {
                    BQDB.UpdateGameInfo(pRomInfo.BasicInfo);
                }
            }
            else
            {
                BQDB.InsertRomInfo(pRomInfo.BasicInfo);
            }
        }

        public static string CreateCNSerial(string pSourceSerial)
        {
            return "CNR-" + pSourceSerial.Substring(pSourceSerial.Length - 4, 4);
        }

        public static List<RomInformation> LoadRom(FileInfo pRomFile,bool isCNRom = false)
        {
            List<RomInformation> lRomInformationList = new List<RomInformation>();

            if (BQSpecs.CompressionFileExtension.Contains(pRomFile.Extension))
            {

                BQCompression.UnCompressionFile(pRomFile, new DirectoryInfo(BQDirectory.TempDir));
                List<FileInfo> lFileList = BQIO.GetRomFile(new DirectoryInfo(BQDirectory.TempDir));
                foreach (var file in lFileList)
                {
                    RomInformation tRomInformation = ParseRom(pRomFile, isCNRom);
                    lRomInformationList.Add(tRomInformation);
                }
            }
            else
            {
                RomInformation tRomInformation = ParseRom(pRomFile, isCNRom);
                lRomInformationList.Add(tRomInformation);
            }

            return lRomInformationList;
        }



        public static bool MergeRomInfo(RomInformation pBaseRomInfo, RomInformation pAdditionRomInfo)
        {
            bool lHasDiff = false;

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
                            lHasDiff = true;
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

            return lHasDiff;
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


        //public static List<RomInformation> LoadRom(DirectoryInfo pInputDir, FileInfo pInputFile, InputFileType pType)
        //{
        //    List<FileInfo> lInputFiles = null;
        //    if (pType == InputFileType.CompressionFile)
        //    {
        //        BQCompression.UnCompressionFile(pInputFile, new DirectoryInfo(BQDirectory.TempDir));
        //        lInputFiles = BQIO.GetRomFile(new DirectoryInfo(BQDirectory.TempDir));
        //    }
        //    else if (pType == InputFileType.Directory)
        //    {
        //        DirectoryInfo tInputDir = new DirectoryInfo(pInput);
        //        lInputFiles = BQIO.GetRomFile(new DirectoryInfo(BQDirectory.TempDir));
        //    }
        //}

        #region 整理好的
        public static List<RomInformation> LoadRom(DirectoryInfo pInputDir)
        {
            List<RomInformation> lResult = new List<RomInformation>();
            List<FileInfo> lFileList = BQIO.GetAllFiles(pInputDir);

            foreach (var file in lFileList)
            {
                if (BQSpecs.CompressionFileExtension.Contains(file.Extension) ||
                    BQSpecs.RomFileExtension.Contains(file.Extension))
                {
                    lResult.AddRange(LoadRom(file)); 
                }
            }
            return lResult;
        }

        public static List<RomInformation> LoadRom(FileInfo pInputFile)
        {
            List<RomInformation> lRomInformationList = new List<RomInformation>();

            List<FileInfo> tRomFiles = null;

            tRomFiles = PreTreatRom(pInputFile);

            foreach (var romFile in tRomFiles)
            {
                RomInformation tRomInfo = ParseRom(romFile);
                if (tRomInfo != null)
                {
                    lRomInformationList.Add(tRomInfo);
                }
            }

            return lRomInformationList;
        }

        private static List<FileInfo> PreTreatRom(FileInfo pRomFile)
        {
            if (BQSpecs.CompressionFileExtension.Contains(pRomFile.Extension))
            {
                BQCompression.UnCompressionFile(pRomFile, new DirectoryInfo(BQDirectory.TempDir));
                return BQIO.GetRomFile(new DirectoryInfo(BQDirectory.TempDir));
            }
            else
            {
                return new List<FileInfo>() { pRomFile };
            }
        }

        private static RomInformation ParseRom(FileInfo pRomFile)
        {
            RomInformation lRomInformation = new RomInformation();

            lRomInformation.ExpandInfo.RomType = pRomFile.Extension.TrimStart('.');

            BQLog.WriteMsgToUI("解析Rom。");

            foreach (var romParser in _RomParserList)
            {
                MergeRomInfo(lRomInformation, romParser.ParseRom(pRomFile));
            }

            if (lRomInformation.BasicInfo.Serial == "")
            {
                BQLog.WriteMsgToUI("解析Rom失败。");
                return null;
            }

            if (lRomInformation.ExpandInfo.LargeIcon != null)
            {
                BQLog.WriteMsgToUI("保持大图标文件");
                BQIO.SaveLargeIco(lRomInformation.ExpandInfo.LargeIcon, lRomInformation);
            }

            if (lRomInformation.ExpandInfo.SmallIcon != null)
            {
                BQLog.WriteMsgToUI("保持小图标文件");
                BQIO.SaveSmallIco(lRomInformation.ExpandInfo.SmallIcon, lRomInformation);
            }

            BQLog.WriteMsgToUI("复制Rom文件");
            BQIO.CopyRomToRomFolder(lRomInformation, pRomFile);
  
            return lRomInformation;
        }

        private static void AfterParseRom()
        {
            BQCompression.ClearUnCompressionFolder();
        }
        #endregion
    }
}
