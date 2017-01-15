using CoCSharp.Csv;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CoCSharp.Test.Csv
{
    [TestFixture]
    public class CsvDataRowCollectionTests
    {
        private CsvDataTable _table;
        private CsvDataRowCollection _rows;

        [SetUp]
        public void SetUp()
        {
            _table = new CsvDataTable<TestData>();
            _rows = _table.Rows;
        }

        [Test]
        public void Add_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _rows.Add(null));
        }

        [Test]
        public void Add_Row_Belong_ToAnother_Table_Exception()
        {
            var table = new CsvDataTable<TestData>();
            var row = table.NewRow("lerow");

            Assert.Throws<ArgumentException>(() => _rows.Add(row));
        }

        [Test]
        public void Add_Row_Already_Belong_ToTable_Exception()
        {
            var row = _table.NewRow("lerow");
            // Added row.
            _rows.Add(row);
            // Adding row again, exception thrown.
            Assert.Throws<ArgumentException>(() => _rows.Add(row));
        }

        [Test]
        public void Add_Row_Ref_Is_Updated()
        {
            var row = _table.NewRow("lerow");
            Assert.Null(row.Ref);
            Assert.AreEqual(-1, row.Id);

            _rows.Add(row);

            var rowRef = row.Ref;
            Assert.NotNull(rowRef);
            Assert.AreEqual(0, rowRef.RowIndex);
            Assert.AreEqual(row.Id, rowRef.Id);
        }

        [Test]
        public void Insert_Row_Ref_Is_Updated_AsWell_As_Old_Rows()
        {
            for (int i = 0; i < 10; i++)
            {
                var nrow = _table.NewRow("lerow");
                _rows.Add(nrow);
            }

            Assert.AreEqual(10, _rows.Count);

            var row = _table.NewRow("insertedrow");
            _rows.InsertAt(row, 2);
            Assert.AreEqual(11, _rows.Count);

            for (int i = 0; i < _rows.Count; i++)
            {
                var rowRef = _rows[i].Ref;

                Assert.NotNull(rowRef);
                Assert.AreEqual(i, rowRef.RowIndex);
            }
        }

        [Test]
        public void Indexer_String_Get_If_NotExists_ReturnNull()
        {
            var row = _rows["row_with_name_that_does_not_exists"];

            Assert.Null(row);
        }

        [Test]
        public void Indexer_String_Get_Return_Row_Instance()
        {
            var expectedRow = _table.NewRow("lerow");
            _rows.Add(expectedRow);

            var actualRow = _rows["lerow"];
            Assert.NotNull(actualRow);
            Assert.AreSame(expectedRow, actualRow);
        }

        [Test]
        public void CopyTo_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _rows.CopyTo(null, 0));
        }

        [Test]
        public void CopyTo_Negative_arrayIndex_Exception()
        {
            var array = new CsvDataRow[10];
            Assert.Throws<ArgumentOutOfRangeException>(() => _rows.CopyTo(array, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _rows.CopyTo(array, int.MinValue));

            Assert.Throws<ArgumentOutOfRangeException>(() => _rows.CopyTo(array, 10));
        }

        [Test]
        public void CopyTo_ValidArgs_ElementsCopied()
        {
            for (int i = 0; i < 10; i++)
                _rows.Add(_table.NewRow("lerow" + i));

            var array = new CsvDataRow[10];

            _rows.CopyTo(array, 2);
            for (int i = 0; i < 2; i++)
                Assert.Null(array[i]);

            for (int i = 2; i < 8; i++)
            {
                var expected = _rows[i - 2];
                var actual = array[i];
                Assert.AreSame(expected, actual);
            }
        }
    }
}
