using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace CoCSharp.Logic
{
    // Class where all the ticking on Villages are done.
    internal static class VillageTicker
    {
        static VillageTicker()
        {
            s_sync = new object();
            s_villages = new List<Village>();
            s_stopwatch = new Stopwatch();
            t_worker = new Thread(DoTick);
        }

        private static object s_sync;
        // List of village which must be ticked.
        private static List<Village> s_villages;
        // Stopwatch to measure lag.
        private static Stopwatch s_stopwatch;
        // Thread which does all the ticking.
        private static Thread t_worker;

        // Registers a village for ticking.
        public static void Register(Village village)
        {
            lock (s_sync)
            {
                s_villages.Add(village);

                // Start the worker thread if its not started yet.
                if (!t_worker.IsAlive)
                    t_worker.Start();
            }
        }

        // Unregisters the specified village for ticking.
        public static void Unregister(Village village)
        {
            lock (s_sync)
            {
                if (!s_villages.Remove(village))
                    Debug.WriteLine("Tried to unregister an unregistered village.");
            }
        }

        // Tick duration in milliseconds.
        private static readonly double TickDuration = 16;
        private static void DoTick()
        {
            var count = 0;
            var lag = (double)0;

            do
            {
                s_stopwatch.Restart();
                count = s_villages.Count;

                TickRegisteredVillages();

                var tickCaughtUp = 0;
                while (lag >= TickDuration)
                {
                    TickRegisteredVillages();
                    lag -= TickDuration;
                    tickCaughtUp++;
                }

                if (tickCaughtUp > 0)
                    Debug.WriteLine("Tick caught up: {0}", args: tickCaughtUp);

                Thread.Sleep((int)TickDuration);
                s_stopwatch.Stop();

                lag += s_stopwatch.Elapsed.TotalMilliseconds - TickDuration;
            }
            while (count > 0);
        }

        private static void TickRegisteredVillages()
        {
            lock (s_sync)
            {
                var count = s_villages.Count;
                for (int i = 0; i < count; i++)
                {
                    var village = s_villages[i];
                    village.Update();
                    village._tick++;
                }
            }
        }
    }
}
