using CoCSharp.Csv;
using CoCSharp.Data;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class.
        /// </summary>
        public Building() : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class
        /// with the specified user token object.
        /// </summary>
        /// <param name="userToken">User token associated with this <see cref="Building"/>.</param>
        public Building(object userToken) : base(userToken)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class with the specified
        /// X coordinate and Y cooridnate.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Building(int x, int y) : base(x, y)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class with the specified
        /// X coordinate, Y cooridnate and user token object.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="userToken">User token associated with this <see cref="Building"/>.</param>
        public Building(int x, int y, object userToken) : base(x, y, userToken)
        {
            // Space
        }

        /// <summary>
        /// Gets or sets whether the <see cref="Building"/> is locked.
        /// </summary>
        [JsonProperty("locked", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsLocked
        {
            get { return _isLocked; }
            set { _isLocked = value; }
        }

        // Building is locked. Mainly for Alliance Castle.
        private bool _isLocked;

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of the <see cref="CsvData"/> expected
        /// by the <see cref="Building"/>.
        /// </summary>
        /// <remarks>
        /// This is needed to make sure that the user provides a proper CsvData type
        /// for the <see cref="VillageObject"/>.
        /// </remarks>
        protected override Type ExpectedDataType
        {
            get
            {
                return typeof(BuildingData);
            }
        }

        /// <summary>
        /// Begins the construction of the <see cref="Building"/> and increases its level by 1
        /// when done.
        /// </summary>
        public override void BeginConstruction()
        {
            if (IsConstructing)
                throw new InvalidOperationException("Building object is already in construction.");
            if (Data == null)
                throw new InvalidOperationException("Building.Data cannot be null.");

            //Console.WriteLine("BeginConstruction called.");

            var data = GetBuildingData();

            // No need to schedule construction logic if its construction is instant. (Walls)
            if (data.BuildTime == TimeSpan.FromSeconds(0))
            {
                DoConstructionFinished();
                return;
            }

            var constructionTime = DateTime.UtcNow.Add(data.BuildTime);
            ConstructionEndTime = constructionTime;

            InternalScheduleBuild();
        }

        /// <summary>
        /// Ends(cancels) the construction of the <see cref="Building"/>.
        /// </summary>
        public override void EndConstruction()
        {
            if (!IsConstructing)
                throw new InvalidOperationException("Building object is not in construction.");

            var endTime = DateTime.UtcNow;

            InternalCancelScheduleBuild();

            ConstructionTimeEnd = 0;
            OnConstructionFinished(new ConstructionFinishEventArgs()
            {
                BuildableConstructed = this,
                EndTime = endTime,
                UserToken = UserToken,
                WasCancelled = true
            });
        }

        /// <summary>
        /// Speeds up the construction of the <see cref="Building"/> and increases its level by 1
        /// when done.
        /// </summary>
        public override void SpeedUpConstruction()
        {
            // Make sure that we not speeding up construction of a building that is not in construction.
            if (!IsConstructing)
                throw new InvalidOperationException("Building object is not in construction.");

            // Remove the schedule because we dont want it to trigger the events.
            InternalCancelScheduleBuild();
            DoConstructionFinished();
        }

        // Schdules the construction logic at ConstructionEndTime.
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

            // Increase level if construction finished succesfully.
            Level++;
            ConstructionTimeEnd = 0;

            OnConstructionFinished(new ConstructionFinishEventArgs()
            {
                BuildableConstructed = this,
                UserToken = UserToken,
                EndTime = endTime
            });
        }

        /// <summary>
        /// Returns the associated <see cref="CsvData"/> with the <see cref="Building"/> as
        /// a <see cref="BuildingData"/>.
        /// </summary>
        /// <returns>Associated <see cref="CsvData"/> with the <see cref="Building"/> as a <see cref="BuildingData"/>.</returns>
        public BuildingData GetBuildingData()
        {
            return (BuildingData)Data;
        }
    }
}
