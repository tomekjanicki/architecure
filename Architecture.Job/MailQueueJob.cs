using System;
using Architecture.Business.Facade.Interface;
using Architecture.Job.Base;
using Architecture.Util.Log4Net;
using log4net;

namespace Architecture.Job
{
    public class MailQueueJob : BaseJob
    {
        private readonly IBusinessLogicFacade _businessLogicFacade;

        public MailQueueJob(IBusinessLogicFacade businessLogicFacade)
        {
            _businessLogicFacade = businessLogicFacade;
        }

        private static readonly ILog Logger = LogManager.GetLogger(typeof(MailQueueJob));

        public override void DoWork()
        {
            try
            {
                var result = _businessLogicFacade.MailManager.Send();
                Logger.Debug(() => string.Format("Successfully sent {0} from {1} messages", result.SuccessfulQty, result.TotalQty));
            }
            catch (Exception ex)
            {
                Logger.Error(() => ex);
            }
        }

    }
}