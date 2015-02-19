using System.Collections.Generic;
using Architecture.Util.Mail;

namespace Architecture.Business.Test.Integration.Helper
{
    public static class MailManagerTestHelper
    {
        public static List<MailDefinition> GetMailDefinitions()
        {
            return new List<MailDefinition> { GetMailDefinition(), GetMailDefinition(), GetMailDefinition() };
        }

        public static MailDefinition GetMailDefinition()
        {
            const string mail = "example@examlpe.com";
            return new MailDefinition { Recipients = new[] { mail }, From = mail, Subject = "subject", Content = "content" };
        }

    }
}
