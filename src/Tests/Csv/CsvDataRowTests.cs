using CoCSharp.Csv;
using NUnit.Framework;
using System;

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
            _table = new CsvDataTable<TestData>();
            _row = _table.NewRow("LeRow");
        }

        [TestCase(0, ExpectedResult = null)]
        [TestCase(1337, ExpectedResult = null)]
        [TestCase(int.MaxValue, ExpectedResult = null)]
        public CsvData Indexer_Getter_Column_AtIndex_DoesNot_Exists_ReturnsNull(int index)
        {
            return _row[index];
        }

        [Test]
        public void Indexer_Setter_Column_DoesNot_Belong_ToTable_Exception()
        {
            var column = new CsvDataColumn();
            Assert.Throws<ArgumentException>(() => _row[column] = new TestData());
        }

        [Test]
        public void Indexer_Getter_Column_Belongs_ToTable_ReturnsValue()
        {
            var column = new CsvDataColumn();
            _table.Columns.Add(column);

            Assert.Null(_row[column]);
        }

        [Test]
        public void Indexer_Setter_Column_Belongs_ToTable_ValueSet()
        {
            var column = new CsvDataColumn();
            _table.Columns.Add(column);

            var value = new TestData();

            _row[column] = value;
            Assert.NotNull(_row[column]);
            Assert.AreSame(value, _row[column]);
        }
    }
}
