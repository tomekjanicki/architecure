using System;
using System.Threading;

namespace Architecture.Util.Threading
{
    public class ReaderWriterLocker : IDisposable
    {
        private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private bool _disposed;

        public ReaderWriterLockProxy AcquireReader()
        {
            EnsureNotDisposed();
            return ReaderWriterLockProxy.AcquireReader(_lock);
        }

        public ReaderWriterLockProxy AcquireWriter()
        {
            EnsureNotDisposed();
            return ReaderWriterLockProxy.AcquireWriter(_lock);
        }

        public ReaderWriterLockProxy AcquireUpgradeableReader()
        {
            EnsureNotDisposed();
            return ReaderWriterLockProxy.AcquireUpgradeableReader(_lock);
        }

        public void Dispose()
        {
            Extension.PublicDispose(() => Dispose(true), this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Extension.ProtectedDispose(ref _disposed, disposing, () => Extension.StandardDispose(ref _lock));
        }

        private void EnsureNotDisposed()
        {
            Extension.EnsureNotDisposed<ReaderWriterLocker>(_disposed);
        }
    }
}