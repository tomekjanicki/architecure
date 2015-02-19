using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.Util;
using Architecture.ViewModel;

namespace Architecture.Business.Test.Integration.Helper
{
    public static class OrderManagerTestHelper
    {
        public static InsertOrder GetValidInsertOrder(Paged<FindProducts> products, Paged<FindCustomers> customers)
        {
            var data = new InsertOrder { Date = DateTime.Now };
            if (customers.Count > 0)
                data.CustomerId = customers.Items.First().Id;
            var orderDetails = new InsertOrder.OrderDetail { Quantity = 1 };
            if (products.Count > 0)
                orderDetails.ProductId = products.Items.First().Id;
            data.OrderDetails = new List<InsertOrder.OrderDetail> { orderDetails };
            return data;
        }
    }
}