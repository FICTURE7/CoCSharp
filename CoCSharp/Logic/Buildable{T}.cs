using CoCSharp.Csv;
using System;
using System.Diagnostics;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans <see cref="VillageObject"/> that can be constructed.
    /// </summary>
    [DebuggerDisplay("ID = {ID}, Data.ID = {Data.ID}")]
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

        internal Buildable(Village village, CsvDataCollectionRef<TCsvData> data, int level) : base(village, data)
        {
            Initialize(level);
        }

        internal Buildable(Village village, CsvDataCollectionRef<TCsvData> data, object userToken, int level) : base(village, data)
        {
            Initialize(level);
            UserToken = userToken;
        }

        internal Buildable(Village village, CsvDataCollectionRef<TCsvData> data, int x, int y, int level) : base(village, data, x, y)
        {
            Initialize(level);
        }

        internal Buildable(Village village, CsvDataCollectionRef<TCsvData> data, int x, int y, object userToken, int level) : base(village, data, x, y)
        {
            Initialize(level);
            UserToken = userToken;
        }

        private void Initialize(int level)
        {
            if (level < NotConstructedLevel)
                throw new ArgumentOutOfRangeException("level", "level cannot be less than NotConstructedLevel, that is, level -1.");

            _level = level;

            // If level is -1 (NotConstructedLevel), VillageObject.Data will be null.
            // and NextUpgrade will point to a CsvData of level 0 from CollectionCache.
            //UpdateData(Data._OldID, level);
            UpdateIsUpgradable();

            if (level == NotConstructedLevel)
            {
                if (NextUpgrade.Level > 0)
                    throw new ArgumentException("A new construction, that is, a level -1 Buildable, Data level must be 0.", "data");

                // Begin construction of the new construction.
                // New buildings are automatically constructed.
                BeginConstruction();
            }
        }
        #endregion

        #region Fields & Properties
        /// <summary>
        /// The event raised when the <see cref="Building"/> construction is finished.
        /// </summary>
        [Obsolete]
        public event EventHandler<ConstructionEventArgs<TCsvData>> ConstructionFinished;

        /// <summary>
        /// Gets or sets the user token associated with the <see cref="Buildable{TCsvData}"/>.
        /// </summary>
        /// 
        /// <remarks>
        /// <see cref="ConstructionEventArgs{TCsvData}.UserToken"/> will refer to this value.
        /// </remarks>
        public object UserToken { get; set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Buildable{TCsvData}"/> object is in construction.
        /// </summary>
        public bool IsConstructing
        {
            get
            {
                return _timer.IsStarted;
            }
        }

        private bool _isUpgradable;
        /// <summary>
        /// Gets a value indicating whether the <see cref="Buildable{TCsvData}"/> object can be upgraded.
        /// </summary>
        public bool IsUpgradable
        {
            get
            {
                return _isUpgradable;
            }
        }

        internal int _level;
        /// <summary>
        /// Gets the level of the <see cref="Buildable{TCsvData}"/> object.
        /// </summary>
        /// 
        /// <remarks>
        /// <see cref="VillageObject{TCsvData}.Data"/>'s level and the <see cref="Level"/> property are
        /// two different values.
        /// </remarks>
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                if (value < NotConstructedLevel)
                    throw new ArgumentOutOfRangeException("value", "value cannot be less than NotConstructedLevel, that is, level -1.");

                _level = value;

                // If level is -1 (NotConstructedLevel), VillageObject.Data will be null.
                // and NextUpgrade will point to a CsvData of level 0 from CollectionCache.
                UpdateData(CollectionCache.ID, value);
                UpdateIsUpgradable();
            }
        }

        private TCsvData _nextUprade;
        /// <summary>
        /// Gets the next upgrade's <typeparamref name="TCsvData"/>.
        /// </summary>
        /// 
        /// <remarks>
        /// All construction data will be taken from this. 
        /// </remarks>
        public TCsvData NextUpgrade
        {
            get
            {
                return _nextUprade;
            }
            internal set
            {
                _nextUprade = value;
            }
        }

        /// <summary>
        /// Gets the duration of the construction of the <see cref="Buildable{TCsvData}"/> object.
        /// </summary>
        /// 
        /// <exception cref="InvalidOperationException">The <see cref="Buildable{TCsvData}"/> object is not in construction.</exception>
        public TimeSpan ConstructionDuration
        {
            get
            {
                if (!IsConstructing)
                    throw new InvalidOperationException("Buildable object is not in construction.");

                return TimeSpan.FromSeconds(_timer.Duration);
            }
        }

        /// <summary>
        /// Gets the UTC time at which the construction of the <see cref="Buildable{TCsvData}"/> object will end.
        /// </summary>
        /// 
        /// <exception cref="InvalidOperationException">The <see cref="Buildable{TCsvData}"/> object is not in construction.</exception>
        public DateTime ConstructionEndTime
        {
            get
            {
                if (!IsConstructing)
                    throw new InvalidOperationException("Buildable object is not in construction.");

                return TimeUtils.FromUnixTimestamp(_timer.EndTime);
            }
        }

        /// <summary>
        /// Gets the duration of construction in seconds. 
        /// </summary>
        /// 
        /// <remarks>
        /// Represents the "const_t" field of the village JSONs.
        /// </remarks>
        protected int ConstructionTSeconds
        {
            get
            {
                return _timer.Duration;
            }
        }

        /// <summary>
        /// Gets or sets the date of when the construction is going to end in UNIX timestamps. 
        /// Everything is relative to this, changing this will also change the other values.
        /// </summary>
        /// 
        /// <remarks>
        /// Represents the "const_t_end" field of village JSONs.
        /// </remarks>
        protected int ConstructionTEndUnixTimestamp
        {
            get
            {
                return (IsConstructing) ? _timer.EndTime : 0;
            }
        }

        // Timer to time constructions.
        internal TickTimer _timer = new TickTimer();
        #endregion

        #region Methods
        #region Construction
        /// <summary>
        /// Begins the construction of the <see cref="Buildable{TCsvData}"/> and increases its level by 1
        /// when done.
        /// </summary>
        public void BeginConstruction()
        {
            if (IsConstructing)
                throw new InvalidOperationException("Buildable object is already in construction.");
            if (!IsUpgradable)
                throw new InvalidOperationException("Buildable object is maxed or Town Hall level too low.");

            Debug.Assert(NextUpgrade != null, "NextUpgrade was null, when trying to BeginConstruciton.");
            var tick = Village.Tick;
            var buildTime = GetBuildTime(NextUpgrade);

            Village.Logger.Info(tick, "started construction of {0}", ID);

            if (buildTime == InstantConstructionTime)
                DoConstructionFinished();
            else
                _timer.Start(tick, (int)buildTime.TotalSeconds);
        }

        /// <summary>
        /// Cancels the construction of the <see cref="Buildable{TCsvData}"/>.
        /// </summary>
        public void CancelConstruction()
        {

        }

        /// <summary>
        /// Speeds up the construction of the <see cref="Buildable{TCsvData}"/> and finishes the construction instantly
        /// and increases its level by 1.
        /// </summary>
        public void SpeedUpConstruction()
        {

        }

        private void DoConstructionFinished()
        {
            _timer.Stop();

            var tick = Village.Tick;
            var endTime = DateTime.UtcNow;
            var args = new ConstructionEventArgs<TCsvData>(LogicOperation.Finished | LogicOperation.Buy, this, endTime, UserToken);

            Village.Logger.Info(tick, "construction finished {0}", ID);

            _level++;
            _data = NextUpgrade;

            // Calling UpdateCanUpgrade will set the CanUpgrade & NextUpgrade property as well.
            UpdateIsUpgradable();

            OnConstructionFinished(args);
        }

        /// <summary>Returns the BuildTime of the specified data.</summary>
        protected abstract TimeSpan GetBuildTime(TCsvData data);

        /// <summary>Returns the Town Hall level at which the Buildable can upgrade from the specified data.</summary>
        protected abstract int GetTownHallLevel(TCsvData data);

        // Updates the Data of the Buildable and update the CollectionCache if its null.
        [Obsolete]
        internal void UpdateData(int dataId, int level)
        {
            Debug.Assert(level >= NotConstructedLevel, "lvl was less than NotConstructedLevel.");

            // If we haven't cached the SubCollection in which Data
            // is found, we do it.
            if (CollectionCache == null)
            {
                //CollectionCache = AssetManager.SearchCsvNoCheck<TCsvData>(dataId);
                if (CollectionCache == null)
                    throw new InvalidOperationException("Could not find CsvData collection with ID '" + dataId + "'.");
            }

            // Data is null when lvl is -1
            // However NextUpgrade should not.
            if (level == NotConstructedLevel)
            {
                _data = null;
            }
            else
            {
                _data = CollectionCache[level];
                if (_data == null)
                    throw new InvalidOperationException("Could not find CsvData with ID '" + dataId + "' and with level '" + level + "'.");
            }
        }

        // Updates CanUpgrade's value based on next upgrade and townhall of Village.
        // This method will also set the NextUpgrade property.
        internal void UpdateIsUpgradable()
        {
            // Can be null sometimes due to concurrency.
            Debug.Assert(CollectionCache != null, "CollectionCache was null.");

            NextUpgrade = CollectionCache[_level + 1];
            if (NextUpgrade == null)
            {
                // There are no upgrades left.
                _isUpgradable = false;
            }
            else
            {
                // Check if the level of the TownHall in the Village suits the Buildable
                // TownHallLevel required.
                Debug.Assert(NextUpgrade != null, "NextUpgrade was null.");

                var thLevel = GetTownHallLevel(NextUpgrade);
                if (thLevel == 0)
                {
                    _isUpgradable = true;
                }
                else
                {
                    if (Village.TownHall == null)
                        throw new InvalidOperationException("Village does not contain a Town Hall.");

                    _isUpgradable = Village.TownHall.Level >= thLevel - 1;
                }
            }
        }
        #endregion

        internal override void ResetVillageObject()
        {
            base.ResetVillageObject();

            _timer.Stop();
            _nextUprade = default(TCsvData);
            _isUpgradable = default(bool);
            UserToken = default(object);
        }

        internal override void Tick(int tick)
        {
            // Check if the construction Timer has completed.
            if (_timer.IsCompleted(tick))
            {
                DoConstructionFinished();
            }
            else
            {
                UpdateIsUpgradable();
            }
        }

        /// <summary>
        /// Use this method to trigger the <see cref="ConstructionFinished"/> event.
        /// </summary>
        /// <param name="e">The arguments data.</param>
        protected virtual void OnConstructionFinished(ConstructionEventArgs<TCsvData> e)
        {
            if (ConstructionFinished != null)
                ConstructionFinished(this, e);

            Village.OnLogic(e);
        }
        #endregion
    }
}
