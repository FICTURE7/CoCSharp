using Newtonsoft.Json;
using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans <see cref="VillageObject"/> that can be constructed.
    /// </summary>
    public abstract class Buildable : VillageObject
    {
        /// <summary>
        /// Level at which a building is not constructed. This field is readonly.
        /// </summary>
        public static readonly int NotConstructedLevel = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="Buildable"/> class.
        /// </summary>
        public Buildable() : base()
        {
            _level = NotConstructedLevel;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Buildable"/> class
        /// with the specified user token object.
        /// </summary>
        /// <param name="userToken">User token associated with this <see cref="Buildable"/>.</param>
        public Buildable(object userToken) : this()
        {
            UserToken = userToken;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Buildable"/> class with the specified
        /// X coordinate and Y coordinate.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Buildable(int x, int y) : base(x, y)
        {
            _level = NotConstructedLevel;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Buildable"/> class with the specified
        /// X coordinate, Y coordinate and user token object.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="userToken">User token associated with this <see cref="Buildable"/>.</param>
        public Buildable(int x, int y, object userToken) : this(x, y)
        {
            UserToken = userToken;
        }

        /// <summary>
        /// Gets or sets the user token associated with the <see cref="Buildable"/>.
        /// </summary>
        /// <remarks>
        /// This object is reference in the <see cref="ConstructionFinishEventArgs.UserToken"/>.
        /// </remarks>
        [JsonIgnore]
        public object UserToken { get; set; }

        /// <summary>
        /// Gets or sets the level of the <see cref="Buildable"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than -1.</exception>
        [JsonProperty("lvl")]
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                if (value < NotConstructedLevel)
                    throw new ArgumentOutOfRangeException("value", "value cannot be less than -1.");

                _level = value;
            }
        }

        // Level of the Buildable object.
        private int _level;

        /// <summary>
        /// Gets whether the <see cref="Buildable"/> object is in construction.
        /// </summary>
        [JsonIgnore]
        public bool IsConstructing
        {
            get
            {
                return ConstructionTSeconds > 0;
            }
        }

        /// <summary>
        /// Gets the duration of the construction of the <see cref="Buildable"/> object.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="Buildable"/> object is not in construction.</exception>
        [JsonIgnore]
        public TimeSpan ConstructionDuration
        {
            get
            {
                if (!IsConstructing)
                    throw new InvalidOperationException("Buildable object is not in construction.");

                return TimeSpan.FromSeconds(ConstructionTSeconds);
            }
        }

        /// <summary>
        /// Gets or sets the UTC time at which the construction of the <see cref="Buildable"/> object will end.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="Buildable"/> object is not in construction.</exception>
        [JsonIgnore]
        public DateTime ConstructionEndTime
        {
            get
            {
                if (!IsConstructing)
                    throw new InvalidOperationException("Buildable object is not in construction.");

                // Converts the UnixTimestamp value into a DateTime.
                return DateTimeConverter.FromUnixTimestamp(ConstructionTEndUnixTimestamp);
            }
            set
            {
                if (value.Kind != DateTimeKind.Utc)
                    throw new ArgumentException("DateTime.Kind of value must a DateTimeKind.Utc.", "value");

                // Converts the provided DateTime into a UnixTimestamp.
                ConstructionTEndUnixTimestamp = (int)DateTimeConverter.ToUnixTimestamp(value);
            }
        }

        // Duration of construction in seconds. Everything is handled from here.
        [JsonProperty("const_t", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private int ConstructionTSeconds
        {
            get
            {
                // Difference between construction end time and time now = duration.
                var constDuration = ConstructionTEndUnixTimestamp - DateTimeConverter.UnixUtcNow;

                if (constDuration < 0)
                {
                    // If construction duration is less than 0 then the construction is finished.
                    // Set ConstructionTimeEnd to 0 because the construction is finished.

                    ConstructionTEndUnixTimestamp = 0;
                    return 0;
                }

                return constDuration;
            }

            // ConstructionTime does not need a setter because it is relative to ConstructionTimeEnd.
            // Changing ConstructionTimeEnd would also change ConstructionTime.
        }

        // Date of when the construction is going to end in UNIX timestamps.
        [JsonProperty("const_t_end", DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal int ConstructionTEndUnixTimestamp { get; set; }

        /// <summary>
        /// Begins the construction of the <see cref="Buildable"/> and increases its level by 1
        /// when done.
        /// </summary>
        public abstract void BeginConstruction();

        /// <summary>
        /// Cancels the construction of the <see cref="Buildable"/>.
        /// </summary>
        public abstract void CancelConstruction();

        /// <summary>
        /// Speeds up the construction of the <see cref="Buildable"/> and increases its level by 1
        /// when done.
        /// </summary>
        public abstract void SpeedUpConstruction();

        /// <summary>
        /// The event raised when the <see cref="Building"/> construction is finished.
        /// </summary>
        public event EventHandler<ConstructionFinishEventArgs> ConstructionFinished;
        /// <summary>
        /// Use this method to trigger the <see cref="ConstructionFinished"/> event.
        /// </summary>
        /// <param name="e">The arguments data.</param>
        protected virtual void OnConstructionFinished(ConstructionFinishEventArgs e)
        {
            if (ConstructionFinished != null)
                ConstructionFinished(this, e);
        }
    }
}
