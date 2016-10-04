using CoCSharp.Csv;
using CoCSharp.Data.Models;
using NUnit.Framework;

namespace CoCSharp.Test.Csv
{
    [TestFixture]
    public class CsvDataRowTests
    {
        private CsvDataTable _table;
        private CsvDataRow _row;

        [SetUp]
        public void SetUp()
        {
            _table = new CsvDataTable<BuildingData>();
            _row = new CsvDataRow<BuildingData>(_table, "Test Collection");
        }

        [Test]
        public void When_Ref_IsNull_ID_Is_Negative_1()
        {
            Assert.Null(_row.Ref);
            Assert.AreEqual(-1, _row.ID);
        }

        [Test]
        public void K()
        {
            var data = _row[0];            
        }
    }
}
