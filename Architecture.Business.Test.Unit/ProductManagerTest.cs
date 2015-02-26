using System.Collections.Generic;
using System.Linq;
using Architecture.Business.Exception;
using Architecture.Business.Test.Unit.Base;
using Architecture.Business.Test.Unit.Helper;
using Architecture.Util;
using Architecture.ViewModel;
using NSubstitute;
using NUnit.Framework;

namespace Architecture.Business.Test.Unit
{

    public class ProductManagerTest : BaseManagerTest
    {

        [Test]
        public void FindProducts_ValidArguments_ReturnResults()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();

            const string code = "c1";
            const int pageSize = 10;
            commandsUnitOfWork.ProductCommand.Returns(ProductManagerTestHelper.GetProductCommand());
            var data = new List<FindProducts> { new FindProducts { Code = code } };
            var paged = new Paged<FindProducts>(data.Count, data);
            commandsUnitOfWork.ProductCommand.FindProducts(code, null, new PageAndSortCriteria(pageSize, data.Count, null)).Returns(paged);

            var returnedData = businessLogicFacade.ProductManager.FindProducts(code, null, new PageAndSortCriteria(pageSize, data.Count, null));

            Assert.True(returnedData.Count == data.Count && returnedData.Items.FirstOrDefault(product => product.Code == code) != null);
        }

        [Test]
        public void DeleteProduct_ValidArguments_PersistData()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();
            var deleteProduct = ProductManagerTestHelper.GetValidDeleteProduct();
            commandsUnitOfWork.ProductCommand.Returns(ProductManagerTestHelper.GetProductCommand());
            commandsUnitOfWork.ProductCommand.CanDelete(Arg.Is(deleteProduct.Id)).Returns(true);
            commandsUnitOfWork.ProductCommand.GetProductVersion(Arg.Is(deleteProduct.Id)).Returns(deleteProduct.Version);

            var result = businessLogicFacade.ProductManager.DeleteProduct(deleteProduct);

            Assert.That(result.Count == 0);
            commandsUnitOfWork.ProductCommand.Received(1).DeleteProduct(Arg.Is(deleteProduct));
            commandsUnitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void DeleteProduct_CantDelete_ReturnsValidationResults()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();

            var deleteProduct = ProductManagerTestHelper.GetValidDeleteProduct();
            commandsUnitOfWork.ProductCommand.Returns(ProductManagerTestHelper.GetProductCommand());
            commandsUnitOfWork.ProductCommand.CanDelete(Arg.Is(deleteProduct.Id)).Returns(false);

            var result = businessLogicFacade.ProductManager.DeleteProduct(deleteProduct);

            Assert.That(result.Count > 0 && result.Values.FirstOrDefault(l => l.Contains(Const.CantDeleteProductMessage)) != null);
        }

        [Test]
        public void DeleteProduct_NotFound_ThrowsObjectNotFoundException()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();

            var data = ProductManagerTestHelper.GetValidDeleteProduct();
            commandsUnitOfWork.ProductCommand.Returns(ProductManagerTestHelper.GetProductCommand());
            commandsUnitOfWork.ProductCommand.CanDelete(Arg.Is(data.Id)).Returns(true);
            commandsUnitOfWork.ProductCommand.GetProductVersion(Arg.Is(data.Id)).Returns(info => null);

            Assert.Catch<ObjectNotFoundException>(() => businessLogicFacade.ProductManager.DeleteProduct(data));
        }

        [Test]
        public void DeleteOrder_WrongVersion_ThrowsOptimisticConcurrencyException()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();

            var data = ProductManagerTestHelper.GetValidDeleteProduct();
            commandsUnitOfWork.ProductCommand.Returns(ProductManagerTestHelper.GetProductCommand());
            commandsUnitOfWork.ProductCommand.CanDelete(Arg.Is(data.Id)).Returns(true);
            commandsUnitOfWork.ProductCommand.GetProductVersion(Arg.Is(data.Id)).Returns(new byte[] { 5, 18 });

            Assert.Catch<OptimisticConcurrencyException>(() => businessLogicFacade.ProductManager.DeleteProduct(data));
        }

    }
}
