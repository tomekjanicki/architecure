using System;
using Architecture.Util.Log4Net;
using log4net;

namespace Architecture.Job.Base
{
    public abstract class BaseJob
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BaseJob));

        public abstract void DoWork();

        public virtual void HandleException(Exception exception)
        {
            Logger.Error(() => exception);
        }

    }
}
