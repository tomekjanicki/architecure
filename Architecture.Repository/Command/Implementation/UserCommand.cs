using Architecture.Repository.Command.Implementation.Base;
using Architecture.Repository.Command.Interface;
using Architecture.ViewModel;

namespace Architecture.Repository.Command.Implementation
{
    public class UserCommand : BaseCommand, IUserCommand
    {
        public UserCommand(ConnectionWithTransaction connectionWithTransaction)
            : base(connectionWithTransaction)
        {
        }

        public FindByLogin FindByLogin(string login)
        {
            var user = QueryReturnsFirstOrDefault<FindByLogin>("SELECT FIRSTNAME, LASTNAME FROM DBO.USERS WHERE LOWER(LOGIN) = @LOGIN", new { LOGIN = login.ToLower() });
            if (user == null)
                return null;
            var roles = QueryReturnsEnumerable<string>("SELECT NAME FROM DBO.ROLES WHERE ID IN (SELECT ROLEID FROM DBO.USERSROLES WHERE USERID IN (SELECT ID FROM DBO.USERS WHERE LOWER(LOGIN) = @LOGIN))", new { LOGIN = login.ToLower() });
            user.Roles = roles;
            return user;
        }
    }
}