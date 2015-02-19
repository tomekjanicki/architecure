using System.Collections.Generic;
using Architecture.Util.Mail;
using Architecture.ViewModel;
using Architecture.ViewModel.Internal;

namespace Architecture.Repository.Command.Interface
{
    public interface IMailCommand
    {
        void Insert(MailDefinition mailDefinition);
        IEnumerable<Find10OldestMailDefinitions> Find10OldestMailDefinitions();
        void UpdateTryCount(UpdateTryCount updateTryCount);
        void UpdateFinished(int id);
    }
}