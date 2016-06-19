using CoCSharp.Data.Model;
using CoCSharp.Logic;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace CoCSharp.Test.Logic
{
    [TestFixture]
    public class VillageObjectTests
    {
        [Test]
        // Test the X and Y property of a village object.
        public void XY_PositionOutOfVillageBounds_Exception()
        {
            var village = new Village();
            var obj = new Building(village);

            // X Coordinate tests.
            Assert.Throws<ArgumentOutOfRangeException>(() => obj.X = Village.Width + 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => obj.X = -1);

            // Y Coordinate tests.
            Assert.Throws<ArgumentOutOfRangeException>(() => obj.Y = Village.Height + 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => obj.Y = -1);
        }

        [Test]
        public void FromJsonToJson_DeserializeAndSerializeJson_DeserializedAndSerializedJsonShouldBeEqual()
        {
            var json = File.ReadAllText(Path.Combine(TestUtils.ContentDirectory, "starting_home.json"));
            // Converts starting_home.json into a Village object.
            var fromJson = Village.FromJson(json);

            // Look for alliance castle.
            var castle1 = fromJson.Buildings.FirstOrDefault(b => b.IsLocked == true);
            Assert.NotNull(castle1, "Did not find alliance castle in starting_home.json");

            //NOTE: The hardcoded values are from starting_home.json.

            // Already checked with LINQ above but just for consistency.
            Assert.That(castle1.IsLocked == true); 

            Assert.That(castle1.DataID == 1000014);
            Assert.That(castle1.Level == 0);
            Assert.That(castle1.X == 28);
            Assert.That(castle1.Y == 35);

            // Converts the Village object into a JSON string.
            var toJson = fromJson.ToJson();
            // Converts that JSON string back into a Village object.
            // Just to make sure it serialize JSON properly.
            var serializedFromJson = Village.FromJson(toJson);

            // Make sure they are 2 different instances.
            Assert.AreNotSame(serializedFromJson, fromJson);

            Assert.AreEqual(serializedFromJson.Buildings.Count, fromJson.Buildings.Count);
            Assert.AreEqual(serializedFromJson.Traps.Count, fromJson.Traps.Count);
            Assert.AreEqual(serializedFromJson.Obstacles.Count, fromJson.Obstacles.Count);
            Assert.AreEqual(serializedFromJson.Decorations.Count, fromJson.Decorations.Count);

            // Look for alliance castle.
            var castle2 = serializedFromJson.Buildings.FirstOrDefault(b => b.IsLocked == true);
            Assert.NotNull(castle1, "Did not find alliance castle in serialized starting_home.json");

            // Already checked with LINQ above but just for consistency.
            Assert.That(castle2.IsLocked == true);

            Assert.That(castle2.DataID == 1000014);
            Assert.That(castle2.Level == 0);
            Assert.That(castle2.X == 28);
            Assert.That(castle2.Y == 35);
        }

        [Test]
        public void Data_UnexpectedType_Exception()
        {
            var village = new Village();
            var testObj = new Building(village);

            Assert.Throws<ArgumentException>(() => testObj.Data = new ResourceData());
            Assert.That(testObj.DataID == 0);

            testObj.Data = new BuildingData()
            {
                Index = 1
            };

            Assert.That(testObj.DataID != 0);

            testObj.Data = null;
            Assert.That(testObj.DataID == 0);
        }
    }
}
