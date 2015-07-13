using System;
using System.Web.Http;
using Architecture.Business.Facade.Interface;
using Architecture.Util;
using Architecture.ViewModel;
using Architecture.Web.Code.Attribute;
using Architecture.Web.Code.Controller;

namespace Architecture.Web.Controllers.Api
{
    public class OrderController : BaseApiController
    {
        public OrderController(IBusinessLogicFacade businessLogicFacade)
            : base(businessLogicFacade)
        {
        }

        [HttpGet]
        public Paged<FindOrders> FindOrders(int pageSize, int skip, string customerName = null, DateTime? from = null, DateTime? to = null, string sort = null)
        {           
            return BusinessLogicFacade.OrderManager.FindOrders(customerName, from, to, new PageAndSortCriteria(pageSize, skip, sort));
        }

        public IHttpActionResult GetOrder(int orderId)
        {
            return HandleGet(() => BusinessLogicFacade.OrderManager.GetOrder(orderId));
        }

        public IHttpActionResult GetOrderDetail(int orderId, int pageSize, int skip, string productCode = null, string productName = null, string sort = null)
        {
            return HandleGet(() => BusinessLogicFacade.OrderManager.GetOrderDetail(orderId, productCode, productName, new PageAndSortCriteria(pageSize, skip, sort)));
        }

        [ValidateWebApiAntiForgeryToken]
        public IHttpActionResult PostOrder(InsertOrder insertOrder)
        {
            return HandlePost(() => BusinessLogicFacade.OrderManager.InsertOrder(insertOrder), insertOrder);
        }

        [ValidateWebApiAntiForgeryToken]
        public IHttpActionResult PutOrder(UpdateOrder updateOrder)
        {
            return HandlePutOrDelete(() => BusinessLogicFacade.OrderManager.UpdateOrder(updateOrder));
        }

        [ValidateWebApiAntiForgeryToken]
        public IHttpActionResult DeleteOrder(DeleteOrder deleteOrder)
        {
            return HandlePutOrDelete(() => BusinessLogicFacade.OrderManager.DeleteOrder(deleteOrder));
        }

    }
}