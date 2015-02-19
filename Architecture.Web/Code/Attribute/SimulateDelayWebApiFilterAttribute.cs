using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Architecture.Web.Code.Attribute
{
    public class SimulateDelayWebApiFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Thread.Sleep(1000);
        }

    }
}