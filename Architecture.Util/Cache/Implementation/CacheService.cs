using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Caching;
using Architecture.Util.Cache.Interface;
using Architecture.Util.Threading;

namespace Architecture.Util.Cache.Implementation
{
    public class CacheService : Disposable, ICacheService
    {
        private ReaderWriterLocker _locker = new ReaderWriterLocker();
        private ReaderWriterLocker _permLocker = new ReaderWriterLocker();
        private readonly Dictionary<string, object> _permamentCache = new Dictionary<string, object>();
        private bool _disposed;

        private class Empty
        {
        }

        public T AddOrGet<T>(string key, Func<T> fetchFunc, TimeSpan timeToLive, bool absoluteExpiration, bool cacheNull) where T : class
        {
            EnsureNotDisposed();
            var fullKey = Extension.GetFullKey(typeof(T), key);
            using (_locker.AcquireUpgradeableReader())
            {
                var cacheData = MemoryCache.Default.Get(fullKey);
                if (MemoryCache.Default.Contains(fullKey))
                {
                    if (cacheNull && cacheData is Empty)
                        return null;
                    var d = cacheData as T;
                    if (d != null)
                        return d;
                }
                var data = fetchFunc();
                var ob = data == null && cacheNull ? (object)new Empty() : data;
                if (ob != null)
                {
                    using (_locker.AcquireWriter())
                    {
                        if (absoluteExpiration)
                        {
                            var date = DateTime.Now.Add(timeToLive);
                            MemoryCache.Default.Add(fullKey, ob, date);
                        }
                        else
                            MemoryCache.Default.Add(new CacheItem(fullKey, ob), new CacheItemPolicy { SlidingExpiration = timeToLive });
                    }
                }
                return data;
            }
        }

        public T AddOrGetPermament<T>(string key, Func<T> fetchFunc, bool cacheNull) where T : class
        {
            EnsureNotDisposed();
            var fullKey = Extension.GetFullKey(typeof(T), key);
            using (_permLocker.AcquireUpgradeableReader())
            {
                if (_permamentCache.ContainsKey(fullKey))
                    return (T)_permamentCache[fullKey];
                var data = fetchFunc();
                using (_permLocker.AcquireWriter())
                {
                    if (!_permamentCache.ContainsKey(key))
                    {
                        if (data != null && !cacheNull || cacheNull)
                            _permamentCache[key] = data;
                        return data;
                    }
                    return (T)_permamentCache[fullKey];
                }
            }
        }

        public void Remove<T>(string key) where T : class
        {
            EnsureNotDisposed();
            var fullKey = Extension.GetFullKey(typeof(T), key);
            using (_locker.AcquireWriter())
                MemoryCache.Default.Remove(fullKey);
        }

        public void RemovePermamant<T>(string key) where T : class
        {
            EnsureNotDisposed();
            var fullKey = Extension.GetFullKey(typeof(T), key);
            using (_permLocker.AcquireWriter())
                _permamentCache.Remove(fullKey);
        }

        public void Clear()
        {
            EnsureNotDisposed();
            using (_locker.AcquireWriter())
                MemoryCache.Default.Dispose();
        }

        public void ClearPermament()
        {
            EnsureNotDisposed();
            using (_permLocker.AcquireWriter())
                _permamentCache.Clear();
        }

        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_locker")]
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "_permLocker")]
        protected override void Dispose(bool disposing)
        {
            ProtectedDispose(ref _disposed, disposing, () =>
            {
                using (_permLocker.AcquireWriter())
                using (_locker.AcquireWriter())
                {
                    Clear();
                    ClearPermament();
                }
                StandardDispose(ref _locker);
                StandardDispose(ref _permLocker);

            });
            base.Dispose(disposing);
        }

        private void EnsureNotDisposed()
        {
            EnsureNotDisposed(_disposed);
        }
    }
}
