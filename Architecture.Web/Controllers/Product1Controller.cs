using System.Web.Mvc;
using Architecture.Business.Facade.Interface;
using Architecture.Web.Code.Controller;

namespace Architecture.Web.Controllers
{
    public class Product1Controller : BaseController
    {
        public Product1Controller(IBusinessLogicFacade businessLogicFacade) :
            base(businessLogicFacade)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}