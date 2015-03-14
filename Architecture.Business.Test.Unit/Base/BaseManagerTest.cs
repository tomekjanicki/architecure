using System.Diagnostics.CodeAnalysis;
using Architecture.Business.Facade.Interface;
using Architecture.Repository.UnitOfWork.Interface;
using Architecture.Util.Cache.Interface;
using Architecture.Util.Ninject;
using Architecture.Util.Ninject.Scope;
using Architecture.Util.Test;

namespace Architecture.Business.Test.Unit.Base
{
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public abstract class BaseManagerTest : BaseTest
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

        protected IBusinessLogicFacade GetBusinessLogicFacade()
        {
            return Factory.Resolve<IBusinessLogicFacade>();
        }

        private static ICacheService GetCacheService()
        {
            return Factory.Resolve<ICacheService>();
        }

        protected ICommandsUnitOfWork GetCommandsUnitOfWork()
        {
            return Factory.Resolve<ICommandsUnitOfWork>();
        }


    }
}
