using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Architecture.Util.Threading
{
    public class ReaderWriterLocker : Disposable
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

        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_lock")]
        protected override void Dispose(bool disposing)
        {
            ProtectedDispose(ref _disposed, disposing, () => StandardDispose(ref _lock));
            base.Dispose(disposing);
        }

        private void EnsureNotDisposed()
        {
            EnsureNotDisposed(_disposed);
        }
    }
}