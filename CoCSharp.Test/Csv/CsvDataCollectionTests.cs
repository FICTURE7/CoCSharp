using CoCSharp.Csv;
using CoCSharp.Data.Models;
using NUnit.Framework;

namespace CoCSharp.Test.Csv
{
    [TestFixture]
    public class CsvDataCollectionTests
    {
        private CsvDataCollection<BuildingData> _data;

        [SetUp]
        public void SetUp()
        {
            _data = new CsvDataCollection<BuildingData>(null);
        }
    }
}
