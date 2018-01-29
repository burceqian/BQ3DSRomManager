using BQInterface;
using BQStructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BQ3DSCore
{
    public class Core
    {
        private List<IRomParser> _RomParserList = new List<IRomParser>();
        private List<IRomInfoNetCrawler> _RomInfoNetCrawlerList = new List<IRomInfoNetCrawler>();

        public void Initilize()
        {
            _RomParserList = Assembly.GetExecutingAssembly().GetTypes()
                .Where(elementClass => elementClass.IsAssignableFrom(typeof(IRomParser))).Select(type => (IRomParser)Activator.CreateInstance(type)).ToList();
            _RomInfoNetCrawlerList = Assembly.GetExecutingAssembly().GetTypes()
                .Where(elementClass => elementClass.IsAssignableFrom(typeof(IRomInfoNetCrawler))).Select(type => (IRomInfoNetCrawler)Activator.CreateInstance(type)).ToList();
        }

        public RomInformation LoadRom(FileInfo pRomFile)
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

            DownloadRomPic(lPicList);

            FiledRomFile(pRomFile);

            return lRomInformation;
        }

        public List<RomInformation> LoadRom(DirectoryInfo pDir)
        {
            List<RomInformation> lResult = new List<RomInformation>();
            pDir.GetFiles().ToList().ForEach(p => { lResult.Add(LoadRom(p)); });
            return lResult;
        }

        public void MergeRomInfo(RomInformation pBaseRomInfo, RomInformation pAdditionRomInfo)
        {
            foreach (var addromInfoProp in pAdditionRomInfo.GetType().GetProperties())
            {
                foreach (var baseRomInfoProp in pBaseRomInfo.GetType().GetProperties())
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

        public void DownloadRomPic(List<string> pPicList)
        {

        }

        public FileInfo UnCompressionRom(FileInfo pRomFile)
        {
            FileInfo lResult = new FileInfo("");
            return lResult;
        }

        public void FiledRomFile(FileInfo pRomFile)
        {

        }
    }
}
