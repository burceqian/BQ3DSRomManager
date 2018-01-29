using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQ3DSQLite.Struct
{
    public class SQLContect
    {
        private List<string> _paraNameList = new List<string>();
        private List<string> _paraValueList = new List<string>();
        public string SQL { get; set; }
        public List<string> ParaNameList { get { return _paraNameList; } set { _paraNameList = value; } }
        public List<string> ParaValueList { get { return _paraValueList; } set { _paraValueList = value; } }
    }
}
