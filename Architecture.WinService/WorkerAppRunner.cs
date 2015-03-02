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

        private readonly SingleTimer _mailQueueTimer;
        private readonly SingleTimer _orderConfirmationReminderTimer;

        public void OnStart(string[] args)
        {
            _mailQueueTimer.Start();
            _orderConfirmationReminderTimer.Start();
        }

        public void OnStop()
        {
            _mailQueueTimer.Stop();
            _orderConfirmationReminderTimer.Stop();
        }
    }
}