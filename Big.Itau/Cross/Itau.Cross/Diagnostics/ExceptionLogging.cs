using NLog;
using System;

namespace Itau.Common.Diagnostics
{
    public static class ExceptionLogging
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void LogException(Exception ex)
        {
            logger.Error(string.Format(
                                        "{0}\n\r{1}"
                                        , ex.Message
                                        , ex.StackTrace));

            if (ex.InnerException != null)
                LogException(ex.InnerException);
        }

        public static void LogInfo(string info)
        {
            logger.Info(string.Format(
                "{0}\n\r"
                , info));
        }

        public static void LogWarn(string info)
        {
            logger.Warn(string.Format(
                "{0}\n\r"
                , info));
        }
    }
}