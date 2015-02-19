using System.Collections.Generic;
using Architecture.Util;
using Architecture.ViewModel;

namespace Architecture.Business.Manager.Interface
{
    public interface IProductManager
    {
        Paged<FindProducts> FindProducts(string code, string name, PageAndSortCriteria pageAndSortCriteria);
        IEnumerable<FindProducts> FindProducts(string code, string name, string sort);
        Dictionary<string, IList<string>> DeleteProduct(DeleteProduct deleteProduct);
    }
}