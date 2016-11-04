using CoCSharp.Csv;
using CoCSharp.Data.Models;
using NUnit.Framework;
using System;
using System.IO;

namespace CoCSharp.Test.Csv
{
    [TestFixture]
    public class CsvConvertTests
    {
        private readonly string _tablePath = Path.Combine(TestUtils.CsvDirectory, "buildings.csv");
        private CsvTable _table;

        [SetUp]
        public void SetUp()
        {
            _table = new CsvTable(_tablePath);
        }

        [Test]
        public void Deserialize_InvalidArgs_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => CsvConvert.Deserialize(null, null));
            Assert.Throws<ArgumentNullException>(() => CsvConvert.Deserialize(_table, null));
            Assert.Throws<ArgumentException>(() => CsvConvert.Deserialize(_table, typeof(TestType)));
            Assert.Throws<ArgumentException>(() => CsvConvert.Deserialize(_table, typeof(CsvData)));
        }

        [Test]
        public void Deserialize_ReturnType()
        {
            var table = CsvConvert.Deserialize(_table, typeof(BuildingData));
            Assert.IsInstanceOf<CsvDataTable<BuildingData>>(table);
        }

        private class TestType
        {
            // Space
        }
    }
}
