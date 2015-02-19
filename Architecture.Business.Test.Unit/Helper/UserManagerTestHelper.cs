using Architecture.Repository.Command.Interface;
using NSubstitute;

namespace Architecture.Business.Test.Unit.Helper
{
    public static class UserManagerTestHelper
    {
        public static IUserCommand GetUserCommand()
        {
            return Substitute.For<IUserCommand>();
        }
    }
}