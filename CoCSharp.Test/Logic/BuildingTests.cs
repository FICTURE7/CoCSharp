using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Logic;
using NUnit.Framework;
using System;
using System.Linq;

namespace CoCSharp.Test.Logic
{
    [TestFixture]
    public class BuildingTests
    {
        private readonly AssetManager _manager;
        private Village _village;
        private Building _building;

        public BuildingTests()
        {
            _manager = new AssetManager(TestUtils.CsvDirectory);
            _manager.LoadCsv<BuildingData>("buildings.csv");
            _manager.LoadCsv<ObstacleData>("obstacles.csv");
            _manager.LoadCsv<TrapData>("traps.csv");
            _manager.LoadCsv<DecorationData>("decos.csv");
        }

        [SetUp]
        public void SetUp()
        {
            _village = new Village(_manager);
            var thdata = _manager.SearchCsv<BuildingData>(1000001, 0);
            var thbuilding = new Building(_village, thdata);

            Assert.AreSame(_village, thbuilding.Village);
            Assert.AreSame(thdata, thbuilding.Data);
            Assert.AreEqual(0, thbuilding.Level);
        }

        [Test]
        public void Constructors_InvalidArgs_Exception()
        {
            var data = _manager.SearchCsv<BuildingData>(1000002, 0);
            Assert.Throws<ArgumentNullException>(() => new Building(null, data));
            Assert.Throws<ArgumentNullException>(() => new Building(_village, null));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Building(_village, data, -2));
        }

        [Test]
        public void Constructor_village_data__Valid__AddedToVillage()
        {
            var data = _manager.SearchCsv<BuildingData>(1000002, 0);
            _building = new Building(_village, data);

            Assert.AreEqual(2, _village.Buildings.Count());
            Assert.AreSame(data, _building.Data);
        }

        [Test]
        public void Constructor_village_data_userToken__Valid__AddedToVillage_UserTokenSet()
        {
            var userToken = new object();
            var data = _manager.SearchCsv<BuildingData>(1000002, 0);
            _building = new Building(_village, data, userToken);

            Assert.AreEqual(2, _village.Buildings.Count());
            Assert.AreSame(data, _building.Data);
            Assert.AreSame(userToken, _building.UserToken);
        }

        [Test]
        public void CanUpgrade_ReturnsTrue()
        {
            
        }
    }
}
