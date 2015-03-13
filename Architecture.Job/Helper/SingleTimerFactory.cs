using System;
using Architecture.Job.Base;
using Architecture.Util;
using Architecture.Util.Ninject;
using Architecture.Util.Ninject.Scope;

namespace Architecture.Job.Helper
{
    public static class SingleTimerFactory<T> where T : BaseJob
    {
        public static SingleTimer Create(int initialDelayInSeconds, int intervalInSeconds)
        {
            var job = GetJob();
            Action workAction = () =>
            {
                using (new CallContextScope())
                    job.DoWork();
            };
            return new SingleTimer(initialDelayInSeconds, intervalInSeconds, workAction, job.HandleException);
        }

        private static T GetJob()
        {
           return Factory.Resolve<T>();
        }
    }
}
