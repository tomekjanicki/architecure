using System.Web.Mvc;
using System.Web.Routing;
using Architecture.Web.Code;

namespace Architecture.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(Const.DefaultNameString, Const.DefaultRouteTemplateString, new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}
