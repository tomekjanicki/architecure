using Architecture.Business.Test.Integration.Base;
using Architecture.Business.Test.Integration.Helper;
using Architecture.Util;
using NUnit.Framework;

namespace Architecture.Business.Test.Integration
{
    public class OrderManagerTest : BaseManagerTest
    {
        [Test]
        public void InsertOrder_ValidArguments_PersistsData()
        {
            var businessLogicFacade = GetBusinessLogicFacade();
            var products = businessLogicFacade.ProductManager.FindProducts("C1", null, new PageAndSortCriteria(1, 0, null));
            var customers = businessLogicFacade.CustomerManager.FindCustomers("C1", new PageAndSortCriteria(1, 0, null));

            var data = OrderManagerTestHelper.GetValidInsertOrder(products, customers);

            var returnedData = businessLogicFacade.OrderManager.InsertOrder(data);

            Assert.True(returnedData.Item2.Count == 0 && returnedData.Item1 != null && returnedData.Item1.Value > 0);
        }

    }
}
