using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Architecture.Business.Exception;
using Architecture.Business.Mail;
using Architecture.Business.Manager.Implementation.Base;
using Architecture.Business.Manager.Interface;
using Architecture.Repository.UnitOfWork.Interface;
using Architecture.Util;
using Architecture.ViewModel;

namespace Architecture.Business.Manager.Implementation
{
    public class OrderManager : BaseManager, IOrderManager
    {
        internal OrderManager(ICommandsUnitOfWork commandsUnitOfWork)
            : base(commandsUnitOfWork)
        {
        }

        public Paged<FindOrders> FindOrders(string customerName, DateTime? from, DateTime? to, PageAndSortCriteria pageAndSortCriteria)
        {
            return CommandsUnitOfWork.OrderCommand.FindOrders(customerName, from, to, pageAndSortCriteria);
        }

        public Paged<GetOrderDetail> GetOrderDetail(int orderId, string productCode, string productName, PageAndSortCriteria pageAndSortCriteria)
        {
            var data = CommandsUnitOfWork.OrderCommand.GetOrderDetail(orderId, productCode, productName, pageAndSortCriteria);
            if (data.Items == null)
                throw new ObjectNotFoundException(orderId.ToString(CultureInfo.InvariantCulture), typeof(GetOrderDetail));
            return data;
        }

        public GetOrder GetOrder(int orderId)
        {
            return ReturnDataWhenFoundOrThrowNotFoundException(() => CommandsUnitOfWork.OrderCommand.GetOrder(orderId), orderId.ToString(CultureInfo.InvariantCulture), typeof(GetOrder));
        }

        public Tuple<int?, Dictionary<string, IList<string>>> InsertOrder(InsertOrder insertOrder)
        {
            return HandleValidation<int?>("insertOrder", insertOrder, () =>            
            {
                var mail = ReturnDataWhenFoundOrThrowNotFoundException(() => CommandsUnitOfWork.CustomerCommand.GetCustomerMail(insertOrder.CustomerId), insertOrder.CustomerId.ToString(CultureInfo.InvariantCulture), typeof(InsertOrder));
                var id = CommandsUnitOfWork.OrderCommand.InsertOrder(insertOrder);
                CommandsUnitOfWork.MailCommand.Insert(new MailProducer().GetInsertOrderMessage(mail, id));
                CommandsUnitOfWork.SaveChanges();
                return id;                
            });
        }

        public Dictionary<string, IList<string>> UpdateOrder(UpdateOrder updateOrder)
        {
            return HandleValidation("updateOrder", updateOrder, () =>
            {
                HandleConcurrency(() => CommandsUnitOfWork.OrderCommand.GetOrderVersion(updateOrder.Id), updateOrder.Version, updateOrder.Id.ToString(CultureInfo.InvariantCulture), typeof(UpdateOrder));
                var mail = ReturnDataWhenFoundOrThrowNotFoundException (() => CommandsUnitOfWork.CustomerCommand.GetCustomerMail(updateOrder.CustomerId), updateOrder.CustomerId.ToString(CultureInfo.InvariantCulture), typeof(UpdateOrder));
                CommandsUnitOfWork.OrderCommand.UpdateOrder(updateOrder);
                CommandsUnitOfWork.MailCommand.Insert(new MailProducer().GetUpdateOrderMessage(mail, updateOrder.Id));
                CommandsUnitOfWork.SaveChanges();
            });
        }

        public Dictionary<string, IList<string>> DeleteOrder(DeleteOrder deleteOrder)
        {
            return HandleValidation("deleteOrder", deleteOrder, () =>
            {
                HandleConcurrency(() => CommandsUnitOfWork.OrderCommand.GetOrderVersion(deleteOrder.Id), deleteOrder.Version, deleteOrder.Id.ToString(CultureInfo.InvariantCulture), typeof(DeleteOrder));
                var mail = ReturnDataWhenFoundOrThrowNotFoundException(() => CommandsUnitOfWork.CustomerCommand.GetCustomerMail(deleteOrder.Id), deleteOrder.Id.ToString(CultureInfo.InvariantCulture), typeof(DeleteOrder)); 
                CommandsUnitOfWork.OrderCommand.DeleteOrder(deleteOrder);
                CommandsUnitOfWork.MailCommand.Insert(new MailProducer().GetDeleteOrderMessage(mail, deleteOrder.Id));
                CommandsUnitOfWork.SaveChanges();                
            });
        }

        public void CreateOrderConfirmationReminders()
        {
            var items = CommandsUnitOfWork.OrderCommand.GetNotConfirmedOrdersToRemind();
            items.ToList().ForEach(item =>
            {
                CommandsUnitOfWork.MailCommand.Insert(new MailProducer().GetRemindConfirmOrderMessage(item.CustomerMail, item.Id));
                CommandsUnitOfWork.OrderCommand.UpdateReminderCreated(item.Id);
            });
            CommandsUnitOfWork.SaveChanges();
        }
    }
}
