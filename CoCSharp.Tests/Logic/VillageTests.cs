using CoCSharp.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace CoCSharp.Tests.Logic
{
    [TestClass]
    public class VillageTests
    {
        [TestMethod]
        public void TestVillageFromJson()
        {
            var villageJson = File.ReadAllText("home.json");
            var village = new Village();
            village.FromJson(villageJson);
        }
    }
}
