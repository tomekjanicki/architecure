using System.Threading.Tasks;
using Architecture.Util;
using Architecture.ViewModel;
using Architecture.ViewModel.Internal;

namespace Architecture.Repository.Command.Interface
{
    public interface ICustomerCommand
    {
        Task<Paged<FindCustomersAsync>> FindCustomersAsync(string name, PageAndSortCriteria pageAndSortCriteria);
        Task<int> InsertCustomerAsync(InsertCustomerAsync insertCustomerAsync);
        Task<bool> IsMailUniqueAsync(IsMailUniqueAsync isMailUniqueAsync);
        string GetCustomerMail(int id);
    }
}