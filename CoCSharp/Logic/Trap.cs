﻿using CoCSharp.Csv;
using CoCSharp.Data.Models;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;

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
        // Constructor that FromJsonReader method is going to use.
        internal Trap(Village village) : base(village)
        {
            // Space
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Trap"/> class with the specified <see cref="Village"/> containing
        /// the <see cref="Trap"/> and <see cref="TrapData"/> which is associated with it.
        /// </summary>
        /// 
        /// <param name="village"><see cref="Village"/> containing the <see cref="Trap"/>.</param>
        /// <param name="data"><see cref="TrapData"/> which is associated with this <see cref="Trap"/>.</param>
        public Trap(Village village, TrapData data) : base(village, data)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trap"/> class with the specified <see cref="Village"/> containing
        /// the <see cref="Trap"/> and <see cref="TrapData"/> which is associated with it and a value indicating the level of
        /// the <see cref="Trap"/>.
        /// </summary>
        /// 
        /// <param name="village"><see cref="Village"/> containing the <see cref="Trap"/>.</param>
        /// <param name="data"><see cref="TrapData"/> which is associated with this <see cref="Trap"/>.</param>
        /// <param name="level">A value indicating the level of the <see cref="Trap"/>.</param>
        public Trap(Village village, TrapData data, int level) : base(village, data, level)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trap"/> class with the specified <see cref="Village"/> containing
        /// the <see cref="Trap"/> and <see cref="TrapData"/> which is associated with it and user token object.
        /// </summary>
        /// 
        /// <param name="village"><see cref="Village"/> which contains the <see cref="Trap"/>.</param>
        /// <param name="data"><see cref="TrapData"/> which is associated with this <see cref="Trap"/>.</param>
        /// <param name="userToken">User token associated with this <see cref="Trap"/>.</param>
        public Trap(Village village, TrapData data, object userToken) : base(village, data, userToken)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trap"/> class with the specified <see cref="Village"/> containing
        /// the <see cref="Trap"/> and <see cref="TrapData"/> which is associated with it and user token object and a value indicating the level of
        /// the <see cref="Trap"/>.
        /// </summary>
        /// 
        /// <param name="village"><see cref="Village"/> which contains the <see cref="Trap"/>.</param>
        /// <param name="data"><see cref="TrapData"/> which is associated with this <see cref="Trap"/>.</param>
        /// <param name="userToken">User token associated with this <see cref="Trap"/>.</param>
        /// <param name="level">A value indicating the level of the <see cref="Trap"/>.</param>
        public Trap(Village village, TrapData data, object userToken, int level) : base(village, data, userToken)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trap"/> class with the specified <see cref="Village"/> containing the <see cref="Trap"/>
        /// and <see cref="TrapData"/> which is associated with it, X coordinate and Y coordinate.
        /// </summary>
        /// 
        /// <param name="village"><see cref="Village"/> which contains the <see cref="Trap"/>.</param>
        /// <param name="data"><see cref="TrapData"/> which is associated with this <see cref="Trap"/>.</param>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Trap(Village village, TrapData data, int x, int y) : base(village, data, x, y)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trap"/> class with the specified <see cref="Village"/> containing the <see cref="Trap"/>
        /// and <see cref="TrapData"/> which is associated with it, X coordinate and Y coordinate and a value indicating the level of
        /// the <see cref="Trap"/>.
        /// </summary>
        /// 
        /// <param name="village"><see cref="Village"/> which contains the <see cref="Trap"/>.</param>
        /// <param name="data"><see cref="TrapData"/> which is associated with this <see cref="Trap"/>.</param>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="level">A value indicating the level of the <see cref="Trap"/>.</param>
        public Trap(Village village, TrapData data, int x, int y, int level) : base(village, data, x, y, level)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trap"/> class with the specified <see cref="Village"/> containing the <see cref="Trap"/>
        /// and <see cref="TrapData"/> which is associated with it, X coordinate, Y coordinate and user token object.
        /// </summary>
        /// 
        /// <param name="village"><see cref="Village"/> which contains the <see cref="Trap"/>.</param>
        /// <param name="data"><see cref="TrapData"/> which is associated with this <see cref="Trap"/>.</param>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="userToken">User token associated with this <see cref="Trap"/>.</param>
        public Trap(Village village, TrapData data, int x, int y, object userToken) : base(village, data, x, y, userToken)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trap"/> class with the specified <see cref="Village"/> containing the <see cref="Trap"/>
        /// and <see cref="TrapData"/> which is associated with it, X coordinate, Y coordinate and user token object and a value indicating the level of
        /// the <see cref="Trap"/>.
        /// </summary>
        /// 
        /// <param name="village"><see cref="Village"/> which contains the <see cref="Trap"/>.</param>
        /// <param name="data"><see cref="TrapData"/> which is associated with this <see cref="Trap"/>.</param>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="userToken">User token associated with this <see cref="Trap"/>.</param>
        /// <param name="level">A value indicating the level of the <see cref="Trap"/>.</param>
        public Trap(Village village, TrapData data, int x, int y, object userToken, int level) : base(village, data, x, y, userToken, level)
        {
            // Space
        }
        #endregion

        #region Fields & Properties
        private bool _broken;
        /// <summary>
        /// Gets or sets a value indicating whether the trap needs to be repaired.
        /// </summary>
        public bool Broken
        {
            get
            {
                return _broken;
            }
            set
            {
                if (_broken == value)
                    return;

                _broken = value;
                OnPropertyChanged(s_brokenChanged);
            }
        }

        internal override int KindID
        {
            get
            {
                return 4;
            }
        }
        #endregion

        #region Methods
        #region Construction
        internal override TimeSpan GetBuildTime(TrapData data)
        {
            return data.BuildTime;
        }

        internal override int GetTownHallLevel(TrapData data)
        {
            return data.TownHallLevel;
        }

        protected override bool CanUpgradeCheckTownHallLevel()
        {
            Debug.Assert(NextUpgrade != null && Data != null);

            var buildData = _constructionLevel > NotConstructedLevel ? NextUpgrade : Data;
            if (buildData.TownHallLevel == 0)
                return true;

            var th = Village.TownHall;
            if (th == null)
                throw new InvalidOperationException("Village does not contain a Town Hall.");

            // TownHallLevel field is not a zero-based so we subtract 1.
            if (th.Data.Level >= buildData.TownHallLevel - 1)
                return true;

            return false;
        }
        #endregion

        internal override void ResetVillageObject()
        {
            base.ResetVillageObject();
            _broken = default(bool);
        }

        internal override void Tick(int tick)
        {
            // Ticks the Buildable{T} parent to update construction stuff.
            base.Tick(tick);
        }

        #region Json Reading/Writing
        internal override void ToJsonWriter(JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("data");
            writer.WriteValue(Data.ID);

            writer.WritePropertyName("id");
            writer.WriteValue(ID);

            writer.WritePropertyName("lvl");
            writer.WriteValue(Level);

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

            if (Broken != default(bool))
            {
                writer.WritePropertyName("need_repair");
                writer.WriteValue(Broken);
            }

            writer.WritePropertyName("x");
            writer.WriteValue(X);

            writer.WritePropertyName("y");
            writer.WriteValue(Y);

            writer.WriteEndObject();
        }

        internal override void FromJsonReader(JsonReader reader)
        {
            var instance = CsvData.GetInstance<TrapData>();
            // const_t_end value.
            var constTimeEnd = -1;
            // const_t value.
            var constTime = -1;

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
                            _broken = reader.ReadAsBoolean().Value;
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
                throw new InvalidOperationException("Trap JSON does not contain a 'data' field.");

            _constructionLevel = lvl;
            // If its not constructed yet, the level is -1,
            // therefore it must be a lvl 0 building.
            if (lvl == NotConstructedLevel)
            {
                //_isConstructed = false;
                lvl = 0;
            }

            if (instance.InvalidDataID(dataId))
                throw new InvalidOperationException("Trap JSON contained an invalid data ID. " + instance.GetArgsOutOfRangeMessage("Data ID"));

            UpdateData(dataId, lvl);
            // UpdateCanUpgade();
            // Village.ReadTrapArray() method will call the UpdateCanUpgrade() method.

            // Try to use const_t if we were not able to get const_t_end's value.
            if (constTimeEnd == -1)
            {
                // We don't have const_t either so we can exit early.
                if (constTime == -1)
                    return;

                ConstructionEndTime = DateTime.UtcNow.AddSeconds(constTime);
            }
            else
            {
                if (constTimeEnd > DateTimeConverter.UnixUtcNow + 100)
                {
                    ConstructionTEndUnixTimestamp = constTimeEnd;
                }
                else
                {
                    // Date at which building construction was going to end has passed.
                    UpdateCanUpgade();
                    //DoConstructionFinished();
                    return;
                }
            }

            // Schedule the build event and all that stuff.
            //ScheduleBuild();
        }
        #endregion

        internal static Trap GetInstance(Village village)
        {
            var obj = (VillageObject)null;
            if (VillageObjectPool.TryPop(BaseGameID, out obj))
            {
                obj.SetVillageInternal(village);
                return (Trap)obj;
            }

            return new Trap(village);
        }
        #endregion
    }
}
