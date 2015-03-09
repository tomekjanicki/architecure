using Architecture.Util;
using Architecture.Util.Ninject;
using Architecture.Util.Ninject.Scope;
using Architecture.Util.WinService;
using Architecture.WinService.Job;
using Architecture.WinService.Job.Base;

namespace Architecture.WinService
{
    public class WorkerAppRunner : IAppRunner
    {
        public WorkerAppRunner()
        {
            _mailQueueTimer = new SingleTimer(5, 30, DoWork<MailQueueJob>, BaseJob.HandleException);
            _orderConfirmationReminderTimer = new SingleTimer(5, 3600, DoWork<OrderConfirmationReminderJob>, BaseJob.HandleException);
        }

        private static void DoWork<T>() where T : BaseJob
        {
            using (new CallContextScope())
                Factory.Resolve<T>().DoWork();
        }

        private SingleTimer _mailQueueTimer;
        private SingleTimer _orderConfirmationReminderTimer;
        private bool _disposed;

        public void OnStart(string[] args)
        {
            EnsureNotDisposed();
            _mailQueueTimer.Start();
            _orderConfirmationReminderTimer.Start();
        }

        public void OnStop()
        {
            EnsureNotDisposed();
            _mailQueueTimer.Stop();
            _orderConfirmationReminderTimer.Stop();
        }

        public void Dispose()
        {
            Extension.PublicDispose(() => Dispose(true), this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Extension.ProtectedDispose(ref _disposed, disposing, () =>
            {
                Extension.StandardDispose(ref _mailQueueTimer);
                Extension.StandardDispose(ref _orderConfirmationReminderTimer);
            });
        }

        private void EnsureNotDisposed()
        {
            Extension.EnsureNotDisposed<WorkerAppRunner>(_disposed);
        }

    }
}