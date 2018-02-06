using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQRomParsers
{
    public enum NCCHPartitionType
    {
        CFADLPChild = 8,
        CFAManual = 4,
        CFASimple = 2,
        CFAUpdate = 0x10,
        CXI = 1,
        Unknown = 0
    }
}
