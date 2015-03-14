using System;

namespace Architecture.Util.Cache.Interface
{
    public interface ICacheService : IDisposable
    {
        T AddOrGet<T>(string key, Func<T> fetchFunc, TimeSpan timeToLive, bool absoluteExpiration, bool cacheNull) where T: class;
        T Get<T>(string key) where T : class;
        void Add<T>(string key, T data, TimeSpan timeToLive, bool absoluteExpiration, bool cacheNull) where T : class;
        T AddOrGetPermament<T>(string key, Func<T> fetchFunc, bool cacheNull) where T : class;
        T GetPermament<T>(string key) where T : class;
        void AddPermament<T>(string key, T data, bool cacheNull) where T : class;
        void Remove<T>(string key) where T: class;
        void RemovePermament<T>(string key) where T : class;
        void Clear();
        void ClearPermament();
    }
}