using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BQStructure
{
    public class Rom3dsGameInfo
    {

        public string Serial { get; set; }
        public string Subserial
        {
            get
            {
                return SetSubSerial(Serial);
            }
        }
        public string Languages { get; set; }
        public string Title
        {
            get
            {
                if (Title_ZHCN != "")
                {
                    return Title_ZHCN;
                }
                else if (Title_ZHTW != "")
                {
                    return Title_ZHTW;
                }
                else if (Title_JA != "")
                {
                    return Title_JA;
                }
                else
                {
                    return Title_EN;
                }

            }
        }
        public string Title_EN { get; set; }
        public string Title_JA { get; set; }
        public string Title_ZHTW { get; set; }
        public string Title_ZHCN { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public string Release_date { get; set; }
        public string Genre { get; set; }
        public string Players { get; set; }
        public string Imagesize { get; set; }
        public string Firmware { get; set; }
        public string Card { get; set; }
        public string Copyserial { get; set; }
        public BitmapImage SmallIcon { get; set; }
        public BitmapImage LargeIcon { get; set; }
        public bool Has3ds { get; set; }
        public bool HasCIA { get; set; }
        public bool Has3dz { get; set; }
        public bool Favorite { get; set; }

        private string SetSubSerial(string pSerial)
        {
            return pSerial.Substring(pSerial.Length - 4, 4);
        }
    }
}
