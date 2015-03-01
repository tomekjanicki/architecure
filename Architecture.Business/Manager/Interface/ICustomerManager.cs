using System;
using System.Collections.Generic;
using Architecture.Util;
using Architecture.ViewModel;

namespace Architecture.Business.Manager.Interface
{
    public interface ICustomerManager
    {
        Paged<FindCustomers> FindCustomers(string name, PageAndSortCriteria pageAndSortCriteria);
        Tuple<int?, Dictionary<string, IList<string>>> InsertCustomer(InsertCustomer insertCustomer);
    }
}