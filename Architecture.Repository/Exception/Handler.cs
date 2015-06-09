using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Architecture.Util;
using Architecture.Util.Exception;

namespace Architecture.Repository.Exception
{
    internal static class Handler
    {
        private static readonly HandlerHelper HandlerHelper;

        static Handler()
        {
            var types = new[] { typeof(SqlException) };
            HandlerHelper = new HandlerHelper(types, exception => new RepostioryException(exception));
        }

        public static void HandleAction(Action action)
        {
            HandlerHelper.HandleAction(action);
        }

        public static T HandleFunction<T>(Func<T> function)
        {
            return HandlerHelper.HandleFunction(function);
        }

        public static async Task HandleActionAsync(Func<Task> actionFunc)
        {
            await HandlerHelper.HandleActionAsync(actionFunc).NoAwait();
        }

        public static async Task<T> HandleFunctionAsync<T>(Func<Task<T>> function)
        {
            return await HandlerHelper.HandleFunctionAsync(function).NoAwait();
        }


    }
}
