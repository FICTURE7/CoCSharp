using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Logic;
using NUnit.Framework;

namespace CoCSharp.Test.Logic
{
    [TestFixture]
    public class AvatarTests
    {
        private readonly AssetManager _manager;
        private Village _village;
        private Avatar _avatar;

        public AvatarTests()
        {
            _manager = new AssetManager(TestUtils.CsvDirectory);
            _manager.LoadCsv<BuildingData>("buildings.csv");
            _manager.LoadCsv<ObstacleData>("obstacles.csv");
            _manager.LoadCsv<TrapData>("traps.csv");
            _manager.LoadCsv<DecorationData>("decos.csv");
            _manager.LoadCsv<ResourceData>("resources.csv");
        }

        [SetUp]
        public void SetUp()
        {
            _village = new Village(_manager);
            // Level 1(2), Town Hall.
            new Building(_village, _manager.SearchCsv<BuildingData>(1000001, 2));

            _avatar = new Avatar();
            _avatar.Home = _village;
        }

        [Test]
        public void UpdateSlots()
        {
            // Level 1(2) Elixir Storage.
            new Building(_village, _manager.SearchCsv<BuildingData>(1000003, 1));
            _avatar.UpdateSlots();

            Assert.AreEqual(2, _avatar.ResourcesCapacity.Count);
            // Gold -> 10,000 (Level 2(3) Town Hall).
            // Elixir -> 13,000 (Level 2(3) Town Hall + Level 1(2) Elixir Storage).
            Assert.AreEqual(10000, _avatar.ResourcesCapacity.GetSlot(3000001).Capacity);
            Assert.AreEqual(13000, _avatar.ResourcesCapacity.GetSlot(3000002).Capacity);
        }
    }
}
