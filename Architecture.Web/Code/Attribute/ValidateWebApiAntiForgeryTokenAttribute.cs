using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Architecture.Web.Code.Attribute
{
    public class ValidateWebApiAntiForgeryTokenAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var request = actionContext.ControllerContext.Request;
            try
            {
                if (IsAjaxRequest(request))
                    ValidateRequestHeader(request);
                else
                    AntiForgery.Validate();
            }
            catch (HttpAntiForgeryException e)
            {
                actionContext.Response = request.CreateErrorResponse(HttpStatusCode.Forbidden, e);
            }
        }

        private static bool IsAjaxRequest(HttpRequestMessage request)
        {
            IEnumerable<string> xRequestedWithHeaders;
            if (request.Headers.TryGetValues("X-Requested-With", out xRequestedWithHeaders))
            {
                var headerValue = xRequestedWithHeaders.FirstOrDefault();
                if (!string.IsNullOrEmpty(headerValue))
                    return string.Equals(headerValue, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private static void ValidateRequestHeader(HttpRequestMessage request)
        {
            var cookie = request.Headers.GetCookies(AntiForgeryConfig.CookieName).FirstOrDefault();
            var cookieToken = cookie != null ? cookie[AntiForgeryConfig.CookieName].Value : string.Empty;
            var formToken = GetTokenValue(request.Headers, "RequestVerificationToken");
            AntiForgery.Validate(cookieToken, formToken);
        }

        private static string GetTokenValue(HttpHeaders headers, string headerName)
        {
            IEnumerable<string> tokenHeaders;
            if (headers.TryGetValues(headerName, out tokenHeaders))
            {
                var tokenValue = tokenHeaders.FirstOrDefault();
                if (!string.IsNullOrEmpty(tokenValue))
                    return tokenValue.Trim();
            }
            return string.Empty;
        }
    }
}