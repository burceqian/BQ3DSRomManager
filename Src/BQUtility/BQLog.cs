using System;

namespace BQUtility
{
    public class BQLog
    {
        public static Action<string, int, int> UpdateProgressHandler{ get; set; }
        public static Action<string> WriteLogHandler { get; set; }
        public static Action<string> WriteUIHandler { get; set; }

        public static void UpdateProgress(string message, int value, int max)
        {
            UpdateProgressHandler?.Invoke(message, value, max);
        }

        public static void WriteLog(string log)
        {
            WriteLogHandler?.Invoke(log);
        }

        public static void WriteMsgToUI(string msg)
        {
            WriteUIHandler?.Invoke(msg);
        }
    }
}
