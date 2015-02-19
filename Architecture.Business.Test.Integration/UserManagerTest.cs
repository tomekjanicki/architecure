using Architecture.Business.Test.Integration.Base;
using NUnit.Framework;

namespace Architecture.Business.Test.Integration
{
    public class UserManagerTest : BaseManagerTest
    {
        [Test]
        public void FindByLogin_ValidLogin_ReturnsData()
        {
            var businessLogicFacade = GetBusinessLogicFacade();
            const string login = @"IMPAQ\TJAK";

            var data = businessLogicFacade.UserManager.FindByLogin(login, false);

            Assert.That(data != null);
        }

        [Test]
        public void FindByLogin_InvalidLogin_ReturnsNull()
        {
            var businessLogicFacade = GetBusinessLogicFacade();
            const string login = "test";

            var data = businessLogicFacade.UserManager.FindByLogin(login, false);

            Assert.That(data == null);
        }
        
    }
}