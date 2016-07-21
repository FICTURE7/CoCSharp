using CoCSharp.Csv;
using System;
using System.Diagnostics;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans <see cref="VillageObject"/> that can be constructed.
    /// </summary>
    public abstract class Buildable<TCsvData> : VillageObject<TCsvData> where TCsvData : CsvData, new()
    {
        #region Constants
        /// <summary>
        /// Level at which a <see cref="Buildable{TCsvData}"/> is not constructed.
        /// </summary>
        public const int NotConstructedLevel = -1;

        /// <summary/>
        protected internal static readonly TimeSpan InstantConstructionTime = new TimeSpan(0);
        #endregion

        #region Constructors
        // Constructor that FromJsonReader method is going to use.
        internal Buildable(Village village) : base(village)
        {
            // Space
        }


        internal Buildable(Village village, TCsvData data) : base(village, data)
        {
            UpdateData(data.ID, data.Level);
            UpdateCanUpgade();

            if (data.Level == 0)
                _isConstructed = false;
        }

        internal Buildable(Village village, TCsvData data, object userToken) : this(village, data)
        {
            UserToken = userToken;
            UpdateData(data.ID, data.Level);
            UpdateCanUpgade();

            if (data.Level == 0)
                _isConstructed = false;
        }

        internal Buildable(Village village, TCsvData data, int x, int y) : base(village, data, x, y)
        {
            UpdateData(data.ID, data.Level);
            UpdateCanUpgade();

            if (data.Level == 0)
                _isConstructed = false;
        }

        internal Buildable(Village village, TCsvData data, int x, int y, object userToken) : this(village, data, x, y)
        {
            UserToken = userToken;
            UpdateData(data.ID, data.Level);
            UpdateCanUpgade();

            if (data.Level == 0)
                _isConstructed = false;
        }
        #endregion

        #region Fields & Properties
        /// <summary>Value indicating whether the <see cref="Buildable{TCsvData}"/> is constructed.</summary>
        protected internal bool _isConstructed = false;

        /// <summary>
        /// The event raised when the <see cref="Building"/> construction is finished.
        /// </summary>
        public event EventHandler<ConstructionFinishedEventArgs<TCsvData>> ConstructionFinished;

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

        // Determines whether the Buildable Build event was scheduled.
        internal bool _scheduled;

        // Next upgrade of the Buildable. This field gets set by the UpdateCanUpgrade method.
        internal TCsvData _nextUpgrade;
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

        // Called by the LogicScheduler when the constructionTime has been reached
        // or by the SpeedUpConstruction() method.
        internal abstract void DoConstructionFinished();

        // Determines if the Buildable can upgraded based on the townhall level required.
        internal abstract bool CanUpgradeCheckTownHallLevel();

        // Schedules the construction logic at ConstructionEndTime with userToken as this object.
        internal void ScheduleBuild()
        {
            Debug.Assert(!_scheduled);

            // Schedule it with the userToken as this object so that it can be cancelled later.
            LogicScheduler.ScheduleLogic(DoConstructionFinished, ConstructionEndTime, userToken: this);
            _scheduled = true;
        }

        // Cancels the construction of the logic with userToken as this object.
        internal void CancelScheduleBuild()
        {
            Debug.Assert(_scheduled);

            LogicScheduler.CancelSchedule(this);
            _scheduled = false;
        }

        // Updates the Data of the Buildable and update the _collectionCache if its null.
        internal void UpdateData(int dataId, int lvl)
        {
            if (_collectionCache == null)
            {
                _collectionCache = AssetManager.SearchCsvNoCheck<TCsvData>(dataId);
                if (_collectionCache == null)
                    throw new InvalidOperationException("Could not find CsvData collection with ID '" + dataId + "'.");
            }

            var data = _collectionCache[lvl];
            if (data == null)
                throw new InvalidOperationException("Could not find CsvData with ID '" + dataId + "' and with level '" + lvl + "'.");

            _data = data;
        }

        // Updates CanUpgrade's value based on next upgrade and townhall of Village.
        internal void UpdateCanUpgade()
        {
            Debug.Assert(Data != null && _collectionCache != null);

            var nextLvl = Data.Level + 1;
            _nextUpgrade = _collectionCache[nextLvl];

            var data = _isConstructed ? _nextUpgrade : Data;
            // Theirs no upgrade left.
            if (data == null)
            {
                _canUpgrade = false;
            }
            else
            {
                _canUpgrade = CanUpgradeCheckTownHallLevel();
            }
        }
        #endregion

        internal override void ResetVillageObject()
        {
            base.ResetVillageObject();
            _data = default(TCsvData);
            _collectionCache = default(CsvDataSubCollection<TCsvData>);
            _canUpgrade = default(bool);
            _scheduled = default(bool);
            UserToken = default(object);
            ConstructionTEndUnixTimestamp = default(int);
        }

        /// <summary>
        /// Use this method to trigger the <see cref="ConstructionFinished"/> event.
        /// </summary>
        /// <param name="e">The arguments data.</param>
        protected virtual void OnConstructionFinished(ConstructionFinishedEventArgs<TCsvData> e)
        {
            if (ConstructionFinished != null)
                ConstructionFinished(this, e);
        }
        #endregion
    }
}
