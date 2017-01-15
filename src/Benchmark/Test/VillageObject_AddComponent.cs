using CoCSharp.Csv;
using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Logic;
using CoCSharp.Logic.Components;

namespace CoCSharp.Benchmark.Test
{
    public class VillageObject_AddComponent : BenchmarkTest
    {
        private readonly Village _village;
        private Building _building;

        public override void SetUp()
        {
            //var table = AssetManager.Default.Get<CsvDataRow<BuildingData>>();
            //var data = table.Rows[0][0];
            //_building = new Building(_village, data);
        }

        public override void Execute()
        {
            //_building.AddComponent<UnitStorageComponent>();
            //_building.AddComponent(UnitProductionComponent.ID);
            _building.AddComponent(new UnitProductionComponent());
        }
    }
}
