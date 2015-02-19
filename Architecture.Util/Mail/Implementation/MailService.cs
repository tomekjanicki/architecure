using System.Net.Mail;
using Architecture.Util.Exception;
using Architecture.Util.Mail.Exception;
using Architecture.Util.Mail.Interface;

namespace Architecture.Util.Mail.Implementation
{
    public class MailService : IMailService
    {
        private readonly HandlerHelper _handler;

        public MailService()
        {
            var types = new[] { typeof(SmtpException) };
            _handler = new HandlerHelper(types, exception => new MailServiceException(exception));
        }

        public void Send(MailMessage message)
        {
            _handler.HandleAction(() => new SmtpClient().Send(message));
        }
    }
}
