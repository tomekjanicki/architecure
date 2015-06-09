using System.Threading.Tasks;
using log4net;
using NUnit.Framework;
using Architecture.Util.Log4Net;

namespace Architecture.Util.Test
{
    public class BaseTest
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BaseTest));

        [TestFixtureSetUp]
        public virtual void TestFixtureSetUp()
        {
            
        }

        [TestFixtureTearDown]
        public virtual void TestFixtureTearDown()
        {
            
        }

        [SetUp]
        public virtual void SetUp()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger.Info(() => string.Format("Setting up test {0}", TestContext.CurrentContext.Test.FullName));
        }

        [TearDown]
        public virtual void TearDown()
        {
            Logger.Info(() => string.Format("Tearing down up test {0}", TestContext.CurrentContext.Test.FullName));
        }

        protected static async Task<T> Delayed<T>(int miliseconds, T result)
        {
            await Task.Delay(miliseconds).NoAwait();
            return result;
        }

        protected static async Task<T> Delayed<T>(T result)
        {
            return await Delayed(10, result).NoAwait();
        }


    }
}
