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
        [ExpectedException(typeof(MailServiceException))]
        public void HandleAction_ThrowConfiguredExceptionType_ThrowsWrapedException()
        {
            _handlerHelper.HandleAction(() =>
            {
                throw new SmtpException();
            });
        }

        [Test]
        [ExpectedException(typeof(MailServiceException))]
        public void HandleAction_ThrowInheritedExceptionType_ThrowsWrapedException()
        {
            _handlerHelper.HandleAction(() =>
            {
                throw new SmtpFailedRecipientException();
            });
        }

        [Test]
        [ExpectedException(typeof(System.Exception))]
        public void HandleAction_ThrowNotConfiguredExceptionType_ThrowsOrginalException()
        {
            _handlerHelper.HandleAction(() =>
            {
                throw new System.Exception();
            });
        }

    }
}
