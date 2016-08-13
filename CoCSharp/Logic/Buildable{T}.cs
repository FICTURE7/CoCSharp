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
            Initialize(NotConstructedLevel);
        }

        internal Buildable(Village village, TCsvData data, int level) : base(village, data)
        {
            Initialize(level);
        }

        internal Buildable(Village village, TCsvData data, object userToken) : this(village, data)
        {
            UserToken = userToken;
        }

        internal Buildable(Village village, TCsvData data, object userToken, int level) : base(village, data)
        {
            Initialize(level);
            UserToken = userToken;
        }

        internal Buildable(Village village, TCsvData data, int x, int y) : base(village, data, x, y)
        {
            Initialize(NotConstructedLevel);
        }

        internal Buildable(Village village, TCsvData data, int x, int y, int level) : base(village, data, x, y)
        {
            Initialize(level);
        }

        internal Buildable(Village village, TCsvData data, int x, int y, object userToken) : this(village, data, x, y)
        {
            UserToken = userToken;
        }

        internal Buildable(Village village, TCsvData data, int x, int y, object userToken, int level) : base(village, data, x, y)
        {
            Initialize(level);
            UserToken = userToken;
        }

        private void Initialize(int level)
        {
            if (level < NotConstructedLevel)
                throw new ArgumentOutOfRangeException("level", "level cannot be less than NotConstructedLevel, that is, level -1.");

            _level = NotConstructedLevel;
            UpdateData(Data.ID, Data.Level);
            UpdateCanUpgade();

            if (level == NotConstructedLevel)
            {
                if (Data.Level != 0)
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
        public event EventHandler<ConstructionFinishedEventArgs<TCsvData>> ConstructionFinished;

        /// <summary>
        /// Gets or sets the user token associated with the <see cref="Buildable{TCsvData}"/>.
        /// </summary>
        /// 
        /// <remarks>
        /// <see cref="ConstructionFinishedEventArgs{TCsvData}.UserToken"/> will refer to this value.
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

        private int _level;
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
            protected set
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

                return TimeSpan.FromSeconds(ConstructionTSeconds);
            }
        }

        /// <summary>
        /// Gets or sets the UTC time at which the construction of the <see cref="Buildable{TCsvData}"/> object will end.
        /// </summary>
        /// 
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
        /// Gets the duration of construction in seconds. 
        /// Changes <see cref="ConstructionTEndUnixTimestamp"/> to 0 if duration is less than 0.
        /// </summary>
        /// 
        /// <remarks>
        /// Represents the "const_t" field of the village JSONs.
        /// </remarks>
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
        /// Gets or sets the date of when the construction is going to end in UNIX timestamps. 
        /// Everything is relative to this, changing this will also change the other values.
        /// </summary>
        /// 
        /// <remarks>
        /// Represents the "const_t_end" field of village JSONs.
        /// </remarks>
        protected int ConstructionTEndUnixTimestamp { get; set; }
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
            if (!CanUpgrade)
                throw new InvalidOperationException("Buildable object is maxed or Town Hall level too low.");

            Console.WriteLine("started construction of building");
            var startTime = DateTimeConverter.UnixUtcNow;
            var buildData = NextUpgrade;
            var buildTime = GetBuildTime(buildData);
            if (buildTime == InstantConstructionTime)
            {
                Console.WriteLine("construction instant");
                DoConstructionFinished();
            }
            else
            {
                var endTime = startTime + (int)buildTime.TotalSeconds;
                ConstructionTEndUnixTimestamp = endTime;
            }
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

        ///<summary>Determines if the Buildable can upgraded based on the townhall level required</summary>
        protected abstract bool CanUpgradeCheckTownHallLevel();

        private void DoConstructionFinished()
        {
            Console.WriteLine("construction finished");
            var endTime = DateTime.UtcNow;

            _level++;
            UpdateCanUpgade();
            ConstructionTEndUnixTimestamp = default(int);
            OnConstructionFinished(new ConstructionFinishedEventArgs<TCsvData>()
            {
                EndTime = endTime,
                BuildableConstructed = this,
                UserToken = UserToken,
                WasCancelled = false
            });
        }

        // Returns the BuildTime of the specified data.
        internal abstract TimeSpan GetBuildTime(TCsvData data);

        // Returns the Town Hall level at which the Buildable can upgrade from the specified data.
        internal abstract int GetTownHallLevel(TCsvData data);

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

            // Can be null when lvl is -1
            if (lvl == NotConstructedLevel)
            {
                _data = null;
            }
            else
            {
                _data = CollectionCache[lvl];
                if (_data == null)
                    throw new InvalidOperationException("Could not find CsvData with ID '" + dataId + "' and with level '" + lvl + "'.");
            }
        }

        // Updates CanUpgrade's value based on next upgrade and townhall of Village.
        // This method will also set the NextUpgrade property.
        internal void UpdateCanUpgade()
        {
            //NOTE: CollectionCache can be null, if we change Data to a data with a different Data ID.
            Debug.Assert(CollectionCache != null, "CollectionCache was null.");

            NextUpgrade = CollectionCache[_level + 1];
            if (NextUpgrade == null)
            {
                // There are no upgrades left.
                _canUpgrade = false;
            }
            else
            {
                // Check if the level of the Buildable suits the
                // TownHallLevel required.
                _canUpgrade = CheckTownHallLevel();
            }
        }

        private bool CheckTownHallLevel()
        {
            Debug.Assert(NextUpgrade != null, "NextUpgrade was null.");

            var thLevel = GetTownHallLevel(NextUpgrade);
            if (Village.TownHall == null)
                throw new InvalidOperationException("Village does not contain a Town Hall.");

            if (Village.TownHall.Level >= thLevel - 1 || thLevel == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        internal override void ResetVillageObject()
        {
            base.ResetVillageObject();

            _data = default(TCsvData);
            _canUpgrade = default(bool);
            CollectionCache = default(CsvDataSubCollection<TCsvData>);
            UserToken = default(object);
            ConstructionTEndUnixTimestamp = default(int);
        }

        internal override void Tick(int tick)
        {
            if (ConstructionTEndUnixTimestamp != default(int))
            {
                var diff = DateTimeConverter.UnixUtcNow - ConstructionTEndUnixTimestamp;
                if (diff >= 0)
                {
                    // Construction done!
                    ConstructionTEndUnixTimestamp = default(int);
                    DoConstructionFinished();
                }
            }
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
