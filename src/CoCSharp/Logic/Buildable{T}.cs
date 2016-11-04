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

        /// <summary>
        /// <see cref="TimeSpan"/> representing an instant construction time.
        /// </summary>
        protected static readonly TimeSpan InstantConstructionTime = new TimeSpan(0);
        #endregion

        #region Constructors
        // Constructor used to load the VillageObject from a JsonTextReader.
        internal Buildable() : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Buildable{TCsvData}"/> class with the specified
        /// <see cref="Village"/> instance and <typeparamref name="TCsvData"/>.
        /// </summary>
        /// 
        /// <param name="village">
        /// <see cref="Village"/> instance which owns this <see cref="Buildable{TCsvData}"/>.
        /// </param>
        /// 
        /// <param name="data"><typeparamref name="TCsvData"/> representing the data of the <see cref="Buildable{TCsvData}"/>.</param>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="village"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        protected Buildable(Village village, TCsvData data) : base(village, data)
        {
            _upgradeLevel = -1;
        }
        #endregion

        #region Fields & Properties
        private TCsvData _nextUprade;
        private bool _isUpgradable;
        private int _upgradeLevel;

        // Timer to time constructions.
        private TickTimer _timer = new TickTimer();
        private CsvDataRow<TCsvData> _rowCache;

        /// <summary>
        /// Gets the cache to the <see cref="CsvDataRow{TCsvData}"/> in which <see cref="Data"/> is found.
        /// </summary>
        protected CsvDataRow<TCsvData> RowCache => _rowCache;

        /// <summary>
        /// Gets a value indicating whether the <see cref="Buildable{TCsvData}"/> object is in construction.
        /// </summary>
        public bool IsConstructing => _timer.IsStarted;

        /// <summary>
        /// Gets a value indicating whether the <see cref="Buildable{TCsvData}"/> object can be upgraded.
        /// </summary>
        public bool IsUpgradable => _isUpgradable;

        /// <summary>
        /// Gets the level of the <see cref="Buildable{TCsvData}"/> object.
        /// </summary>
        public int Level
        {
            get
            {
                return _upgradeLevel;
            }
            set
            {
                if (value < NotConstructedLevel)
                    throw new ArgumentOutOfRangeException(nameof(value), "value cannot be less than NotConstructedLevel, that is, level -1.");

                _upgradeLevel = value;

                // If level is -1 (NotConstructedLevel), VillageObject.Data will be null.
                // and NextUpgrade will point to a CsvData of level 0 from CollectionCache.
                UpdateData(RowCache.ID, value);
                UpdateIsUpgradable();
            }
        }

        /// <summary>
        /// Gets the next upgrade's <typeparamref name="TCsvData"/>.
        /// </summary>
        /// 
        /// <remarks>
        /// All construction data will be taken from this. 
        /// </remarks>
        public TCsvData NextUpgrade => _nextUprade;

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
        protected int ConstructionTSeconds => _timer.Duration;

        /// <summary>
        /// Gets or sets the date of when the construction is going to end in UNIX timestamps. 
        /// Everything is relative to this, changing this will also change the other values.
        /// </summary>
        /// 
        /// <remarks>
        /// Represents the "const_t_end" field of village JSONs.
        /// </remarks>
        protected int ConstructionTEndUnixTimestamp => IsConstructing ? _timer.EndTime : 0;
        #endregion

        #region Methods
        /// <summary>
        /// Begins the construction of the <see cref="Buildable{TCsvData}"/> and increases its level by 1
        /// when done.
        /// </summary>
        public void BeginConstruction(int tick)
        {
            if (IsConstructing)
                throw new InvalidOperationException("Buildable object is already in construction.");
            if (!IsUpgradable)
                throw new InvalidOperationException("Buildable object is maxed or Town Hall level too low.");

            Debug.Assert(NextUpgrade != null, "NextUpgrade was null, when trying to BeginConstruciton.");

            var buildTime = GetBuildTime(NextUpgrade);
        }

        /// <summary>
        /// Cancels the construction of the <see cref="Buildable{TCsvData}"/>.
        /// </summary>
        public void CancelConstruction(int tick)
        {

        }

        /// <summary>
        /// Speeds up the construction of the <see cref="Buildable{TCsvData}"/> and finishes the construction instantly
        /// and increases its level by 1.
        /// </summary>
        public void SpeedUpConstruction(int tick)
        {

        }

        // Called when construction has finished.
        private void FinishConstruction(int tick)
        {
            _timer.Stop();

            //var tick = Village.Tick;
            var endTime = DateTime.UtcNow;

            _upgradeLevel++;
            _data = NextUpgrade;

            // Calling UpdateCanUpgrade will set the CanUpgrade & NextUpgrade property as well.
            UpdateIsUpgradable();
        }

        /// <summary>
        /// Returns the BuildTime of the specified data.
        /// </summary>
        /// <param name="data"><typeparamref name="TCsvData"/> from which to obtain the BuildTime.</param>
        protected abstract TimeSpan GetBuildTime(TCsvData data);

        /// <summary>
        /// Returns the Town Hall level at which the Buildable can upgrade from the specified data.
        /// </summary>
        /// <param name="data"><typeparamref name="TCsvData"/> from which to obtain the TownHallLevel.</param>
        protected abstract int GetTownHallLevel(TCsvData data);

        /// <summary>
        /// Updates the <see cref="VillageObject{TCsvData}.Data"/> associated with this <see cref="Buildable{TCsvData}"/> using
        /// <see cref="VillageObject.Assets"/>, the specified data ID and level.
        /// </summary>
        /// <param name="dataId"></param>
        /// <param name="level"></param>
        protected virtual void UpdateData(int dataId, int level)
        {
            Debug.Assert(level >= NotConstructedLevel, "Level was less than NotConstructedLevel.");

            // If we haven't cached the CsvDataRow in which Data
            // is found, we do it.
            if (RowCache == null)
            {
                var tableCollections = Assets.Get<CsvDataTableCollection>();
                var dataRef = new CsvDataRowRef<TCsvData>(dataId);
                var row = dataRef.Get(tableCollections);
                if (row == null)
                    throw new InvalidOperationException("Could not find CsvDataRow with ID '" + dataId + "'.");

                _rowCache = row;
            }

            // Data is null when lvl is -1
            // However NextUpgrade should not.
            if (level == NotConstructedLevel)
            {
                _data = null;
            }
            else
            {
                _data = RowCache[level];
                if (_data == null)
                    throw new InvalidOperationException("Could not find CsvData with ID '" + dataId + "' and with level '" + level + "'.");
            }
        }

        /// <summary>
        /// Update the <see cref="IsUpgradable"/> field by taking into consideration the town hall
        /// level required to do so.
        /// </summary>
        protected virtual void UpdateIsUpgradable()
        {
            // Can be null sometimes due to concurrency.
            Debug.Assert(RowCache != null, "RowCache was null.");

            if (RowCache == null)
                UpdateData(Data.ID, _upgradeLevel);

            _nextUprade = RowCache[_upgradeLevel + 1];
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

        /// <summary/>
        protected internal override void ResetVillageObject()
        {
            base.ResetVillageObject();

            _timer.Stop();
            _nextUprade = default(TCsvData);
            _isUpgradable = default(bool);
            _rowCache = default(CsvDataRow<TCsvData>);
        }

        /// <summary/>
        protected internal override void Tick(int tick)
        {
            // Check if the construction Timer has completed.
            if (_timer.IsCompleted(tick))
                FinishConstruction(tick);
        }
        #endregion
    }
}
