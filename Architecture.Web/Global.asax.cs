using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Architecture.Web.Code.Security;

namespace Architecture.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngineConfig.RegisterViewEngine();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelValidatorProvidersConfig.Configure(ModelValidatorProviders.Providers);
        }

        protected void Application_PostAuthenticateRequest()
        {
            PostAuthenticate();
        }

        private static void PostAuthenticate()
        {
            var principal = new ClaimsTransformer().Authenticate(string.Empty, ClaimsPrincipal.Current);
            Thread.CurrentPrincipal = principal;
            HttpContext.Current.User = principal;
        }

    }
}
