using Architecture.Util;
using Architecture.ViewModel;
using Architecture.ViewModel.Internal;

namespace Architecture.Repository.Command.Interface
{
    public interface ICustomerCommand
    {
        Paged<FindCustomers> FindCustomers(string name, PageAndSortCriteria pageAndSortCriteria);
        int InsertCustomer(InsertCustomer insertCustomer);
        bool IsMailUnique(IsMailUnique isMailUnique);
        string GetCustomerMail(int id);
    }
}