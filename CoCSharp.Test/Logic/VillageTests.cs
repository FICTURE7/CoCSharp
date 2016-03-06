using CoCSharp.Logic;
using NUnit.Framework;
using System;
using System.IO;

namespace CoCSharp.Test.Logic
{
    [TestFixture]
    public class VillageTests
    {
        [Test]
        public void TestVillageGetVillageObjects()
        {
            var json = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content/starting_home.json"));
            var village = Village.FromJson(json);

            Assert.That(village.GetBuilding(500000000) != null);
            Assert.That(village.GetBuilding(500000004) != null);

            Assert.Throws<ArgumentException>(() => village.GetBuilding(500000005));
            Assert.Throws<ArgumentException>(() => village.GetBuilding(499999999));
        }
    }
}
