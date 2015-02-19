using System.Net.Mail;

namespace Architecture.Util.Mail.Interface
{
    public interface IMailService
    {
        void Send(MailMessage message);
    }
}