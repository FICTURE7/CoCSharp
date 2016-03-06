using System;
using CoCSharp.Logic;
using NUnit.Framework;
using System.IO;
using System.Linq;
using CoCSharp.Data;

namespace CoCSharp.Test.Logic
{
    [TestFixture]
    public class VillageObjectTests
    {
        [Test]
        public void TestVillageObjectCoordinates()
        {
            // Check if checks for valid coordinates.
            var obj = new TestObject();

            // X Coordinate tests.
            Assert.Throws<ArgumentOutOfRangeException>(() => obj.X = 45);
            Assert.Throws<ArgumentOutOfRangeException>(() => obj.X = -1);

            // Y Coordinate tests.
            Assert.Throws<ArgumentOutOfRangeException>(() => obj.Y = 45);
            Assert.Throws<ArgumentOutOfRangeException>(() => obj.Y = -1);
        }

        [Test]
        public void TestVillageObjectJsonSerialization()
        {
            var json = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content/starting_home.json"));
            // Converts starting_home.json into a Village object.
            var fromJson = Village.FromJson(json);

            // Look for alliance castle.
            var castle1 = fromJson.Buildings.FirstOrDefault(b => b.IsLocked == true);
            Assert.NotNull(castle1, "Did not find alliance castle in starting_home.json");

            //NOTE: The hardcoded values are from starting_home.json.

            // Already checked with linq above but just for consistency.
            Assert.That(castle1.IsLocked == true); 

            Assert.That(castle1.DataID == 1000014);
            Assert.That(castle1.Level == 0);
            Assert.That(castle1.X == 28);
            Assert.That(castle1.Y == 35);

            // Converts the Village object into a json string.
            var toJson = fromJson.ToJson();
            // Converts that json string backinto a Village object.
            // Just to make sure it serialize json properly.
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

            // Already checked with linq above but just for consistency.
            Assert.That(castle2.IsLocked == true);

            Assert.That(castle2.DataID == 1000014);
            Assert.That(castle2.Level == 0);
            Assert.That(castle2.X == 28);
            Assert.That(castle2.Y == 35);
        }

        [Test]
        public void TestVillageObjectData()
        {
            var testObj = new TestObject();
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

        private class TestObject : VillageObject
        {
            protected override Type ExpectedDataType
            {
                get
                {
                    return typeof(BuildingData);
                }
            }
        }
    }
}
