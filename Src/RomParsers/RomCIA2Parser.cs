using BQInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BQStructure;
using System.IO;

namespace BQRomParsers
{
    public class RomCIA2Parser : IRomParser
    {
        RomInformation IRomParser.ParseRom(FileInfo pRomFile)
        {
            RomInformation lRomInfo = new RomInformation();
            CIAGame cIAGame = new CIAGame(pRomFile.FullName);
            lRomInfo.Serial = cIAGame.Serial;
            lRomInfo.Title_ID = cIAGame.TitleId;
            lRomInfo.English_Title = cIAGame.Titles[0].ShortDescription;
            return lRomInfo;
        }
    }
}
