using System.Diagnostics.CodeAnalysis;
using Architecture.Business.Facade.Implementation;
using Architecture.Business.Facade.Interface;
using Architecture.Repository.UnitOfWork.Interface;
using Architecture.Util;
using Architecture.Util.Cache.Implementation;
using Architecture.Util.Cache.Interface;
using Architecture.Util.Mail.Interface;
using Architecture.Util.Ninject;
using Ninject;
using NSubstitute;

namespace Architecture.Business.Test.Unit.Base
{
    public static class IoC
    {
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        static IoC()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IBusinessLogicFacade>().To<BusinessLogicFacade>().InCallContextScope();
            kernel.Bind<ICommandsUnitOfWork>().ToMethod(context => Substitute.For<ICommandsUnitOfWork>()).InCallContextScope();
            kernel.Bind<IMailService>().ToMethod(context => Substitute.For<IMailService>()).InCallContextScope();
            kernel.Bind<ICacheService>().To<CacheService>().InCallContextScope();
            Factory.Init(kernel);            
        }

        public static void Configure()
        {
            
        }
    }
}