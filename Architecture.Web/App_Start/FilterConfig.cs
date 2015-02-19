using System.Web.Mvc;
using Architecture.Web.Code.Attribute;
using Thinktecture.IdentityModel.SystemWeb.Mvc;

namespace Architecture.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleMvcErrorAttribute());
            filters.Add(new ResourceActionAuthorizeAttribute());
            filters.Add(new SimulateDelayMvcFilterAttribute());
        }
    }
}
