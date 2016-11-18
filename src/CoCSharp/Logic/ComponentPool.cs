using System.Collections.Concurrent;

namespace CoCSharp.Logic
{
    internal static class ComponentPool
    {
        private const int PoolArraySize = 8;

        static ComponentPool()
        {
            _pools = new ConcurrentBag<Component>[PoolArraySize];
            for (int i = 0; i < _pools.Length; i++)
                _pools[i] = new ConcurrentBag<Component>();
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

        private static readonly ConcurrentBag<Component>[] _pools;

        public static void Push(Component component)
        {
            if (component == null)
                return;

            GetPool(component.ComponentID).Add(component);
        }

        public static bool TryPop(int componentId, out Component component)
        {
            return GetPool(componentId).TryTake(out component);
        }

        public static int GetCount(int componentId)
        {
            return GetPool(componentId).Count;
        }

        public static ConcurrentBag<Component> GetPool(int componentId)
        {
            return _pools[componentId];
        }
    }
}
