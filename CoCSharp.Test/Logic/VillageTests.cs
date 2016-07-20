using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Logic;
using NUnit.Framework;
using System;
using System.IO;

namespace CoCSharp.Test.Logic
{
    [TestFixture]
    public class VillageTests
    {
        private readonly AssetManager _manager;
        private Village _village;

        public VillageTests()
        {
            _manager = new AssetManager(TestUtils.CsvDirectory);
            _manager.LoadCsv<BuildingData>("buildings.csv");
            _manager.LoadCsv<ObstacleData>("obstacles.csv");
            _manager.LoadCsv<TrapData>("traps.csv");
            _manager.LoadCsv<DecorationData>("decos.csv");

            var villagePath = Path.Combine(TestUtils.LayoutDirectory, "test_village.json");
            var villageJson = File.ReadAllText(villagePath);

            _village = Village.FromJson(villageJson, _manager);
        }

        [Test]
        public void TownHall_VillageContainTownHall_ReturnsTownHall()
        {
            Assert.That(_village.TownHall != null);
        }

        [Test, Ignore("Not implemented yet.")]
        public void TownHall_ChangeTownHall_ReferenceChanges()
        {
            
        }

        #region Village.GetBuilding Tests

        [Test]
        public void GetBuilding_ArgsRange_ReturnBuilding()
        {
            // Check minimum range.
            Assert.That(_village.GetBuilding(500000000), Is.TypeOf<Building>());

            // Check one between the range.
            Assert.That(_village.GetBuilding(500000001), Is.TypeOf<Building>());

            // Check maximum range in the list.
            Assert.That(_village.GetBuilding(500000004), Is.TypeOf<Building>());
        }

        [Test]
        public void GetBuilding_InvalidArgsRange_Exception()
        {
            // Make sure it throws an exception when the gameId is not in the village.
            var couldNotFindEx = Assert.Throws<ArgumentException>(() => _village.GetBuilding(500000005));
            StringAssert.Contains("Could not find Building", couldNotFindEx.Message);

            // Extreme bounds.
            couldNotFindEx = Assert.Throws<ArgumentException>(() => _village.GetBuilding(501000000));
            StringAssert.Contains("Could not find Building", couldNotFindEx.Message);

            // Make sure it throws an exception when the gameId is not valid.
            var invalidIdEx = Assert.Throws<ArgumentException>(() => _village.GetBuilding(499999999));
            StringAssert.Contains("is not a valid Building game ID", invalidIdEx.Message);

            invalidIdEx = Assert.Throws<ArgumentException>(() => _village.GetBuilding(501000001));
            StringAssert.Contains("is not a valid Building game ID", invalidIdEx.Message);
        }

        #endregion

        #region Village.GetObstacle Tests

        [Test]
        public void GetObstacle_ArgsRange_ReturnObstalce()
        {
            // Check min range.
            Assert.That(_village.GetObstacle(503000000), Is.TypeOf<Obstacle>());

            // Check one between.
            Assert.That(_village.GetObstacle(503000001), Is.TypeOf<Obstacle>());

            // Check max range.
            Assert.That(_village.GetObstacle(503000004), Is.TypeOf<Obstacle>());
        }

        [Test]
        public void GetObstacle_InvalidArgsRange_Exception()
        {
            // Make sure it throws an exception when the gameId is not in the village.
            var couldNotFindEx = Assert.Throws<ArgumentException>(() => _village.GetObstacle(503000005));
            StringAssert.Contains("Could not find Obstacle", couldNotFindEx.Message);

            couldNotFindEx = Assert.Throws<ArgumentException>(() => _village.GetObstacle(504000000));
            StringAssert.Contains("Could not find Obstacle", couldNotFindEx.Message);

            // Make sure it throws an exception when the gameId is not valid.
            var invalidIdEx = Assert.Throws<ArgumentException>(() => _village.GetObstacle(502999999));
            StringAssert.Contains("is not a valid Obstacle game ID", invalidIdEx.Message);

            invalidIdEx = Assert.Throws<ArgumentException>(() => _village.GetObstacle(504000001));
            StringAssert.Contains("is not a valid Obstacle game ID", invalidIdEx.Message);
        }

        #endregion

        #region Village.GetTrap Tests

        [Test]
        public void GetTrap_ArgsRange_ReturnTrap()
        {
            // Check min range.
            Assert.That(_village.GetTrap(504000000), Is.TypeOf<Trap>());

            // Check one between.
            Assert.That(_village.GetTrap(504000001), Is.TypeOf<Trap>());

            // Check max range.
            Assert.That(_village.GetTrap(504000004), Is.TypeOf<Trap>());
        }

        [Test]
        public void GetTrap_InvalidArgsRange_Exception()
        {
            // Make sure it throws an exception when the gameId is not in the village.
            var couldNotFindEx = Assert.Throws<ArgumentException>(() => _village.GetTrap(504000005));
            StringAssert.Contains("Could not find Trap", couldNotFindEx.Message);

            couldNotFindEx = Assert.Throws<ArgumentException>(() => _village.GetTrap(505000000));
            StringAssert.Contains("Could not find Trap", couldNotFindEx.Message);

            // Make sure it throws an exception when the gameId is not valid.
            var invalidIdEx = Assert.Throws<ArgumentException>(() => _village.GetTrap(503999999));
            StringAssert.Contains("is not a valid Trap game ID", invalidIdEx.Message);

            invalidIdEx = Assert.Throws<ArgumentException>(() => _village.GetTrap(505000001));
            StringAssert.Contains("is not a valid Trap game ID", invalidIdEx.Message);
        }

        #endregion

        #region Village.GetDecoration Tests

        [Test]
        public void GetDecoration_ArgsRange_ReturnDecoration()
        {
            // Check min range.
            Assert.That(_village.GetDecoration(506000000), Is.TypeOf<Decoration>());

            // Check one between.
            Assert.That(_village.GetDecoration(506000001), Is.TypeOf<Decoration>());

            // Check max range.
            Assert.That(_village.GetDecoration(506000004), Is.TypeOf<Decoration>());
        }

        [Test]
        public void GetDecoration_InvalidArgsRange_Exception()
        {
            // Make sure it throws an exception when the gameId is not in the village.
            var couldNotFindEx = Assert.Throws<ArgumentException>(() => _village.GetDecoration(506000005));
            StringAssert.Contains("Could not find Decoration", couldNotFindEx.Message);

            couldNotFindEx = Assert.Throws<ArgumentException>(() => _village.GetDecoration(507000000));
            StringAssert.Contains("Could not find Decoration", couldNotFindEx.Message);

            // Make sure it throws an exception when the gameId is not valid.
            var invalidIdEx = Assert.Throws<ArgumentException>(() => _village.GetDecoration(507000001));
            StringAssert.Contains("is not a valid Decoration game ID", invalidIdEx.Message);
        }

        #endregion

        [Test]
        public void GetVillageObject_ArgsRange_ReturnVillageObject()
        {
            Assert.That(_village.GetVillageObject(500000000), Is.TypeOf<Building>());

            Assert.That(_village.GetVillageObject(503000000), Is.TypeOf<Obstacle>());
        }
    }
}
