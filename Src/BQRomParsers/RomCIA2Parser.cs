using BQInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BQStructure;
using System.IO;
using BQUtility;

namespace BQRomParsers
{
    public class RomCIA2Parser : IRomParser
    {
        RomInformation IRomParser.ParseRom(FileInfo pRomFile)
        {
            RomInformation lRomInfo = new RomInformation();
            CIAGame cIAGame = new CIAGame(pRomFile.FullName);
            lRomInfo.BasicInfo.Serial = cIAGame.Serial;
            lRomInfo.BasicInfo.Title_ID = cIAGame.TitleId;
            lRomInfo.BasicInfo.Publisher = cIAGame.Publisher;
            lRomInfo.BasicInfo.Manufacturer = cIAGame.MakerCode;
            lRomInfo.BasicInfo.English_Title = cIAGame.Titles[0].ShortDescription;
            lRomInfo.ExpandInfo.LargeIcon = BQIO.BitmapToBitmapImage(cIAGame.LargeIcon);
            lRomInfo.ExpandInfo.SmallIcon = BQIO.BitmapToBitmapImage(cIAGame.SmallIcon);
            return lRomInfo;
        }
    }
}
