using CoCSharp.Csv;
using CoCSharp.Data.Models;
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

        private static readonly PropertyChangedEventArgs s_isLockedChanged = new PropertyChangedEventArgs("IsLocked");
        #endregion

        #region Constructors
        // Constructor that FromJsonReader method is going to use.
        internal Building(Village village) : base(village)
        {
            // Space
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class with the specified <see cref="Village"/> containing
        /// the <see cref="Building"/> and <see cref="BuildingData"/> which is associated with it and a value indicating the level of
        /// the <see cref="Building"/>.
        /// </summary>
        /// 
        /// <param name="village"><see cref="Village"/> containing the <see cref="Building"/>.</param>
        /// <param name="data"><see cref="BuildingData"/> which is associated with this <see cref="Building"/>.</param>
        /// <param name="level">A value indicating the level of the <see cref="Building"/>.</param>
        public Building(Village village, CsvDataRowRef<BuildingData> data, int level) : base(village, data, level)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class with the specified <see cref="Village"/> containing
        /// the <see cref="Building"/> and <see cref="BuildingData"/> which is associated with it and user token object and a value indicating the level of
        /// the <see cref="Building"/>.
        /// </summary>
        /// 
        /// <param name="village"><see cref="Village"/> which contains the <see cref="Building"/>.</param>
        /// <param name="data"><see cref="BuildingData"/> which is associated with this <see cref="Building"/>.</param>
        /// <param name="userToken">User token associated with this <see cref="Building"/>.</param>
        /// <param name="level">A value indicating the level of the <see cref="Building"/>.</param>
        public Building(Village village, CsvDataRowRef<BuildingData> data, object userToken, int level) : base(village, data, userToken, level)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class with the specified <see cref="Village"/> containing the <see cref="Building"/>
        /// and <see cref="BuildingData"/> which is associated with it, X coordinate and Y coordinate and a value indicating the level of
        /// the <see cref="Building"/>.
        /// </summary>
        /// 
        /// <param name="village"><see cref="Village"/> which contains the <see cref="Building"/>.</param>
        /// <param name="data"><see cref="BuildingData"/> which is associated with this <see cref="Building"/>.</param>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="level">A value indicating the level of the <see cref="Building"/>.</param>
        public Building(Village village, CsvDataRowRef<BuildingData> data, int x, int y, int level) : base(village, data, x, y, level)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class with the specified <see cref="Village"/> containing the <see cref="Building"/>
        /// and <see cref="BuildingData"/> which is associated with it, X coordinate, Y coordinate and user token object and a value indicating the level of
        /// the <see cref="Building"/>.
        /// </summary>
        /// 
        /// <param name="village"><see cref="Village"/> which contains the <see cref="Building"/>.</param>
        /// <param name="data"><see cref="BuildingData"/> which is associated with this <see cref="Building"/>.</param>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="userToken">User token associated with this <see cref="Building"/>.</param>
        /// <param name="level">A value indicating the level of the <see cref="Building"/>.</param>
        public Building(Village village, CsvDataRowRef<BuildingData> data, int x, int y, object userToken, int level) : base(village, data, x, y, userToken, level)
        {
            // Space
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

        internal override int KindID
        {
            get
            {
                return 0;
            }
        }
        #endregion

        #region Methods
        #region Construction
        /// <summary/>
        protected override TimeSpan GetBuildTime(BuildingData data)
        {
            return data.BuildTime;
        }

        /// <summary/>
        protected override int GetTownHallLevel(BuildingData data)
        {
            return data.TownHallLevel;
        }
        #endregion

        internal override void ResetVillageObject()
        {
            base.ResetVillageObject();

            _isLocked = default(bool);
        }

        internal override void Tick(int tick)
        {
            // Determines if the current VillageObject is a TownHall building based on Data.TID
            // and set the townhall of the Village to this VillageObject. 
            if (RowCache.Name == "Town Hall")
            {
                // A Village cannot contain more than 1 townhall.
                if (Village.TownHall != this && Village.TownHall != null)
                    throw new InvalidOperationException("Cannot add a Town Hall building to Village if it already contains one.");

                Village._townhall = this;
            }

            // Ticks the Buildable{T} parent to update construction stuff.
            base.Tick(tick);
        }

        #region Json Reading/Writing
        internal override void ToJsonWriter(JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("data");
            writer.WriteValue(RowCache.ID);

            writer.WritePropertyName("id");
            writer.WriteValue(ID);

            writer.WritePropertyName("lvl");
            writer.WriteValue(Level);

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

            writer.WriteEndObject();
        }

        internal override void FromJsonReader(JsonReader reader)
        {
            var instance = CsvData.GetInstance<BuildingData>();
            // const_t_end value.
            var constTimeEnd = -1;
            // const_t value.
            var constTime = -1;

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
                            break;

                        case "const_t":
                            constTime = reader.ReadAsInt32().Value;
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
                throw new InvalidOperationException("Building JSON does not contain a 'data' field.");
            if (!lvlSet)
                throw new InvalidOperationException("Building JSON does not contain a 'lvl' field.");

            if (instance.InvalidDataID(dataId))
                throw new InvalidOperationException("Building JSON contained an invalid BuildingData ID. " + instance.GetArgsOutOfRangeMessage("Data ID"));

            // Don't use the setter of the Level property to prevent
            // UpdateIsUpgradable from being called which can cause an InvalidOperationException when
            // the TownHall building is no loaded yet.
            _level = lvl;
            UpdateData(dataId, lvl);

            // Try to use const_t_end if we were not able to get const_t's value.
            if (constTime == -1)
            {
                // We don't have const_t_end either so we can exit early.
                if (constTimeEnd == -1)
                {
                    if (_level == NotConstructedLevel)
                        _level = 0;

                    return;
                }

                _timer.Start(constTimeEnd);
            }
            else
            {
                _timer.Start(TimeUtils.UnixUtcNow + constTime);
            }
        }
        #endregion

        // Tries to return an instance of the Building class from the VillageObjectPool.
        internal static Building GetInstance(Village village)
        {
            var obj = (VillageObject)null;
            if (VillageObjectPool.TryPop(Kind, out obj))
            {
                obj.SetVillageInternal(village);
                return (Building)obj;
            }

            return new Building(village);
        }
        #endregion
    }
}
