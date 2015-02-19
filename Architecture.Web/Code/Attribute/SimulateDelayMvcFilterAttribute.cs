using System.Threading;
using System.Web.Mvc;

namespace Architecture.Web.Code.Attribute
{
    public class SimulateDelayMvcFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Thread.Sleep(1000);
        }
    }
}