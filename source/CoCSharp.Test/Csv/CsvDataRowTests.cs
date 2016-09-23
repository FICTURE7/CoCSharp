using CoCSharp.Csv;
using CoCSharp.Data.Models;
using NUnit.Framework;

namespace CoCSharp.Test.Csv
{
    [TestFixture]
    public class CsvDataRowTests
    {
        private CsvDataTable<BuildingData> _table;
        private CsvDataRow<BuildingData> _row;

        [SetUp]
        public void SetUp()
        {
            _table = new CsvDataTable<BuildingData>();
            _row = new CsvDataRow<BuildingData>(_table, "Test Collection");
        }
    }
}
