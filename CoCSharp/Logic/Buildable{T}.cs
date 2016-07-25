using CoCSharp.Csv;
using System;
using System.Diagnostics;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans <see cref="VillageObject"/> that can be constructed.
    /// </summary>
    [DebuggerDisplay("ID = {ID}, Data.TID = {Data.TID}")]
    public abstract class Buildable<TCsvData> : VillageObject<TCsvData> where TCsvData : CsvData, new()
    {
        #region Constants
        /// <summary>
        /// Level at which a <see cref="Buildable{TCsvData}"/> is not constructed.
        /// </summary>
        public const int NotConstructedLevel = -1;

        /// <summary/>
        protected static readonly TimeSpan InstantConstructionTime = new TimeSpan(0);
        #endregion

        #region Constructors
        // Constructor that FromJsonReader method is going to use.
        internal Buildable(Village village) : base(village)
        {
            // Space
        }


        internal Buildable(Village village, TCsvData data) : base(village, data)
        {
            Initialize();
        }

        internal Buildable(Village village, TCsvData data, bool newConstruction) : base(village, data)
        {
            Initialize(newConstruction);
        }

        internal Buildable(Village village, TCsvData data, object userToken) : this(village, data)
        {
            UserToken = userToken;
        }

        internal Buildable(Village village, TCsvData data, object userToken, bool newConstruction) : base(village, data)
        {
            Initialize(newConstruction);
            UserToken = userToken;
        }

        internal Buildable(Village village, TCsvData data, int x, int y) : base(village, data, x, y)
        {
            Initialize();
        }
        
        internal Buildable(Village village, TCsvData data, int x, int y, bool newConstruction) : base(village, data, x, y)
        {
            Initialize(newConstruction);
        }

        internal Buildable(Village village, TCsvData data, int x, int y, object userToken) : this(village, data, x, y)
        {
            UserToken = userToken;
        }

        internal Buildable(Village village, TCsvData data, int x, int y, object userToken, bool newConstruction) : base(village, data, x, y)
        {
            Initialize(newConstruction);
            UserToken = userToken;
        }

        private void Initialize()
        {
            // If the level of the Data associated with the Buildable is 0,
            // we assume its a new construction.
            var newConstruction = Data.Level == 0;
            Initialize(newConstruction);
        }

        private void Initialize(bool newConstruction)
        {
            UpdateData(Data.ID, Data.Level);
            UpdateCanUpgade();

            if (newConstruction)
            {
                if (Data.Level != 0)
                    throw new ArgumentException("A new construction's Data level must be 0.", "data");

                _constructionLevel = NotConstructedLevel;
                // Begin construction of the new construction.
                BeginConstruction();
            }
            else
            {
                _constructionLevel = Data.Level;
            }
        }
        #endregion

        #region Fields & Properties
        /// <summary>
        /// The event raised when the <see cref="Building"/> construction is finished.
        /// </summary>
        public event EventHandler<ConstructionFinishedEventArgs<TCsvData>> ConstructionFinished;
        
        //private LogicOperations _operations;
        //public LogicOperations AvailableOperations
        //{
        //    get
        //    {
        //        return _operations;
        //    }
        //}

        /// <summary>
        /// Gets or sets the user token associated with the <see cref="Buildable{TCsvData}"/>.
        /// </summary>
        /// <remarks>
        /// Reference to the object in <see cref="ConstructionFinishedEventArgs{TCsvData}.UserToken"/>.
        /// </remarks>
        public object UserToken { get; set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Buildable{TCsvData}"/> object is in construction.
        /// </summary>
        public bool IsConstructing
        {
            get
            {
                return ConstructionTSeconds > 0;
            }
        }

        private bool _canUpgrade;
        /// <summary>
        /// Gets a value indicating whether the <see cref="Buildable{TCsvData}"/> object can be upgraded.
        /// </summary>
        public bool CanUpgrade
        {
            get
            {
                return _canUpgrade;
            }
        }

        /// <summary>
        /// Gets the duration of the construction of the <see cref="Buildable{TCsvData}"/> object.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="Buildable{TCsvData}"/> object is not in construction.</exception>
        public TimeSpan ConstructionDuration
        {
            get
            {
                if (!IsConstructing)
                    throw new InvalidOperationException("Buildable object is not in construction.");

                return TimeSpan.FromSeconds(ConstructionTSeconds);
            }
        }

        /// <summary>
        /// Gets or sets the UTC time at which the construction of the <see cref="Buildable{TCsvData}"/> object will end.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="Buildable{TCsvData}"/> object is not in construction.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/>.Kind is not of <see cref="DateTimeKind.Utc"/>.</exception>
        public DateTime ConstructionEndTime
        {
            get
            {
                if (!IsConstructing)
                    throw new InvalidOperationException("Buildable object is not in construction.");

                // Converts the UnixTimestamp value into a DateTime.
                return DateTimeConverter.FromUnixTimestamp(ConstructionTEndUnixTimestamp);
            }
            set
            {
                if (value.Kind != DateTimeKind.Utc)
                    throw new ArgumentException("DateTime.Kind of value must a DateTimeKind.Utc.", "value");

                // Converts the provided DateTime into a UnixTimestamp.
                ConstructionTEndUnixTimestamp = (int)DateTimeConverter.ToUnixTimestamp(value);
            }
        }

        /// <summary>
        /// Duration of construction in seconds. 
        /// Changes <see cref="ConstructionTEndUnixTimestamp"/> to 0 if duration is less than 0.
        /// </summary>
        protected int ConstructionTSeconds
        {
            get
            {
                // Difference between construction end time and time now = duration.
                var constDuration = ConstructionTEndUnixTimestamp - DateTimeConverter.UnixUtcNow;

                if (constDuration <= 0)
                {
                    // If construction duration is less than 0 then the construction is finished.
                    // Set ConstructionTimeEnd to 0 because the construction is finished.

                    ConstructionTEndUnixTimestamp = 0;
                    return 0;
                }

                return constDuration;
            }

            // ConstructionTime does not need a setter because it is relative to ConstructionTimeEnd.
            // Changing ConstructionTimeEnd would also change ConstructionTime.
        }
        /// <summary>
        /// Date of when the construction is going to end in UNIX timestamps. Everything is relative to this.
        /// </summary>
        protected int ConstructionTEndUnixTimestamp { get; set; }

        /// <sumary>Determines whether the Buildable Build event was scheduled.</sumary>
        protected bool Scheduled { get; set; }
        /// <summary>Next upgrade of the Buildable. This field gets set by the UpdateCanUpgrade method.</summary>
        protected TCsvData NextUpgrade { get; set; }
        #endregion

        #region Methods
        #region Construction
        /// <summary>
        /// Begins the construction of the <see cref="Buildable{TCsvData}"/> and increases its level by 1
        /// when done.
        /// </summary>
        public abstract void BeginConstruction();

        /// <summary>
        /// Cancels the construction of the <see cref="Buildable{TCsvData}"/>.
        /// </summary>
        public abstract void CancelConstruction();

        /// <summary>
        /// Speeds up the construction of the <see cref="Buildable{TCsvData}"/> and finishes the construction instantly
        /// and increases its level by 1.
        /// </summary>
        public abstract void SpeedUpConstruction();

        ///<summary>Called by the LogicScheduler when the constructionTime has been reached or by the SpeedUpConstruction() method.</summary>
        protected abstract void DoConstructionFinished();

        ///<summary>Determines if the Buildable can upgraded based on the townhall level required</summary>
        protected abstract bool CanUpgradeCheckTownHallLevel();

        ///<summary>Schedules the construction logic at ConstructionEndTime with userToken as this object.</summary>
        protected void ScheduleBuild()
        {
            Debug.Assert(!Scheduled);

            // Schedule it with the userToken as this object so that it can be cancelled later.
            LogicScheduler.ScheduleLogic(DoConstructionFinished, ConstructionEndTime, userToken: this);
            Scheduled = true;
        }

        ///<summary>Cancels the construction of the logic with userToken as this object.</summary>
        protected void CancelScheduleBuild()
        {
            Debug.Assert(Scheduled);

            LogicScheduler.CancelSchedule(this);
            Scheduled = false;
        }

        // Updates the Data of the Buildable and update the _collectionCache if its null.
        internal void UpdateData(int dataId, int lvl)
        {
            // If we haven't cached the SubCollection in which Data
            // is found, we do it.
            if (CollectionCache == null)
            {
                CollectionCache = AssetManager.SearchCsvNoCheck<TCsvData>(dataId);
                if (CollectionCache == null)
                    throw new InvalidOperationException("Could not find CsvData collection with ID '" + dataId + "'.");
            }

            var data = CollectionCache[lvl];
            if (data == null)
                throw new InvalidOperationException("Could not find CsvData with ID '" + dataId + "' and with level '" + lvl + "'.");

            _data = data;
        }

        // Updates CanUpgrade's value based on next upgrade and townhall of Village.
        // This method will also set the _nextUpgrade field.
        internal void UpdateCanUpgade()
        {
            //NOTE: CollectionCache can be null, if we change Data to a data with a different Data ID.
            Debug.Assert(Data != null && CollectionCache != null);

            var nextLvl = Data.Level + 1;
            NextUpgrade = CollectionCache[nextLvl];

            var data = _constructionLevel > NotConstructedLevel ? NextUpgrade : Data;
            if (NextUpgrade == null)
            {
                // Theirs no upgrade left.
                _canUpgrade = false;
            }
            else
            {
                // Check if the level of the Buildable suits the
                // TownHallLevel required.
                _canUpgrade = CanUpgradeCheckTownHallLevel();
            }
        }
        #endregion

        internal override void ResetVillageObject()
        {
            base.ResetVillageObject();
            _data = default(TCsvData);
            CollectionCache = default(CsvDataSubCollection<TCsvData>);
            _canUpgrade = default(bool);
            Scheduled = default(bool);
            UserToken = default(object);
            ConstructionTEndUnixTimestamp = default(int);
        }

        /// <summary>
        /// Use this method to trigger the <see cref="ConstructionFinished"/> event.
        /// </summary>
        /// <param name="e">The arguments data.</param>
        protected virtual void OnConstructionFinished(ConstructionFinishedEventArgs<TCsvData> e)
        {
            if (!e.WasCancelled)
                _constructionLevel++;

            if (ConstructionFinished != null)
                ConstructionFinished(this, e);
        }
        #endregion
    }
}
