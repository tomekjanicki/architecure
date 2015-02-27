using System;
using System.Runtime.Remoting.Messaging;

namespace Architecture.Util.Ninject.Scope
{
    public class CallContextScope: IDisposable
    {

        public CallContextScope()
        {
            CallContext.LogicalSetData(GetKey(), new ScopingObject());
        }

        public static ScopingObject Current
        {
            get { return GetScopingObject(); }
        }

        public void Dispose()
        {
            var so = Current;
            if (so != null)
            {
                so.Dispose();
                CallContext.LogicalSetData(GetKey(), null);
            }
        }

        private static string GetKey()
        {
            return Extension.GetFullKey(typeof(CallContextScope), string.Empty);
        }

        private static ScopingObject GetScopingObject()
        {
            var ob = CallContext.LogicalGetData(GetKey());
            var so = ob as ScopingObject;
            return so;
        }
    }
}