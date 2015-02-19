using System;
using Ninject.Infrastructure.Disposal;

namespace Architecture.Util.Ninject.Scope
{
    public class ScopingObject : INotifyWhenDisposed
    {
        public void Dispose()
        {
            IsDisposed = true;
            Extension.RaiseEvent(Disposed, this, () => new EventArgs());
        }

        public bool IsDisposed { get; private set; }
        public event EventHandler Disposed;
    }
}