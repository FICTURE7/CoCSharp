using CoCSharp.Logic;
using NUnit.Framework;
using System;

namespace CoCSharp.Test.Logic
{
    [TestFixture]
    public class TickTimerTests
    {
        [SetUp]
        public void SetUp()
        {
            _timer = new TickTimer();
        }

        public TickTimer _timer;

        [Test]
        public void Start_NegativeArgs_Exception()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _timer.Start(DateTime.Now, -1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => _timer.Start(DateTime.Now, 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _timer.Start(DateTime.Now, -1, -1));
        }

        [Test]
        public void Tick_()
        {
            _timer.Start(DateTime.UtcNow, 0, 10);

            var k = TimeUtils.ToTick(10);
            _timer.Tick(k);
            Assert.False(_timer.IsActive);
            Assert.True(_timer.IsComplete);
            Assert.AreEqual(0.0, _timer.Duration);
        }
    }
}
