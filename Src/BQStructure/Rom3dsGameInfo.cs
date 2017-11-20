using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQStructure
{
    public class Rom3dsGameInfo
    {

        public string serial { get; set; }
        public string subserial
        {
            get
            {
                return SetSubSerial(serial);
            }
        }
        public string languages { get; set; }
        public string title_EN { get; set; }
        public string title_JA { get; set; }
        public string title_ZHTW { get; set; }
        public string title_ZHCN { get; set; }
        public string developer { get; set; }
        public string publisher { get; set; }
        public string release_date { get; set; }
        public string genre { get; set; }
        public string players { get; set; }
        public string imagesize { get; set; }
        public string firmware { get; set; }
        public string card { get; set; }
        public string copyserial { get; set; }
        public bool has3ds { get; set; }
        public bool hasCIA { get; set; }
        public bool has3dz { get; set; }
        public bool favorite { get; set; }

        private string SetSubSerial(string pSerial)
        {
            return pSerial.Substring(pSerial.Length - 4, 4);
        }
    }
}
