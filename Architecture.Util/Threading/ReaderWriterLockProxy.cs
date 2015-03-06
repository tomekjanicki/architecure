using System;
using System.Threading;

namespace Architecture.Util.Threading
{
    public class ReaderWriterLockProxy : IDisposable
    {
        private enum Type
        {
            Read,
            Write,
            UpgradeRead
        }

        private readonly Type _lockType;
        private readonly ReaderWriterLockSlim _rwLock;

        private ReaderWriterLockProxy(Type lockType, ReaderWriterLockSlim rwLock)
        {
            _lockType = lockType;
            _rwLock = rwLock;
        }

        internal static ReaderWriterLockProxy AcquireReader(ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterReadLock();
            return new ReaderWriterLockProxy(Type.Read, rwLock);
        }

        internal static ReaderWriterLockProxy AcquireWriter(ReaderWriterLockSlim rwLock)
        {
            rwLock.EnterWriteLock();
            return new ReaderWriterLockProxy(Type.Write, rwLock);
        }

        internal static ReaderWriterLockProxy AcquireUpgradeableReader(ReaderWriterLockSlim @lock)
        {
            @lock.EnterUpgradeableReadLock();
            return new ReaderWriterLockProxy(Type.UpgradeRead, @lock);
        }

        public void Dispose()
        {
            switch (_lockType)
            {
                case Type.Read:
                    _rwLock.ExitReadLock();
                    break;
                case Type.Write:
                    _rwLock.ExitWriteLock();
                    break;
                case Type.UpgradeRead:
                    _rwLock.ExitUpgradeableReadLock();
                    break;
            }
        }


    }
}
