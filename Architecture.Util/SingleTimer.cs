using System;
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

        private bool CanWork()
        {
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
            lock (_locker)
                _working = false;
        }

        public void Start()
        {
            _timer = new Timer(Elapsed, null, _initialDelayInSeconds * 1000, _intervalInSeconds * 1000);
        }

        public void Stop()
        {
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

        public void Dispose()
        {
            if (_timer != null) 
                _timer.Dispose();
        }
    }
}