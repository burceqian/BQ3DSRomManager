using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQStructure
{
    public class BQException:Exception
    {
        public string BQErrorMessage { get; set; }
    }
}
