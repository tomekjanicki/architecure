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
        static IoC()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IBusinessLogicFacade>().ToMethod(context => Substitute.For<IBusinessLogicFacade>()).InThreadProcessingScope();
            kernel.Bind<ICacheService>().To<CacheService>().InThreadProcessingScope();
            Factory.Init(kernel);
        }

        public static void Configure()
        {

        }
    }
}