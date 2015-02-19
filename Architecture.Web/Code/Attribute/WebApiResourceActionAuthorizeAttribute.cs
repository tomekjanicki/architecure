using System.Security.Claims;
using System.Web.Http.Controllers;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityModel.WebApi;

namespace Architecture.Web.Code.Attribute
{
    public class WebApiResourceActionAuthorizeAttribute : ResourceActionAuthorizeAttribute
    {
        protected override bool CheckAccess(HttpActionContext actionContext, ClaimsPrincipal principal)
        {
            var actionName = actionContext.ActionDescriptor.ActionName;
            var controllerName = string.Format(@"api/{0}", actionContext.ControllerContext.ControllerDescriptor.ControllerName);
            return ClaimsAuthorization.CheckAccess(principal, actionName, new[] { controllerName });
        }
    }
}