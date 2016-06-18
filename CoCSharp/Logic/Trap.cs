using CoCSharp.Csv;
using CoCSharp.Data.Model;
using Newtonsoft.Json;
using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents an in-game Clash of Clans trap.
    /// </summary>
    public class Trap : Buildable
    {
        internal const int BaseGameID = 504000000;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Trap"/> class.
        /// </summary>
        public Trap() : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trap"/> class
        /// with the specified user token object.
        /// </summary>
        /// <param name="userToken">User token associated with this <see cref="Trap"/>.</param>
        public Trap(object userToken) : base(userToken)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trap"/> class with the specified
        /// X coordinate and Y coordinate.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Trap(int x, int y) : base(x, y)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trap"/> class with the specified
        /// X coordinate, Y coordinate and user token object.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="userToken">User token associated with this <see cref="Trap"/>.</param>
        public Trap(int x, int y, object userToken) : base(x, y, userToken)
        {
            // Space
        }
        #endregion

        #region Fields & Properties
        /// <summary>
        /// Gets the <see cref="Type"/> of the <see cref="CsvData"/> expected
        /// by the <see cref="Trap"/>.
        /// </summary>
        /// <remarks>
        /// This is needed to make sure that the user provides a proper <see cref="CsvData"/> type
        /// for the <see cref="VillageObject"/>.
        /// </remarks>
        protected override Type ExpectedDataType
        {
            get
            {
                return typeof(TrapData);
            }
        }

        /// <summary>
        /// Gets or sets whether the trap needs to be repaired.
        /// </summary>
        public bool Broken { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Begins the construction of the <see cref="Trap"/> and increases its level by 1
        /// when done if <see cref="Buildable.IsConstructing"/> is <c>false</c> and <see cref="VillageObject.Data"/>
        /// is not null; otherwise it throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="Buildable.IsConstructing"/> is <c>true</c>.</exception>
        /// <exception cref="InvalidOperationException"><see cref="VillageObject.Data"/> is <c>null</c>.</exception>
        public override void BeginConstruction()
        {
            if (IsConstructing)
                throw new InvalidOperationException("Trap object is already in construction.");
            if (Data == null)
                throw new InvalidOperationException("Trap.Data cannot be null.");

            //Console.WriteLine("BeginConstruction called.");

            var data = GetTrapData();

            // No need to schedule construction logic if its construction is instant. (Initial construction)
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
        /// Cancels the construction of the <see cref="Trap"/> if <see cref="Buildable.IsConstructing"/> is <c>true</c>; otherwise
        /// it throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="Buildable.IsConstructing"/> is <c>false</c>.</exception>
        public override void CancelConstruction()
        {
            if (!IsConstructing)
                throw new InvalidOperationException("Trap object is not in construction.");

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
        /// Speeds up the construction of the <see cref="Trap"/> and increases its level by 1
        /// instantly if <see cref="Buildable.IsConstructing"/> is <c>true</c>; otherwise it throws an
        /// <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="Buildable.IsConstructing"/> is <c>false</c>.</exception>
        public override void SpeedUpConstruction()
        {
            // Make sure that we not speeding up construction of a trap that is not in construction.
            if (!IsConstructing)
                throw new InvalidOperationException("Trap object is not in construction.");

            // Remove the schedule because we don't want it to trigger the events at
            // the scheduled time.
            InternalCancelScheduleBuild();
            DoConstructionFinished();
        }

        /// <summary>
        /// Returns the associated <see cref="CsvData"/> with the <see cref="Trap"/> as
        /// a <see cref="TrapData"/>.
        /// </summary>
        /// <returns>Associated <see cref="CsvData"/> with the <see cref="Trap"/> as a <see cref="TrapData"/>.</returns>
        /// <exception cref="InvalidOperationException"><see cref="VillageObject.Data"/> is null.</exception>
        public TrapData GetTrapData()
        {
            if (Data == null)
                throw new InvalidOperationException("Trap.Data is null.");

            return (TrapData)Data;
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

                        case "needs_repair":
                            Broken = reader.ReadAsBoolean().Value;
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
        }
        #endregion
    }
}
