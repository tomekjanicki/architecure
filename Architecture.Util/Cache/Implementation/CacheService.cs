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
                StoreValue(data, timeToLive, absoluteExpiration, cacheNull, fullKey);
                return data;
            }
        }

        public T Get<T>(string key) where T : class
        {
            EnsureNotDisposed();
            var fullKey = Extension.GetFullKey(typeof(T), key);
            using (_locker.AcquireUpgradeableReader())
                return (T)MemoryCache.Default.Get(fullKey);
        }

        public void Add<T>(string key, T data, TimeSpan timeToLive, bool absoluteExpiration, bool cacheNull) where T : class
        {
            EnsureNotDisposed();
            var fullKey = Extension.GetFullKey(typeof(T), key);
            StoreValue(data, timeToLive, absoluteExpiration, cacheNull, fullKey);
        }

        private void StoreValue<T>(T data, TimeSpan timeToLive, bool absoluteExpiration, bool cacheNull, string fullKey) where T : class
        {
            EnsureNotDisposed();
            var ob = data == null && cacheNull ? (object) new Empty() : data;
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
                        MemoryCache.Default.Add(new CacheItem(fullKey, ob), new CacheItemPolicy {SlidingExpiration = timeToLive});
                }
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
                    if (!_permamentCache.ContainsKey(fullKey))
                    {
                        StoreValue(data, cacheNull, fullKey);
                        return data;
                    }
                    return (T)_permamentCache[fullKey];
                }
            }
        }

        public T GetPermament<T>(string key) where T : class
        {
            EnsureNotDisposed();
            var fullKey = Extension.GetFullKey(typeof(T), key);
            using (_permLocker.AcquireUpgradeableReader())
            {
                if (_permamentCache.ContainsKey(fullKey))
                    return (T)_permamentCache[fullKey];
            }
            return null;
        }

        public void AddPermament<T>(string key, T data, bool cacheNull) where T : class
        {
            EnsureNotDisposed();
            var fullKey = Extension.GetFullKey(typeof(T), key);
            using (_permLocker.AcquireWriter())
                StoreValue(data, cacheNull, fullKey);
        }

        private void StoreValue<T>(T data, bool cacheNull, string fullKey) where T : class
        {
            EnsureNotDisposed();
            if (data != null && !cacheNull || cacheNull)
                _permamentCache[fullKey] = data;
        }

        public void Remove<T>(string key) where T : class
        {
            EnsureNotDisposed();
            var fullKey = Extension.GetFullKey(typeof(T), key);
            using (_locker.AcquireWriter())
                MemoryCache.Default.Remove(fullKey);
        }

        public void RemovePermament<T>(string key) where T : class
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
                Clear();
                ClearPermament();
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
