using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Architecture.Repository.Command.Implementation;
using Architecture.Repository.Command.Implementation.Base;
using Architecture.Repository.Command.Interface;
using Architecture.Repository.Exception;
using Architecture.Repository.UnitOfWork.Interface;
using Architecture.Util;

namespace Architecture.Repository.UnitOfWork.Implementation
{
    public class CommandsUnitOfWork : Disposable, ICommandsUnitOfWork
    {
        private DbConnection _connection;
        private DbTransaction _transaction;
        private bool _transactionStarted;
        private bool _disposed;
        private CancellationTokenSource _disposeCts = new CancellationTokenSource();

        private readonly Lazy<IOrderCommand> _lazyOrderCommand;
        private readonly Lazy<IProductCommand> _lazyProductCommand;
        private readonly Lazy<IMailCommand> _lazyMailCommand;
        private readonly Lazy<ICustomerCommand> _lazyCustomerCommand;
        private readonly Lazy<IUserCommand> _lazyUserCommand;
        
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
                await _connection.OpenAsync(_disposeCts.Token).NoAwait();
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

        public virtual void SaveChanges()
        {
            EnsureNotDisposed();
            Handler.HandleAction(() =>
            {
                if (_transactionStarted)
                {
                    _transaction.Commit();
                    _transactionStarted = false;
                    _transaction.Dispose();
                    _transaction = null;
                }                
            });
        }

        private ConnectionWithTransaction GetConnectionWithTransaction()
        {
            EnsureNotDisposed();
            return new ConnectionWithTransaction(GetOpenConnection, GetOpenConnectionAsync, GetTransaction, IsActiveTransaction);
        }

        protected override void Dispose(bool disposing)
        {
            ProtectedDispose(ref _disposed, disposing, () =>
            {
                StandardDisposeWithAction(ref _transaction, () =>
                {
                    _transaction.Rollback();
                    _transactionStarted = false;
                });
                StandardDisposeWithAction(ref _disposeCts, () => _disposeCts.Cancel());                
                StandardDisposeWithAction(ref _connection, () =>
                {
                    if (_connection.State == ConnectionState.Open)
                        _connection.Close();                    
                });                
            });
            base.Dispose(disposing);
        }

        private void EnsureNotDisposed()
        {
            EnsureNotDisposed(_disposed);
        }

    }
}