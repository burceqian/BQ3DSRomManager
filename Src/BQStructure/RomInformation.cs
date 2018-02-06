using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BQStructure
{
    public class RomInformation
    {
        public RomBasicInfo BasicInfo { get; set; }
        public RomExpandInfo ExpandInfo { get; set; }

        public RomInformation()
        {
            BasicInfo = new RomBasicInfo();
            ExpandInfo = new RomExpandInfo();
        }
    }
}
