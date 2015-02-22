using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Architecture.Util;
using Architecture.ViewModel;

namespace Architecture.Business.Manager.Interface
{
    public interface ICustomerManager
    {
        Task<Paged<FindCustomersAsync>> FindCustomersAsync(string name, PageAndSortCriteria pageAndSortCriteria);
        Task<Tuple<int?, Dictionary<string, IList<string>>>> InsertCustomerAsync(InsertCustomerAsync insertCustomerAsync);
    }
}