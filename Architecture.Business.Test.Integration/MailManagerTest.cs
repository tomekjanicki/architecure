using Architecture.Business.Test.Integration.Base;
using Architecture.Business.Test.Integration.Helper;
using Architecture.Repository.UnitOfWork.Interface;
using Architecture.Util.Ninject;
using NUnit.Framework;

namespace Architecture.Business.Test.Integration
{
    public class MailManagerTest : BaseManagerTest
    {
        [Test]
        public void Send_ValidItems_AreSent()
        {
            var businessLogicFacade = GetBusinessLogicFacade();
            var commandsUnitOfWork = Factory.Resolve<ICommandsUnitOfWork>();
            var data = MailManagerTestHelper.GetMailDefinitions();
            data.ForEach(item => commandsUnitOfWork.MailCommand.Insert(item));

            var returnedData = businessLogicFacade.MailManager.Send();

            Assert.That(returnedData != null && returnedData.TotalQty == data.Count && returnedData.SuccessfulQty == data.Count);
        }

    }
}
