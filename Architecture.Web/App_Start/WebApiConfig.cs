using System.Web.Http;
using System.Web.Http.Validation;
using Architecture.Web.Code;
using Architecture.Web.Code.Attribute;
using Newtonsoft.Json.Serialization;

namespace Architecture.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(Const.DefaultApiNameString, Const.DefaultApiRouteTemplateString, new { id = RouteParameter.Optional });
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Services.Clear(typeof(IBodyModelValidator));
            config.Filters.Add(new CustomHandleWebApiErrorAttribute());
            config.Filters.Add(new WebApiResourceActionAuthorizeAttribute()); 
            config.Filters.Add(new SimulateDelayWebApiFilterAttribute());
        }
    }
}
