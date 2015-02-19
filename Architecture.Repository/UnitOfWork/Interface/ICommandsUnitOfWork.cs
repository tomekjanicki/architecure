using System;
using Architecture.Repository.Command.Interface;

namespace Architecture.Repository.UnitOfWork.Interface
{
    public interface ICommandsUnitOfWork : IDisposable
    {
        IOrderCommand OrderCommand { get; }
        IProductCommand ProductCommand { get; }
        ICustomerCommand CustomerCommand { get; }
        IMailCommand MailCommand { get; }
        IUserCommand UserCommand { get; }
        void SaveChanges();
    }
}