using BQStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQInterface
{
    public interface IRomInfoNetCrawler
    {
        /// <summary>
        /// get pic like this
        /// "http://art.gametdb.com/3ds/coverM/JA/AVSJ.jpg"
        /// </summary>
        /// <param name="pRomInfo"></param>
        /// <returns></returns>
        List<string> ScanRomPic(RomInformation pRomInfo);
        RomInformation ScanRomInfo(RomInformation pRomInfo);
    }
}
