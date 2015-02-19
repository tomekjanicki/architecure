using Architecture.Util;
using Architecture.ViewModel;

namespace Architecture.Repository.Command.Interface
{
    public interface ICustomerCommand
    {
        Paged<FindCustomers> FindCustomers(string name, PageAndSortCriteria pageAndSortCriteria);
        string GetCustomerMail(int id);
    }
}