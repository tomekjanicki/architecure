using System;
using System.Data.SqlClient;
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

    }
}
