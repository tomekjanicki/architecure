using System.Diagnostics.CodeAnalysis;
using Architecture.Business.Facade.Interface;
using Architecture.Util;
using Architecture.Util.Cache.Implementation;
using Architecture.Util.Cache.Interface;
using Architecture.Util.Ninject;
using Ninject;
using NSubstitute;

namespace Architecture.Web.Test.Unit.Api.Base
{
    public static class IoC
    {
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        static IoC()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IBusinessLogicFacade>().ToMethod(context => Substitute.For<IBusinessLogicFacade>()).InCallContextScope();
            kernel.Bind<ICacheService>().To<CacheService>().InCallContextScope();
            Factory.Init(kernel);
        }

        public static void Configure()
        {

        }
    }
}