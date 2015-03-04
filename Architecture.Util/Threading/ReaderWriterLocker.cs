using System.Threading;

namespace Architecture.Util.Threading
{
    public class ReaderWriterLocker
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public ReaderWriterLockProxy AcquireReader()
        {
            return ReaderWriterLockProxy.AcquireReader(_lock);
        }

        public ReaderWriterLockProxy AcquireWriter()
        {
            return ReaderWriterLockProxy.AcquireWriter(_lock);
        }

        public ReaderWriterLockProxy AcquireUpgradeableReader()
        {
            return ReaderWriterLockProxy.AcquireUpgradeableReader(_lock);
        }

    }
}