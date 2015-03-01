using Architecture.Repository.Command.Interface;
using Architecture.ViewModel;
using NSubstitute;

namespace Architecture.Business.Test.Unit.Helper
{
    public static class CustomerManagerTestHelper
    {
        public static InsertCustomer GetValidInsertCustomerAsync()
        {
            return new InsertCustomer {Mail = "example@example.com", Name = "name" };
        }

        public static ICustomerCommand GetCustomerCommand()
        {
            return Substitute.For<ICustomerCommand>();
        }

    }
}