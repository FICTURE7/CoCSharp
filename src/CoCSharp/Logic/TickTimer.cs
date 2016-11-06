using System;
using System.Diagnostics;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a simple timer.
    /// </summary>
    [DebuggerDisplay("IsActive = {IsActive}")]
    public class TickTimer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TickTimer"/> class.
        /// </summary>
        public TickTimer()
        {
            // Space
        }

        // Tick at which the timer should end.
        private int _endTick;
        private bool _isActive;
        private int _startTime;
        private int _endTime;

        /// <summary>
        /// Gets a value indicating whether the <see cref="TickTimer"/> is active.
        /// </summary>
        public bool IsActive => _isActive;

        /// <summary>
        /// Gets the time when the <see cref="TickTimer"/> was started in the UNIX epoch time.
        /// </summary>
        public int StartTime => _startTime;

        /// <summary>
        /// Gets the time at which the <see cref="TickTimer"/> will complete in the UNIX epoch time.
        /// </summary>
        /// 
        /// <remarks>
        /// Represents the "const_t_end" field in village JSONs.
        /// </remarks>
        public int EndTime => _isActive ? _endTime : 0;

        /// <summary>
        /// Gets the remaining time until the <see cref="TickTimer"/> will complete in seconds.
        /// </summary>
        /// 
        /// <remarks>
        /// Represents the "const_t" field in village JSONs.
        /// </remarks>
        public int Duration
        {
            get
            {
                if (!_isActive)
                    return 0;

                var duration = EndTime - TimeUtils.UnixUtcNow;
                return duration < 0 ? 0 : duration;
            }
        }

        /// <summary>
        /// Gets the tick at which the timer should end.
        /// </summary>
        public int EndTick => _endTick;

        public void Start(DateTime startTime, int startingTick, int durationSeconds)
        {
            _startTime = (int)TimeUtils.ToUnixTimestamp(startTime);
            _endTime = _startTime + durationSeconds;

            _endTick = TimeUtils.ToTick(durationSeconds) + startingTick;
            _isActive = true;

            Debug.WriteLine($"Starting TickTimer at tick {startingTick} with end-tick {_endTick}.");
        }

        /// <summary>
        /// Stops the <see cref="TickTimer"/>.
        /// </summary>
        public void Stop()
        {
            _isActive = false;
        }

        /// <summary>
        /// Determines whether or not the <see cref="TickTimer"/> has completed using the specified
        /// tick.
        /// </summary>
        /// <param name="tick">Tick at which to check whether the <see cref="TickTimer"/> has completed.</param>
        /// <returns><c>true</c> if the <see cref="TickTimer"/> has completed; otherwise <c>false</c>.</returns>
        public bool IsCompleted(int tick)
        {
            return IsActive && tick >= _endTick;
        }
    }
}
