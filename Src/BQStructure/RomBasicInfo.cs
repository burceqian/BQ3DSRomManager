using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQStructure
{
    public class RomBasicInfo
    {
        /// <summary>
        /// CTR-A22J
        /// </summary>
        public string Serial { get; set; }
        /// <summary>
        /// A22J
        /// </summary>
        public string SubSerial
        {
            get
            {
                return SetSubSerial(Serial);
            }
        }
        /// <summary>
        /// 4 GB
        /// </summary>
        public string Capacity { get; set; }
        /// <summary>
        /// These flags tell the 3DS the 'Age Rating' of the software for the below regions
        /// CERO (Japan),ESRB (USA),USK (German),PEGI GEN (Europe)
        /// PEGI PRT (Portugal),PEGI BBFC (England),COB (Australia)
        /// GRB (South Korea),CGSRR (Taiwan)
        /// Japan,North America,Europe,Australia,China,Korea,Taiwan
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// This u32 flag is what the Home Menu uses to determine the Region Lockout of a title.
        /// Japan	0x01
        /// North America	0x02
        /// Europe	0x04
        /// Australia	0x08
        /// China	0x10
        /// Korea	0x20
        /// Taiwan	0x40
        /// </summary>
        public string Region_Lockout { get; set; }
        /// <summary>
        /// en,fr,de,it,es
        /// </summary>
        public string Languages { get; set; }
        /// <summary>
        /// 00040000001CC400
        /// </summary>
        public string Title_ID { get; set; }
        public string Game_Title { get; set; }
        public string Japanese_Title { get; set; }
        public string English_Title { get; set; }
        public string French_Title { get; set; }
        public string German_Title { get; set; }
        public string Italian_Title { get; set; }
        public string Spanish_Title { get; set; }
        public string Simplified_Chinese_Title { get; set; }
        public string Korean_Title { get; set; }
        public string Dutch_Title { get; set; }
        public string Portuguese_Title { get; set; }
        public string Russian_Title { get; set; }
        public string Traditional_Chinese_Title { get; set; }
        /// <summary>
        /// 1 = CTR, 2 = snake (New 3DS).
        /// </summary>
        public string Platform { get; set; }
        /// <summary>
        /// CARD2: Writable Address In Media Units (For 'On-Chip' Savedata). 
        /// CARD1: Always 0xFFFFFFFF.
        /// </summary>
        public string Card_Type { get; set; }
        public string Card_ID { get; set; }
        public string Chip_ID { get; set; }
        /// <summary>
        /// Macronix,SanDisk,OKI Semiconductor
        /// </summary>
        public string Manufacturer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OriginalName { get; set; }
        public string Developer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Publisher { get; set; }
        /// <summary>
        /// 2008
        /// </summary>
        public string ReleaseDate { get; set; }
        /// <summary>
        /// type of game
        /// </summary>
        public string Genre { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Rating { get; set; }
        /// <summary>
        /// 1 player,2 players
        /// </summary>
        public string Players { get; set; }
        /// <summary>
        /// 4G 8G
        /// </summary>
        public string Imagesize { get; set; }
        /// <summary>
        /// 10.02
        /// </summary>
        public string Firmware { get; set; }
        /// <summary>
        /// 0 No 1 Yes
        /// </summary>
        public string Favorite { get; set; }

        /// <summary>
        /// 0:No Change 1:HarkRom
        /// </summary>
        public string IsCustomsizeRom { get; set; }

        public string SourceSerial { get; set; }

        private string SetSubSerial(string pSerial)
        {
            if (pSerial != null && pSerial.Length > 4)
            {
                return pSerial.Substring(pSerial.Length - 4, 4);
            }
            return "";
        }
        public List<KeyValuePair<string, string>> DuplicateRomInfo = new List<KeyValuePair<string, string>>();
    }
}
