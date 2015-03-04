using System;
using System.Transactions;
using Architecture.Business.Facade.Interface;
using Architecture.Util.Cache.Interface;
using Architecture.Util.Ninject;
using Architecture.Util.Ninject.Scope;
using Architecture.Util.Test;

namespace Architecture.Business.Test.Integration.Base
{
    public abstract class BaseManagerTest : BaseTest
    {
        private Scope _scope;

        public override void TestFixtureSetUp()
        {
            Setup.Configure();
        }

        public override void SetUp()
        {
            base.SetUp();
            _scope = new Scope();
        }

        public override void TearDown()
        {
            base.TearDown();
            GetCacheService().Clear();
            _scope.Dispose();
            _scope = null;
        }

        protected IBusinessLogicFacade GetBusinessLogicFacade()
        {
            return Factory.Resolve<IBusinessLogicFacade>();
        }

        protected ICacheService GetCacheService()
        {
            return Factory.Resolve<ICacheService>();
        }


        private class Scope : IDisposable
        {
            private TransactionScope _transactionScope;
            private CallContextScope _callContextScope;

            public Scope()
            {
                _callContextScope = new CallContextScope();
                _transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);                
            }

            public void Dispose()
            {
                _transactionScope.Dispose();
                _transactionScope = null;
                _callContextScope.Dispose();
                _callContextScope = null;
            }
        }

    }
}
