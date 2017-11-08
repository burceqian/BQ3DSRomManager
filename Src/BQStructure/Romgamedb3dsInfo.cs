using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQStructure
{
    public class Romgamedb3dsInfo
    {
        public string serial { get; set; }
        public string id { get; set; }
        public string region { get; set; }
        public string type { get; set; }
        public string languages { get; set; }
        public string title_1EN_2 { get; set; }//_1 = ( _2 =)
        public string title_1JA_2 { get; set; }//_1 = ( _2 =)
        public string title_1ZHTW_2 { get; set; }//_1 = ( _2 =)
        public string developer { get; set; }
        public string publisher { get; set; }
        public string release_3date{ get; set; } // _3 = " "
        public string genre { get; set; }
        public string rating { get; set; }
        public string players { get; set; }
        public string req_4accessories { get; set; } // _4 = .
        public string accessories { get; set; }
        public string online_3players { get; set; }
        public string save_3blocks { get; set; }
        public string __case { get; set; }// __ = ""
    }
}
