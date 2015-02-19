using System;
using Architecture.Util.Log4Net;
using log4net;

namespace Architecture.WinService.Job.Base
{
    public abstract class BaseJob
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BaseJob));

        public abstract void DoWork();

        public static void HandleException(Exception exception)
        {
            Logger.Error(() => exception);
        }

    }
}
