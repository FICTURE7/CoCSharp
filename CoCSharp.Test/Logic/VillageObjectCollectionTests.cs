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
        }

        private AssetManager _manager;
        private Village _village;
        private VillageObjectCollection _collection;

        [SetUp]
        public void SetUp()
        {
            _village = new Village(_manager);
            var thdata = _manager.SearchCsv<BuildingData>(1000001, 0);
            var thbuilding = new Building(_village, thdata);

            _collection = _village.VillageObjects;
            Assert.AreEqual(500000000, thbuilding.ID);
            Assert.AreEqual(0, thbuilding._columnIndex);
        }

        [Test]
        public void Add_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _collection.Add(null));
        }

        [Test]
        public void Add_NewItem_IDsUpdated()
        {
            var data = _manager.SearchCsv<BuildingData>(1000000, 2);
            var troopbuilding = new Building(_village, data);

            Assert.AreEqual(500000001, troopbuilding.ID);
            Assert.AreEqual(1, troopbuilding._columnIndex);
        }

        [Test]
        public void Add_MultipleItem_IDUpdated()
        {
            Assert.AreEqual(1, _collection.Count);
            for (int i = 0; i < 50; i++)
            {
                var data = _manager.SearchCsv<BuildingData>(1000000, 2);
                var troopbuilding = new Building(_village, data);

                Assert.AreEqual(500000001 + i, troopbuilding.ID);
                Assert.AreEqual(1 + i, troopbuilding._columnIndex);
            }
            Assert.AreEqual(51, _collection.Count);
        }

        [Test]
        public void Remove_Item_IDsUpdated()
        {
            for (int i = 0; i < 50; i++)
            {
                var data = _manager.SearchCsv<BuildingData>(1000000, 2);
                var troopbuilding = new Building(_village, data);

                Assert.AreEqual(500000001 + i, troopbuilding.ID);
                Assert.AreEqual(1 + i, troopbuilding._columnIndex);
            }
            Assert.AreEqual(51, _collection.Count);

            var index = TestUtils.Random.Next(30);
            _collection.Remove(500000000 + index);

            Assert.AreEqual(50, _collection.Count);
            for (int i = 0; i < 49; i++)
            {
                var building = _collection[500000000 + i];
                Assert.AreEqual(i, building._columnIndex);
                Assert.AreEqual(500000000 + i, building.ID);
            }
        }
    }
}
