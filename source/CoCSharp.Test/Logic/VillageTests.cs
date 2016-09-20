using CoCSharp.Data;
using CoCSharp.Logic;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace CoCSharp.Test.Logic
{
    [TestFixture]
    public class VillageTests
    {
        private readonly AssetManager _manager;
        public VillageTests()
        {
            _manager = new AssetManager(TestUtils.CsvDirectory);
            AssetManager.Default = _manager;
        }

        [Test]
        public void FromJson_NullArgs_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => Village.FromJson(null));
            Assert.Throws<ArgumentNullException>(() => Village.FromJson("somejsonstuff", null));
        }

        [Test]
        public void FromJson_NoTownHall_Exception()
        {
            Assert.Throws<InvalidOperationException>(() => Village.FromJson(Load("village_notownhall")));
        }

        [Test]
        public void FromJson_exp_ver_Field_ValueSet()
        {
            var village = Village.FromJson(Load("village_exp_ver"));
            Assert.AreEqual(2, village.ExperienceVersion);
        }

        [Test]
        public void FromJson_builings1_Field_ValueSet()
        {
            var village = Village.FromJson(Load("village_buildings1"));
            Assert.AreEqual(1, village.Buildings.Count());
        }

        [Test]
        public void FromJson_buildings2_Field_ValueSet()
        {
            var village = Village.FromJson(Load("village_buildings2"));
            Assert.AreEqual(3, village.Buildings.Count());

            var townhall = village.TownHall;

            Assert.NotNull(townhall);
            Assert.AreEqual(24, townhall.X);
            Assert.AreEqual(23, townhall.Y);
            Assert.AreEqual(500000000, townhall.ID);
            //Assert.AreEqual(1000001, townhall.Data._OldID);
            Assert.AreEqual(1, townhall.Level);

            var building2 = village.Buildings.Where(b => b.ID == 500000002).FirstOrDefault();

            Assert.NotNull(building2);
            Assert.AreEqual(-1, building2.Level);
            //Assert.AreEqual(0, building2.NextUpgrade.Level);
        }

        public static string Load(string name)
        {
            return File.ReadAllText(Path.Combine(TestUtils.LayoutDirectory, name + ".json"));
        }
    }
}
