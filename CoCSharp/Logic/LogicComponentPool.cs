using System.Collections.Concurrent;

namespace CoCSharp.Logic
{
    internal static class LogicComponentPool
    {
        private const int PoolArraySize = 8;

        static LogicComponentPool()
        {
            _pools = new ConcurrentBag<LogicComponent>[PoolArraySize];
            for (int i = 0; i < _pools.Length; i++)
                _pools[i] = new ConcurrentBag<LogicComponent>();
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

        private static readonly ConcurrentBag<LogicComponent>[] _pools;

        public static void Push(LogicComponent component)
        {
            GetPool(component.ComponentID).Add(component);
        }

        public static bool TryPop(int componentId, out LogicComponent component)
        {
            return GetPool(componentId).TryTake(out component);
        }

        public static int GetCount(int componentId)
        {
            return GetPool(componentId).Count;
        }

        public static ConcurrentBag<LogicComponent> GetPool(int componentId)
        {
            return _pools[componentId];
        }
    }
}
