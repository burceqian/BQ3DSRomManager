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
        RomInformation ScanRomInfo(RomInformation pRomInfo);
    }
}
