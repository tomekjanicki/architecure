using System;
using System.Collections.Generic;
using Architecture.Repository.Command.Interface;
using Architecture.ViewModel;
using NSubstitute;

namespace Architecture.Business.Test.Unit.Helper
{
    public static class OrderManagerTestHelper
    {
        public static IOrderCommand GetOrderCommand()
        {
            return Substitute.For<IOrderCommand>();
        }

        public static IMailCommand GetMailCommand()
        {
            return Substitute.For<IMailCommand>();
        }

        public static InsertOrder GetValidInsertOrder()
        {
            return new InsertOrder
            {
                CustomerId = 5,
                Date = DateTime.Now,
                OrderDetails = new List<InsertOrder.OrderDetail>
                {
                    new InsertOrder.OrderDetail {ProductId = 5, Quantity = 6}
                }
            };
        }

        public static UpdateOrder GetValidUpdateOrder()
        {
            return new UpdateOrder
            {
                Id = 5,
                CustomerId = 5,
                Date = DateTime.Now,
                Version = new byte[] { 5, 15 },
                OrderDetails = new List<UpdateOrder.OrderDetail>
                {
                    new UpdateOrder.OrderDetail {Id = 5, ProductId = 5, Quantity = 6}
                }
            };
        }

        public static DeleteOrder GetValidDeleteOrder()
        {
            return new DeleteOrder { Version = new byte[] { 5, 17 }, Id = 1 };            
        }
        
    }
}