using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Architecture.Business.Exception;
using Architecture.Repository.UnitOfWork.Interface;
using Architecture.Util;
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
            var ret = GetValidationResultCommon(prefix, obj);
            if (additionalValidationProviderFunc != null)
            {
                var r = additionalValidationProviderFunc();
                ModelStateAdapter.Merge(prefix, ret, r);
            }            
            return ret;
        }

        private static Dictionary<string, IList<string>> GetValidationResultCommon(string prefix, object obj)
        {
            var result = DataAnnotationsValidator.Validate(obj);
            return ModelStateAdapter.ToDictionary(prefix, result);
        }

        protected async Task<Dictionary<string, IList<string>>> GetValidationResultAsync(string prefix, object obj, Func<Task<List<Tuple<string, string>>>> additionalValidationProviderFunc)
        {
            var ret = GetValidationResultCommon(prefix, obj); 
            if (additionalValidationProviderFunc != null)
            {
                var r = await additionalValidationProviderFunc().NoAwait();
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

        protected async Task<Tuple<T, Dictionary<string, IList<string>>>> HandleValidationAsync<T>(string prefix, object obj, Func<Task<T>> func, Func<Task<List<Tuple<string, string>>>> additionalValidationProviderFunc = null)
        {
            var validationResults = await GetValidationResultAsync(prefix, obj, additionalValidationProviderFunc).NoAwait();
            if (validationResults.Count > 0)
                return new Tuple<T, Dictionary<string, IList<string>>>(default(T), validationResults);
            var result = await func().NoAwait();
            return new Tuple<T, Dictionary<string, IList<string>>>(result, new Dictionary<string, IList<string>>());
        }

        protected void HandleConcurrency(Func<byte[]> getVersionFunc, byte[] localVersion, string key, Type exceptionType)
        {
            var version = ReturnDataWhenFoundOrThrowNotFoundException(getVersionFunc, key, exceptionType);
            HandleConcurrencyCommon(localVersion, key, exceptionType, version);
        }

        protected async Task HandleConcurrencyAsync(Func<Task<byte[]>> getVersionFunc, byte[] localVersion, string key, Type exceptionType)
        {
            var version = await ReturnDataWhenFoundOrThrowNotFoundExceptionAsync(getVersionFunc, key, exceptionType).NoAwait();
            HandleConcurrencyCommon(localVersion, key, exceptionType, version);
        }

        private static void HandleConcurrencyCommon(IEnumerable<byte> localVersion, string key, Type exceptionType, IEnumerable<byte> version)
        {
            var areEqual = AreEqual(localVersion, version);
            if (!areEqual)
                throw new OptimisticConcurrencyException(key, exceptionType);
        }

        protected T ReturnDataWhenFoundOrThrowNotFoundException<T>(Func<T> dataFunc, string key, Type exceptionType) where T: class 
        {
            var data = dataFunc();
            return ReturnDataWhenFoundOrThrowNotFoundExceptionCommon(key, exceptionType, data);
        }

        protected async Task<T> ReturnDataWhenFoundOrThrowNotFoundExceptionAsync<T>(Func<Task<T>> dataFunc, string key, Type exceptionType) where T : class
        {
            var data = await dataFunc();
            return ReturnDataWhenFoundOrThrowNotFoundExceptionCommon(key, exceptionType, data);
        }

        private static T ReturnDataWhenFoundOrThrowNotFoundExceptionCommon<T>(string key, Type exceptionType, T data) where T : class
        {
            if (data == null)
                throw new ObjectNotFoundException(key, exceptionType);
            return data;
        }
    }
}