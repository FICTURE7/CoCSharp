using CoCSharp.Csv;
using CoCSharp.Data.Models;
using NUnit.Framework;
using System.IO;

namespace CoCSharp.Test.Csv
{
    [TestFixture]
    public class CsvConvertTests
    {
        [SetUp]
        public void SetUp()
        {
            var tablePath = Path.Combine(TestUtils.CsvDirectory, "com_buildings.csv");
            _table = new CsvTable(tablePath, true);
        }

        private CsvTable _table;

        [Test]
        public void DeserializeNew()
        {
            var t = CsvConvert.DeserializeNew<BuildingData>(_table);
        }

        [Test]
        public void Deserialize()
        {
            var t = CsvConvert.Deserialize<BuildingData>(_table);
        }
    }
}
