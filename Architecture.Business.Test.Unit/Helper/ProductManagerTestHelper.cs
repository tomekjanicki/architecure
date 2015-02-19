using Architecture.Repository.Command.Interface;
using Architecture.ViewModel;
using NSubstitute;

namespace Architecture.Business.Test.Unit.Helper
{
    public static class ProductManagerTestHelper
    {
        public static IProductCommand GetProductCommand()
        {
            return Substitute.For<IProductCommand>();
        }

        public static DeleteProduct GetValidDeleteProduct()
        {
            return new DeleteProduct { Id = 5, Version = new byte[] { 2, 3 } };
        }

    }
}