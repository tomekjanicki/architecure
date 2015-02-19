using System;
using System.Linq;

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

        public void HandleAction(Action action)
        {
            HandleFunctionOrAction<object>(null, action);
        }

        public T HandleFunction<T>(Func<T> function)
        {
            return HandleFunctionOrAction(function, null);
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