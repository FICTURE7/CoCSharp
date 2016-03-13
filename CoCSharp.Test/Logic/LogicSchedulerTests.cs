using CoCSharp.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CoCSharp.Test.Logic
{
    [TestFixture]
    public class LogicSchedulerTests
    {
        // Needed for testing of whens.
        private List<ScheduleTestInfo> _info;

        // Schedule index.
        private int _index;

        [SetUp]
        public void SetUp()
        {
            _info = new List<ScheduleTestInfo>();
            _index = 0;
        }

        [Test, Ignore("This test is slow and inconsistent.")]
        public void TestLogicSchedulerScheduleLogic()
        {
            // This is a slow test as it
            // schedules 10 logic actions at 1 second intervals.

            for (int i = 0; i < 10; i++)
            {
                var time = DateTime.UtcNow;
                var when = time.AddSeconds(i + 1);

                var info = new ScheduleTestInfo();
                info.ScheduledTime = time;
                info.ExpectedWhen = when;

                _info.Add(info);
                LogicScheduler.ScheduleLogic(Logic, when, "test" + i);

                Console.WriteLine("Logic scheduled at {0}", when);
            }

            // Find out how much time we need to sleep plus 1 second.
            // This is to make sure all the schedule logic was called.
            var timeout = (_info[_info.Count - 1].ExpectedWhen - DateTime.UtcNow).Add(TimeSpan.FromSeconds(1));
            Thread.Sleep(timeout);

            // Check that the logic was called at the expected when time.
            for (int i = 0; i < _info.Count; i++)
            {
                var info = _info[i];
                Assert.That(info.When, Is.EqualTo(info.ExpectedWhen).Within(TimeSpan.FromMilliseconds(110)));
            }
        }

        [Test]
        public void TestLogicSchedulerOverlappedScheduleLogic()
        {
            var time = DateTime.UtcNow;
            var when = time.AddSeconds(1);

            var info1 = new ScheduleTestInfo();
            info1.ScheduledTime = time;
            info1.ExpectedWhen = when;
            _info.Add(info1);
            Console.WriteLine("Logic scheduled at {0}", when);

            var info2 = new ScheduleTestInfo();
            info2.ScheduledTime = time;
            info2.ExpectedWhen = when;
            _info.Add(info2);
            Console.WriteLine("Logic scheduled at {0}", when);

            LogicScheduler.ScheduleLogic(Logic, when, "overlapped_test1");
            LogicScheduler.ScheduleLogic(Logic, when, "overlapped_test2");

            // Wait for 2 seconds.
            Thread.Sleep(TimeSpan.FromSeconds(2));

            for (int i = 0; i < _info.Count; i++)
            {
                var info = _info[i];
                Assert.That(info.When, Is.EqualTo(info.ExpectedWhen).Within(TimeSpan.FromMilliseconds(110)));
            }
        }

        [Test]
        public void TestLogicSchedulerCancelLogic()
        {
            var time = DateTime.UtcNow;
            var when = time.AddSeconds(1);

            var info1 = new ScheduleTestInfo();
            info1.ScheduledTime = time;
            info1.ExpectedWhen = when;
            _info.Add(info1);
            Console.WriteLine("Logic scheduled at {0}", when);

            var usertoken = new object();
            LogicScheduler.ScheduleLogic(Logic, when, "cancel_logic", usertoken);

            // Cancels the Schedule associated with the specified usertoken.
            LogicScheduler.CancelSchedule(usertoken);

            // Waits for 2 seconds just to be sure.
            Thread.Sleep(TimeSpan.FromSeconds(2));

            // Check if it was never called.
            for (int i = 0; i < _info.Count; i++)
            {
                var info = _info[i];
                Assert.That(info.When == default(DateTime));
            }
        }

        private void Logic()
        {
            // When the logic was actually called.
            var time = DateTime.UtcNow;

            _info[_index].When = time;
            _index++;

            Console.WriteLine("Logic called at {0}", time);
        }

        // NUnit only check assertions on test threads.
        private class ScheduleTestInfo
        {
            // When it actually was called.
            public DateTime When;

            // When it was supposed to call.
            public DateTime ExpectedWhen;

            // When it was scheduled.
            public DateTime ScheduledTime;
        }
    }
}
