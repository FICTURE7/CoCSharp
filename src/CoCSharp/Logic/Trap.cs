using CoCSharp.Csv;
using CoCSharp.Data.Models;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents an in-game Clash of Clans trap.
    /// </summary>
    public class Trap : Buildable<TrapData>
    {
        #region Constants
        internal const int Kind = 4;
        internal const int BaseGameID = 504000000;

        private static readonly PropertyChangedEventArgs s_brokenChanged = new PropertyChangedEventArgs("Broken");
        #endregion

        #region Constructors
        // Constructor used to load the VillageObject from a JsonTextReader.
        internal Trap() : base()
        {
            // Space
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
        public Trap(Village village, TrapData data) : base(village, data)
        {
            // Space
        }
        #endregion

        #region Fields & Properties
        private bool _isBroken;

        /// <summary>
        /// Gets or sets a value indicating whether the trap needs to be repaired.
        /// </summary>
        public bool IsBroken
        {
            get
            {
                return _isBroken;
            }
            set
            {
                if (_isBroken == value)
                    return;

                _isBroken = value;
                OnPropertyChanged(s_brokenChanged);
            }
        }

        internal override int KindId => 4;
        #endregion

        #region Methods
        /// <summary/>
        protected override TimeSpan GetBuildTime(TrapData data) => data.BuildTime;

        /// <summary/>
        protected override int GetBuildCost(TrapData data) => data.BuildCost;

        /// <summary/>
        protected override string GetBuildResource(TrapData data) => data.BuildResource;


        /// <summary/>
        protected override int GetTownHallLevel(TrapData data) => data.TownHallLevel;

        /// <summary/>
        protected internal override void ResetVillageObject()
        {
            base.ResetVillageObject();

            _isBroken = default(bool);
        }

        /// <summary/>
        protected internal override void Tick(int tick)
        {
            base.Tick(tick);
        }

        #region Json Reading/Writing
        /// <summary/>
        protected internal override void ToJsonWriter(JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("data");
            writer.WriteValue(RowCache.Id);

            writer.WritePropertyName("id");
            writer.WriteValue(Id);

            writer.WritePropertyName("lvl");
            writer.WriteValue(UpgradeLevel);

            if (ConstructionTEndUnixTimestamp != default(int))
            {
                writer.WritePropertyName("const_t_end");
                writer.WriteValue(ConstructionTEndUnixTimestamp);
            }

            if (ConstructionTSeconds != default(int))
            {
                writer.WritePropertyName("const_t");
                writer.WriteValue(ConstructionTSeconds);
            }

            if (IsBroken != default(bool))
            {
                writer.WritePropertyName("need_repair");
                writer.WriteValue(IsBroken);
            }

            writer.WritePropertyName("x");
            writer.WriteValue(X);

            writer.WritePropertyName("y");
            writer.WriteValue(Y);

            writer.WriteEndObject();
        }

        /// <summary/>
        protected internal override void FromJsonReader(JsonReader reader)
        {
            var instance = CsvData.GetInstance<TrapData>();
            // const_t_end value.
            var constTimeEnd = -1;
            var constTimeEndSet = false;
            // const_t value.
            var constTime = -1;
            var constTimeSet = false;

            var dataId = -1;
            var dataIdSet = false;

            var lvl = 0;

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
                            // Ignore it for now.
                            break;

                        case "data":
                            dataId = reader.ReadAsInt32().Value;
                            dataIdSet = true;
                            break;

                        case "lvl":
                            lvl = reader.ReadAsInt32().Value;
                            break;

                        case "needs_repair":
                            _isBroken = reader.ReadAsBoolean().Value;
                            break;

                        case "const_t_end":
                            constTimeEnd = reader.ReadAsInt32().Value;
                            constTimeEndSet = true;
                            break;

                        case "const_t":
                            constTime = reader.ReadAsInt32().Value;
                            constTimeSet = true;
                            break;

                        case "x":
                            X = reader.ReadAsInt32().Value;
                            break;

                        case "y":
                            Y = reader.ReadAsInt32().Value;
                            break;
                    }
                }
            }

            if (!dataIdSet)
                throw new InvalidOperationException($"Trap JSON at {reader.Path} does not contain a 'data' field.");

            if (instance.InvalidDataID(dataId))
                throw new InvalidOperationException($"Trap JSON at {reader.Path} contained an invalid BuildingData ID. {instance.GetArgsOutOfRangeMessage("Data ID")}");

            UpdateData(dataId, lvl);
            UpdateIsUpgradable();
        }
        #endregion

        // Tries to return an instance of the Building class from the VillageObjectPool.
        internal static Trap GetInstance(Village village)
        {
            var obj = (VillageObject)null;
            if (!VillageObjectPool.TryPop(Kind, out obj))
                obj = new Trap();

            obj.SetVillageInternal(village);
            return (Trap)obj;
        }
        #endregion
    }
}
