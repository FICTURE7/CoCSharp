using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Logic;
using NUnit.Framework;
using System;

namespace CoCSharp.Test.Logic
{
    [TestFixture]
    public class VillageObjectCollectionTests
    {
        public VillageObjectCollectionTests()
        {
            _manager = new AssetManager(TestUtils.CsvDirectory);
            _manager.LoadCsv<BuildingData>("buildings.csv");
            _manager.LoadCsv<ObstacleData>("obstacles.csv");
            _manager.LoadCsv<TrapData>("traps.csv");
            _manager.LoadCsv<DecorationData>("decos.csv");

            _village = new Village(_manager);
            var thdata = _manager.SearchCsv<BuildingData>(1000001, 0);
            var thbuilding = new Building(_village, thdata);
        }

        private AssetManager _manager;
        private Village _village;
        private VillageObjectCollection _collection;

        [SetUp]
        public void SetUp()
        {
            _collection = new VillageObjectCollection();
        }

        [Test]
        public void Add_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _collection.Add(null));
        }

        [Test]
        public void Add_NewItem_IDUpdated()
        {
            var data = _manager.SearchCsv<BuildingData>(1000000, 2);
            var troopbuilding = new Building(_village, data);

            _collection.Add(troopbuilding);
            Assert.AreEqual(500000000, troopbuilding.ID);
        }
    }
}
