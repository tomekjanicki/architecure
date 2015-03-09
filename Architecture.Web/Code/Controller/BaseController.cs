using System.Security.Claims;
using Architecture.Business.Facade.Interface;

namespace Architecture.Web.Code.Controller
{
    public abstract class BaseController : System.Web.Mvc.Controller
    {
        protected IBusinessLogicFacade BusinessLogicFacade;

        protected BaseController(IBusinessLogicFacade businessLogicFacade)
        {
            BusinessLogicFacade = businessLogicFacade;
        }

        protected new virtual ClaimsPrincipal User
        {
            get { return HttpContext.User as ClaimsPrincipal; }
        }


    }
}