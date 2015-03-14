using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using Architecture.Util.Cache.Interface;
using Architecture.Util.Ninject;
using Architecture.Util.Ninject.Scope;
using Architecture.Util.Test;
using Architecture.Web.Code.Controller;

namespace Architecture.Web.Test.Unit.Api.Base
{
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
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
            GetCacheService().Dispose();
            Util.Extension.StandardDispose(ref _callContextScope);
        }

        private static ICacheService GetCacheService()
        {
            return Factory.Resolve<ICacheService>();
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
