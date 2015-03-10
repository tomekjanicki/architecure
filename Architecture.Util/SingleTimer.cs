using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Architecture.Util
{
    public class SingleTimer : IDisposable
    {
        private readonly int _initialDelayInSeconds;
        private readonly int _intervalInSeconds;
        private readonly Action _workAction;
        private readonly Action<System.Exception> _exceptionAction;
        private Timer _timer;
        private readonly object _locker = new object();
        private bool _working;
        private bool _disposed;

        private bool CanWork()
        {
            EnsureNotDisposed();
            lock (_locker)
            {
                if (!_working)
                {
                    _working = true;
                    return true;
                }
                return false;
            }
        }

        private void SetNotWorking()
        {
            EnsureNotDisposed();
            lock (_locker)
                _working = false;
        }

        public void Start()
        {
            EnsureNotDisposed();
            _timer = new Timer(Elapsed, null, _initialDelayInSeconds * 1000, _intervalInSeconds * 1000);
        }

        public void Stop()
        {
            EnsureNotDisposed();
            while (!CanWork())
            {                
            }
            _timer.Dispose();
            _timer = null;
        }

        public SingleTimer(int initialDelayInSeconds, int intervalInSeconds, Action workAction, Action<System.Exception> exceptionAction)
        {
            _initialDelayInSeconds = initialDelayInSeconds;
            _intervalInSeconds = intervalInSeconds;
            _workAction = workAction;
            _exceptionAction = exceptionAction;
        }

        private void Elapsed(object state)
        {
            EnsureNotDisposed();
            if (CanWork())
            {
                try
                {
                    _workAction();
                }
                catch (System.Exception ex)
                {
                    _exceptionAction(ex);
                }
                finally
                {
                    SetNotWorking();
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            Extension.PublicDispose(() => Dispose(true), this);
        }

        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_timer")]
        protected virtual void Dispose(bool disposing)
        {
            Extension.ProtectedDispose(ref _disposed, disposing, () => Extension.StandardDispose(ref _timer));
        }

        private void EnsureNotDisposed()
        {
            Extension.EnsureNotDisposed<SingleTimer>(_disposed);
        }
    }
}