using CoCSharp.Logic;
using NUnit.Framework;
using System;

namespace CoCSharp.Test.Logic
{
    [TestFixture]
    public class VillageObjectCollectionTests
    {
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

        [Test, Ignore("Need to figure stuff out.")]
        public void Add_NewItem_IDsUpdated()
        {
            //var data = new CsvDataRowRef<BuildingData>(1000001);
            //var troopbuilding = new Building(_village, data, 0);

            //Assert.AreEqual(500000001, troopbuilding.ID);
            //Assert.AreEqual(1, troopbuilding._columnIndex);
        }

        [Test, Ignore("Need to figure stuff out.")]
        public void Add_MultipleItem_IDUpdated()
        {
            Assert.AreEqual(1, _collection.Count);
            for (int i = 0; i < 50; i++)
            {
                //var data = new CsvDataRowRef<BuildingData>(1000001);
                //var troopbuilding = new Building(_village, data, 0);

                //Assert.AreEqual(500000001 + i, troopbuilding.ID);
                //Assert.AreEqual(1 + i, troopbuilding._columnIndex);
            }
            Assert.AreEqual(51, _collection.Count);
        }

        [Test, Ignore("Need to figure stuff out.")]
        public void Remove_Item_IDsUpdated()
        {
            for (int i = 0; i < 50; i++)
            {
                //var data = new CsvDataRowRef<BuildingData>(1000001);
                //var troopbuilding = new Building(_village, data, 0);

                //Assert.AreEqual(500000001 + i, troopbuilding.ID);
                //Assert.AreEqual(1 + i, troopbuilding._columnIndex);
            }
            Assert.AreEqual(51, _collection.Count);

            var index = TestUtils.Random.Next(30);
            _collection.Remove(500000000 + index);

            Assert.AreEqual(50, _collection.Count);
            for (int i = 0; i < 49; i++)
            {
                var building = _collection[500000000 + i];
                Assert.AreEqual(i, building._columnIndex);
                Assert.AreEqual(500000000 + i, building.Id);
            }
        }
    }
}
