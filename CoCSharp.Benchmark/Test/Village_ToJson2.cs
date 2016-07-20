using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Logic;
using System;

namespace CoCSharp.Benchmark.Test
{
    public class Village_ToJson2 : BenchmarkTest
    {
        public Village_ToJson2()
        {
            var manager = AssetManager.DefaultInstance;
            _village = new Village();

            for (int i = 0; i < 395; i++)
            {
                var data = manager.SearchCsv<BuildingData>(1000000, 1);
                new Building(_village, data);
            }

            for (int i = 0; i < 45; i++)
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
    }
}
