using Architecture.Business.Manager.Implementation.Base;
using Architecture.Business.Manager.Interface;
using Architecture.Repository.UnitOfWork.Interface;
using Architecture.Util.Exception;
using Architecture.Util.Log4Net;
using Architecture.Util.Mail.Interface;
using Architecture.ViewModel;
using Architecture.ViewModel.Internal;
using log4net;
using System.Linq;

namespace Architecture.Business.Manager.Implementation
{
    public class MailManager : BaseManager, IMailManager
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MailManager));

        private readonly IMailService _mailService;

        internal MailManager(ICommandsUnitOfWork commandsUnitOfWork, IMailService mailService)
            : base(commandsUnitOfWork)
        {
            _mailService = mailService;
        }

        public Send Send()
        {
            var successCounter = 0;
            var list = CommandsUnitOfWork.MailCommand.Find10OldestMailDefinitions().ToList();
            list.ForEach(item =>
            {
                if (ProcessItem(item))
                    successCounter++;
            });
            return new Send { TotalQty = list.Count, SuccessfulQty = successCounter };
        }

        private bool ProcessItem(Find10OldestMailDefinitions item)
        {
            try
            {
                CommandsUnitOfWork.MailCommand.UpdateTryCount(new UpdateTryCount { Id = item.Id, TryCount = item.TryCount + 1 });
                CommandsUnitOfWork.SaveChanges();
                _mailService.Send(item.MailDefinition.ConvertToMailMessage());
                CommandsUnitOfWork.MailCommand.UpdateFinished(item.Id);
                CommandsUnitOfWork.SaveChanges();
                return true;
            }
            catch (BaseException ex)
            {
                Logger.Error(() => ex);
                return false;
            }
        }

    }
}