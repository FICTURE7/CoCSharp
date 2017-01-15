using System.Collections.Concurrent;

namespace CoCSharp.Logic
{
    // An array of pools each representing an object type.
    // VillageObject.PushToPool will push the VillageObject here.
    internal static class VillageObjectPool
    {
        private const int PoolArraySize = 8;

        static VillageObjectPool()
        {
            // Initialize a pool array of 8 element, each representing a VillageObject type.
            _pools = new ConcurrentBag<VillageObject>[PoolArraySize];
            for (int i = 0; i < PoolArraySize; i++)
                _pools[i] = new ConcurrentBag<VillageObject>();
        }

        public static int TotalCount
        {
            get
            {
                var sum = 0;
                for (int i = 0; i < PoolArraySize; i++)
                    sum += _pools[i].Count;
                return sum;
            }
        }

        private static readonly ConcurrentBag<VillageObject>[] _pools;

        // Pushes the specified VillageObject to the corresponding pool.
        public static void Push(VillageObject obj)
        {
            GetPool(obj.KindId).Add(obj);
        }

        // Tries to get the VillageObject with the corresponding game ID.
        public static bool TryPop(int kindId, out VillageObject obj)
        {
            //return GetPool(gameId).TryTake(out obj);

            obj = default(VillageObject);
            var success = GetPool(kindId).TryTake(out obj);
            if (success)
                obj.ResetVillageObject();

            return success;
        }

        // Returns the pool which corresponds to the specified game ID.
        private static ConcurrentBag<VillageObject> GetPool(int kindId)
        {
            // Potential IndexOutOfRangeException here.
            return _pools[kindId];
        }
    }
}
