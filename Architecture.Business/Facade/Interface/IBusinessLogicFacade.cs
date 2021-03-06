﻿using Architecture.Business.Manager.Interface;

namespace Architecture.Business.Facade.Interface
{
    public interface IBusinessLogicFacade
    {
        IOrderManager OrderManager { get; }
        ICustomerManager CustomerManager { get; }
        IProductManager ProductManager { get; }
        IMailManager MailManager { get; }
        IUserManager UserManager { get; }
    }
}