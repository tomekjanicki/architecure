using System;

namespace Architecture.Util.Cache.Interface
{
    public interface ICacheService
    {
        T Get<T>(string key, Func<T> fetchFunc, TimeSpan timeToLive, bool absoluteExpiration);
        void Remove<T>(string key);
        void Clear();
    }
}