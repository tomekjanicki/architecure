using System;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Architecture.Util.Ninject.Scope;
using Ninject.Activation;
using Ninject.Syntax;

namespace Architecture.Util
{
    public static class Extension
    {
        private const string SqlClient = "System.Data.SqlClient";

        public static void StandardDisposeWithAction<T>(ref T obj, Action additionalAction) where T : class, IDisposable
        {
            if (obj != null)
            {
                if (additionalAction != null)
                    additionalAction();
                obj.Dispose();
                obj = null;
            }
        }

        public static void StandardDispose<T>(ref T obj) where T: class, IDisposable
        {
            StandardDisposeWithAction(ref obj, null);
        }

        public static void ProtectedDispose(ref bool disposed, bool disposing, Action disposingAction)
        {
            if (disposed)
                return;
            if (disposing)
            {
                disposingAction();
                disposed = true;
            }
        }

        public static void PublicDispose(Action disposeAction, object obj)
        {
            disposeAction();
            GC.SuppressFinalize(obj);
        }

        public static void EnsureNotDisposed<T>(bool disposed) where T : class
        {
            if (disposed)
                throw new ObjectDisposedException(typeof(T).FullName);
        }

        public static void EnsureArgumentIsInRange(bool notInRange, string errorMessage)
        {
            if (notInRange)
                throw new ArgumentOutOfRangeException(errorMessage);
        }

        public static void EnsureIsNotNull<T>(T obj, string argument) where T: class 
        {
            if (obj == null)
                throw new ArgumentNullException(argument);
        }

        public static void EnsureIsNotNullOrEmpty(string obj, string argument)
        {
            if (obj == null)
                throw new ArgumentNullException(argument);
            if (obj == string.Empty)
                throw new ArgumentException(argument);
        }

        private static string GetConnectionStringWithMasterDb(ConnectionStringSettings css)
        {
            EnsureIsNotNull(css, "css");
            if (css.ProviderName == SqlClient)
                return new SqlConnectionStringBuilder(css.ConnectionString) {InitialCatalog = "master"}.ToString();
            throw new NotImplementedException();
            
        }
        public static string GetDatabaseName(string key)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[key];
            if (connectionString.ProviderName == SqlClient)
            {
                var sb = new SqlConnectionStringBuilder(connectionString.ConnectionString);
                return sb.InitialCatalog;
            }
            throw new NotImplementedException();
        }

        public static DbConnection GetConnection(string key, bool switchToMaster)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[key];
            var factory = DbProviderFactories.GetFactory(connectionString.ProviderName);
            var connection = factory.CreateConnection();
            Debug.Assert(connection != null, "_connection != null");
            connection.ConnectionString = switchToMaster ? GetConnectionStringWithMasterDb(connectionString) : connectionString.ConnectionString;
            return connection;            
        }

        public static string GetFullKey(Type type, string key)
        {
            return string.Format("{0}_{1}", type.FullName, key);
        }

        public static string ToLikeString(this string input)
        {
            return string.Format("%{0}%", input);
        }

        public static IBindingNamedWithOrOnSyntax<T> InCallContextScope<T>(this IBindingInSyntax<T> syntax)
        {
            return syntax.InScope(GetScope);
        }

        private static ScopingObject GetScope(IContext context)
        {
            return CallContextScope.Current;
        }

        public static string GetPropertyName<T>(Expression<Func<T, object>> expr)
        {
            string body;
            switch (expr.Body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    body = expr.Body.ToString();
                    break;
                case ExpressionType.Convert:
                    body = GetCleared(expr.Body.ToString(), expr.Body.NodeType.ToString(), "(", ")");
                    break;
                default:
                    throw new NotImplementedException(string.Format("{0} is not impelemented.", expr.Body.NodeType));
            }
            if (expr.Parameters.Count < 1)
                throw new ApplicationException("Not sufficent parameters");
            return GetWithoutParam(body, expr.Parameters[0].ToString());
        }

        private static string GetCleared(string input, string function, string begin, string end)
        {
            input = input.Trim();
            if (input.StartsWith(function))
                input = input.Substring(function.Length);
            if (input.StartsWith(begin))
                input = input.Substring(begin.Length);
            if (input.EndsWith(end))
                input = input.Substring(0, input.Length - end.Length);
            return input;
        }

        private static string GetWithoutParam(string input, string param)
        {
            return input.StartsWith(param) ? input.Substring(param.Length + 1) : input;
        }

        public static void RaiseEvent(EventHandler handler, object sender, Func<EventArgs> func)
        {
            EnsureIsNotNull(sender, "sender");
            EnsureIsNotNull(func, "func");
            if (handler != null)
                handler(sender, func());
        }

        public static void RaiseEvent<T>(EventHandler<T> handler, object sender, Func<T> func) where T: EventArgs
        {
            EnsureIsNotNull(sender, "sender");
            EnsureIsNotNull(func, "func");
            if (handler != null)
                handler(sender, func());
        }

        public static ConfiguredTaskAwaitable<T> NoAwait<T>(this Task<T> task)
        {
            return task.ConfigureAwait(false);
        }

        public static ConfiguredTaskAwaitable NoAwait(this Task task)
        {
            return task.ConfigureAwait(false);
        }

    }
}
