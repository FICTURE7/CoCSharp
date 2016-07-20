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
            var townhalldata = _manager.SearchCsv<BuildingData>(1000001, 1);
            var townhall = new Building(_village, townhalldata);

            Assert.AreSame(townhall, _village.TownHall);
        }

        [Test]
        public void Constructors_InvalidArgs_Exceptions()
        {
            var data = _manager.SearchCsv<BuildingData>(1000000, 0);
            Assert.Throws<ArgumentNullException>(() => new Building(null, data));
            Assert.Throws<ArgumentNullException>(() => new Building(_village, null));
        }

        [Test]
        public void CanUpgrade_True()
        {
            // Gold Mine.
            var data1 = _manager.SearchCsv<BuildingData>(1000002, 0);
            var building1 = new Building(_village, data1);
            Assert.True(building1.CanUpgrade);

            // Army Camp.
            var data2 = _manager.SearchCsv<BuildingData>(1000000, 0);
            var building2 = new Building(_village, data2);
            Assert.True(building2.CanUpgrade);
        }

        [Test]
        public void CanUpgrade_False()
        {
            var collection = _manager.SearchCsv<BuildingData>(1000002);
            var data = collection[collection.Count - 1];
            var building = new Building(_village, data);

            Assert.False(building.CanUpgrade);
        }

        [Test]
        public void DoConstructionFinished_isConstructed()
        {
            var data = _manager.SearchCsv<BuildingData>(1000002, 0);
            var building = new Building(_village, data);
            Assert.False(building._isConstructed);

            building.DoConstructionFinished();
            Assert.AreEqual(0, building.Data.Level);

            Assert.True(building._isConstructed);

            building.DoConstructionFinished();
            Assert.AreEqual(1, building.Data.Level);

            Assert.True(building._isConstructed);
        }

        [Test]
        public void Data_Null_Exception()
        {
            var data = _manager.SearchCsv<BuildingData>(1000002, 0);
            var building = new Building(_village, data);

            Assert.Throws<ArgumentNullException>(() => building.Data = null);
        }

        [Test]
        public void FromJsonReader_isConstructed()
        {
            var villagePath = Path.Combine(TestUtils.LayoutDirectory, "test_village.json");
            var villageJson = File.ReadAllText(villagePath);

            var village = Village.FromJson(villageJson, _manager);

            var building0 = village.Buildings[0];
            Assert.False(building0._isConstructed);

            var building1 = village.Buildings[1];
            Assert.True(building1._isConstructed);
        }

        [Test]
        public void Constructor_ValidArgs_RegisteredInstanceID()
        {
            _village = new Village(_manager);
            // Add townhall to village.
            var townhalldata = _manager.SearchCsv<BuildingData>(1000001, 1);
            var townhall = new Building(_village, townhalldata);

            var data = _manager.SearchCsv<BuildingData>(1000000, 0);

            var building1 = new Building(_village, data);
            Assert.AreSame(data, building1.Data);
            var building2 = new Building(_village, data);
            Assert.AreSame(data, building2.Data);

            Assert.AreEqual(Building.BaseGameID + 1, building1.ID);
            Assert.AreEqual(Building.BaseGameID + 2, building2.ID);
        }

        private bool _raised = false;
        [Test]
        public void PropertyChanged_IsLocked_EventRaised()
        {
            _raised = false;
            var building = new Building(_village);
            building.IsPropertyChangedEnabled = true;
            building.PropertyChanged += PropertyChanged;

            building.IsLocked = true;
            Assert.True(_raised);

            _raised = false;
            building.IsPropertyChangedEnabled = false;
            building.IsLocked = false;
            Assert.False(_raised);
        }

        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _raised = true;
            Assert.AreEqual("IsLocked", e.PropertyName);
        }
    }
}
