using System;

namespace Architecture.Util.Ninject.Scope
{
    public class ThreadProcessingScope: IDisposable
    {
        [ThreadStatic]
        private static ScopingObject _scopingObject;

        public ThreadProcessingScope()
        {
            _scopingObject = new ScopingObject();
        }

        public static ScopingObject Current
        {
            get { return _scopingObject; }
        }

        public void Dispose()
        {
            _scopingObject.Dispose();
            _scopingObject = null;
        }
    }
}