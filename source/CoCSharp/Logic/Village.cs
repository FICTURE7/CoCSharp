using CoCSharp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans village.
    /// </summary>
    public partial class Village : IDisposable
    {
        #region Constants
        /// <summary>
        /// Represents the width of a <see cref="Village"/> layout.
        /// </summary>
        public const int Width = 48;

        /// <summary>
        /// Represents the height of a <see cref="Village"/> layout.
        /// </summary>
        public const int Height = 48;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Village"/> class and uses <see cref="AssetManager.Default"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><see cref="AssetManager.Default"/> is null.</exception>
        public Village() : this(AssetManager.Default, true)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Village"/> class with the specified
        /// <see cref="Data.AssetManager"/>.
        /// </summary>
        /// <param name="manager"><see cref="Data.AssetManager"/> to use.</param>
        /// <exception cref="ArgumentNullException"><paramref name="manager"/> is null.</exception>
        public Village(AssetManager manager) : this(manager, true)
        {
            // Space
        }

        private Village(AssetManager manager, bool register)
        {
            if (manager == null)
                throw new ArgumentNullException("manager");

            _assetManager = manager;
            _villageObjects = new VillageObjectCollection();
            _logger = new VillageLogger();

            if (register)
                VillageTicker.Register(this);
        }
        #endregion

        #region Fields & Properties 
        private bool _disposed;
        // Unix timestamp at which the village was registered to the ticker.
        internal int _registeredTime;

        // VillageLogger which will log logic stuff happening.
        private readonly VillageLogger _logger;
        internal VillageLogger Logger
        {
            get
            {
                return _logger;
            }
        }

        /// <summary>
        /// The event raised when a logic action has taken place.
        /// </summary>
        public event EventHandler<LogicEventArgs> Logic;

        private readonly AssetManager _assetManager;
        /// <summary>
        /// Gets the <see cref="AssetManager"/> from which data will be
        /// used.
        /// </summary>
        public AssetManager AssetManager
        {
            get
            {
                return _assetManager;
            }
        }

        /// <summary>
        /// Gets or sets the experience version? (Not completely sure if thats its full name).
        /// </summary>
        /// 
        /// <remarks>
        /// I don't know what this is needed for but I found it in the 8.x.x update
        /// and the client needs it when there is a "loot_multiplier_ver" in an Obstacle object; it crashes
        /// if it does not find it.
        /// </remarks>
        public int ExperienceVersion { get; set; }

        internal int _tick;
        /// <summary>
        /// Gets the current tick the <see cref="Village"/> is in.
        /// </summary>
        public int Tick
        {
            get
            {
                return Thread.VolatileRead(ref _tick);
            }
            set
            {
                Thread.VolatileWrite(ref _tick, value);
            }
        }

        private readonly VillageObjectCollection _villageObjects;
        /// <summary>
        /// Gets the <see cref="VillageObjectCollection"/> which contains all the <see cref="VillageObject"/> in
        /// this <see cref="Village"/>.
        /// </summary>
        public VillageObjectCollection VillageObjects
        {
            get
            {
                return _villageObjects;
            }
        }

        /// <summary>
        /// Gets an enumerator that iterates through the <see cref="Building"/> objects in the <see cref="Village"/>.
        /// </summary>
        public IEnumerable<Building> Buildings
        {
            get
            {
                return _villageObjects.GetRow(Building.Kind).Select(k => (Building)k);
            }
        }

        /// <summary>
        /// Gets an enumerator that iterates through the <see cref="Obstacle"/> objects in the <see cref="Village"/>.
        /// </summary>
        public IEnumerable<Obstacle> Obstacles
        {
            get
            {
                return _villageObjects.GetRow(Obstacle.Kind).Select(k => (Obstacle)k);
            }
        }

        /// <summary>
        /// Gets an enumerator that iterates through <see cref="Trap"/> objects in the <see cref="Village"/>.
        /// </summary>
        public IEnumerable<Trap> Traps
        {
            get
            {
                return _villageObjects.GetRow(Trap.Kind).Select(k => (Trap)k);
            }
        }

        /// <summary>
        /// Gets an enumerator that iterates through the <see cref="Decoration"/> objects in the <see cref="Village"/>.
        /// </summary>
        public IEnumerable<Decoration> Decorations
        {
            get
            {
                return _villageObjects.GetRow(Decoration.Kind).Select(k => (Decoration)k);
            }
        }

        internal Building _townhall;
        /// <summary>
        /// Gets the TownHall <see cref="Building"/> of the <see cref="Village"/>; 
        /// returns <c>null</c> if there is no TownHall in the <see cref="Village"/>.
        /// </summary>
        /// 
        /// <remarks>
        /// Only one TownHall building is allowed in a Village. 
        /// </remarks>
        public Building TownHall
        {
            get
            {
                return _townhall;
            }
        }
        #endregion

        #region Methods
        // Call the Tick method in all the VillageObject contained in this village. 
        internal void Update()
        {
            // Exit out of the Update function gently.
            if (_disposed)
                return;

            _villageObjects.TickAll(_tick);
        }

        /// <summary>
        /// Releases all resources used by this <see cref="Village"/> instance.
        /// </summary>
        /// 
        /// <remarks>
        /// All the <see cref="VillageObject"/> in the <see cref="Village"/> will be pushed
        /// to an internal pool, <see cref="VillageObjectPool"/>.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases all resources used by this <see cref="Village"/> instance.
        /// </summary>
        /// <param name="disposing">Value indicating if it should dispose managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                VillageTicker.Unregister(this);

                foreach (var building in Buildings)
                    building.PushToPool();
                foreach (var obstacle in Obstacles)
                    obstacle.PushToPool();
                foreach (var trap in Traps)
                    trap.PushToPool();
                foreach (var deco in Decorations)
                    deco.PushToPool();

                _logger.Dump();
            }

            _disposed = true;
        }

        internal void OnLogic(LogicEventArgs args)
        {
            if (Logic != null)
                Logic(this, args);
        }
        #endregion
    }
}
