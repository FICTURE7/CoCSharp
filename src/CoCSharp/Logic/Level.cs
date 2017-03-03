using CoCSharp.Csv;
using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Network.Messages;
using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a level.
    /// </summary>
    public class Level : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Level"/> class with the specified
        /// <see cref="AssetManager"/> from which data and other resources will be retrieved.
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="assets"/> is null.</exception>
        public Level(AssetManager assets)
        {
            if (assets == null)
                throw new ArgumentNullException(nameof(assets));

            if (!assets.LockMode.HasFlag(AssetManagerLockMode.Unloading))
                throw new ArgumentException("Unloading of assets must be locked.");

            // Make sure the AssetManager specified has loaded all the
            // required assets.
            if (!assets.IsLoaded<CsvDataTable<BuildingData>>() ||
                !assets.IsLoaded<CsvDataTable<ObstacleData>>() ||
                !assets.IsLoaded<CsvDataTable<DecorationData>>() ||
                !assets.IsLoaded<CsvDataTable<TrapData>>() ||
                !assets.IsLoaded<CsvDataTable<GlobalData>>() ||
                !assets.IsLoaded<CsvDataTable<CharacterData>>() ||
                !assets.IsLoaded<CsvDataTable<ExperienceLevelData>>() ||
                !assets.IsLoaded<CsvDataTable<ResourceData>>())
                throw new ArgumentException("Specified AssetManager has not loaded all the required assets.");

            _assets = assets;

            _logs = new LevelLog(this);
            _dateInit = DateTime.UtcNow;
            _lastTick = DateTime.UtcNow;
            Avatar = new Avatar(this);
        }
        #endregion

        #region Fields & Properties
        private bool _disposed;

        // Logger which is going to log information about the level.
        private readonly LevelLog _logs;

        private TimeSpan _playTime;
        // The tick value of the last time the Level was ticked.
        private int _lastTickValue;
        // Time of when the last time the Level was ticked.
        private DateTime _lastTick;
        // Time of when the level instance was initialized.
        private DateTime _dateInit;

        // The AssetManager that is going to provide the CsvDataTables and other
        // assets.
        private AssetManager _assets;

        /// <summary>
        /// Gets the <see cref="AssetManager"/> from which all data will be fetched.
        /// </summary>
        public AssetManager Assets => _assets;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Level"/> is owned
        /// by an NPC.
        /// </summary>
        public bool IsNpc { get; set; }

        /// <summary>
        /// Gets or sets the token of the <see cref="Level"/>.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Logic.Village"/> associated with the <see cref="Level"/>.
        /// </summary>
        public Village Village { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Logic.Avatar"/> associated with the <see cref="Level"/>.
        /// </summary>
        public Avatar Avatar { get; set; }

        /// <summary>
        /// Gets the <see cref="LevelLog"/> associated with the <see cref="Level"/>.
        /// </summary>
        public LevelLog Logs => _logs;

        /// <summary>
        /// Gets the <see cref="LastTickValue"/> of the <see cref="Level"/>.
        /// </summary>
        public int LastTickValue => _lastTickValue;

        /// <summary>
        /// Gets or sets the state of the <see cref="Level"/>.
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Gets the time of when the <see cref="Level"/> was last ticked.
        /// </summary>
        public DateTime LastTickTime => _lastTick;

        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> of when the <see cref="Level"/> was last saved.
        /// </summary>
        public DateTime DateLastSave { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> of when the <see cref="Level"/> was created.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the amount of time the <see cref="Level"/> was active.
        /// </summary>
        public TimeSpan PlayTime
        {
            get
            {
                return _playTime + (DateTime.UtcNow - _dateInit);
            }
            set
            {
                _dateInit = DateTime.UtcNow;
                _playTime = value;
            }
        }

        /// <summary>
        /// Gets or sets the amount of time the <see cref="Level"/> was logged in.
        /// </summary>
        public int LoginCount { get; set; }

        /// <summary>
        /// Gets a new instance of <see cref="OwnHomeDataMessage"/> representing this instance.
        /// </summary>
        public OwnHomeDataMessage OwnHomeData
        {
            get
            {
                var now = DateTime.UtcNow;
                var villageData = new VillageMessageComponent(this)
                {
                    EventJson = "{\"event\":[]}"
                };
                var avatarData = new AvatarMessageComponent(this);
                var ohdMessage = new OwnHomeDataMessage
                {
                    LastVisit = now - DateLastSave,
                    Timestamp = now,

                    OwnVillageData = villageData,
                    OwnAvatarData = avatarData,

                    Unkonwn4 = 1482421076000,
                    Unknown5 = 1482421076000,
                    Unknown6 = 1482421076000,
                };

                //var ohdMessage = InternalUtils.ReadMessageAt<OwnHomeDataMessage>("ohd2.bin");

                return ohdMessage;
            }
        }

        /// <summary>
        /// Gets a new instance of the <see cref="EnemyHomeDataMessage"/> representing this instance.
        /// </summary>
        /// 
        /// <remarks>
        /// <see cref="EnemyHomeDataMessage.OwnAvatarData"/> must be set on the returning <see cref="EnemyHomeDataMessage"/> instance
        /// as the <see cref="AvatarMessageComponent"/> of the client that requested it.
        /// </remarks>
        public EnemyHomeDataMessage EnemyHomeData
        {
            get
            {
                var now = DateTime.UtcNow;
                var villageData = new VillageMessageComponent(this)
                {
                    EventJson = "{\"event\":[]}"
                };
                var avatarData = new AvatarMessageComponent(this);
                var ehdMessage = new EnemyHomeDataMessage
                {
                    LastVisit = now - DateLastSave,
                    Timestamp = now,

                    EnemyVillageData = villageData,
                    EnemyAvatarData = avatarData,

                    Unknown2 = 3
                };

                return ehdMessage;
            }
        }

        /// <summary>
        /// Gets a new instance of the <see cref="VisitHomeDataMessage"/> representing this instance.
        /// </summary>
        /// 
        /// <remarks>
        /// <see cref="VisitHomeDataMessage.OwnAvatarData"/> must be set on the returning <see cref="VisitHomeDataMessage"/> instance
        /// as the <see cref="AvatarMessageComponent"/> of the client that requested it.
        /// </remarks>
        public VisitHomeDataMessage VisitHomeData
        {
            get
            {
                var now = DateTime.UtcNow;
                var villageData = new VillageMessageComponent(this)
                {
                    EventJson = "{\"event\":[]}"
                };
                var avatarData = new AvatarMessageComponent(this);
                var vhdMessage = new VisitHomeDataMessage
                {
                    LastVisit = now - DateLastSave,
                    Timestamp = now,

                    VisitVillageData = villageData,
                    VisitAvatarData = avatarData,

                    Unknown2 = 1
                };
                return vhdMessage;
            }
        }

        /// <summary>
        /// Gets a new instance of the <see cref="AvatarProfileResponseMessage"/> representing this instance.
        /// </summary>
        public AvatarProfileResponseMessage AvatarProfileResponse
        {
            get
            {
                var aprMessage = new AvatarProfileResponseMessage
                {
                    AvatarData = new AvatarMessageComponent(this),
                    VillageJson = Village.ToJson()
                };
                return aprMessage;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Ticks all <see cref="VillageObject"/> in the <see cref="Level"/>.
        /// </summary>
        public void Tick(int tick)
        {
            // In case the level is being processed by more than 1 thread.
            lock (Village)
            {
                _lastTick = DateTime.UtcNow;
                _lastTickValue = tick;

                Village.LastTickTime = _lastTick;
                Village.Update(tick);
            }
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="Level"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and optionally unmanaged resources used by the current 
        /// instance of the <see cref="Level"/> class.
        /// </summary>
        /// <param name="disposing">Releases managed resources if set to <c>true</c>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Disposal of the Village will push back the VillageObjects to the VillageObjectPool.
                // And of course, cause really nice memory leaks. Nice!
                Village?.Dispose();
            }

            _disposed = true;
        }
        #endregion
    }
}
