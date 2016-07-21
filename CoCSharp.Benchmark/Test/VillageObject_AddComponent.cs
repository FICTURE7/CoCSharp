using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Logic;
using CoCSharp.Logic.Components;

namespace CoCSharp.Benchmark.Test
{
    public class VillageObject_AddComponent : BenchmarkTest
    {
        public VillageObject_AddComponent()
        {
            _village = new Village();

            var thdata = AssetManager.DefaultInstance.SearchCsv<BuildingData>(1000001, 0);
            var th = new Building(_village, thdata);
        }

        private readonly Village _village;
        private Building _building;

        public override void SetUp()
        {
            var data = AssetManager.DefaultInstance.SearchCsv<BuildingData>(1000000, 0);
            _building = new Building(_village, data);
        }

        public override void Execute()
        {
            //_building.AddComponent<UnitStorageComponent>();
            //_building.AddComponent(UnitProductionComponent.ID);
            _building.AddComponent(new UnitProductionComponent());
        }
    }
}
