using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BQUtility
{
    public class BQLog
    {
        public static Action<string, int, int> UpdateProgressHandler{ get; set; }
        public static Action<string> WriteLogHandler { get; set; }

        public static void initilize(Action<string,int,int> updateProgressHandler)
        {
            UpdateProgressHandler = updateProgressHandler;
        }

        public static void UpdateProgress(string message, int value, int max)
        {
            if (UpdateProgressHandler != null)
            {
                UpdateProgressHandler(message, value, max);
            }
        }

        public void writelog(string log)
        {
            if (WriteLogHandler != null)
            {
                WriteLogHandler(log);
            }
        }
    }
}
