using CoCSharp.Csv;
using CoCSharp.Data.Models;
using NUnit.Framework;

namespace CoCSharp.Test.Csv
{
    [TestFixture]
    public class CsvDataRowTests
    {
        private CsvDataRow<BuildingData> _data;

        [SetUp]
        public void SetUp()
        {
            _data = new CsvDataRow<BuildingData>("Test Collection");
        }
    }
}
