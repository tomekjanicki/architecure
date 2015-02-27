using System.IO;
using Architecture.Business.Facade.Implementation;
using Architecture.Business.Facade.Interface;
using Architecture.Repository.UnitOfWork.Implementation;
using Architecture.Repository.UnitOfWork.Interface;
using Architecture.Util;
using Architecture.Util.Cache.Implementation;
using Architecture.Util.Cache.Interface;
using Architecture.Util.Mail.Interface;
using Architecture.Util.Ninject;
using Dapper;
using Ninject;
using NSubstitute;

namespace Architecture.Business.Test.Integration.Base
{
    public static class Setup
    {
        static Setup()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IBusinessLogicFacade>().To<BusinessLogicFacade>().InCallContextScope();
            kernel.Bind<ICommandsUnitOfWork>().To<CommandsUnitOfWork>().InCallContextScope();
            kernel.Bind<IMailService>().ToMethod(context => Substitute.For<IMailService>()).InCallContextScope();
            kernel.Bind<ICacheService>().To<CacheService>().InCallContextScope();
            Factory.Init(kernel);
        }

        private static readonly object Locker = new object();

        private static bool _initialized;

        private static void InitializeDatabase()
        {
            const string name = "Main";
            using (var connection = Extension.GetConnection(name, true))
                connection.Execute(string.Format(CreateDb, Extension.GetDatabaseName(name)));
            using (var connection = Extension.GetConnection(name, false))
            {
                connection.Execute(Schema);
                connection.Execute(Seed);
            }
        }

        public static void Configure()
        {
            lock (Locker)
            {
                if (!_initialized)
                {
                    InitializeDatabase();
                    _initialized = true;
                }
            }
        }

        private static string Seed
        {
            get { return GetResourceTextFile("03_Seed"); }
        }

        private static string Schema
        {
            get { return GetResourceTextFile("02_Schema"); }
        }

        private static string CreateDb
        {
            get { return GetResourceTextFile("01_CreateDb"); }
        }

        private static string GetResourceTextFile(string fileName)
        {
            var assembly = typeof(Setup).Assembly;
            var resourceName = string.Format("Architecture.Business.Test.Integration.Sql.{0}.sql", fileName);
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                    using (var sr = new StreamReader(stream))
                        return sr.ReadToEnd();
            }
            return null;
        }

    }
}
