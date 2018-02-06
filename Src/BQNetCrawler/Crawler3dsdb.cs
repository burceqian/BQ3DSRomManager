using BQInterface;
using System.Collections.Generic;
using BQStructure;
using System.IO;
using System.Xml;
using BQUtility;
using System.Linq;

namespace BQNetCrawlers
{
    public class Crawler3dsdb : IRomInfoNetCrawler
    {
        List<string> IRomInfoNetCrawler.ScanRomPic(RomInformation pRomInfo)
        {
            return new List<string>();
        }

        RomInformation IRomInfoNetCrawler.ScanRomInfo(RomInformation pRomInfo)
        {
            return BQ3dsdb.GetRomInfo(pRomInfo);
        }
    }
}
