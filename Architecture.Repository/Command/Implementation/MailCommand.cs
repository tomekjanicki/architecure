using System.Collections.Generic;
using System.Linq;
using Architecture.Repository.Command.Implementation.Base;
using Architecture.Repository.Command.Interface;
using Architecture.Util.Mail;
using Architecture.ViewModel.Internal;

namespace Architecture.Repository.Command.Implementation
{
    public class MailCommand : BaseCommand, IMailCommand
    {
        public MailCommand(ConnectionWithTransaction connectionWithTransaction)
            : base(connectionWithTransaction)
        {
        }

        public void Insert(MailDefinition mailDefinition)
        {
            Execute("INSERT INTO DBO.MAILS (DATA) VALUES(@DATA)", new { DATA = mailDefinition.ToBytes() });
        }

        public IEnumerable<Find10OldestMailDefinitions> Find10OldestMailDefinitions()
        {
            var data = QueryReturnsEnumerable<MailDefinitionHelper>("SELECT TOP 10 ID, TRYCOUNT, DATA FROM DBO.MAILS WHERE TRYCOUNT < 3 AND SENT = 0 ORDER BY CREATED DESC");
            return data.Select(helper => new Find10OldestMailDefinitions { Id = helper.Id, TryCount = helper.TryCount, MailDefinition = MailDefinition.FromBytes(helper.Data) });
        }

        public void UpdateTryCount(UpdateTryCount updateTryCount)
        {
            Execute("UPDATE DBO.MAILS SET TRYCOUNT = @TRYCOUNT WHERE ID = @ID", new { ID = updateTryCount.Id, TRYCOUNT = updateTryCount.TryCount });
        }

        public void UpdateFinished(int id)
        {
            Execute("UPDATE DBO.MAILS SET SENT = 1 WHERE ID = @ID", new { ID = id });
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class MailDefinitionHelper
        {
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            public int Id { get; set; }
            public byte[] Data { get; set; }
            public int TryCount { get; set; }
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }
    }
}