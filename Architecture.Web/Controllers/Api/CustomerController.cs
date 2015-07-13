using System.Web.Http;
using Architecture.Business.Facade.Interface;
using Architecture.Util;
using Architecture.ViewModel;
using Architecture.Web.Code.Attribute;
using Architecture.Web.Code.Controller;

namespace Architecture.Web.Controllers.Api
{
    public class CustomerController : BaseApiController
    {
        public CustomerController(IBusinessLogicFacade businessLogicFacade)
            : base(businessLogicFacade)
        {
        }

        [HttpGet]
        public Paged<FindCustomers> FindCustomers(int pageSize, int skip, string name = null, string sort = null)
        {
            return BusinessLogicFacade.CustomerManager.FindCustomers(name, new PageAndSortCriteria(pageSize, skip, sort));
        }

        [ValidateWebApiAntiForgeryToken]
        public IHttpActionResult PostCustomer(InsertCustomer insertCustomer)
        {
            return HandlePost(() => BusinessLogicFacade.CustomerManager.InsertCustomer(insertCustomer), insertCustomer);
        }

    }
}