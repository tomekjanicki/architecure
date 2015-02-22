using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Architecture.Util.Test
{
    public class BaseTest
    {
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
            Message(string.Format("Setting up test: {0}", TestContext.CurrentContext.Test.FullName));            
        }

        [TearDown]
        public virtual void TearDown()
        {
            Message(string.Format("Tearing down up test: {0}", TestContext.CurrentContext.Test.FullName));            
        }

        protected void Message(string message)
        {
            Trace.WriteLine(string.Format("Time: {0}: Thread Id: {1}, Msg: {2}", DateTime.Now.ToString("hh:mm:ss.ffff"), Thread.CurrentThread.ManagedThreadId, message));
        }

        protected static async Task<T> Delayed<T>(int miliseconds, T result)
        {
            await Task.Delay(miliseconds);
            return result;
        }

        protected static async Task<T> Delayed<T>(T result)
        {
            return await Delayed(100, result);
        }


    }
}
