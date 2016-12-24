using CoCSharp.Server.Api.Core;
using CoCSharp.Server.Api.Db;
using System;
using System.Collections.Concurrent;

namespace CoCSharp.Server.Core
{
    // Caching that check if the Level has been picked up by the GC as policy.
    public class CacheManager : ICacheManager
    {
        public CacheManager()
        {
            _levelCache = new ConcurrentDictionary<long, WeakReference<LevelSave>>();
            _clanCache = new ConcurrentDictionary<long, WeakReference<ClanSave>>();
        }

        // Dictionary that is going to map UserIds to their respective level.
        private readonly ConcurrentDictionary<long, WeakReference<LevelSave>> _levelCache;
        // Dictionary that is going to map ClanIds to their respective clan.
        private readonly ConcurrentDictionary<long, WeakReference<ClanSave>> _clanCache;

        public LevelSave GetLevel(long userId)
        {
            var weakRef = (WeakReference<LevelSave>)null;
            if (!_levelCache.TryGetValue(userId, out weakRef))
                return null;

            var target = (LevelSave)null;
            if (!weakRef.TryGetTarget(out target))
                _levelCache.TryRemove(userId, out weakRef);
            return target;
        }

        public void RegisterLevel(LevelSave level, long userId)
        {
            if (level == null)
                throw new ArgumentNullException(nameof(level));

            var weakRef = new WeakReference<LevelSave>(level);
            if (_levelCache.ContainsKey(userId))
                _levelCache[userId] = weakRef;
            else
                _levelCache.TryAdd(userId, weakRef);
        }

        public ClanSave GetClan(long clanId)
        {
            var weakRef = (WeakReference<ClanSave>)null;
            if (!_clanCache.TryGetValue(clanId, out weakRef))
                return null;

            var target = (ClanSave)null;
            if (!weakRef.TryGetTarget(out target))
                _clanCache.TryRemove(clanId, out weakRef);
            return target;
        }

        public void RegisterClan(ClanSave clan, long clanId)
        {
            if (clan == null)
                throw new ArgumentNullException(nameof(clan));

            var weakRef = new WeakReference<ClanSave>(clan);
            if (_clanCache.ContainsKey(clanId))
                _clanCache[clanId] = weakRef;
            else
                _clanCache.TryAdd(clanId, weakRef);
        }

        public void Clear()
        {
            _levelCache.Clear();
            _clanCache.Clear();
        }
    }
}
