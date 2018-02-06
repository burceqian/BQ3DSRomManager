using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQRomParsers
{
    public enum NCCHContentType : byte
    {
        Child = 12,
        ExeFS = 2,
        Manual = 8,
        RomFS = 1,
        SystemUpdate = 4,
        Trial = 0x10
    }
}
