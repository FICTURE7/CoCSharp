using System.Diagnostics;

namespace CoCSharp.Logic
{
    [DebuggerDisplay("IsStarted = {IsStarted}")]
    internal class TickTimer
    {
        public TickTimer()
        {
            // Space
        }

        // Tick at which the timer should end.
        private int _tickEnd;

        // Value indicating if started or not.
        public bool IsStarted { get; private set; }
        // Time started.
        public int StartTime { get; private set; }
        // Time ending, "const_t_end" in village JSONs.
        public int EndTime { get; private set; }
        // Time remaining, "const_t" in village JSONs.
        public int Duration
        {
            get
            {
                var duration = EndTime - TimeUtils.UnixUtcNow;
                return duration < 0 ? 0 : duration;
            }
        }

        public void Start(int startingTick, int duration)
        {
            StartTime = TimeUtils.UnixUtcNow;
            EndTime = StartTime + duration;
            _tickEnd = startingTick + TimeUtils.ToTick(duration);
            IsStarted = true;
        }

        public void Start(int endTime)
        {
            StartTime = TimeUtils.UnixUtcNow;
            EndTime = endTime;
            _tickEnd = TimeUtils.ToTick(EndTime - StartTime);
            IsStarted = true;
        }

        public void Stop()
        {
            IsStarted = false;
        }

        public bool IsCompleted(int tick)
        {
            return (IsStarted && tick >= _tickEnd);
        }
    }
}
