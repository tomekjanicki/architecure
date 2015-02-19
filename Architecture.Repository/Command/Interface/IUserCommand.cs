using Architecture.ViewModel;

namespace Architecture.Repository.Command.Interface
{
    public interface IUserCommand
    {
        FindByLogin FindByLogin(string login);
    }
}