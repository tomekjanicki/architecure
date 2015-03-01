using System;
using System.Collections.Generic;
using Architecture.Business.Manager.Implementation.Base;
using Architecture.Business.Manager.Interface;
using Architecture.Repository.UnitOfWork.Interface;
using Architecture.Util;
using Architecture.ViewModel;
using Architecture.ViewModel.Internal;

namespace Architecture.Business.Manager.Implementation
{
    public class CustomerManager : BaseManager, ICustomerManager
    {
        internal CustomerManager(ICommandsUnitOfWork commandsUnitOfWork)
            : base(commandsUnitOfWork)
        {
        }

        public Paged<FindCustomers> FindCustomers(string name, PageAndSortCriteria pageAndSortCriteria)
        {
            return CommandsUnitOfWork.CustomerCommand.FindCustomers(name, pageAndSortCriteria);
        }

        public Tuple<int?, Dictionary<string, IList<string>>> InsertCustomer(InsertCustomer insertCustomer)
        {
            Func<List<Tuple<string, string>>> additionalValidationProviderFunc = () =>
            {
                var isUnique = CommandsUnitOfWork.CustomerCommand.IsMailUnique(new IsMailUnique{Mail = insertCustomer.Mail});
                return isUnique ? new List<Tuple<string, string>>() : new List<Tuple<string, string>> { new Tuple<string, string>(string.Empty, Const.CustomerMailIsNotUniqueMessage) } ;
            };
            return HandleValidation<int?>("insertCustomer", insertCustomer, () =>
            {
                var id = CommandsUnitOfWork.CustomerCommand.InsertCustomer(insertCustomer);
                CommandsUnitOfWork.SaveChanges();
                return id;
            }, additionalValidationProviderFunc);

        }
    }
}