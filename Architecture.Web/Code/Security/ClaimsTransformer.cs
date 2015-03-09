using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Architecture.Business.Facade.Interface;
using Architecture.Util.Ninject;
using Thinktecture.IdentityModel;

namespace Architecture.Web.Code.Security
{
    public class ClaimsTransformer : ClaimsAuthenticationManager
    {
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            return !incomingPrincipal.Identity.IsAuthenticated ? base.Authenticate(resourceName, incomingPrincipal) : CreatePrincipal(incomingPrincipal);
        }

        private static ClaimsPrincipal CreatePrincipal(IPrincipal principal)
        {
            var login = principal.Identity.Name;
            if (!string.IsNullOrEmpty(login))
            {
                var businessLogicFacade = Factory.Resolve<IBusinessLogicFacade>();
                var findByLogin = businessLogicFacade.UserManager.FindByLogin(login, true);
                if (findByLogin != null)
                {
                    var p = Principal.Create("Application", new Claim(ClaimTypes.Name, login), new Claim(ClaimTypes.GivenName, findByLogin.FirstName), new Claim(ClaimTypes.Surname, findByLogin.LastName));
                    findByLogin.Roles.ToList().ForEach(role => p.Identities.First().AddClaim(new Claim(ClaimTypes.Role, role)));
                    return p;
                }
            }
            return null;
        }

    }
}