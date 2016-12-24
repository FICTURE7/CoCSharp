using CoCSharp.Csv;
using CoCSharp.Data.Models;
using CoCSharp.Logic.Components;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents an in-game Clash of Clans building.
    /// </summary>
    public class Building : Buildable<BuildingData>
    {
        #region Constants
        internal const int Kind = 0;
        internal const int BaseGameID = 500000000;

        private static readonly PropertyChangedEventArgs s_isLockedChanged = new PropertyChangedEventArgs(nameof(IsLocked));
        #endregion

        #region Constructors
        // Constructor used to load the VillageObject from a JsonTextReader.
        internal Building() : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class with the specified
        /// <see cref="Village"/> instance and <see cref="BuildingData"/>.
        /// </summary>
        /// 
        /// <param name="village">
        /// <see cref="Village"/> instance which owns this <see cref="Building"/>.
        /// </param>
        /// 
        /// <param name="data"><see cref="BuildingData"/> representing the data of the <see cref="Building"/>.</param>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="village"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        public Building(Village village, BuildingData data) : base(village, data)
        {
            SetComponents();
        }
        #endregion

        #region Fields & Properties
        // Building is locked. Mainly for Alliance Castle.
        private bool _isLocked;

        /// <summary>
        /// Gets or sets whether the <see cref="Building"/> is locked.
        /// </summary>
        public bool IsLocked
        {
            get
            {
                return _isLocked;
            }
            set
            {
                if (_isLocked == value)
                    return;

                _isLocked = value;
                OnPropertyChanged(s_isLockedChanged);
            }
        }

        internal override int KindId => 0;
        #endregion

        #region Methods
        /// <summary/>
        protected override TimeSpan GetBuildTime(BuildingData data) => data.BuildTime;

        /// <summary/>
        protected override int GetBuildCost(BuildingData data) => data.BuildCost;

        /// <summary/>
        protected override string GetBuildResource(BuildingData data) => data.BuildResource;

        /// <summary/>
        protected override int GetTownHallLevel(BuildingData data) => data.TownHallLevel;

        /// <summary/>
        protected internal override void ResetVillageObject()
        {
            base.ResetVillageObject();

            _isLocked = default(bool);
        }

        /// <summary/>
        protected internal override void Tick(int tick)
        {
            // Ticks the Buildable{T} parent to update construction stuff.
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

            if (IsLocked != default(bool))
            {
                writer.WritePropertyName("locked");
                writer.WriteValue(IsLocked);
            }

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

            writer.WritePropertyName("x");
            writer.WriteValue(X);

            writer.WritePropertyName("y");
            writer.WriteValue(Y);

            // Writes our list of component.
            foreach (var comp in Components)
                comp?.ToJsonWriter(writer);

            writer.WriteEndObject();
        }

        /// <summary/>
        protected internal override void FromJsonReader(JsonReader reader)
        {
            var instance = CsvData.GetInstance<BuildingData>();
            // const_t_end value.
            var constTimeEnd = -1;
            var constTimeEndSet = false;
            // const_t value.
            var constTime = -1;
            var constTimeSet = false;

            var dataId = -1;
            var dataIdSet = false;

            var lvl = -1;
            var lvlSet = false;

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
                            lvlSet = true;
                            break;

                        case "locked":
                            IsLocked = reader.ReadAsBoolean().Value;
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
                throw new InvalidOperationException($"Building JSON at {reader.Path} does not contain a 'data' field.");
            if (!lvlSet)
                throw new InvalidOperationException($"Building JSON at {reader.Path} does not contain a 'lvl' field.");

            if (instance.InvalidDataID(dataId))
                throw new InvalidOperationException($"Building JSON at {reader.Path} contained an invalid BuildingData ID. {instance.GetArgsOutOfRangeMessage("Data ID")}");

            UpdateData(dataId, lvl);
            UpdateIsUpgradable();

            //SetComponents();
            //if (UpgradeLevel == -1)
            //{

            //}

            if (RowCache.Name == "Town Hall")
                Village._townhall = this;

            if (constTimeEndSet)
            {
                var startTime = (int)TimeUtils.ToUnixTimestamp(Village.LastTickTime);
                var duration = constTimeEnd - startTime;

                // Clamp value to 0.
                if (duration < 0)
                    duration = 0;

                Village.WorkerManager.AllocateWorker(this);
                Timer.Start(Village.LastTickTime, 0, duration);
            }
        }
        #endregion

        // Tries to return an instance of the Building class from the VillageObjectPool.
        // Used by ReadBuildingArray.
        internal static Building GetInstance(Village village)
        {
            var obj = (VillageObject)null;
            if (!VillageObjectPool.TryPop(Kind, out obj))
                obj = new Building();

            obj.SetVillageInternal(village);
            return (Building)obj;
        }

        private void SetComponents()
        {
            //var data = (BuildingData)null;
            //if (Data != null)
            //    data = Data;
            //else if (NextUpgrade != null)
            //    data = NextUpgrade;

            //if (data == null)
            //{
            //    Level.Logs.Log("Unable to SetComponents.");
            //    return;
            //}

            //if (data.UnitProduction > 0)
            //    AddComponent(new UnitProductionComponent());
        }
        #endregion
    }
}
