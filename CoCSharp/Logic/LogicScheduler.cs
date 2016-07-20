using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace CoCSharp.Logic
{
    // Scheduler/Clock that fires an Action on the specified time. Used for handling of
    // construction time etc.
    internal static class LogicScheduler
    {
        //TODO: Consider pooling Schedule object to reduce GC pressure.

        static LogicScheduler()
        {
            s_lock = new object();
            s_scheduleList = new List<Schedule>();
            s_stopwatch = new Stopwatch();

            t_worker = new Thread(Update);
            t_worker.Start();
        }

        private readonly static object s_lock;
        private readonly static List<Schedule> s_scheduleList;
        private readonly static Stopwatch s_stopwatch;
        private readonly static Thread t_worker;

        // Schedules the specified Action at the specified DateTime.
        public static void ScheduleLogic(Action logic, DateTime utcWhen, string name = null, object userToken = null)
        {
            if (logic == null)
                throw new ArgumentNullException("logic");
            if (utcWhen.Kind != DateTimeKind.Utc)
                throw new ArgumentException("Kind of utcWhen must be Utc.", "utcWhen");

            var schedule = (Schedule)null;
            var index = 0;
            lock (s_lock)
            {
                // Sort the schedule list in order of DateTime, earliest to latest.
                // Iterate through the schedule list and find the index of the schedule.
                for (int i = 0; i < s_scheduleList.Count; i++)
                {
                    schedule = s_scheduleList[i];

                    // Update the schedule with the same UserToken.
                    if (schedule.UserToken == userToken)
                    {
                        schedule.When = utcWhen;
                        break;
                    }

                    // The schedule being schedule is called earlier than the older one.
                    if (schedule.When < utcWhen)
                        index = i + 1;
                }

                schedule = new Schedule(logic, utcWhen, name, userToken);
                s_scheduleList.Insert(index, schedule);
            }

            //Console.WriteLine("Scheduled logic at {0}", utcWhen);
        }

        // Prevents the t_worker from executing the scheduled logic.
        public static void CancelSchedule(object userToken)
        {
            if (userToken == null)
                throw new ArgumentNullException("userToken");

            lock (s_lock)
            {
                for (int i = 0; i < s_scheduleList.Count; i++)
                {
                    var schedule = s_scheduleList[i];

                    // Cancel schedule logic with the same UserToken reference.
                    if (schedule.UserToken == userToken)
                        schedule.Cancelled = true;
                }
            }
        }

        // Loops through the list of schedules and check if it must be executed.
        private static void Update()
        {
            while (true)
            {
                // Might not need a lock here, because this code
                // is run by only one thread that is t_worker.

                lock (s_lock)
                {
                    s_stopwatch.Restart();

                    for (int i = 0; i < s_scheduleList.Count; i++)
                    {
                        var schedule = s_scheduleList[i];

                        if (schedule.Cancelled)
                        {
                            s_scheduleList.RemoveAt(i);

                            // Check for the schedule before it.
                            i--;

                            // No need to check if it needs to be executed.
                            continue;
                        }

                        if (DateTime.UtcNow >= schedule.When)
                        {
                            schedule.Logic();
                            s_scheduleList.RemoveAt(i);

                            // Check for the schedule before it.
                            i--;
                        }
                        else
                        {
                            // Exit early because the schedule list was sorted.
                            break;
                        }
                    }

                    s_stopwatch.Stop();
                }
                Thread.Sleep(100);
            }
        }

        // Represents a schedule.
        private class Schedule
        {
            public Schedule(Action logic, DateTime when, string name = null, object userToken = null)
            {
                Logic = logic;
                When = when;
                Name = name;
                UserToken = userToken;
            }

            // Logic Action to call when the When DateTime was reached.
            public Action Logic { get; set; }

            // DateTime at which to call the Logic Action.
            public DateTime When { get; set; }

            // Name of the schedule.
            public string Name { get; set; }

            // State whether the logic has been cancelled.
            public bool Cancelled { get; set; }

            // Something to identify the schedule.
            public object UserToken { get; set; }

            public override string ToString()
            {
                return string.Format("Name: {0}, When: {1}", Name, When);
            }
        }
    }
}
