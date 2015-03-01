using System.Collections.Generic;
using Architecture.Repository.Command.Interface;
using Architecture.Util.Mail;
using Architecture.ViewModel.Internal;
using NSubstitute;

namespace Architecture.Business.Test.Unit.Helper
{
    public static class MailManagerTestHelper
    {
        public static IMailCommand GetMailCommand()
        {
            return Substitute.For<IMailCommand>();
        }

        public static List<Find10OldestMailDefinitions> GetFind10OldestMailDefinitions()
        {
            return new List<Find10OldestMailDefinitions>
            {
                new Find10OldestMailDefinitions {Id = 1, MailDefinition = GetMailDefinition()},
                new Find10OldestMailDefinitions {Id = 2, MailDefinition = GetMailDefinition()},
                new Find10OldestMailDefinitions {Id = 3, MailDefinition = GetMailDefinition()}
            };
        }

        public static MailDefinition GetMailDefinition()
        {
            const string mail = "example@examlpe.com";
            return new MailDefinition { Recipients = new[] { mail }, From = mail, Subject = "subject", Content = "content" };
        }
        
    }
}