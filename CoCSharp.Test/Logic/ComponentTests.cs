using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Logic;
using CoCSharp.Logic.Components;
using NUnit.Framework;

namespace CoCSharp.Test.Logic
{
    [TestFixture]
    public class ComponentTests
    {
        public ComponentTests()
        {
            _manager = new AssetManager(TestUtils.CsvDirectory);
            _manager.LoadCsv<BuildingData>("buildings.csv");
            _manager.LoadCsv<ObstacleData>("obstacles.csv");
            _manager.LoadCsv<TrapData>("traps.csv");
            _manager.LoadCsv<DecorationData>("decos.csv");

            _village = new Village(_manager);

            // Level 0(1), Town Hall.
            var thdata = _manager.SearchCsv<BuildingData>(1000001, 0);
            var th = new Building(_village, thdata);
        }

        private readonly AssetManager _manager;
        private readonly Village _village;
        private Building _building;

        [SetUp]
        public void SetUp()
        {
            // Level 0(1), Gold Mine.
            var data = _manager.SearchCsv<BuildingData>(1000004, 0);
            _building = new Building(_village, data);
        }

        [Test]
        public void GetComponent_ComponentNotFound_ReturnsNull()
        {
            var component = _building.GetComponent<UnitStorageComponent>();
            Assert.Null(component);
        }

        [Test]
        public void GetComponent_ComponentFound_ReturnsComponentInstance()
        {
            // Internal stuff.
            var component = new UnitStorageComponent();
            _building.AddComponent(component);

            // Public stuff.
            var componentget = _building.GetComponent<UnitStorageComponent>();
            Assert.AreSame(component, componentget);
        }
    }
}
