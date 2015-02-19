using System;
using System.Collections.Generic;
using Architecture.Util;
using Architecture.ViewModel;

namespace Architecture.Business.Manager.Interface
{
    public interface IOrderManager
    {
        Paged<FindOrders> FindOrders(string customerName, DateTime? from, DateTime? to, PageAndSortCriteria pageAndSortCriteria);
        Paged<GetOrderDetail> GetOrderDetail(int orderId, string productCode, string productName, PageAndSortCriteria pageAndSortCriteria);
        GetOrder GetOrder(int orderId);
        Tuple<int?, Dictionary<string, IList<string>>> InsertOrder(InsertOrder insertOrder);
        Dictionary<string, IList<string>> UpdateOrder(UpdateOrder updateOrder);
        Dictionary<string, IList<string>> DeleteOrder(DeleteOrder deleteOrder);
        void CreateOrderConfirmationReminders();
    }
}