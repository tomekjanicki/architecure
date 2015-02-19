using Architecture.Util.Mail;

namespace Architecture.ViewModel.Internal
{
    public class Find10OldestMailDefinitions
    {
        public int Id { get; set; }

        public MailDefinition MailDefinition { get; set; }

        public int TryCount { get; set; }
    }
}