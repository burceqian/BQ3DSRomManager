using BQStructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQInterface
{
    public interface IRomParser
    {
        RomInformation ParseRom(FileInfo pRomFile);
    }
}
