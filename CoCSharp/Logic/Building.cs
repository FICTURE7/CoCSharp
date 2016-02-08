using CoCSharp.Data;
using Newtonsoft.Json;
using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans building.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Building : VillageObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class.
        /// </summary>
        public Building() : base()
        {
            Level = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class
        /// with the specified data ID.
        /// </summary>
        /// <param name="dataID">Data ID of the <see cref="Building"/>.</param>
        public Building(int dataID) : base(dataID)
        {
            Level = -1;
        }

        /// <summary>
        /// Gets the base data ID of the <see cref="Building"/>.
        /// </summary>
        internal override int BaseDataID { get { return 1000000; } }

        /// <summary>
        /// Gets the base game ID of the <see cref="Building"/>.
        /// </summary>
        internal override int BaseGameID { get { return 500000000; } }

        /// <summary>
        /// Gets or sets the level of the <see cref="Building"/>.
        /// </summary>
        [JsonProperty("lvl")]
        public int Level { get; set; }

        /// <summary>
        /// Gets whether the <see cref="Building"/> is in construction.
        /// That is upgrading and constructing.
        /// </summary>
        public bool IsConstructing { get { return ConstructionDurationSecounds > 0; } }

        /// <summary>
        /// Gets or sets whether the <see cref="Building"/> is locked.
        /// This is for mostly for the alliance castle building.
        /// </summary>
        [JsonProperty("locked", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Locked { get; set; }

        /// <summary>
        /// Gets the duration of the construction.
        /// </summary>
        public TimeSpan ConstructionDuration
        {
            get
            {
                if (ConstructionEndSecounds <= 0)
                    return TimeSpan.FromSeconds(0);

                return TimeSpan.FromSeconds(ConstructionEndSecounds - DateTimeConverter.UtcNow);
            }
        }
        [JsonProperty("const_t", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private int ConstructionDurationSecounds
        {
            get
            {
                var endTime = ConstructionEndSecounds - DateTimeConverter.UtcNow;
                if (endTime <= 0) // exceeded construction end time
                {
                    ConstructionEndSecounds = 0;
                    return 0;
                }
                return endTime;
            }
        }

        /// <summary>
        /// Gets or sets the date that the construction will finish in UTC time. Returns <see cref="DateTime.MinValue"/>
        /// if not in construction.
        /// </summary>
        public DateTime ConstructionEnd // might wanna use a nullable type.
        {
            get
            {
                if (ConstructionEndSecounds <= 0) // exceeded construction end time
                    return DateTime.MinValue;

                return DateTimeConverter.FromUnixTimestamp(ConstructionEndSecounds);
            }
            set
            {
                ConstructionEndSecounds = (int)DateTimeConverter.ToUnixTimestamp(value);
            }
        }
        [JsonProperty("const_t_end", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private int ConstructionEndSecounds { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="BuildingData"/> of the <see cref="Building"/>.
        /// </summary>
        public BuildingData Data { get; set; } //TODO: Add this to VillageObject.

        /// <summary>
        /// Begins the construction of the <see cref="Building"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="Data"/> is null.</exception>
        public void BeginConstruct()
        {
            if (Data == null)
                throw new InvalidOperationException("Building.Data cannot be null.");

            if (!IsConstructing)
            {
                //Console.WriteLine("Construction of {0} started.", Data.Name);
                var constEnd = DateTime.UtcNow.Add(Data.BuildTime);
                ConstructionEnd = constEnd;
                LogicScheduler.ScheduleLogic(ConstructFinished, constEnd);
            }
        }

        private void ConstructFinished()
        {
            var args = new ConstructionFinishEventArgs() { Building = this, EndTime = ConstructionEnd };
            Level++;
            ConstructionEndSecounds = 0;
            OnConstructionFinished(args);
            //Console.WriteLine("Construction of {0} ended.", Data.Name);
        }

        /// <summary>
        /// Ends the construction of the <see cref="Building"/>.
        /// </summary>
        public void EndConstruct()
        {
            if (!IsConstructing)
                throw new InvalidOperationException("Building is not in construction.");

            ConstructEnded();
            //Console.WriteLine("Construction endded because someone wanted it.");
        }

        private void ConstructEnded()
        {
            var args = new ConstructionFinishEventArgs() { Building = this, EndTime = DateTime.UtcNow, WasEnded = true };
            ConstructionEndSecounds = 0;
            OnConstructionFinished(args);
        }

        /// <summary>
        /// The event raised when the <see cref="Building"/> construction is finished.
        /// </summary>
        public event EventHandler<ConstructionFinishEventArgs> ConstructionFinished;
        /// <summary>
        /// Use this method to trigger the <see cref="ConstructionFinished"/> event/
        /// </summary>
        /// <param name="e">The arguments</param>
        protected virtual void OnConstructionFinished(ConstructionFinishEventArgs e)
        {
            if (ConstructionFinished != null)
                ConstructionFinished(this, e);
        }
    }
}
