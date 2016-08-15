using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace CoCSharp.Logic
{
    // Class where all the ticking on Villages are done.
    // Aka the game loop.
    internal static class VillageTicker
    {
        static VillageTicker()
        {
            s_sync = new object();
            s_villages = new List<Village>();
            s_stopwatch = new Stopwatch();
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
                village.Logger.Info(village.Tick, "registering Village for ticking");

                village._registeredTime = TimeUtils.UnixUtcNow;
                s_villages.Add(village);

                // Start the worker thread if its not started yet.
                if (t_worker == null || !t_worker.IsAlive)
                {
                    t_worker = new Thread(DoTick);
                    t_worker.Name = "VillageTicker_Thread";
                    t_worker.Start();
                }
            }
        }

        // Unregisters the specified village for ticking.
        public static void Unregister(Village village)
        {
            var join = false;
            lock (s_sync)
            {
                village.Logger.Info(village.Tick, "unregistering Village for ticking");

                if (!s_villages.Remove(village))
                {
                    Debug.WriteLine("Tried to unregister an unregistered village.");
                }
                else
                {
                    // Wait for the t_worker thread to end if it has to villages left
                    // to tick. 
                    if (s_villages.Count == 0)
                        join = true;
                }
            }

            Debug.WriteLine("Waiting for t_worker to end.");
            t_worker.Join();
            t_worker = null;
        }

        private static void DoTick()
        {
            var count = 0;
            // Measure how much we are lagging.
            var lag = (double)0;

            do
            {
                s_stopwatch.Restart();
                count = TickAllRegisteredVillages();

                // Catch up with the lag.
                var tickCaughtUp = 0;
                while (lag >= TimeUtils.TickDuration)
                {
                    count = TickAllRegisteredVillages();
                    lag -= TimeUtils.TickDuration;
                    tickCaughtUp++;
                }

                Thread.Sleep((int)TimeUtils.TickDuration);
                s_stopwatch.Stop();

                lag += s_stopwatch.Elapsed.TotalMilliseconds - TimeUtils.TickDuration;
            }
            while (count > 0);
        }

        private static int TickAllRegisteredVillages()
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
                return count;
            }
        }
    }
}
