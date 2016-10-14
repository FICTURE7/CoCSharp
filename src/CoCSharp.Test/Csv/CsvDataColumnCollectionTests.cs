using CoCSharp.Csv;
using NUnit.Framework;

namespace CoCSharp.Test.Csv
{
    [TestFixture]
    public class CsvDataColumnCollectionTests
    {
        private CsvDataTable _table;

        [SetUp]
        public void Setup()
        {
            _table = new CsvDataTable<TestData>();
        }

        [Test]
        public void Add_Column_DataLevel_IsUpdated()
        {
            var column = new CsvDataColumn();
            Assert.Null(column.Table);
            Assert.AreEqual(-1, column.DataLevel);

            _table.Columns.Add(column);

            Assert.AreSame(column.Table, _table);
            Assert.AreEqual(0, column.DataLevel);
        }
    }
}
