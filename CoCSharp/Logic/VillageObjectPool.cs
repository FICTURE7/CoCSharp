using System.Collections.Concurrent;

namespace CoCSharp.Logic
{
    // An array of pools each representing an object type.
    // VillageObject.PushToPool will push the VillageObject here.
    internal static class VillageObjectPool
    {
        private const int PoolSize = 8;

        //internal static int s_popped = 0;

        static VillageObjectPool()
        {
            // Initialize a pool array of 8 element, each representing a VillageObject type.
            _pools = new ConcurrentBag<VillageObject>[PoolSize];
            for (int i = 0; i < PoolSize; i++)
                _pools[i] = new ConcurrentBag<VillageObject>();
        }

        public static int TotalCount
        {
            get
            {
                var sum = 0;
                for (int i = 0; i < PoolSize; i++)
                    sum += _pools[i].Count;
                return sum;
            }
        }

        private static ConcurrentBag<VillageObject>[] _pools;

        // Pushes the specified VillageObject to the corresponding pool.
        public static void Push(VillageObject obj)
        {
            GetPool(obj.ID).Add(obj);          
        }

        // Tries to get the VillageObject with the corresponding game ID.
        public static bool TryPop(int gameId, out VillageObject obj)
        {
            obj = default(VillageObject);
            var success = GetPool(gameId).TryTake(out obj);
            if (success)
            {
                obj._recycled++;
                obj.ResetVillageObject();
                //s_popped++;
            }

            return success;
        }

        // Returns the amount of VillageObject in a pool with the corresponding game ID.
        public static int GetCount(int gameId)
        {
            return GetPool(gameId).Count;
        }

        // Returns the pool which corresponds to the specified game ID.
        private static ConcurrentBag<VillageObject> GetPool(int gameId)
        {
            // Potential IndexOutOfRangeException here.
            return _pools[(gameId / VillageObject.Base) - 500];
        }
    }
}
