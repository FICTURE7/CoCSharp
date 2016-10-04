using CoCSharp.Csv;
using CoCSharp.Data.Models;
using NUnit.Framework;
using System;

namespace CoCSharp.Test.Csv
{
    [TestFixture]
    public class CsvDataTableCollectionTests
    {
        private CsvDataTableCollection _table;

        [SetUp]
        public void SetUp()
        {
            _table = new CsvDataTableCollection();
        }

        [Test]
        public void Add_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _table.Add(null));
        }

        [Test]
        public void Add_ValidItem_Added()
        {
            _table.Add(new CsvDataTable<BuildingData>());
            Assert.AreEqual(1, _table.Count);
        }

        [Test]
        public void GetTableT_ValidItem_ReturnsSameInstance()
        {
            var row1 = new CsvDataTable<BuildingData>();
            _table.Add(row1);

            var retRow1 = _table.GetTable<BuildingData>();
            Assert.AreSame(row1, retRow1);
        }

        [Test]
        public void GetTable_InvalidArgs_Exception()
        {
            var row1 = new CsvDataTable<BuildingData>();
            _table.Add(row1);

            Assert.Throws<ArgumentNullException>(() => _table.GetTable(null));
            Assert.Throws<ArgumentException>(() => _table.GetTable(typeof(object)));
            Assert.Throws<ArgumentException>(() => _table.GetTable(typeof(CsvData)));
        }

        [Test]
        public void GetTable_ValidItem_ReturnsSameInstance()
        {
            var row1 = new CsvDataTable<ObstacleData>();
            _table.Add(row1);

            var retRow1 = _table.GetTable(typeof(ObstacleData));
            Assert.AreSame(row1, retRow1);
        }
    }
}
