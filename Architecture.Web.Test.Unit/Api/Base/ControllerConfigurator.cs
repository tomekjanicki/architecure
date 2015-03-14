using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using Architecture.Util.Ninject;
using Architecture.Web.Code;
using Architecture.Web.Code.Controller;

namespace Architecture.Web.Test.Unit.Api.Base
{
    public class ControllerConfigurator<TController>: IDisposable where TController : BaseApiController
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

        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            Util.Extension.PublicDispose(() => Dispose(true), this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Util.Extension.ProtectedDispose(ref _disposed, disposing, () =>
            {
                Util.Extension.StandardDispose(ref _controller);
                Util.Extension.StandardDispose(ref _configuration);
            });
        }

        private void EnsureNotDisposed()
        {
            Util.Extension.EnsureNotDisposed<ControllerConfigurator<TController>>(_disposed);
        }
    }
}