using CoCSharp.Csv;
using CoCSharp.Data.Models;
using NUnit.Framework;
using System;

namespace CoCSharp.Test.Csv
{
    [TestFixture]
    public class CsvDataRowCollectionTests
    {
        private CsvDataRowCollection _rows;

        [SetUp]
        public void SetUp()
        {
            _rows = new CsvDataRowCollection<BuildingData>(null);
        }

        [Test]
        public void Add_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _rows.Add(null));
        }

        [Test]
        public void Add()
        {

        }
    }
}
