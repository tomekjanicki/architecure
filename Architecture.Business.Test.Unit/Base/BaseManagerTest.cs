﻿using Architecture.Business.Facade.Interface;
using Architecture.Repository.UnitOfWork.Interface;
using Architecture.Util.Ninject;
using Architecture.Util.Ninject.Scope;
using Architecture.Util.Test;

namespace Architecture.Business.Test.Unit.Base
{
    public abstract class BaseManagerTest : BaseTest
    {
        private ThreadProcessingScope _threadProcessingScope;

        public override void TestFixtureSetUp()
        {
            IoC.Configure();
        }

        public override void SetUp()
        {
            base.SetUp();
            _threadProcessingScope = new ThreadProcessingScope();
        }

        public override void TearDown()
        {
            base.TearDown();
            _threadProcessingScope.Dispose();
            _threadProcessingScope = null;
        }

        protected IBusinessLogicFacade GetBusinessLogicFacade()
        {
            return Factory.Resolve<IBusinessLogicFacade>();
        }

        protected ICommandsUnitOfWork GetCommandsUnitOfWork()
        {
            return Factory.Resolve<ICommandsUnitOfWork>();
        }


    }
}
