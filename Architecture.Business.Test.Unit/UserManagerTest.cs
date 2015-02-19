using Architecture.Business.Test.Unit.Base;
using Architecture.Business.Test.Unit.Helper;
using Architecture.ViewModel;
using NSubstitute;
using NUnit.Framework;

namespace Architecture.Business.Test.Unit
{
    public class UserManagerTest : BaseManagerTest
    {

        [Test]
        public void FindByLogin_ValidLogin_ReturnsData()
        {
            const string login = "test";
            var d = new FindByLogin();
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();

            commandsUnitOfWork.UserCommand.Returns(UserManagerTestHelper.GetUserCommand());
            commandsUnitOfWork.UserCommand.FindByLogin(Arg.Is(login)).Returns(d);

            var data = businessLogicFacade.UserManager.FindByLogin(login, false);

            Assert.That(data != null);
        }

        [Test]
        public void FindByLogin_InvalidLogin_ReturnsNull()
        {
            const string login = "test";
            var commandsUnitOfWork = GetCommandsUnitOfWork();
            var businessLogicFacade = GetBusinessLogicFacade();
            commandsUnitOfWork.UserCommand.Returns(UserManagerTestHelper.GetUserCommand());
            commandsUnitOfWork.UserCommand.FindByLogin(Arg.Is(login)).Returns(info => null);

            var data = businessLogicFacade.UserManager.FindByLogin(login, false);

            Assert.That(data == null);
        }
    }
}