using System;
using System.Web.Http;
using Architecture.Business.Facade.Interface;
using Architecture.Util;
using Architecture.ViewModel;
using Architecture.Web.Code.Controller;

namespace Architecture.Web.Controllers.Api
{
    public class OrderController : BaseApiController
    {
        public OrderController(IBusinessLogicFacade businessLogicFacade)
            : base(businessLogicFacade)
        {
        }

        public Paged<FindOrders> GetOrders(string customerName, DateTime? from, DateTime? to, int pageSize, int skip, string sort)
        {
            return BusinessLogicFacade.OrderManager.FindOrders(customerName, from, to, new PageAndSortCriteria(pageSize, skip, sort));
        }

        public IHttpActionResult GetOrder(int orderId)
        {
            return HandleGet(() => BusinessLogicFacade.OrderManager.GetOrder(orderId));
        }

        public IHttpActionResult GetOrderDetail(int orderId, string productCode, string productName, int pageSize, int skip, string sort)
        {
            return HandleGet(() => BusinessLogicFacade.OrderManager.GetOrderDetail(orderId, productCode, productName, new PageAndSortCriteria(pageSize, skip, sort)));
        }

        public IHttpActionResult PostOrder(InsertOrder insertOrder)
        {
            return HandlePost(() => BusinessLogicFacade.OrderManager.InsertOrder(insertOrder), insertOrder);
        }

        public IHttpActionResult PutOrder(UpdateOrder updateOrder)
        {
            return HandlePutOrDelete(() => BusinessLogicFacade.OrderManager.UpdateOrder(updateOrder));
        }

        public IHttpActionResult DeleteOrder(DeleteOrder deleteOrder)
        {
            return HandlePutOrDelete(() => BusinessLogicFacade.OrderManager.DeleteOrder(deleteOrder));
        }

    }
}