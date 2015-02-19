using System.Web.Mvc;
using Architecture.Business.Facade.Interface;
using Architecture.Web.Code.Controller;

namespace Architecture.Web.Controllers
{
    public class OrderController : BaseController
    {
        public OrderController(IBusinessLogicFacade businessLogicFacade)
            : base(businessLogicFacade)
        {
        }

        public ActionResult Create()
        {
            return View();
        }

    }
}
