using System.Web.Mvc;
using Architecture.Business.Facade.Interface;
using Architecture.Web.Code.Controller;

namespace Architecture.Web.Controllers
{
    public class CustomerController : BaseController
    {
        public CustomerController(IBusinessLogicFacade businessLogicFacade) : base(businessLogicFacade)
        {
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}