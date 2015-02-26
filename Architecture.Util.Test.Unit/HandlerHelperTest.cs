using System.Net.Mail;
using Architecture.Util.Exception;
using Architecture.Util.Mail.Exception;
using NUnit.Framework;

namespace Architecture.Util.Test.Unit
{
    public class HandlerHelperTest : BaseTest
    {
        private HandlerHelper _handlerHelper;

        public override void TestFixtureSetUp()
        {
            var types = new[] {typeof(SmtpException)};
            _handlerHelper = new HandlerHelper(types, exception => new MailServiceException(exception));
        }

        [Test]
        public void HandleAction_ThrowConfiguredExceptionType_ThrowsWrapedException()
        {
            Assert.Catch<MailServiceException>(() => _handlerHelper.HandleAction(() => { throw new SmtpException(); }));
        }

        [Test]
        public void HandleAction_ThrowInheritedExceptionType_ThrowsWrapedException()
        {
            Assert.Catch<MailServiceException>(() => _handlerHelper.HandleAction(() => { throw new SmtpFailedRecipientException(); }));
        }

        [Test]
        public void HandleAction_ThrowNotConfiguredExceptionType_ThrowsOrginalException()
        {
            Assert.Catch<System.Exception>(() => _handlerHelper.HandleAction(() => { throw new System.Exception(); }));
        }

    }
}
