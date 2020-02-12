using System.Diagnostics;

namespace CrazyBuy.Common
{
    public class MDebugLog
    {
        private static readonly bool DEBUG_MODE = true;

        public static void debug(string content)
        {
            writeLog("[debug]" + content);
        }

        public static void error(string content)
        {
            writeLog("[error]" + content);
        }

        private static void writeLog(string content)
        {
            if (DEBUG_MODE)
            {
                Debug.WriteLine(content);
            }
        }
    }
}
