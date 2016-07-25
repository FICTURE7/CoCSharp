using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Logic;
using NUnit.Framework;
using System;
using System.ComponentModel;
using System.IO;

namespace CoCSharp.Test.Logic
{
    [TestFixture]
    public class BuildingTests
    {
        private readonly AssetManager _manager;
        private Village _village;

        public BuildingTests()
        {
            _manager = new AssetManager(TestUtils.CsvDirectory);
            _manager.LoadCsv<BuildingData>("buildings.csv");
            _manager.LoadCsv<ObstacleData>("obstacles.csv");
            _manager.LoadCsv<TrapData>("traps.csv");
            _manager.LoadCsv<DecorationData>("decos.csv");

            _village = new Village(_manager);

            // Add townhall to village.
            // Level 1(2), Town Hall.
            new Building(_village, _manager.SearchCsv<BuildingData>(1000001, 1));
        }

        [Test]
        public void Constructors__ValidArgs__Building_Added_To_Village()
        {
            var count = _village.Buildings.Count;
            // Level 0(1), Elixir Storage.
            var building = new Building(_village, _manager.SearchCsv<BuildingData>(1000003, 0));
            Assert.AreEqual(count + 1, _village.Buildings.Count);
            Assert.AreSame(building, _village.GetBuilding(building.ID));
        }

        [Test]
        public void Constructors__newConstruction_True__Level_Neg1()
        {
            var building = new Building(_village, _manager.SearchCsv<BuildingData>(1000003, 0), true);
            Assert.AreEqual(-1, building._constructionLevel);
        }

        [Test]
        public void Constructors_newConstruction_False__Level_0()
        {
            var building = new Building(_village, _manager.SearchCsv<BuildingData>(1000003, 0), false);
            Assert.AreEqual(0, building._constructionLevel);
        }

        [Test]
        public void Constructors__Building_Is_A_TownHall__Village_TownHall_Is_Set_To_Building()
        {
            var village = new Village(_manager);
            Assert.Null(village.TownHall);
            Assert.AreEqual(0, village.Buildings.Count);

            // Level 0(1), Town Hall.
            var townhalldata = _manager.SearchCsv<BuildingData>(1000001, 0);
            var townhall = new Building(village, townhalldata);

            Assert.AreSame(townhall, village.TownHall);
            Assert.AreEqual(1, village.Buildings.Count);
        }

        [Test]
        public void Constructors__Building_Is_A_TownHall_And_Already_A_TownHall__Exception()
        {
            var village = new Village(_manager);
            Assert.Null(village.TownHall);
            Assert.AreEqual(0, village.Buildings.Count);

            // Level 0(1), Town Hall.
            var townhalldata = _manager.SearchCsv<BuildingData>(1000001, 0);
            var townhall1 = new Building(village, townhalldata);

            Assert.AreSame(townhall1, village.TownHall);
            Assert.AreEqual(1, village.Buildings.Count);

            // Village already contains a TownHall.
            Assert.Throws<InvalidOperationException>(() => new Building(village, townhalldata));
        }

        [Test]
        public void Constructors_InvalidArgs_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => new Building(null));
            Assert.Throws<ArgumentNullException>(() => new Building(_village, null));
        }

        [Test]
        public void Data_Null_Exception()
        {
            // Level 0(1), Elixir Storage.
            var building = new Building(_village, _manager.SearchCsv<BuildingData>(1000003, 0));

            Assert.Throws<ArgumentNullException>(() => building.Data = null);
        }

        [Test]
        public void CanUpgrade__Data_TownHallLevel_LowerThan_TownHall_Level__ReturnFalse()
        {
            // Level 2(3), Elixir Storage.
            var building = new Building(_village, _manager.SearchCsv<BuildingData>(1000003, 2));
            Assert.False(building.CanUpgrade);
        }

        [Test]
        public void CanUpgrade__Data_TownHallLevel_GreaterEqualThan_TownHall_Level__ReturnTrue()
        {
            // Level 1(2), Elixir Storage.
            // In village with Town Hall level 1(2).
            var building = new Building(_village, _manager.SearchCsv<BuildingData>(1000003, 1));
            Assert.True(building.CanUpgrade);
        }

        [Test]
        public void BeginConstruction__CanUpgrade_False__Exception()
        {
            var building = new Building(_village, _manager.SearchCsv<BuildingData>(1000003, 2));
            Assert.Throws<InvalidOperationException>(() => building.BeginConstruction());
        }

        [Test]
        public void BeginConstruction__SpeedUpConstruction__Data_Level_Increases()
        {
            // Level 1(2), Elixir Storage.
            var building = new Building(_village, _manager.SearchCsv<BuildingData>(1000003, 1));

            Assert.AreEqual(1, building._constructionLevel);
            building.BeginConstruction();
            Assert.True(building.IsConstructing);
            Assert.AreEqual(1, building.Data.Level);

            building.SpeedUpConstruction();
            Assert.False(building.IsConstructing);
            Assert.AreEqual(2, building.Data.Level);
        }

        [Test]
        public void BeginConstruction_BarbarianKing()
        {
            var building = new Building(_village, _manager.SearchCsv<BuildingData>(1000022, 0));
            //Assert.AreEqual(-1, building._constructionLevel);
            Assert.AreEqual(0, building._constructionLevel);

            //building.BeginConstruction();

            //Assert.AreEqual(0, building._constructionLevel);
        }

        [Test]
        public void BeginConstruction_Walls()
        {
            var building = new Building(_village, _manager.SearchCsv<BuildingData>(1000010, 0));
            //Assert.AreEqual(-1, building._constructionLevel);
            Assert.AreEqual(0, building._constructionLevel);

            //building.BeginConstruction();

            //Assert.AreEqual(0, building._constructionLevel);
        }
    }
}
