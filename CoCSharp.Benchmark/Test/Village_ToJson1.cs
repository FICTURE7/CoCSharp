using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Logic;

namespace CoCSharp.Benchmark.Test
{
    public class Village_ToJson1 : BenchmarkTest
    {
        public Village_ToJson1()
        {
            var manager = AssetManager.DefaultInstance;
            _village = new Village();

            for (int i = 0; i < 100; i++)
            {
                var data = manager.SearchCsv<BuildingData>(1000000, 1);
                new Building(_village, data);
            }

            for (int i = 0; i < 100; i++)
            {
                var data = manager.SearchCsv<ObstacleData>(8000000, 0);
                new Obstacle(_village, data);
            }

            for (int i = 0; i < 100; i++)
            {
                var data = manager.SearchCsv<TrapData>(12000000, 1);
                new Trap(_village, data);
            }

            for (int i = 0; i < 100; i++)
            {
                var data = manager.SearchCsv<DecorationData>(18000000, 0);
                new Decoration(_village, data);
            }
        }

        private readonly Village _village;

        public override void Execute()
        {
            _village.ToJson();
        }

        public override void TearDown()
        {
            _village.Dispose();
        }
    }
}
