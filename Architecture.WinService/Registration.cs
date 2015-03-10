using System.Diagnostics.CodeAnalysis;
using Architecture.Business.Facade.Implementation;
using Architecture.Business.Facade.Interface;
using Architecture.Repository.UnitOfWork.Implementation;
using Architecture.Repository.UnitOfWork.Interface;
using Architecture.Util;
using Architecture.Util.Cache.Implementation;
using Architecture.Util.Cache.Interface;
using Architecture.Util.Mail.Implementation;
using Architecture.Util.Mail.Interface;
using Ninject;

namespace Architecture.WinService
{
    public static class Registration
    {
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static IKernel GetRegisteredKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<ICacheService>().To<CacheService>().InSingletonScope();
            kernel.Bind<IMailService>().To<MailService>().InSingletonScope();
            kernel.Bind<IBusinessLogicFacade>().To<BusinessLogicFacade>().InCallContextScope();
            kernel.Bind<ICommandsUnitOfWork>().To<CommandsUnitOfWork>().InCallContextScope();
            return kernel;
        }

    }
}
