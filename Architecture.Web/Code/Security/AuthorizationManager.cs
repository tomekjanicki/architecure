using System.Linq;
using System.Security.Claims;

namespace Architecture.Web.Code.Security
{
    public class AuthorizationManager : ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
            if (!context.Principal.Identity.IsAuthenticated)
                return false;
            var action = context.Action.First().Value.ToLower();
            var resource = context.Resource.First().Value.ToLower();
            var roles = context.Principal.Claims.Where(claim => claim.Type == ClaimTypes.Role).Select(claim => claim.Value.ToLower()).ToList();
            return true;
        }
    }
}