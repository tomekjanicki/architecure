using System;
using System.Runtime.Caching;
using Architecture.Util.Cache.Interface;

namespace Architecture.Util.Cache.Implementation
{
    public class CacheService : ICacheService
    {
        private readonly object _locker = new object();

        public T Get<T>(string key, Func<T> fetchFunc, TimeSpan timeToLive, bool absoluteExpiration)
        {
            var fullKey = Extension.GetFullKey(typeof(T), key);
            lock (_locker)
            {
                if (MemoryCache.Default.Contains(fullKey))
                    return (T) MemoryCache.Default.Get(fullKey);
            }
            var data = fetchFunc();
            lock (_locker)
            {
                if (!MemoryCache.Default.Contains(fullKey))
                {
                    if (absoluteExpiration)
                    {
                        var date = DateTime.Now.Add(timeToLive);
                        MemoryCache.Default.Add(fullKey, data, date);
                    }
                    else
                        MemoryCache.Default.Add(new CacheItem(fullKey, data), new CacheItemPolicy { SlidingExpiration = timeToLive });                    
                }
                return data;
            }            
        }

        public void Remove<T>(string key)
        {
            var fullKey = Extension.GetFullKey(typeof(T), key);            
            lock (_locker)
            {
                if (MemoryCache.Default.Contains(fullKey))
                    MemoryCache.Default.Remove(fullKey);                
            }
        }

        public void Clear()
        {
            lock (_locker)
                MemoryCache.Default.Dispose();
        }
    }
}
