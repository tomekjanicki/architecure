using Architecture.Repository.Command.Interface;
using Architecture.ViewModel;
using NSubstitute;

namespace Architecture.Business.Test.Unit.Helper
{
    public static class CustomerManagerTestHelper
    {
        public static InsertCustomerAsync GetValidInsertCustomerAsync()
        {
            return new InsertCustomerAsync {Mail = "example@example.com", Name = "name" };
        }

        public static ICustomerCommand GetCustomerCommand()
        {
            return Substitute.For<ICustomerCommand>();
        }

    }
}