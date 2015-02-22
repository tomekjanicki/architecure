using System.Threading.Tasks;
using Architecture.Util;
using Architecture.ViewModel;

namespace Architecture.Repository.Command.Interface
{
    public interface ICustomerCommand
    {
        Task<Paged<FindCustomersAsync>> FindCustomersAsync(string name, PageAndSortCriteria pageAndSortCriteria);
        Task<int> InsertCustomerAsync(InsertCustomerAsync insertCustomerAsync);
        string GetCustomerMail(int id);
    }
}