using System;
using System.Diagnostics.CodeAnalysis;
using System.Transactions;
using Architecture.Business.Facade.Interface;
using Architecture.Util;
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
            GetCacheService().Dispose();
            Extension.StandardDispose(ref _scope);
        }

        protected IBusinessLogicFacade GetBusinessLogicFacade()
        {
            return Factory.Resolve<IBusinessLogicFacade>();
        }

        private static ICacheService GetCacheService()
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

            [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_transactionScope"), SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_callContextScope")]
            public void Dispose()
            {
                Extension.StandardDispose(ref _transactionScope);
                Extension.StandardDispose(ref _callContextScope);
            }
        }

    }
}
