using System;
using System.Diagnostics;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a timer which can be updated in terms of game ticks.
    /// </summary>
    public class TickTimer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TickTimer"/> class.
        /// </summary>
        public TickTimer()
        {
            // Space
        }

        /// <summary>
        /// Gets the value indicating whether the <see cref="TickTimer"/> is active.
        /// </summary>
        public bool IsActive => _isActive;

        /// <summary>
        /// Gets a value indicating whether the <see cref="TickTimer"/> is completed.
        /// </summary>
        public bool IsComplete => !_isActive && _isComplete;

        /// <summary>
        /// Gets the time in the UNIX epoch time of when the <see cref="TickTimer"/> was started.
        /// </summary>
        public int StartTime => _isActive ? _startTime : 0;

        /// <summary>
        /// Gets the value of the game tick of when the <see cref="TickTimer"/> was started.
        /// </summary>
        public int StartTick => _isActive ? _startTick : 0;

        /// <summary>
        /// Gets the time in the UNIX epoch time of when the <see cref="TickTimer"/> will end.
        /// </summary>
        public int EndTime => _isActive ? _endTime : 0;

        /// <summary>
        /// Gets the value of the game tick of when the <see cref="TickTimer"/> will end.
        /// </summary>
        public int EndTick => _isActive ? _endTick : 0;

        /// <summary>
        /// Gets the remaining seconds until the <see cref="TickTimer"/> will end.
        /// </summary>
        /// 
        /// <remarks>
        /// The value of <see cref="Duration"/> will change according to when <see cref="Tick(int)"/> was
        /// called.
        /// </remarks>
        public double Duration => _isActive ? _duration : 0;

        private int _lastTick;
        private bool _isActive;
        private bool _isComplete;

        private int _startTime;
        private int _endTime;

        private int _startTick;
        private int _endTick;

        private double _oriDuration;
        private double _duration;

        /// <summary>
        /// Starts the <see cref="TickTimer"/> at the specified starting time, starting tick and duration in seconds.
        /// </summary>
        /// <param name="startTime"><see cref="DateTime"/> of when the <see cref="TickTimer"/> was started.</param>
        /// <param name="startTick">Game tick of when the <see cref="TickTimer"/> was started.</param>
        /// <param name="durationSeconds">Duration in seconds.</param>
        /// 
        /// <exception cref="InvalidOperationException"><see cref="IsActive"/> is <c>true</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startTick"/> is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="durationSeconds"/> is negative.</exception>
        public void Start(DateTime startTime, int startTick, int durationSeconds)
        {
            if (startTick < 0 || durationSeconds < 0)
                throw new ArgumentOutOfRangeException(nameof(startTick) + ", " + nameof(durationSeconds), "Starting tick and duration must be non-negative.");

            // Convert the duration which is in seconds into game ticks duration.
            var tick = TimeUtils.ToTick(durationSeconds);

            _startTime = (int)TimeUtils.ToUnixTimestamp(startTime);
            _endTime = _startTime + durationSeconds;

            _startTick = startTick;
            _endTick = startTick + tick;

            _oriDuration = durationSeconds;
            _duration = durationSeconds;

            _isComplete = false;
            _isActive = true;

            Debug.WriteLine($"Started TickTimer on tick {startTick} and is going to end on tick: {_endTick}.");
        }

        /// <summary>
        /// Stops the <see cref="TickTimer"/>.
        /// </summary>
        public void Stop()
        {
            _isActive = false;
            _isComplete = false;
        }

        /// <summary>
        /// Ticks the <see cref="TickTimer"/> at the specified game tick.
        /// </summary>
        /// 
        /// <remarks>
        /// If <see cref="IsActive"/> is <c>false</c> the <see cref="Tick(int)"/> method
        /// will exit early and not do any ticking logic.
        /// </remarks>
        /// 
        /// <param name="ctick">Game tick.</param>
        public void Tick(int ctick)
        {
            if (IsActive)
            {
                if (ctick < _lastTick)
                    Debug.WriteLine("TickTimer went back in time!! Forgot to rest or stop?");

                _lastTick = ctick;

                // Prevent some extra calculations.
                if (ctick >= _endTick)
                {
                    _duration = 0;
                    _isActive = false;
                    _isComplete = true;
                }
                else
                {
                    var diff = ctick - _startTick;
                    var diffSeconds = TimeUtils.FromTick(diff);
                    var newDuration = _oriDuration - diffSeconds;

                    // Clamp value to 0.
                    if (newDuration < 0)
                    {
                        newDuration = 0;
                        _isActive = false;
                        _isComplete = true;
                    }

                    _duration = newDuration;
                }
            }
        }

        public void Reset()
        {
            _isActive = default(bool);
            _isComplete = default(bool);
            _duration = default(double);
            _oriDuration = default(double);
            _startTime = default(int);
            _endTime = default(int);
            _startTick = default(int);
            _endTick = default(int);
            _lastTick = default(int);
        }
    }
}
