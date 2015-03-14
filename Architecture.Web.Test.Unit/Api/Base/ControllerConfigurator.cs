using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using Architecture.Util;
using Architecture.Util.Ninject;
using Architecture.Web.Code.Controller;
using Const = Architecture.Web.Code.Const;

namespace Architecture.Web.Test.Unit.Api.Base
{
    public class ControllerConfigurator<TController>: Disposable where TController : BaseApiController
    {
        private readonly HttpMethod _method;
        private readonly string _key;
        private HttpConfiguration _configuration;
        private TController _controller;
        private bool _disposed;

        public ControllerConfigurator(HttpMethod method, string key)
        {
            _method = method;
            _key = key;
        }

        public TController GetConfigured() 
        {
            EnsureNotDisposed();
            _configuration = new HttpConfiguration();
            using (var request = new HttpRequestMessage(_method, string.Format("http://localhost/{0}/{1}", Const.DefaultApiString, _key)))
            {
                var route = _configuration.Routes.MapHttpRoute(Const.DefaultApiNameString, Const.DefaultApiRouteTemplateString);
                var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", _key } });
                _controller = Factory.Resolve<TController>();
                _controller.ControllerContext = new HttpControllerContext(_configuration, routeData, request);
                _controller.Request = request;
                _controller.Url = new UrlHelper { Request = request };
                return _controller;
            }
        }

        protected override void Dispose(bool disposing)
        {
            ProtectedDispose(ref _disposed, disposing, () =>
            {
                StandardDispose(ref _controller);
                StandardDispose(ref _configuration);
            });
            base.Dispose(disposing);
        }

        private void EnsureNotDisposed()
        {
            EnsureNotDisposed(_disposed);
        }
    }
}