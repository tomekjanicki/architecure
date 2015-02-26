using Architecture.Business.Test.Unit.Base;
using Architecture.Business.Test.Unit.Helper;
using Architecture.Repository.Exception;
using NSubstitute;
using NUnit.Framework;

namespace Architecture.Business.Test.Unit
{
    public class MailManagerTest : BaseManagerTest
    {

        [Test]
        public void Send_ValidItems_AreSent()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();
            var data = MailManagerTestHelper.GetFind10OldestMailDefinitions();
            commandsUnitOfWork.MailCommand.Returns(MailManagerTestHelper.GetMailCommand());
            commandsUnitOfWork.MailCommand.Find10OldestMailDefinitions().Returns(data);

            var returnedData = businessLogicFacade.MailManager.Send();

            Assert.That(returnedData != null && returnedData.TotalQty == data.Count && returnedData.SuccessfulQty == data.Count);
        }

        [Test]
        public void Send_ThrowsBaseException_IsHandled()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();
            var data = MailManagerTestHelper.GetFind10OldestMailDefinitions();
            commandsUnitOfWork.MailCommand.Returns(MailManagerTestHelper.GetMailCommand());
            commandsUnitOfWork.MailCommand.Find10OldestMailDefinitions().Returns(data);
            commandsUnitOfWork.MailCommand.When(command => command.UpdateFinished(Arg.Is(data[1].Id))).Do(info => { throw new RepostioryException(null); });

            var returnedData = businessLogicFacade.MailManager.Send();

            Assert.That(returnedData != null && returnedData.TotalQty == data.Count && returnedData.SuccessfulQty == data.Count - 1);
        }

        [Test]
        public void Send_ThrowsException_IsNotHandled()
        {
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();
            var data = MailManagerTestHelper.GetFind10OldestMailDefinitions();
            commandsUnitOfWork.MailCommand.Returns(MailManagerTestHelper.GetMailCommand());
            commandsUnitOfWork.MailCommand.Find10OldestMailDefinitions().Returns(data);
            commandsUnitOfWork.MailCommand.When(command => command.UpdateFinished(Arg.Is(data[1].Id))).Do(info => { throw new System.Exception(); });

            Assert.Catch<System.Exception>(() => businessLogicFacade.MailManager.Send());
        }

    }
}