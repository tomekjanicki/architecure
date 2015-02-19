using System.Web.Mvc;
using Architecture.Business.Facade.Interface;
using Architecture.Web.Code.Controller;

namespace Architecture.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IBusinessLogicFacade businessLogicFacade)
            : base(businessLogicFacade)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}