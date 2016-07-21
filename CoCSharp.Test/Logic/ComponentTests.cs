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
            _village = new Village();

            var thdata = AssetManager.DefaultInstance.SearchCsv<BuildingData>(1000001, 0);
            _building = new Building(_village, thdata);
        }

        private readonly Village _village;
        private Building _building;

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
