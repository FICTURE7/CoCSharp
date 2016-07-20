using CoCSharp.Csv;
using CoCSharp.Data.Models;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans obstacle.
    /// </summary>
    public class Obstacle : VillageObject<ObstacleData>
    {
        #region Constants
        internal const int BaseGameID = 503000000;

        private static readonly PropertyChangedEventArgs s_lootMultiplierChanged = new PropertyChangedEventArgs("LootMultiplier");

        /// <summary/>
        protected internal static readonly TimeSpan InstantClearTime = new TimeSpan(0);
        #endregion

        #region Constructors
        // Constructor that FromJsonReader method is going to use.
        internal Obstacle(Village village) : base(village)
        {
            // Space
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Obstacle"/> class with the specified <see cref="Village"/> containing
        /// the <see cref="Obstacle"/> and <see cref="ObstacleData"/> which is associated with it.
        /// </summary>
        /// 
        /// <param name="village"><see cref="Village"/> containing the <see cref="Obstacle"/>.</param>
        /// <param name="data"><see cref="ObstacleData"/> which is associated with this <see cref="Obstacle"/>.</param>
        public Obstacle(Village village, ObstacleData data) : base(village, data)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Obstacle"/> class with the specified <see cref="Village"/> containing
        /// the <see cref="Obstacle"/> and <see cref="ObstacleData"/> which is associated with it and user token object.
        /// </summary>
        /// 
        /// <param name="village"><see cref="Village"/> which contains the <see cref="Obstacle"/>.</param>
        /// <param name="data"><see cref="ObstacleData"/> which is associated with this <see cref="Obstacle"/>.</param>
        /// <param name="userToken">User token associated with this <see cref="Obstacle"/>.</param>
        public Obstacle(Village village, ObstacleData data, object userToken) : base(village, data)
        {
            UserToken = userToken;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Obstacle"/> class with the specified <see cref="Village"/> containing the <see cref="Obstacle"/>
        /// and <see cref="ObstacleData"/> which is associated with it, X coordinate and Y coordinate.
        /// </summary>
        /// 
        /// <param name="village"><see cref="Village"/> which contains the <see cref="Obstacle"/>.</param>
        /// <param name="data"><see cref="ObstacleData"/> which is associated with this <see cref="Obstacle"/>.</param>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Obstacle(Village village, ObstacleData data, int x, int y) : base(village, data, x, y)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Obstacle"/> class with the specified <see cref="Village"/> containing the <see cref="Obstacle"/>
        /// and <see cref="ObstacleData"/> which is associated with it, X coordinate, Y coordinate and user token object.
        /// </summary>
        /// 
        /// <param name="village"><see cref="Village"/> which contains the <see cref="Obstacle"/>.</param>
        /// <param name="data"><see cref="ObstacleData"/> which is associated with this <see cref="Obstacle"/>.</param>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="userToken">User token associated with this <see cref="Obstacle"/>.</param>
        public Obstacle(Village village, ObstacleData data, int x, int y, object userToken) : base(village, data, x, y)
        {
            UserToken = userToken;
        }
        #endregion

        #region Fields & Properties
        /// <summary>
        /// The event raised when the clearing of the <see cref="Obstacle"/> is finished.
        /// </summary>
        public event EventHandler<ClearingFinishedEventArgs> ClearingFinished;

        /// <summary>
        /// Gets or sets the user token associated with the <see cref="VillageObject"/>.
        /// </summary>
        /// <remarks>
        /// This object is referenced in the <see cref="ConstructionFinishedEventArgs{ObstacleData}.UserToken"/>.
        /// </remarks>
        public object UserToken { get; set; }

        /// <summary>
        /// Gets whether the <see cref="Obstacle"/> is being cleared.
        /// </summary>
        public bool IsClearing
        {
            get
            {
                return ClearTSeconds > 0;
            }
        }

        private int _lootMultiplier;
        /// <summary>
        /// Gets or sets the loot multiplier of the <see cref="Obstacle"/>.
        /// </summary>
        public int LootMultiplier
        {
            get
            {
                return _lootMultiplier;
            }
            set
            {
                if (_lootMultiplier == value)
                    return;

                OnPropertyChanged(s_lootMultiplierChanged);
                _lootMultiplier = value;
            }
        }

        /// <summary>
        /// Gets the duration of the clearing of the <see cref="Obstacle"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IsClearing"/> is <c>false</c>.</exception>
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
        /// <exception cref="InvalidOperationException"><see cref="IsClearing"/> is <c>false</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/>'s kind is not <see cref="DateTimeKind.Utc"/>.</exception>
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

                // Convert the specified DateTime into UNIX timestamps.
                ClearTEndUnixTimestamp = (int)DateTimeConverter.ToUnixTimestamp(value);
            }
        }

        // Seconds remaining to clear to the obstacle.
        private int ClearTSeconds
        {
            get
            {
                // Json.Net will get this value then write it to a Json string or whatever.
                var clearDuration = ClearTEndUnixTimestamp - DateTimeConverter.UnixUtcNow;

                // Make sure we don't get any negative durations.
                if (clearDuration < 0)
                {
                    ClearTEndUnixTimestamp = 0;
                    return 0;
                }

                // NOTE: Should add 100 with the value?
                return clearDuration;
            }
            set
            {
                ClearTEndUnixTimestamp = (DateTimeConverter.UnixUtcNow + value);
            }
        }

        // End time of the clearing of the obstacle in UNIX timestamps.
        private int ClearTEndUnixTimestamp { get; set; }

        // Determines whether the Obstacle is scheduled.
        private bool _scheduled;
        #endregion

        #region Methods
        #region Clearing
        /// <summary>
        /// Begins the clearing of the <see cref="Obstacle"/> if <see cref="IsClearing"/> is <c>false</c>
        /// and <see cref="VillageObject{ObstacleData}.Data"/> is not null; otherwise 
        /// it throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IsClearing"/> is <c>true</c>.</exception>
        /// <exception cref="InvalidOperationException"><see cref="VillageObject{ObstacleData}.Data"/> is <c>null</c>.</exception>
        public void BeginClearing()
        {
            if (IsClearing)
                throw new InvalidOperationException("Obstacle object is already clearing.");

            Debug.Assert(Data != null);
            if (Data.ClearTime == InstantClearTime)
            {
                DoClearingFinished();
                return;
            }

            ClearEndTime = DateTime.UtcNow.Add(Data.ClearTime);
            ScheduleClearing();
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

            CancelScheduleClearing();

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
        /// Use this method to trigger the <see cref="ClearingFinished"/> event.
        /// </summary>
        /// <param name="e">The arguments data.</param>
        protected virtual void OnClearingFinished(ClearingFinishedEventArgs e)
        {
            if (ClearingFinished != null)
                ClearingFinished(this, e);
        }

        internal void ScheduleClearing()
        {
            Debug.Assert(!_scheduled);

            LogicScheduler.ScheduleLogic(DoClearingFinished, ClearEndTime, userToken: this);
            _scheduled = true;
        }

        internal void CancelScheduleClearing()
        {
            Debug.Assert(_scheduled);

            LogicScheduler.CancelSchedule(this);
            _scheduled = false;
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

            var index = ID - BaseGameID;
            Village.Obstacles.RemoveAt(index);
            PushToPool();

            OnClearingFinished(args);

            _scheduled = false;
        }
        #endregion

        internal override void ResetVillageObject()
        {
            base.ResetVillageObject();
            _lootMultiplier = default(int);
            ClearTEndUnixTimestamp = default(int);
        }

        internal override void RegisterVillageObject()
        {
            ID = BaseGameID + Village.Obstacles.Count;
            Village.Obstacles.Add(this);
        }

        #region Json Reading/Writing
        internal override void ToJsonWriter(JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("data");
            writer.WriteValue(Data.ID);

            writer.WritePropertyName("id");
            writer.WriteValue(ID);

            if (ClearTSeconds != default(int))
            {
                writer.WritePropertyName("clear_t");
                writer.WriteValue(ClearTSeconds);
            }

            writer.WritePropertyName("x");
            writer.WriteValue(X);

            writer.WritePropertyName("y");
            writer.WriteValue(Y);

            if (LootMultiplier != default(int))
            {
                writer.WritePropertyName("loot_multiply_ver");
                writer.WriteValue(LootMultiplier);
            }

            writer.WriteEndObject();
        }

        internal override void FromJsonReader(JsonReader reader)
        {
            var instance = CsvData.GetInstance<ObstacleData>();
            // clear_t value.
            var clearTime = -1;

            var dataId = -1;
            var dataIdSet = false;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                    break;

                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var propertyName = (string)reader.Value;
                    switch (propertyName)
                    {
                        case "data":
                            dataId = reader.ReadAsInt32().Value;
                            dataIdSet = true;
                            break;

                        case "clear_t":
                            clearTime = reader.ReadAsInt32().Value;
                            break;

                        case "x":
                            X = reader.ReadAsInt32().Value;
                            break;

                        case "y":
                            Y = reader.ReadAsInt32().Value;
                            break;

                        case "loot_multiply_ver":
                            LootMultiplier = reader.ReadAsInt32().Value;
                            break;
                    }
                }
            }

            if (!dataIdSet)
                throw new InvalidOperationException("Obstacle JSON does not contain 'data' field.");

            if (instance.InvalidDataID(dataId))
                throw new InvalidOperationException("Obstacle JSON contained an invalid data ID. " + instance.GetArgsOutOfRangeMessage("Data ID"));

            // No need to cache the sub-collection the ObstacleData is in, because we can't upgrade Obstacles.
            var data = AssetManager.SearchCsvNoCheck<ObstacleData>(dataId, 0);
            if (data == null)
                throw new InvalidOperationException("Could not find ObstacleData with ID '" + dataId + "'.");

            _data = data;

            if (clearTime == -1)
                return;

            ClearTSeconds = clearTime;
            ScheduleClearing();
        }
        #endregion

        internal static Obstacle GetInstance(Village village)
        {
            var obj = (VillageObject)null;
            if (VillageObjectPool.TryPop(BaseGameID, out obj))
            {
                obj.Village = village;
                return (Obstacle)obj;
            }

            return new Obstacle(village);
        }
        #endregion
    }
}
