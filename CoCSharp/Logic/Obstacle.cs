using CoCSharp.Csv;
using CoCSharp.Data;
using Newtonsoft.Json;
using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans obstacle.
    /// </summary>
    public class Obstacle : VillageObject
    {
        internal const int BaseGameID = 503000000;

        /// <summary>
        /// Initializes a new instance of the <see cref="Obstacle"/> class.
        /// </summary>
        public Obstacle() : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Obstacle"/> class
        /// with the specified user token object.
        /// </summary>
        /// <param name="userToken">User token associated with this <see cref="Obstacle"/>.</param>
        public Obstacle(object userToken)
        {
            UserToken = userToken;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Obstacle"/> class with the specified
        /// X coordinate and Y coordinate.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Obstacle(int x, int y) : base(x, y)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Obstacle"/> class with the specified
        /// X coordinate, Y coordinate and user token object.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="userToken">User token associated with this <see cref="Obstacle"/>.</param>
        public Obstacle(int x, int y, object userToken) : base(x, y)
        {
            UserToken = userToken;
        }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of the <see cref="CsvData"/> expected
        /// by the <see cref="Obstacle"/>.
        /// </summary>
        /// <remarks>
        /// This is needed to make sure that the user provides a proper <see cref="CsvData"/> type
        /// for the <see cref="VillageObject"/>.
        /// </remarks>
        protected override Type ExpectedDataType
        {
            get
            {
                return typeof(ObstacleData);
            }
        }

        /// <summary>
        /// Gets or sets the user token associated with the <see cref="VillageObject"/>.
        /// </summary>
        /// <remarks>
        /// This object is referenced in the <see cref="ConstructionFinishEventArgs.UserToken"/>.
        /// </remarks>
        [JsonIgnore]
        public object UserToken { get; set; }

        /// <summary>
        /// Gets whether the <see cref="Obstacle"/> is being cleared.
        /// </summary>
        [JsonIgnore]
        public bool IsClearing
        {
            get { return ClearTSeconds > 0; }
        }

        /// <summary>
        /// Gets or sets the loot multiplier of the <see cref="Obstacle"/>.
        /// </summary>
        [JsonProperty("loot_multiply_ver", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int LootMultiplier { get; set; }

        /// <summary>
        /// Gets the duration of the clearing of the <see cref="Obstacle"/>.
        /// </summary>
        [JsonIgnore]
        public TimeSpan ClearDuration
        {
            get
            {
                if (!IsClearing)
                    throw new InvalidOperationException("Obstacle object is not clearing.");

                return TimeSpan.FromSeconds(ClearTSeconds);
            }
        }

        /// <summary>
        /// Gets or sets the UTC time at which the clearing of the <see cref="Obstacle"/> will end.
        /// </summary>
        [JsonIgnore]
        public DateTime ClearEndTime
        {
            get
            {
                if (!IsClearing)
                    throw new InvalidOperationException("Obstacle object is not clearing.");

                return DateTimeConverter.FromUnixTimestamp(ClearTEndUnixTimestamp);
            }
            set
            {
                if (value.Kind != DateTimeKind.Utc)
                    throw new ArgumentException("DateTime.Kind of value must be a DateTimeKind.Utc.", "value");

                //// Calculate the duration of the clearing operation.
                //ClearTSeconds = (int)(value - DateTime.UtcNow).TotalSeconds;

                // Convert the specified DateTime into UNIX timestamps.
                ClearTEndUnixTimestamp = (int)DateTimeConverter.ToUnixTimestamp(value);
            }
        }

        // Seconds remaining to clear to the obstacle.
        [JsonProperty("clear_t", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private int ClearTSeconds
        {
            get
            {
                // Json.Net will get this value then write it to a Json string or whatever.
                var duration = ClearTEndUnixTimestamp - DateTimeConverter.UnixUtcNow;

                // Make sure we don't get any negative durations.
                if (duration < 0)
                    return 0;

                // Should add 100 with the value?
                return duration;
            }
            set
            {
                // Json.Net will set this value by reading it from a Json string or whatever.
                ClearTEndUnixTimestamp = (DateTimeConverter.UnixUtcNow + value);
                _clearTime = value;
            }
        }

        // Duration of clearing, its pointless but just keeping it.
        private int _clearTime;

        // End time of the clearing of the obstacle in UNIX timestamps.
        [JsonIgnore]
        private int ClearTEndUnixTimestamp { get; set; }

        /// <summary>
        /// Begins the clearing of the <see cref="Obstacle"/> if <see cref="IsClearing"/> is <c>false</c>
        /// and <see cref="VillageObject.Data"/> is not null; otherwise 
        /// it throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        public void BeginClearing()
        {
            if (IsClearing)
                throw new InvalidOperationException("Obstacle object is already clearing.");
            if (Data == null)
                throw new InvalidOperationException("Obstacle.Data cannot be null.");

            var data = GetObstacleData();
            ClearEndTime = DateTime.UtcNow.Add(data.ClearTime);
            InternalScheduleClearing();
        }

        /// <summary>
        /// Cancel the clearing of the <see cref="Obstacle"/> if <see cref="IsClearing"/> is <c>true</c>; otherwise
        /// it throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IsClearing"/> is <c>false</c>.</exception>
        public void CancelClearing()
        {
            if (!IsClearing)
                throw new InvalidOperationException("Obstacle object is not being cleared.");

            var endTime = DateTime.UtcNow;

            InternalCancelScheduleBuild();

            ClearTEndUnixTimestamp = 0;
            OnClearingFinished(new ClearingFinishedEventArgs()
            {
                ClearedObstacle = this,
                EndTime = endTime,
                UserToken = UserToken,
                WasCancelled = true
            });
        }

        /// <summary>
        /// Returns the associated <see cref="CsvData"/> with the <see cref="Obstacle"/> as
        /// a <see cref="ObstacleData"/>.
        /// </summary>
        /// <returns>Associated <see cref="CsvData"/> with the <see cref="Obstacle"/> as a <see cref="ObstacleData"/>.</returns>
        /// <exception cref="InvalidOperationException"><see cref="VillageObject.Data"/> is null.</exception>
        public ObstacleData GetObstacleData()
        {
            if (Data == null)
                throw new InvalidOperationException("Obstacle.Data is null.");

            return (ObstacleData)Data;
        }

        /// <summary>
        /// The event raised when the clearing of the <see cref="Obstacle"/> is finished.
        /// </summary>
        public event EventHandler<ClearingFinishedEventArgs> ClearingFinished;
        /// <summary>
        /// Use this method to trigger the <see cref="ClearingFinished"/> event.
        /// </summary>
        /// <param name="e">The arguments data.</param>
        protected virtual void OnClearingFinished(ClearingFinishedEventArgs e)
        {
            if (ClearingFinished != null)
                ClearingFinished(this, e);
        }

        internal void InternalScheduleClearing()
        {
            LogicScheduler.ScheduleLogic(DoClearingFinished, ClearEndTime, userToken: this);
        }

        internal void InternalCancelScheduleBuild()
        {
            LogicScheduler.CancelSchedule(this);
        }

        private void DoClearingFinished()
        {
            var endTime = DateTime.UtcNow;
            var args = new ClearingFinishedEventArgs()
            {
                ClearedObstacle = this,
                EndTime = endTime,
                UserToken = UserToken
            };

            OnClearingFinished(args);
        }
    }
}
