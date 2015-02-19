using System;
using Architecture.Business.Facade.Interface;
using Architecture.Util.Log4Net;
using Architecture.WinService.Job.Base;
using log4net;

namespace Architecture.WinService.Job
{
    public class OrderConfirmationReminderJob : BaseJob
    {
        public OrderConfirmationReminderJob(IBusinessLogicFacade businessLogicFacade)
        {
            _businessLogicFacade = businessLogicFacade;
        }

        private static readonly ILog Logger = LogManager.GetLogger(typeof(OrderConfirmationReminderJob));
        private readonly IBusinessLogicFacade _businessLogicFacade;

        public override void DoWork()
        {
            try
            {
                _businessLogicFacade.OrderManager.CreateOrderConfirmationReminders();
            }
            catch (Exception ex)
            {
                Logger.Error(() => ex);
            }
        }
    }
}
