using CoCSharp.Data;
using CoCSharp.Data.Csv;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoCSharp.Tests.Data.Csv
{
    [TestClass]
    public class CsvSerializerTests
    {
        [TestMethod]
        public void TestDeserialization()
        {
            var table = new CsvTable("buildings.csv");
            var data = CsvSerializer.Deserialize(table, typeof(BuildingData));
        }
    }
}
