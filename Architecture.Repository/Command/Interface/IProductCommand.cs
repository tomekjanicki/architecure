using System.Collections.Generic;
using Architecture.Util;
using Architecture.ViewModel;

namespace Architecture.Repository.Command.Interface
{
    public interface IProductCommand
    {
        Paged<FindProducts> FindProducts(string code, string name, PageAndSortCriteria pageAndSortCriteria);
        IEnumerable<FindProducts> FindProducts(string code, string name, string sort);
        byte[] GetProductVersion(int id);
        void DeleteProduct(DeleteProduct deleteProduct);
        bool CanDelete(int id);
    }
}