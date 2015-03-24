using System.IdentityModel.Claims;
using System.Web.Helpers;

namespace Architecture.Web
{
    public static class MiscConfig
    {
        public static void Configure()
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;
        }
    }
}