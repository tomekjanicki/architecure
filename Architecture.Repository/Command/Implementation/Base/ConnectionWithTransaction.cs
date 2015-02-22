using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Architecture.Repository.Command.Implementation.Base
{
    public class ConnectionWithTransaction
    {
        public ConnectionWithTransaction(Func<DbConnection> connectionFunc, Func<Task<DbConnection>> connectionFuncAsync, Func<DbTransaction> transactionFunc, Func<bool> isActiveTransactionFunc)
        {
            IsActiveTransactionFunc = isActiveTransactionFunc;
            TransactionFunc = transactionFunc;
            ConnectionFunc = connectionFunc;
            ConnectionFuncAsync = connectionFuncAsync;
        }

        public Func<DbConnection> ConnectionFunc { get; private set; }
        public Func<Task<DbConnection>> ConnectionFuncAsync { get; private set; }
        public Func<DbTransaction> TransactionFunc { get; private set; }
        public Func<bool> IsActiveTransactionFunc { get; private set; }

    }
}