using System.Web.Http;
using System.Web.Http.Validation;
using Architecture.Web.Code;
using Architecture.Web.Code.Attribute;

namespace Architecture.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(Const.DefaultApiNameString, Const.DefaultApiRouteTemplateString, new { id = RouteParameter.Optional });
            config.Services.Clear(typeof(IBodyModelValidator));
            config.Filters.Add(new CustomHandleWebApiErrorAttribute());
            config.Filters.Add(new WebApiResourceActionAuthorizeAttribute()); 
            config.Filters.Add(new SimulateDelayWebApiFilterAttribute());
        }
    }
}
