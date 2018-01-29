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
        List<string> ScanRomPic(RomInformation pRomInfo);
        RomInformation ScanRomInfo(RomInformation pRomInfo);
    }
}
