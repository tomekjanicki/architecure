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

        public Paged<FindCustomers> FindCustomers(string name, PageAndSortCriteria pageAndSortCriteria)
        {
            return CommandsUnitOfWork.CustomerCommand.FindCustomers(name, pageAndSortCriteria);
        }
    }
}