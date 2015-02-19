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

        public Paged<FindCustomers> FindCustomers(string name, int pageSize, int skip, string sort)
        {
            return BusinessLogicFacade.CustomerManager.FindCustomers(name, new PageAndSortCriteria(pageSize, skip, sort));
        }
    }
}