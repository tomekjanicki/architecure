using System;
using System.Linq;
using System.Threading.Tasks;

namespace Architecture.Util.Exception
{
    public class HandlerHelper
    {
        private readonly Type[] _handledExceptionTypes;
        private readonly Func<System.Exception, System.Exception> _func;

        public HandlerHelper(Type[] handledExceptionTypes, Func<System.Exception, System.Exception> func)
        {
            _handledExceptionTypes = handledExceptionTypes;
            _func = func;
        }

        public async Task HandleActionAsync(Func<Task> actionFunc)
        {
            await HandleFunctionOrActionAsync<object>(null, actionFunc).NoAwait();
        }

        public async Task<T> HandleFunctionAsync<T>(Func<Task<T>> function)
        {
            return await HandleFunctionOrActionAsync(function, null).NoAwait();
        }

        public void HandleAction(Action action)
        {
            HandleFunctionOrAction<object>(null, action);
        }

        public T HandleFunction<T>(Func<T> function)
        {
            return HandleFunctionOrAction(function, null);
        }

        private async Task<T> HandleFunctionOrActionAsync<T>(Func<Task<T>> function, Func<Task> action)
        {
            try
            {
                if (function != null)
                    return await function().NoAwait();
                await action().NoAwait();
                return default(T);
            }
            catch (System.Exception ex)
            {
                var exceptionType = ex.GetType();
                if (_handledExceptionTypes.Contains(exceptionType) || _handledExceptionTypes.Any(exceptionType.IsSubclassOf))
                    throw _func(ex);
                throw;
            }            
        }

        private T HandleFunctionOrAction<T>(Func<T> function, Action action)
        {
            try
            {
                if (function != null)
                    return function();
                action();
                return default(T);
            }
            catch (System.Exception ex)
            {
                var exceptionType = ex.GetType();
                if (_handledExceptionTypes.Contains(exceptionType) || _handledExceptionTypes.Any(exceptionType.IsSubclassOf))
                    throw _func(ex);
                throw;
            }
        }

    }
}