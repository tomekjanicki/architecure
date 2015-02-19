using System;
using System.Collections.Generic;
using Architecture.Util;
using Architecture.ViewModel;
using Architecture.ViewModel.Internal;

namespace Architecture.Repository.Command.Interface
{
    public interface IOrderCommand
    {
        Paged<FindOrders> FindOrders(string customerName, DateTime? from, DateTime? to, PageAndSortCriteria pageAndSortCriteria); 
        Paged<GetOrderDetail> GetOrderDetail(int orderId, string productCode, string productName, PageAndSortCriteria pageAndSortCriteria);
        GetOrder GetOrder(int orderId);
        byte[] GetOrderVersion(int orderId);
        string GetCustomerMail(int orderId);

        int InsertOrder(InsertOrder insertOrder);
        void UpdateOrder(UpdateOrder updateOrder);
        void DeleteOrder(DeleteOrder deleteOrder);
        IEnumerable<GetNotConfirmedOrdersToRemind> GetNotConfirmedOrdersToRemind();
        void UpdateReminderCreated(int id);
    }
}