using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ahead.Data
{
    public class LogInterface
    {
        private static log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogInfo(string msg)
        {
            Logger.Info(msg);
        }

    }
}