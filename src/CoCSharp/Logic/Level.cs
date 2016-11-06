using CoCSharp.Data;
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
        /// Initializes a new instance of the <see cref="Level"/> class.
        /// </summary>
        public Level(AssetManager assets)
        {
            if (assets == null)
                throw new ArgumentNullException(nameof(assets));

            _lastTick = DateTime.UtcNow;

            _assets = assets;
            //_commands = new Queue<Command>();
            Avatar = new Avatar(this);
        }
        #endregion

        #region Fields & Properties
        private bool _disposed;

        private int _lastTickValue;
        // Time of when the last time the Level was ticked.
        private DateTime _lastTick;
        // Queue of commands to be processed.
        //private readonly Queue<Command> _commands;
        private readonly AssetManager _assets;

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
        /// Gets or sets the <see cref="Logic.Village"/> associated with the <see cref="Level"/>.
        /// </summary>
        public Village Village { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Logic.Avatar"/> associated with the <see cref="Level"/>.
        /// </summary>
        public Avatar Avatar { get; set; }

        /// <summary>
        /// Gets the <see cref="LastTickValue"/> of the <see cref="Level"/>.
        /// </summary>
        public int LastTickValue => _lastTickValue;

        /// <summary>
        /// Gets the time of when the <see cref="Level"/> was last ticked.
        /// </summary>
        public DateTime LastTick => _lastTick;

        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> of when the <see cref="Level"/> was last saved.
        /// </summary>
        public DateTime LastSave { get; set; }

        /// <summary>
        /// Gets a new instance of <see cref="OwnHomeDataMessage"/> representing this instance.
        /// </summary>
        public OwnHomeDataMessage OwnHomeData
        {
            get
            {
                var now = DateTime.UtcNow;
                var villageData = new VillageMessageComponent(this);
                var avatarData = new AvatarMessageComponent(this);
                var ohdMessage = new OwnHomeDataMessage
                {
                    LastVisit = now - LastSave,
                    Timestamp = now,

                    OwnVillageData = villageData,
                    OwnAvatarData = avatarData,

                    Unkonwn4 = 1462629754000,
                    Unknown5 = 1462629754000,
                    Unknown6 = 1462631554000,
                };

                return ohdMessage;
            }
        }

        /// <summary>
        /// Gets a new instance of the <see cref="EnemyHomeDataMessage"/> representing this instance.
        /// </summary>
        public EnemyHomeDataMessage EnemyHomeData
        {
            get
            {
                var now = DateTime.UtcNow;
                var villageData = new VillageMessageComponent(this);
                var avatarData = new AvatarMessageComponent(this);
                var ehdMessage = new EnemyHomeDataMessage
                {
                    LastVisit = now - LastSave,
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
        public VisitHomeDataMessage VisitHomeData
        {
            get
            {
                var now = DateTime.UtcNow;
                var villageData = new VillageMessageComponent(this);
                var avatarData = new AvatarMessageComponent(this);
                var vhdMessage = new VisitHomeDataMessage
                {
                    LastVisit = now - LastSave,

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
                    VillageJson = Village.ToJson(),
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
            _lastTick = DateTime.UtcNow;
            _lastTickValue = tick;

            Village.LastTick = _lastTick;
            Village.Update(tick);
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="Level"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(false);
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
                // And of course, cause really nice memory leaks.
                Village?.Dispose();
            }

            _disposed = true;
        }
        #endregion
    }
}
