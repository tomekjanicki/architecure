using System.Net;
using System.Web.Http.Filters;
using Architecture.Util.Log4Net;
using log4net;

namespace Architecture.Web.Code.Attribute
{
    public class CustomHandleWebApiErrorAttribute : ExceptionFilterAttribute
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CustomHandleWebApiErrorAttribute));

        public override void OnException(HttpActionExecutedContext context)
        {
            Logger.Error(() => context.Exception);
            context.Response.StatusCode = HttpStatusCode.InternalServerError;
        }
    }
}