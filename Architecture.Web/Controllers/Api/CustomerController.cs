using System.Threading.Tasks;
using System.Web.Http;
using Architecture.Business.Facade.Interface;
using Architecture.Util;
using Architecture.ViewModel;
using Architecture.Web.Code.Controller;

namespace Architecture.Web.Controllers.Api
{
    public class CustomerController : BaseApiController
    {
        public CustomerController(IBusinessLogicFacade businessLogicFacade)
            : base(businessLogicFacade)
        {
        }

        public async Task<Paged<FindCustomers>> FindCustomersAsync(string name, int pageSize, int skip, string sort)
        {
            return await BusinessLogicFacade.CustomerManager.FindCustomersAsync(name, new PageAndSortCriteria(pageSize, skip, sort));
        }

        public async Task<IHttpActionResult> PostCustomerAsync(InsertCustomer insertCustomer)
        {
            return await HandlePostAsync(() => BusinessLogicFacade.CustomerManager.InsertCustomerAsync(insertCustomer), insertCustomer);
        }

    }
}