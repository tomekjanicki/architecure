using System.Diagnostics.CodeAnalysis;
using Architecture.Job;
using Architecture.Job.Helper;
using Architecture.Util;
using Architecture.Util.WinService;

namespace Architecture.WinService
{

    public class WorkerAppRunner : IAppRunner
    {
        public WorkerAppRunner()
        {
            _mailQueueTimer = SingleTimerFactory<MailQueueJob>.Create(5, 30);
            _orderConfirmationReminderTimer = SingleTimerFactory<OrderConfirmationReminderJob>.Create(5, 3600);
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

        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            Extension.PublicDispose(() => Dispose(true), this);
        }

        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_orderConfirmationReminderTimer"), SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_mailQueueTimer")]
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