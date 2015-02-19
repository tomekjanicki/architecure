using Architecture.Util.Mail;

namespace Architecture.Business.Mail
{
    public class MailProducer
    {
        private static MailDefinition GetPreConfiguredMailMessage(string to)
        {
            return new MailDefinition {From = "deamon@architecture.com", Recipients = new []{to}};
        }

        public MailDefinition GetInsertOrderMessage(string mail, int orderId)
        {
            var message = GetPreConfiguredMailMessage(mail);
            message.Subject = string.Format("Order with id = {0} has been created", orderId);
            return message;
        }

        public MailDefinition GetUpdateOrderMessage(string mail, int orderId)
        {
            var message = GetPreConfiguredMailMessage(mail);
            message.Subject = string.Format("Order with id = {0} has been updated", orderId);
            return message;
        }

        public MailDefinition GetDeleteOrderMessage(string mail, int orderId)
        {
            var message = GetPreConfiguredMailMessage(mail);
            message.Subject = string.Format("Order with id = {0} has been deleted", orderId);
            return message;
        }

        public MailDefinition GetRemindConfirmOrderMessage(string mail, int orderId)
        {
            var message = GetPreConfiguredMailMessage(mail);
            message.Subject = string.Format("Order with id = {0} has not been confirmed yet.", orderId);
            return message;
        }


    }
}
