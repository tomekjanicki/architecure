using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using Architecture.Util.Cache.Interface;
using Architecture.Util.Ninject;
using Architecture.Util.Ninject.Scope;
using Architecture.Util.Test;
using Architecture.Web.Code;
using Architecture.Web.Code.Controller;

namespace Architecture.Web.Test.Unit.Api.Base
{
    public abstract class BaseControllerTest : BaseTest
    {
        private CallContextScope _callContextScope;

        public override void TestFixtureSetUp()
        {
            IoC.Configure();
        }

        public override void SetUp()
        {
            base.SetUp();
            _callContextScope = new CallContextScope();
        }

        public override void TearDown()
        {
            base.TearDown();
            GetCacheService().Clear();
            _callContextScope.Dispose();
            _callContextScope = null;
        }

        protected ICacheService GetCacheService()
        {
            return Factory.Resolve<ICacheService>();
        }

        protected TController GetConfiguredWebApiController<TController>(Func<TController> newInstanceFunc, HttpMethod method, string key) where TController : BaseApiController
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(method, string.Format("http://localhost/{0}/{1}", Const.DefaultApiString, key));
            var route = config.Routes.MapHttpRoute(Const.DefaultApiNameString, Const.DefaultApiRouteTemplateString);
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", key } });
            var controller = newInstanceFunc();
            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Url = new UrlHelper { Request = request };
            return controller;
        }

        protected TResult Call<TResult>(Func<IHttpActionResult> methodFunc) where TResult : class, IHttpActionResult
        {
            return methodFunc() as TResult;
        }

        protected TResult CallWithModelValidation<TResult, TController, TModel>(TController controller, Func<TController, IHttpActionResult> action, TModel model)
            where TResult : class, IHttpActionResult
            where TController : BaseApiController
            where TModel : class
        {
            return Call<TResult>(() => controller.CallWithModelValidation(action, model));
        }


    }
}
