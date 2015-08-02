using CoCSharp.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoCSharp.Tests.Data
{
    [TestClass]
    public class DatabaseTests
    {
        [TestMethod]
        public void TestLoadBuildingData()
        {
            var buildingDb = new BuildingDatabase("buildings.csv");
        }
    }
}
