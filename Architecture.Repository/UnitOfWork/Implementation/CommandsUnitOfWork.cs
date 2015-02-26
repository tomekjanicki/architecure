using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Architecture.Repository.Command.Implementation;
using Architecture.Repository.Command.Implementation.Base;
using Architecture.Repository.Command.Interface;
using Architecture.Repository.Exception;
using Architecture.Repository.UnitOfWork.Interface;
using Architecture.Util;

namespace Architecture.Repository.UnitOfWork.Implementation
{
    public class CommandsUnitOfWork : ICommandsUnitOfWork
    {
        private readonly DbConnection _connection;
        private DbTransaction _transaction;
        private bool _transactionStarted;

        private bool _disposed;

        public CommandsUnitOfWork()
        {
            _connection = Handler.HandleFunction(() => Extension.GetConnection("Main", false));
        }

        private DbConnection GetOpenConnection()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
            return _connection;
        }

        private async Task<DbConnection> GetOpenConnectionAsync()
        {
            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync().NoAwait();
            return _connection;
        }

        private DbTransaction GetTransaction()
        {
            if (_transaction == null)
            {
                _transaction = _connection.BeginTransaction();
                _transactionStarted = true;
            }
            return _transaction;
        }

        private bool IsActiveTransaction()
        {
            return _transactionStarted;
        }

        public virtual ICustomerCommand CustomerCommand
        {
            get { return new CustomerCommand(GetConnectionWithTransaction()); }
        }

        public virtual IMailCommand MailCommand
        {
            get { return new MailCommand(GetConnectionWithTransaction()); }
        }

        public virtual IUserCommand UserCommand
        {
            get { return new UserCommand(GetConnectionWithTransaction()); }
        }

        public virtual IOrderCommand OrderCommand
        {
            get { return new OrderCommand(GetConnectionWithTransaction()); }
        }

        public virtual IProductCommand ProductCommand
        {
            get { return new ProductCommand(GetConnectionWithTransaction()); }
        }

        public void SaveChanges()
        {
            Handler.HandleAction(() =>
            {
                if (_transactionStarted)
                {
                    _transaction.Commit();
                    _transactionStarted = false;
                    _transaction = null;
                }                
            });
        }

        public void Dispose()
        {
            Handler.HandleAction(() =>
            {
                if (!_disposed)
                {
                    if (_transaction != null)
                    {
                        _transaction.Rollback();
                        _transaction = null;
                        _transactionStarted = false;
                    }
                    if (_connection.State == ConnectionState.Open)
                        _connection.Close();
                    _connection.Dispose();
                    _disposed = true;
                }                
            });
        }

        private ConnectionWithTransaction GetConnectionWithTransaction()
        {
            return new ConnectionWithTransaction(GetOpenConnection, GetOpenConnectionAsync, GetTransaction, IsActiveTransaction);
        }

    }
}