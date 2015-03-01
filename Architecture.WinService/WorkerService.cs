using System.ServiceProcess;
using Architecture.Util;
using Architecture.Util.Ninject;
using Architecture.Util.Ninject.Scope;
using Architecture.WinService.Job;
using Architecture.WinService.Job.Base;

namespace Architecture.WinService
{
    public partial class WorkerService : ServiceBase
    {
        public WorkerService()
        {
            InitializeComponent();
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

        protected override void OnStart(string[] args)
        {
            _mailQueueTimer.Start();
            _orderConfirmationReminderTimer.Start();
        }

        protected override void OnStop()
        {
            _mailQueueTimer.Stop();
            _orderConfirmationReminderTimer.Stop();
        }
    }
}
