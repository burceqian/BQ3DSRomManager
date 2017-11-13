using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQStructure
{
    public class RomInfo
    {
        public string Serial { get; set; }
        public string SubSerial
        {
            get
            {
                return SetSubSerial(Serial);
            }
        }
        public string Capacity { get; set; }
        public string Title_ID { get; set; }
        public CardType Card_Type { get; set; }
        public string Card_ID { get; set; }
        public string Chip_ID { get; set; }
        public string Manufacturer { get; set; }
        public string OriginalName { get; set; }
        private string SetSubSerial(string pSerial)
        {
            return pSerial.Substring(pSerial.Length - 4, 4);
        }
    }
}
