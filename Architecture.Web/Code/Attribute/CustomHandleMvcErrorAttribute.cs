using System.Web.Mvc;
using Architecture.Util.Log4Net;
using log4net;

namespace Architecture.Web.Code.Attribute
{
    public class CustomHandleMvcErrorAttribute : HandleErrorAttribute
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CustomHandleMvcErrorAttribute));

        public override void OnException(ExceptionContext filterContext)
        {
            Logger.Error(() => filterContext.Exception);
            base.OnException(filterContext);
        }
    }
}