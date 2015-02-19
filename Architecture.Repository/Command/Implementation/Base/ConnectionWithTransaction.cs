using System;
using System.Data;

namespace Architecture.Repository.Command.Implementation.Base
{
    public class ConnectionWithTransaction
    {
        public ConnectionWithTransaction(Func<IDbConnection> connectionFunc, Func<IDbTransaction> transactionFunc, Func<bool> isActiveTransactionFunc)
        {
            IsActiveTransactionFunc = isActiveTransactionFunc;
            TransactionFunc = transactionFunc;
            ConnectionFunc = connectionFunc;
        }

        public Func<IDbConnection> ConnectionFunc { get; private set; }
        public Func<IDbTransaction> TransactionFunc { get; private set; }
        public Func<bool> IsActiveTransactionFunc { get; private set; }

    }
}