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
        public Village() : this(AssetManager.Default)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Village"/> class with the specified
        /// <see cref="AssetManager"/>.
        /// </summary>
        /// <param name="assets"><see cref="Data.AssetManager"/> to use.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assets"/> is null.</exception>
        public Village(AssetManager assets)
        {
            if (assets == null)
                throw new ArgumentNullException(nameof(assets));

            _assets = assets;
            _villageObjects = new VillageObjectCollection();
        }
        #endregion

        #region Fields & Properties 
        private bool _disposed;

        // Building which is the town hall.
        internal Building _townhall;

        private readonly AssetManager _assets;
        private readonly VillageObjectCollection _villageObjects;

        /// <summary>
        /// The event raised when a logic action has taken place.
        /// </summary>
        public event EventHandler<LogicEventArgs> Logic;

        /// <summary>
        /// Gets the <see cref="Assets"/> from which data will be
        /// used.
        /// </summary>
        public AssetManager Assets => _assets;

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

        /// <summary>
        /// Gets the <see cref="VillageObjectCollection"/> which contains all the <see cref="VillageObject"/> in
        /// this <see cref="Village"/>.
        /// </summary>
        public VillageObjectCollection VillageObjects => _villageObjects;

        /// <summary>
        /// Gets an enumerator that iterates through the <see cref="Building"/> objects in the <see cref="Village"/>.
        /// </summary>
        public IEnumerable<Building> Buildings => _villageObjects.GetRow(Building.Kind).Select(k => (Building)k);
        /// <summary>
        /// Gets an enumerator that iterates through the <see cref="Obstacle"/> objects in the <see cref="Village"/>.
        /// </summary>
        public IEnumerable<Obstacle> Obstacles => _villageObjects.GetRow(Obstacle.Kind).Select(k => (Obstacle)k);

        /// <summary>
        /// Gets an enumerator that iterates through <see cref="Trap"/> objects in the <see cref="Village"/>.
        /// </summary>
        public IEnumerable<Trap> Traps => _villageObjects.GetRow(Trap.Kind).Select(k => (Trap)k);

        /// <summary>
        /// Gets an enumerator that iterates through the <see cref="Decoration"/> objects in the <see cref="Village"/>.
        /// </summary>
        public IEnumerable<Decoration> Decorations => _villageObjects.GetRow(Decoration.Kind).Select(k => (Decoration)k);

        /// <summary>
        /// Gets the TownHall <see cref="Building"/> of the <see cref="Village"/>; 
        /// returns <c>null</c> if there is no TownHall in the <see cref="Village"/>.
        /// </summary>
        /// 
        /// <remarks>
        /// Only one TownHall building is allowed in a Village. 
        /// </remarks>
        public Building TownHall => _townhall;
        #endregion

        #region Methods
        /// <summary>
        /// Updates all <see cref="VillageObject"/> in the <see cref="Village"/>.
        /// </summary>
        public void Update()
        {
            // Exit out of the Update function gently.
            if (_disposed)
                return;

            _villageObjects.TickAll(1);
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
                //TODO: Limit the pool size because we don't want to have a memory leak.

                foreach (var building in Buildings)
                    building.PushToPool();
                foreach (var obstacle in Obstacles)
                    obstacle.PushToPool();
                foreach (var trap in Traps)
                    trap.PushToPool();
                foreach (var deco in Decorations)
                    deco.PushToPool();
            }

            _disposed = true;
        }

        /// <summary>
        /// Raises the <see cref="Logic"/> event with the specified <see cref="LogicEventArgs"/>.
        /// </summary>
        /// <param name="args"></param>
        protected internal virtual void OnLogic(LogicEventArgs args)
        {
            Logic?.Invoke(this, args);
        }
        #endregion
    }
}
