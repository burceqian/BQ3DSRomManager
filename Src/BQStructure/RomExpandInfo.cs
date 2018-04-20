using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace BQStructure
{
    public class RomExpandInfo
    {
        public BitmapImage SmallIcon { get; set; }
        public BitmapImage LargeIcon { get; set; }
        public List<BitmapImage> Convers { get; set; }
        public string RomType { get; set; }
        public bool Existed3DS { get; set; }
        public bool Existed3DZ { get; set; }
        public bool ExistedCIA { get; set; }

        public RomExpandInfo()
        {

        }
    }
}
