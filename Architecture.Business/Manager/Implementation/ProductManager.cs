using System;
using System.Collections.Generic;
using System.Globalization;
using Architecture.Business.Manager.Implementation.Base;
using Architecture.Business.Manager.Interface;
using Architecture.Repository.UnitOfWork.Interface;
using Architecture.Util;
using Architecture.ViewModel;

namespace Architecture.Business.Manager.Implementation
{
    public class ProductManager : BaseManager, IProductManager
    {
        internal ProductManager(ICommandsUnitOfWork commandsUnitOfWork)
            : base(commandsUnitOfWork)
        {
        }

        public Paged<FindProducts> FindProducts(string code, string name, PageAndSortCriteria pageAndSortCriteria)
        {
            return CommandsUnitOfWork.ProductCommand.FindProducts(code, name, pageAndSortCriteria);
        }

        public IEnumerable<FindProducts> FindProducts(string code, string name, string sort)
        {
            return CommandsUnitOfWork.ProductCommand.FindProducts(code, name, sort);
        }

        public Dictionary<string, IList<string>> DeleteProduct(DeleteProduct deleteProduct)
        {
            Func<List<Tuple<string, string>>> additionalValidationProviderFunc = () => !CommandsUnitOfWork.ProductCommand.CanDelete(deleteProduct.Id) ? new List<Tuple<string, string>> { new Tuple<string, string>(string.Empty, Const.CantDeleteProductMessage) } : new List<Tuple<string, string>>();
            return HandleValidation("deleteProduct", deleteProduct, () =>
            {
                HandleConcurrency(() => CommandsUnitOfWork.ProductCommand.GetProductVersion(deleteProduct.Id), deleteProduct.Version, deleteProduct.Id.ToString(CultureInfo.InvariantCulture), typeof(DeleteProduct));
                CommandsUnitOfWork.ProductCommand.DeleteProduct(deleteProduct);
                CommandsUnitOfWork.SaveChanges();
            }, additionalValidationProviderFunc);
        }

    }
}