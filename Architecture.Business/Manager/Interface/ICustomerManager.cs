using Architecture.Util;
using Architecture.ViewModel;

namespace Architecture.Business.Manager.Interface
{
    public interface ICustomerManager
    {
        Paged<FindCustomers> FindCustomers(string name, PageAndSortCriteria pageAndSortCriteria);
    }
}