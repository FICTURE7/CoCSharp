using CoCSharp.Csv;
using CoCSharp.Data.Model;
using Newtonsoft.Json;
using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents an in-game Clash of Clans building.
    /// </summary>
    public class Building : Buildable
    {
        internal const int BaseGameID = 500000000;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class with the specified
        /// <see cref="Village"/> which contains the <see cref="Building"/>.
        /// </summary>
        /// <param name="village"><see cref="Village"/> which contains the <see cref="Building"/>.</param>
        public Building(Village village) : base(village)
        {
            village.Buildings.Add(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class with the specified 
        /// <see cref="Village"/> which contains the <see cref="Building"/> and user token object.
        /// </summary>
        /// <param name="village"><see cref="Village"/> which contains the <see cref="Building"/>.</param>
        /// <param name="userToken">User token associated with this <see cref="Building"/>.</param>
        public Building(Village village, object userToken) : base(village, userToken)
        {
            village.Buildings.Add(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class with the specified
        /// <see cref="Village"/> which contains the <see cref="Building"/>, X coordinate and Y coordinate.
        /// </summary>
        /// <param name="village"><see cref="Village"/> which contains the <see cref="Building"/>.</param>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Building(Village village, int x, int y) : base(village, x, y)
        {
            village.Buildings.Add(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class with the specified
        /// <see cref="Village"/> which contains the <see cref="Building"/>, X coordinate, Y coordinate and user token object.
        /// </summary>
        /// <param name="village"><see cref="Village"/> which contains the <see cref="Building"/>.</param>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="userToken">User token associated with this <see cref="Building"/>.</param>
        public Building(Village village, int x, int y, object userToken) : base(village, x, y, userToken)
        {
            village.Buildings.Add(this);
        }
        #endregion

        #region Fields & Properties
        /// <summary>
        /// Gets or sets the <see cref="Type"/> of the <see cref="CsvData"/> expected
        /// by the <see cref="Building"/>.
        /// </summary>
        /// <remarks>
        /// This is needed to make sure that the user provides a proper <see cref="CsvData"/> type
        /// for the <see cref="VillageObject"/>.
        /// </remarks>
        protected override Type ExpectedDataType
        {
            get
            {
                return typeof(BuildingData);
            }
        }

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
                _isLocked = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Begins the construction of the <see cref="Building"/> and increases its level by 1
        /// when done if <see cref="Buildable.IsConstructing"/> is <c>false</c> and <see cref="VillageObject.Data"/>
        /// is not null; otherwise it throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="Buildable.IsConstructing"/> is <c>true</c>.</exception>
        /// <exception cref="InvalidOperationException"><see cref="VillageObject.Data"/> is <c>null</c>.</exception>
        public override void BeginConstruction()
        {
            if (IsConstructing)
                throw new InvalidOperationException("Building object is already in construction.");
            if (Data == null)
                throw new InvalidOperationException("Building.Data cannot be null.");

            var data = GetBuildingData();

            // No need to schedule construction logic if its construction is instant. (Walls)
            if (data.BuildTime.TotalSeconds == 0)
            {
                DoConstructionFinished();
                return;
            }

            var constructionTime = DateTime.UtcNow.Add(data.BuildTime);
            ConstructionEndTime = constructionTime;

            InternalScheduleBuild();
        }

        /// <summary>
        /// Cancel the construction of the <see cref="Building"/> if <see cref="Buildable.IsConstructing"/> is <c>true</c>; otherwise
        /// it throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="Buildable.IsConstructing"/> is <c>false</c>.</exception>
        public override void CancelConstruction()
        {
            if (!IsConstructing)
                throw new InvalidOperationException("Building object is not in construction.");

            var endTime = DateTime.UtcNow;

            InternalCancelScheduleBuild();

            ConstructionTEndUnixTimestamp = 0;
            OnConstructionFinished(new ConstructionFinishedEventArgs()
            {
                BuildableConstructed = this,
                EndTime = endTime,
                UserToken = UserToken,
                WasCancelled = true
            });
        }

        /// <summary>
        /// Speeds up the construction of the <see cref="Building"/> and increases its level by 1
        /// instantly if <see cref="Buildable.IsConstructing"/> is <c>true</c>; otherwise it throws an
        /// <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="Buildable.IsConstructing"/> is <c>false</c>.</exception>
        public override void SpeedUpConstruction()
        {
            // Make sure that we not speeding up construction of a building that is not in construction.
            if (!IsConstructing)
                throw new InvalidOperationException("Building object is not in construction.");

            // Remove the schedule because we don't want it to trigger the events.
            InternalCancelScheduleBuild();
            DoConstructionFinished();
        }

        /// <summary>
        /// Returns the associated <see cref="CsvData"/> with the <see cref="Building"/> as
        /// a <see cref="BuildingData"/>.
        /// </summary>
        /// <returns>Associated <see cref="CsvData"/> with the <see cref="Building"/> as a <see cref="BuildingData"/>.</returns>
        /// <exception cref="InvalidOperationException"><see cref="VillageObject.Data"/> is null.</exception>
        public BuildingData GetBuildingData()
        {
            if (Data == null)
                throw new InvalidOperationException("Building.Data is null.");

            return (BuildingData)Data;
        }


        // Schedules the construction logic at ConstructionEndTime.
        internal void InternalScheduleBuild()
        {
            // Schedule it with the userToken as this object so that it can be cancelled later.
            LogicScheduler.ScheduleLogic(DoConstructionFinished, ConstructionEndTime, userToken: this);
        }

        // Cancels the construction of the logic associated with this object as userToken.
        internal void InternalCancelScheduleBuild()
        {
            LogicScheduler.CancelSchedule(this);
        }

        // Called by the LogicScheduler when the constructionTime has been reached
        // or by the SpeedUpConstruction() method.
        private void DoConstructionFinished()
        {
            var endTime = DateTime.UtcNow;

            // Increase level if construction finished successfully.
            Level++;
            ConstructionTEndUnixTimestamp = 0;

            OnConstructionFinished(new ConstructionFinishedEventArgs()
            {
                BuildableConstructed = this,
                UserToken = UserToken,
                EndTime = endTime
            });
        }

        internal override void ToJsonWriter(JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("data");
            writer.WriteValue(DataID);

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

        // Reads the Building from a JsonReader.
        internal override void FromJsonReader(JsonReader reader)
        {
            // const_t_end value.
            var constTimeEnd = -1;
            // const_t value.
            var constTime = -1;

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
                            DataID = reader.ReadAsInt32().Value;
                            break;

                        case "lvl":
                            Level = reader.ReadAsInt32().Value;
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

            // Try to use const_t if we was not able to get const_t_end's value.
            if (constTimeEnd == -1)
            {
                // We don't have const_t either so we can exit early.
                if (constTime == -1)
                    return;

                ConstructionEndTime = DateTime.UtcNow.AddSeconds(constTime);
            }
            else
            {
                if (constTimeEnd > 0)
                    ConstructionTEndUnixTimestamp = constTimeEnd;
            }
        }
        #endregion
    }
}
