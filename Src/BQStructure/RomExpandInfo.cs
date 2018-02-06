using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BQStructure
{
    public class RomExpandInfo
    {
        public BitmapImage SmallIcon { get; set; }
        public BitmapImage LargeIcon { get; set; }
        public List<BitmapImage> Convers { get; set; }
        public bool Existed { get; set; }

        public RomExpandInfo()
        {

        }
    }
}
