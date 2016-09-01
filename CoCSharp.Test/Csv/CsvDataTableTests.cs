using CoCSharp.Csv;
using CoCSharp.Data.Models;
using NUnit.Framework;
using System;

namespace CoCSharp.Test.Csv
{
    [TestFixture]
    public class CsvDataTableTests
    {
        private CsvDataTable _table;

        [SetUp]
        public void SetUp()
        {
            _table = new CsvDataTable();
        }

        [Test]
        public void Add_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _table.Add(null));
        }

        [Test]
        public void Add_ValidItem_Added()
        {
            _table.Add(new CsvDataRow<BuildingData>());
            Assert.AreEqual(1, _table.Count);
        }

        [Test]
        public void GetTableT_ValidItem_ReturnsSameInstance()
        {
            var row1 = new CsvDataRow<BuildingData>();
            _table.Add(row1);

            var retRow1 = _table.GetRow<BuildingData>();
            Assert.AreSame(row1, retRow1);
        }

        [Test]
        public void GetTable_InvalidArgs_Exception()
        {
            var row1 = new CsvDataRow<BuildingData>();
            _table.Add(row1);

            Assert.Throws<ArgumentNullException>(() => _table.GetRow(null));
            Assert.Throws<ArgumentException>(() => _table.GetRow(typeof(object)));
            Assert.Throws<ArgumentException>(() => _table.GetRow(typeof(CsvData)));
        }

        [Test]
        public void GetTable_ValidItem_ReturnsSameInstance()
        {
            var row1 = new CsvDataRow<ObstacleData>();
            _table.Add(row1);

            var retRow1 = _table.GetRow(typeof(ObstacleData));
            Assert.AreSame(row1, retRow1);
        }
    }
}
