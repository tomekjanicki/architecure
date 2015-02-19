using System;
using System.Collections.Generic;
using System.Linq;
using Architecture.Business.Exception;
using Architecture.Repository.UnitOfWork.Interface;
using Architecture.Util.Validation;

namespace Architecture.Business.Manager.Implementation.Base
{
    public abstract class BaseManager
    {

        protected ICommandsUnitOfWork CommandsUnitOfWork { get; private set; }

        protected BaseManager(ICommandsUnitOfWork commandsUnitOfWork)
        {
            CommandsUnitOfWork = commandsUnitOfWork;
        }

        protected static bool AreEqual<T>(IEnumerable<T> ar1, IEnumerable<T> ar2)
        {
            if (ar1 == null || ar2 == null)
                return false;
            return ar1.SequenceEqual(ar2);
        }

        protected Dictionary<string, IList<string>> GetValidationResult(string prefix, object obj, Func<List<Tuple<string, string>>> additionalValidationProviderFunc)
        {            
            var result = DataAnnotationsValidator.Validate(obj);
            var ret = ModelStateAdapter.ToDictionary(prefix, result);
            if (additionalValidationProviderFunc != null)
            {
                var r = additionalValidationProviderFunc();
                ModelStateAdapter.Merge(prefix, ret, r);
            }            
            return ret;
        }

        protected Dictionary<string, IList<string>> HandleValidation(string prefix, object obj, Action action, Func<List<Tuple<string, string>>> additionalValidationProviderFunc = null)
        {
            var validationResults = GetValidationResult(prefix, obj, additionalValidationProviderFunc);
            if (validationResults.Count > 0)
                return validationResults;
            action();
            return new Dictionary<string, IList<string>>();            
        }

        protected Tuple<T, Dictionary<string, IList<string>>> HandleValidation<T>(string prefix, object obj, Func<T> func, Func<List<Tuple<string, string>>> additionalValidationProviderFunc = null)
        {
            var validationResults = GetValidationResult(prefix, obj, additionalValidationProviderFunc);
            if (validationResults.Count > 0)
                return new Tuple<T, Dictionary<string, IList<string>>>(default(T), validationResults);
            var result = func();
            return new Tuple<T, Dictionary<string, IList<string>>>(result, new Dictionary<string, IList<string>>());
        }

        protected void HandleConcurrency(Func<byte[]> getVersionFunc, byte[] localVersion, string key, Type exceptionType)
        {
            var version = ReturnDataWhenFoundOrThrowNotFoundException(getVersionFunc, key, exceptionType);
            var areEqual = AreEqual(localVersion, version);
            if (!areEqual)
                throw new OptimisticConcurrencyException(key, exceptionType);            
        }

        protected T ReturnDataWhenFoundOrThrowNotFoundException<T>(Func<T> dataFunc, string key, Type exceptionType) where T: class 
        {
            var data = dataFunc();
            if (data == null)
                throw new ObjectNotFoundException(key, exceptionType);
            return data;            
        }

    }
}