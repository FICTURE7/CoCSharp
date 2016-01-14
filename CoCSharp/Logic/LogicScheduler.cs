using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace CoCSharp.Logic
{
    internal static class LogicScheduler
    {
        static LogicScheduler()
        {
            s_objLock = new object();
            s_scheduleList = new List<Schedule>();
            s_stopwatch = new Stopwatch();

            t_worker = new Thread(Update);
            t_worker.Start();
        }

        private readonly static object s_objLock;
        private readonly static List<Schedule> s_scheduleList;
        private readonly static Stopwatch s_stopwatch;
        private readonly static Thread t_worker;

        public static void ScheduleLogic(Action logic, DateTime when)
        {
            lock(s_objLock)
            {
                var schedule = new Schedule(logic, when);
                s_scheduleList.Add(schedule);
            }
        }

        private static void Update()
        {
            while (true)
            {
                s_stopwatch.Restart();
                for (int i = 0; i < s_scheduleList.Count; i++)
                {
                    var schedule = s_scheduleList[i];
                    if (DateTime.UtcNow >= schedule.When)
                    {
                        schedule.Logic();
                        s_scheduleList.RemoveAt(i);
                    }
                }
                s_stopwatch.Stop();
                Thread.Sleep(100); // we dont wanna kill the cpu, do we?
            }
        }

        private class Schedule
        {
            public Schedule(Action logic, DateTime when)
            {
                Logic = logic;
                When = when;
            }

            public Action Logic { get; set; }
            public DateTime When { get; set; }
        }
    }
}
