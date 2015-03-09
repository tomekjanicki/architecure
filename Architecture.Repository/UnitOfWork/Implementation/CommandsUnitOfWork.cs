using System;
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
        private DbConnection _connection;
        private DbTransaction _transaction;
        private bool _transactionStarted;

        private readonly Lazy<IOrderCommand> _lazyOrderCommand;
        private readonly Lazy<IProductCommand> _lazyProductCommand;
        private readonly Lazy<IMailCommand> _lazyMailCommand;
        private readonly Lazy<ICustomerCommand> _lazyCustomerCommand;
        private readonly Lazy<IUserCommand> _lazyUserCommand; 

        private bool _disposed;

        public CommandsUnitOfWork()
        {
            _connection = Handler.HandleFunction(() => Extension.GetConnection("Main", false));
            _lazyOrderCommand = new Lazy<IOrderCommand>(() => new OrderCommand(GetConnectionWithTransaction()));
            _lazyCustomerCommand = new Lazy<ICustomerCommand>(() => new CustomerCommand(GetConnectionWithTransaction()));
            _lazyProductCommand = new Lazy<IProductCommand>(() => new ProductCommand(GetConnectionWithTransaction()));
            _lazyMailCommand = new Lazy<IMailCommand>(() => new MailCommand(GetConnectionWithTransaction()));
            _lazyUserCommand = new Lazy<IUserCommand>(() => new UserCommand(GetConnectionWithTransaction()));
        }

        private DbConnection GetOpenConnection()
        {
            EnsureNotDisposed();
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
            return _connection;
        }

        private async Task<DbConnection> GetOpenConnectionAsync()
        {
            EnsureNotDisposed();
            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync().NoAwait();
            return _connection;
        }

        private DbTransaction GetTransaction()
        {
            EnsureNotDisposed();
            if (_transaction == null)
            {
                _transaction = _connection.BeginTransaction();
                _transactionStarted = true;
            }
            return _transaction;
        }

        private bool IsActiveTransaction()
        {
            EnsureNotDisposed();
            return _transactionStarted;
        }

        public virtual ICustomerCommand CustomerCommand
        {
            get
            {
                EnsureNotDisposed();
                return _lazyCustomerCommand.Value;
            }
        }

        public virtual IMailCommand MailCommand
        {
            get
            {
                EnsureNotDisposed();
                return _lazyMailCommand.Value;
            }
        }

        public virtual IUserCommand UserCommand
        {
            get
            {
                EnsureNotDisposed();
                return _lazyUserCommand.Value;
            }
        }

        public virtual IOrderCommand OrderCommand
        {
            get
            {
                EnsureNotDisposed();
                return _lazyOrderCommand.Value;
            }
        }

        public virtual IProductCommand ProductCommand
        {
            get
            {
                EnsureNotDisposed();
                return _lazyProductCommand.Value;
            }
        }

        public void SaveChanges()
        {
            EnsureNotDisposed();
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

        private ConnectionWithTransaction GetConnectionWithTransaction()
        {
            EnsureNotDisposed();
            return new ConnectionWithTransaction(GetOpenConnection, GetOpenConnectionAsync, GetTransaction, IsActiveTransaction);
        }

        public void Dispose()
        {
            Extension.PublicDispose(() => Dispose(true), this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Extension.ProtectedDispose(ref _disposed, disposing, () =>
            {
                if (_transaction != null)
                {
                    _transaction.Rollback();
                    _transaction = null;
                    _transactionStarted = false;
                }
                Extension.StandardDisposeWithAction(ref _connection, () =>
                {
                    if (_connection.State == ConnectionState.Open)
                        _connection.Close();                    
                });
            });
        }

        private void EnsureNotDisposed()
        {
            Extension.EnsureNotDisposed<CommandsUnitOfWork>(_disposed);
        }

    }
}