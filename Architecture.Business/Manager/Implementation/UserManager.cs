using System;
using Architecture.Business.Manager.Implementation.Base;
using Architecture.Business.Manager.Interface;
using Architecture.Repository.UnitOfWork.Interface;
using Architecture.Util.Cache.Interface;
using Architecture.ViewModel;

namespace Architecture.Business.Manager.Implementation
{
    public class UserManager : BaseManager, IUserManager
    {
        private readonly ICacheService _cacheService;

        public UserManager(ICommandsUnitOfWork commandsUnitOfWork, ICacheService cacheService)
            : base(commandsUnitOfWork)
        {
            _cacheService = cacheService;
        }

        public FindByLogin FindByLogin(string login, bool useCache)
        {
            Func<FindByLogin> f = () => CommandsUnitOfWork.UserCommand.FindByLogin(login);
            return useCache ? _cacheService.Get(login, f, new TimeSpan(0, 0, 30), false) : f();
        }
    }
}