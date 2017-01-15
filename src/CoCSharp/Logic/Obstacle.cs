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
        internal const int Kind = 3;
        internal const int BaseGameID = 503000000;

        private static readonly PropertyChangedEventArgs s_lootMultiplierChanged = new PropertyChangedEventArgs(nameof(LootMultiplier));

        /// <summary>
        /// <see cref="TimeSpan"/> representing an instant clear time.
        /// </summary>
        protected internal static readonly TimeSpan InstantClearTime = new TimeSpan(0);
        #endregion

        #region Constructors
        // Constructor used to load the VillageObject from a JsonTextReader.
        internal Obstacle() : base()
        {
            _timer = new TickTimer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Obstacle"/> class with the specified
        /// <see cref="Village"/> instance and <see cref="ObstacleData"/>.
        /// </summary>
        /// 
        /// <param name="village">
        /// <see cref="Village"/> instance which owns this <see cref="Obstacle"/>.
        /// </param>
        /// 
        /// <param name="data"><see cref="ObstacleData"/> representing the data of the <see cref="Obstacle"/>.</param>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="village"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        public Obstacle(Village village, ObstacleData data) : base(village, data)
        {
            _timer = new TickTimer();
        }
        #endregion

        #region Fields & Properties
        private TickTimer _timer;
        private int _lootMultiplier;

        /// <summary>
        /// Gets whether the <see cref="Obstacle"/> is being cleared.
        /// </summary>
        public bool IsClearing => _timer.IsActive;

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

                _lootMultiplier = value;
                OnPropertyChanged(s_lootMultiplierChanged);
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

                return TimeSpan.FromSeconds(_timer.Duration);
            }
        }

        /// <summary>
        /// Gets or sets the UTC time at which the clearing of the <see cref="Obstacle"/> will end.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IsClearing"/> is <c>false</c>.</exception>
        public DateTime ClearEndTime
        {
            get
            {
                if (!IsClearing)
                    throw new InvalidOperationException("Obstacle object is not clearing.");

                return TimeUtils.FromUnixTimestamp(_timer.EndTime);
            }
        }

        // Seconds remaining to clear to the obstacle.
        private int ClearTSeconds => (int)_timer.Duration;

        internal override int KindId => 3;
        #endregion

        #region Methods
        /// <summary>
        /// Begins the clearing of the <see cref="Obstacle"/> if <see cref="IsClearing"/> is <c>false</c>
        /// and <see cref="VillageObject{ObstacleData}.Data"/> is not null; otherwise 
        /// it throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IsClearing"/> is <c>true</c>.</exception>
        /// <exception cref="InvalidOperationException"><see cref="VillageObject{ObstacleData}.Data"/> is <c>null</c>.</exception>
        public void BeginClearing(int ctick)
        {
            if (IsClearing)
                throw new InvalidOperationException("Obstacle object is already clearing.");

            Debug.Assert(_data != null, "Obstacle Data was null.");
            var clearTime = _data.ClearTime;
            var seconds = (int)clearTime.TotalSeconds;

            Village.WorkerManager.AllocateWorker(this);
            _timer.Start(Village.LastTickTime, ctick, seconds);
        }

        /// <summary>
        /// Cancel the clearing of the <see cref="Obstacle"/> if <see cref="IsClearing"/> is <c>true</c>; otherwise
        /// it throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IsClearing"/> is <c>false</c>.</exception>
        public void CancelClearing(int ctick)
        {
            if (!IsClearing)
                throw new InvalidOperationException("Obstacle object is not being cleared.");

            Village.WorkerManager.DeallotateWorker(this);
            _timer.Stop();
        }

        // Gem drop sequence when the respawnVars in the home JSON are not set.
        private static readonly int[] s_gemDrops =
        {
            3, 0, 1, 2, 0, 1, 1, 0, 0, 3,
            1, 0, 2, 2, 0, 0, 3, 0, 1, 0
        };
        internal void FinishClear(int ctick)
        {
            Debug.WriteLine($"FinishClear: Construction for {Id} finished on tick {ctick} expected {_timer.EndTick}...");

            _timer.Stop();
            Village.WorkerManager.DeallotateWorker(this);

            var duration = _data.ClearTime;
            var player = Village.Level;

            var gems = s_gemDrops[Village._obstacleClearCount++];
            if (Village._obstacleClearCount >= s_gemDrops.Length)
                Village._obstacleClearCount = 0;

            var expPointsGained = LogicUtils.CalculateExpPoints(duration);
            var expPoints = player.Avatar.ExpPoints + expPointsGained;
            var expCurLevel = player.Avatar.ExpLevels;
            var expLevel = LogicUtils.CalculateExpLevel(Assets, ref expCurLevel, ref expPoints);
            player.Avatar.ExpPoints = expPoints;
            player.Avatar.ExpLevels = expLevel;
            player.Avatar.Gems += gems;

            Village.VillageObjects.Remove(Id);
        }

        /// <summary/>
        protected internal override void Tick(int ctick)
        {
            _timer.Tick(ctick);
            if (_timer.IsComplete)
                FinishClear(ctick);
        }

        /// <summary/>
        protected internal override void ResetVillageObject()
        {
            base.ResetVillageObject();

            //_timer.Stop();
            _timer.Reset();
            Village?.WorkerManager.DeallotateWorker(this);
            _lootMultiplier = default(int);
        }

        #region Json Reading/Writing
        /// <summary/>
        protected internal override void ToJsonWriter(JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("data");
            writer.WriteValue(Data.Id);

            writer.WritePropertyName("id");
            writer.WriteValue(Id);

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

        /// <summary/>
        protected internal override void FromJsonReader(JsonReader reader)
        {
            var instance = CsvData.GetInstance<ObstacleData>();
            // clear_t value.
            var clearTime = -1;
            var clearTimeSet = false;

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
                        case "id":
                            // Ignore for now.
                            break;

                        case "data":
                            dataId = reader.ReadAsInt32().Value;
                            dataIdSet = true;
                            break;

                        case "clear_t":
                            clearTime = reader.ReadAsInt32().Value;
                            clearTimeSet = true;
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
                throw new InvalidOperationException($"Obstacle JSON at {reader.Path} does not contain a 'data' field.");

            if (instance.InvalidDataID(dataId))
                throw new InvalidOperationException($"Obstacle JSON at {reader.Path} contained an invalid ObstacleData ID. {instance.GetArgsOutOfRangeMessage("Data ID")}");

            var tableCollection = Assets.DataTables;
            var dataRef = new CsvDataRowRef<ObstacleData>(dataId);
            var data = dataRef.Get(tableCollection)[0];
            if (data == null)
                throw new InvalidOperationException("Could not find ObstacleData with ID '" + dataId + "'.");

            _data = data;

            if (clearTimeSet)
            {
                Village.WorkerManager.AllocateWorker(this);
                _timer.Start(Village.LastTickTime, 0, clearTime);
            }
        }
        #endregion

        internal static Obstacle GetInstance(Village village)
        {
            var obj = (VillageObject)null;
            if (!VillageObjectPool.TryPop(Kind, out obj))
                obj = new Obstacle();

            obj.SetVillageInternal(village);
            return (Obstacle)obj;
        }
        #endregion
    }
}
