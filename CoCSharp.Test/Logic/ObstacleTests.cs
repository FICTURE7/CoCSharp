using CoCSharp.Logic;
using NUnit.Framework;
using System;

namespace CoCSharp.Test.Logic
{
    [TestFixture]
    public class ObstacleTests
    {
        private Obstacle _obstacle;

        public ObstacleTests()
        {
            _obstacle = new Obstacle();
        }

        [Test]
        public void ClearEndTime_IsNotClearing_Exception()
        {
            // Should throw an and InvalidOperationException when the obstacle is not being cleared.
            Assert.Throws<InvalidOperationException>(() =>
            {
                var test = _obstacle.ClearEndTime;
            });
        }

        [Test]
        public void ClearDuration_IsNotClearing_Exception()
        {
            // Should throw an and InvalidOperationException when the obstacle is not in cleared.
            Assert.Throws<InvalidOperationException>(() =>
            {
                var test = _obstacle.ClearDuration;
            });
        }

        [Test]
        public void ClearEndTime_InvalidDateTimeKind_Exception()
        {
            // Should have been a DateTime.UtcNow.AddSeconds(10); instead.
            Assert.Throws<ArgumentException>(() => _obstacle.ClearEndTime = DateTime.Now.AddSeconds(10));
        }

        [Test]
        public void ClearEndTime_RandomTests()
        {
            _obstacle.ClearEndTime = DateTime.UtcNow.AddDays(1);
        }
    }
}
