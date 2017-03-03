using CoCSharp.Csv;
using CoCSharp.Data.Models;
using NUnit.Framework;

namespace CoCSharp.Test.Csv
{
    [TestFixture]
    public class CsvDataTableTests
    {
        private CsvDataTable _table;

        [SetUp]
        public void SetUp()
        {
            _table = new CsvDataTable<BuildingData>();
        }

        [Test]
        public void Rows_And_Column_Are_GenericTypes()
        {
            var castedTable = (CsvDataTable<BuildingData>)_table;
            //Assert.IsAssignableFrom(typeof(CsvDataColumnCollection<BuildingData>), castedTable.Columns);
            Assert.IsAssignableFrom(typeof(CsvDataRowCollection<BuildingData>), castedTable.Rows);
        }

        [Test]
        public void NewRow_Table_Name_Set()
        {
            var row = _table.NewRow("test");

            var actualTable = row.Table;
            var actualName = row.Name;
             
            Assert.AreSame(_table, actualTable);
            Assert.AreEqual("test", actualName);
        }
    }
}
