using Architecture.Business.Facade.Interface;
using Architecture.Business.Manager.Implementation;
using Architecture.Business.Manager.Interface;
using Architecture.Repository.UnitOfWork.Interface;
using Architecture.Util.Cache.Interface;
using Architecture.Util.Mail.Interface;

namespace Architecture.Business.Facade.Implementation
{
    public class BusinessLogicFacade : IBusinessLogicFacade
    {
        private readonly ICommandsUnitOfWork _commandsUnitOfWork;
        private readonly IMailService _mailService;
        private readonly ICacheService _cacheService;

        public BusinessLogicFacade(ICommandsUnitOfWork commandsUnitOfWork, IMailService mailService, ICacheService cacheService)
        {
            _commandsUnitOfWork = commandsUnitOfWork;
            _mailService = mailService;
            _cacheService = cacheService;
        }

        public IOrderManager OrderManager
        {
            get { return new OrderManager(_commandsUnitOfWork); }
        }

        public ICustomerManager CustomerManager
        {
            get { return new CustomerManager(_commandsUnitOfWork); }
        }

        public IProductManager ProductManager
        {
            get { return new ProductManager(_commandsUnitOfWork); }
        }

        public IMailManager MailManager
        {
            get { return new MailManager(_commandsUnitOfWork, _mailService); }
        }

        public IUserManager UserManager
        {
            get { return new UserManager(_commandsUnitOfWork, _cacheService); }
        }

        public void Dispose()
        {
            _commandsUnitOfWork.Dispose(); 
        }
    }
}
