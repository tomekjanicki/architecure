using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Architecture.Business.Manager.Implementation.Base;
using Architecture.Business.Manager.Interface;
using Architecture.Repository.UnitOfWork.Interface;
using Architecture.Util;
using Architecture.ViewModel;

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
            return await HandleValidationAsync<int?>("insertCustomer", insertCustomerAsync, async () =>
            {
                var id = await CommandsUnitOfWork.CustomerCommand.InsertCustomerAsync(insertCustomerAsync);
                CommandsUnitOfWork.SaveChanges();
                return id;
            });

        }
    }
}