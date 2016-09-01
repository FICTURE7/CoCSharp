using CoCSharp.Csv;
using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Logic;

namespace CoCSharp.Benchmark.Test
{
    public class Village_ToJson1 : BenchmarkTest
    {
        public Village_ToJson1()
        {
            var manager = AssetManager.Default;
            _village = new Village();

            var table = manager.Get<CsvDataTable>();
            for (int i = 0; i < 100; i++)
            {
                var data = table.GetRow<BuildingData>()[1][1];
                new Building(_village, data);
            }

            for (int i = 0; i < 100; i++)
            {
                var data = table.GetRow<ObstacleData>()[1][0];
                new Obstacle(_village, data);
            }

            for (int i = 0; i < 100; i++)
            {
                var data = table.GetRow<TrapData>()[0][0];
                new Trap(_village, data);
            }

            for (int i = 0; i < 100; i++)
            {
                var data = table.GetRow<DecorationData>()[0][0];
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
