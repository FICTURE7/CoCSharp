using CoCSharp.Logic;
using NUnit.Framework;
using System;

namespace CoCSharp.Test.Logic
{
    [TestFixture]
    public class ObstacleTests
    {
        private Village _village;
        private Obstacle _obstacle;

        public ObstacleTests()
        {
            _village = new Village();
            _obstacle = new Obstacle(_village);
        }

        [Test]
        public void Was_AddedToVillage()
        {
            Assert.That(_village.Obstacles.Contains(_obstacle));
        }

        [Test]
        public void ClearEndTime_IsNotClearing_Exception()
        {
            // Should throw an InvalidOperationException when the obstacle is not being cleared.
            Assert.Throws<InvalidOperationException>(() =>
            {
                var test = _obstacle.ClearEndTime;
            });
        }

        [Test]
        public void ClearDuration_IsNotClearing_Exception()
        {
            // Should throw an InvalidOperationException when the obstacle is not being cleared
            Assert.Throws<InvalidOperationException>(() =>
            {
                var test = _obstacle.ClearDuration;
            });
        }

        [Test]
        public void ClearEndTime_InvalidDateTimeKind_Exception()
        {
            // Should throw an InvalidOperationException when DateTimeKind is not of DateTimeKind.Utc.
            Assert.Throws<ArgumentException>(() => _obstacle.ClearEndTime = DateTime.Now.AddSeconds(10));
        }

        [Test]
        public void ClearEndTime_RandomTests()
        {
            _obstacle.ClearEndTime = DateTime.UtcNow.AddDays(1);
        }
    }
}
