using Architecture.ViewModel;

namespace Architecture.Business.Manager.Interface
{
    public interface IUserManager
    {
        FindByLogin FindByLogin(string login, bool useCache);
    }
}