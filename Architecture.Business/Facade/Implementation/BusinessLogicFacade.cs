using System;
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

        private readonly Lazy<IOrderManager> _lazyOrderManager;
        private readonly Lazy<ICustomerManager> _lazyCustomerManager;
        private readonly Lazy<IProductManager> _lazyProductManager;
        private readonly Lazy<IMailManager> _lazyMailManager;
        private readonly Lazy<IUserManager> _lazyUserManager;

        public BusinessLogicFacade(ICommandsUnitOfWork commandsUnitOfWork, IMailService mailService, ICacheService cacheService)
        {
            _commandsUnitOfWork = commandsUnitOfWork;
            _mailService = mailService;
            _cacheService = cacheService;
            _lazyOrderManager = new Lazy<IOrderManager>(() => new OrderManager(_commandsUnitOfWork));
            _lazyCustomerManager = new Lazy<ICustomerManager>(() => new CustomerManager(_commandsUnitOfWork));
            _lazyProductManager = new Lazy<IProductManager>(() => new ProductManager(_commandsUnitOfWork));
            _lazyMailManager = new Lazy<IMailManager>(() => new MailManager(_commandsUnitOfWork, _mailService));
            _lazyUserManager = new Lazy<IUserManager>(() => new UserManager(_commandsUnitOfWork, _cacheService));
        }

        public IOrderManager OrderManager
        {
            get { return _lazyOrderManager.Value; }
        }

        public ICustomerManager CustomerManager
        {
            get { return _lazyCustomerManager.Value; }
        }

        public IProductManager ProductManager
        {
            get { return _lazyProductManager.Value; }
        }

        public IMailManager MailManager
        {
            get { return _lazyMailManager.Value; }
        }

        public IUserManager UserManager
        {
            get { return _lazyUserManager.Value; }
        }

    }
}
