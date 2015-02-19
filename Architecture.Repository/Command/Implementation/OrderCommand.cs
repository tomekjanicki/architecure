using System;
using System.Collections.Generic;
using Architecture.Repository.Command.Implementation.Base;
using Architecture.Repository.Command.Interface;
using Architecture.Util;
using Architecture.ViewModel;
using Architecture.ViewModel.Internal;

namespace Architecture.Repository.Command.Implementation
{
    public class OrderCommand : BaseCommand, IOrderCommand
    {
        internal OrderCommand(ConnectionWithTransaction connectionWithTransaction)
            : base(connectionWithTransaction)
        {
        }

        public Paged<FindOrders> FindOrders(string customerName, DateTime? from, DateTime? to, PageAndSortCriteria pageAndSortCriteria)
        {
            throw new NotImplementedException();
        }

        public Paged<GetOrderDetail> GetOrderDetail(int orderId, string productCode, string productName, PageAndSortCriteria pageAndSortCriteria)
        {
            throw new NotImplementedException();
        }

        public GetOrder GetOrder(int orderId)
        {
            throw new NotImplementedException();
        }

        public byte[] GetOrderVersion(int orderId)
        {
            throw new NotImplementedException();
        }

        public string GetCustomerMail(int orderId)
        {
            throw new NotImplementedException();
        }

        public int InsertOrder(InsertOrder insertOrder)
        {
            var id = ExecuteScalar<int>("INSERT INTO DBO.ORDERS (DATE, CUSTOMERID, STATUSID) OUTPUT INSERTED.ID VALUES (@DATE, @CUSTOMERID, 0)", new { DATE = insertOrder.Date, CUSTOMERID = insertOrder.CustomerId});
            foreach (var orderDetail in insertOrder.OrderDetails)
                Execute("INSERT INTO DBO.ORDERSDETAILS (ORDERID, PRODUCTID, QTY) VALUES(@ORDERID, @PRODUCTID, @QTY)", new { ORDERID = id, PRODUCTID = orderDetail.ProductId, QTY = orderDetail.Quantity });
            return id;
        }

        public void UpdateOrder(UpdateOrder updateOrder)
        {
            throw new NotImplementedException();
        }

        public void DeleteOrder(DeleteOrder deleteOrder)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GetNotConfirmedOrdersToRemind> GetNotConfirmedOrdersToRemind()
        {
            throw new NotImplementedException();
        }

        public void UpdateReminderCreated(int id)
        {
            throw new NotImplementedException();
        }
    }
}
