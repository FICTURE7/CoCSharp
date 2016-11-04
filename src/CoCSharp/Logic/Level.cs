using CoCSharp.Network.Messages;
using System;
using System.Collections.Generic;

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
        public Level()
        {
            _lastTick = DateTime.UtcNow;

            _commands = new Queue<Command>();
            Avatar = new Avatar();
        }
        #endregion

        #region Fields & Properties
        private bool _disposed;

        // Time of when the last time the Level was ticked.
        private DateTime _lastTick;
        // Queue of commands to be processed.
        private readonly Queue<Command> _commands;

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
        /// Gets the time of when the <see cref="Level"/> was last ticked.
        /// </summary>
        public DateTime LastTick => _lastTick;

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
                    LastVisit = now - _lastTick,
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
                throw new NotImplementedException();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Ticks all <see cref="VillageObject"/> in the <see cref="Level"/>.
        /// </summary>
        public void Tick()
        {
            Village.Update();

            _lastTick = DateTime.UtcNow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indent"></param>
        /// <returns></returns>
        public string ToJson(bool indent)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Level FromJson(string json)
        {
            return null;
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
