using System;
using System.Runtime.Caching;
using Architecture.Util.Cache.Interface;

namespace Architecture.Util.Cache.Implementation
{
    public class CacheService : ICacheService
    {
        public T Get<T>(string key, Func<T> fetchFunc, TimeSpan timeToLive, bool absoluteExpiration)
        {
            var fullKey = Extension.GetFullKey(typeof(T), key);
            var cache = MemoryCache.Default;
            if (cache.Contains(fullKey))
                return (T) cache.Get(fullKey);
            var data = fetchFunc();
            if (absoluteExpiration)
            {
                var date = DateTime.Now.Add(timeToLive);
                cache.Add(fullKey, data, date);
            }
            else
                cache.Add(new CacheItem(fullKey, data), new CacheItemPolicy {SlidingExpiration = timeToLive});
            return data;
        }

        public void Remove<T>(string key)
        {
            var fullKey = Extension.GetFullKey(typeof(T), key);
            var cache = MemoryCache.Default;
            if (cache.Contains(fullKey))
                cache.Remove(fullKey);
        }
    }
}
