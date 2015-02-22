using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<Paged<FindCustomersAsync>> FindCustomersAsync(string name, PageAndSortCriteria pageAndSortCriteria)
        {
            return await CommandsUnitOfWork.CustomerCommand.FindCustomersAsync(name, pageAndSortCriteria);
        }

        public async Task<Tuple<int?, Dictionary<string, IList<string>>>> InsertCustomerAsync(InsertCustomerAsync insertCustomerAsync)
        {
            Func<Task<List<Tuple<string, string>>>> additionalValidationProviderFunc = async () =>
            {
                var isUnique = await CommandsUnitOfWork.CustomerCommand.IsMailUniqueAsync(new IsMailUniqueAsync{Mail = insertCustomerAsync.Mail});
                return isUnique ? new List<Tuple<string, string>>() : new List<Tuple<string, string>> { new Tuple<string, string>(string.Empty, Const.CustomerMailIsNotUniqueMessage) } ;
            };
            return await HandleValidationAsync<int?>("insertCustomer", insertCustomerAsync, async () =>
            {
                var id = await CommandsUnitOfWork.CustomerCommand.InsertCustomerAsync(insertCustomerAsync);
                CommandsUnitOfWork.SaveChanges();
                return id;
            }, additionalValidationProviderFunc);

        }
    }
}