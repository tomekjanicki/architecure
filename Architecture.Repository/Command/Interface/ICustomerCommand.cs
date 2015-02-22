using System.Threading.Tasks;
using Architecture.Util;
using Architecture.ViewModel;

namespace Architecture.Repository.Command.Interface
{
    public interface ICustomerCommand
    {
        Task<Paged<FindCustomers>> FindCustomersAsync(string name, PageAndSortCriteria pageAndSortCriteria);
        Task<int> InsertCustomerAsync(InsertCustomer insertCustomer);
        string GetCustomerMail(int id);
    }
}