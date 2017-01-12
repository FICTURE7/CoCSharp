using CoCSharp.Server.Api.Core;
using System;
using System.Collections.Concurrent;

namespace CoCSharp.Server.Core
{
    // Caching that check if the Level has been picked up by the GC as policy.
    public class CacheManager : ICacheManager
    {
        public CacheManager()
        {
            _cache = new ConcurrentDictionary<Type, ConcurrentDictionary<long, WeakReference<object>>>();
        }

        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<long, WeakReference<object>>> _cache;

        public bool TryGet<T>(long id, out T obj)
        {
            var type = typeof(T);
            var dict = GetDictionary(type);
            if (dict == null)
            {
                obj = default(T);
                return false;
            }

            var weakRef = default(WeakReference<object>);
            if (!dict.TryGetValue(id, out weakRef))
            {
                obj = default(T);
                return false;
            }

            var outObj = default(object);
            if (!weakRef.TryGetTarget(out outObj))
            {
                // Remove from cache, since the object has been picked up by the GC.
                dict.TryRemove(id, out weakRef);

                obj = default(T);
                return false;
            }

            obj = (T)outObj;
            return true;
        }

        public bool Register<T>(long id, T obj)
        {
            var type = typeof(T);
            var dict = GetDictionary(type);
            if (dict == null)
            {
                dict = new ConcurrentDictionary<long, WeakReference<object>>();
                if (!_cache.TryAdd(type, dict))
                {
                    // Ops.
                }
            }

            var overwritten = dict.ContainsKey(id);
            var weakRef = new WeakReference<object>(obj);
            if (overwritten)
            {
                dict[id] = weakRef;
            }
            else if (!dict.TryAdd(id, weakRef))
            {
                // Ops.
            }
            return overwritten;
        }

        public void Unregister<T>(long id)
        {
            var type = typeof(T);
            var dict = GetDictionary(type);
            if (dict == null)
                return;

            var weakRef = default(WeakReference<object>);
            dict.TryRemove(id, out weakRef);
        }

        private ConcurrentDictionary<long, WeakReference<object>> GetDictionary(Type type)
        {
            var dict = default(ConcurrentDictionary<long, WeakReference<object>>);
            if (!_cache.TryGetValue(type, out dict))
                return null;
            return dict;
        }
    }
}
