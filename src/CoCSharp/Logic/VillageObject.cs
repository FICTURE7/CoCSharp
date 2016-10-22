using CoCSharp.Data;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents an object in a <see cref="Logic.Village"/>.
    /// </summary>
    [DebuggerDisplay("ID = {ID}")]
    public abstract class VillageObject : INotifyPropertyChanged
    {
        #region Constants
        // Represents the Base ID of every game ID & data ID.
        //internal const int Base = 1000000;

        internal static int s_rested = 0;
        internal static int s_created = 0;

        private static readonly PropertyChangedEventArgs s_xChanged = new PropertyChangedEventArgs("X");
        private static readonly PropertyChangedEventArgs s_yChanged = new PropertyChangedEventArgs("Y");
        #endregion

        #region Constructors
        internal VillageObject(Village village)
        {
            if (village == null)
                throw new ArgumentNullException("village");

            _components = new LogicComponent[8];
            SetVillageInternal(village);
            Interlocked.Increment(ref s_created);

            Village.Logger.Info(village.Tick, "created new VillageObject {0}", ID);
        }

        internal VillageObject(Village village, int x, int y) : this(village)
        {
            X = x;
            Y = y;
        }
        #endregion

        #region Fields & Properties
        // X coordinate of object.
        private int _x;
        // Y coordinate of object.
        private int _y;
        // Village in which the VillageObject is in.
        private Village _village;

        // Array containing the LogicComponent attached to this VillageObject.
        private readonly LogicComponent[] _components;
        // Amount of time the VillageObject was pushed back to the pool.
        private int _count;
        // Amount of time the VillageObject was reused.
        internal int _recycled;

        /// <summary>
        /// The event raised when a property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether to raise the <see cref="PropertyChanged"/> event.
        /// </summary>
        public bool IsPropertyChangedEnabled { get; set; }

        /// <summary>
        /// Gets the <see cref="Logic.Village"/> in which the current <see cref="VillageObject"/> is in.
        /// </summary>
        public Village Village => _village;

        internal int _columnIndex;
        // ID of the VillageObject (IDs above or equal to 500000000). E.g: 500000001
        // This value should be relative to the Village the VillageObject is in.

        /// <summary>
        /// Gets the game ID of the <see cref="VillageObject"/>.
        /// </summary>
        /// 
        /// <remarks>
        /// The value of <see cref="ID"/> is dependent on its <see cref="VillageObject"/> kind
        /// and on <see cref="Village.VillageObjects"/> collection.
        /// </remarks>
        public int ID
        {
            get
            {
                return Thread.VolatileRead(ref _columnIndex) + ((500 + KindID) * InternalConstants.IDBase);
            }
        }

        /// <summary>
        /// Gets or sets the X coordinate of the <see cref="VillageObject"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is not between 0 and <see cref="Village.Width"/>.</exception>
        public int X
        {
            get
            {
                return _x;
            }
            set
            {
                if (value < 0 || value > Village.Width)
                    throw new ArgumentOutOfRangeException("value", "value must be between 0 and Village.Width.");

                if (_x == value)
                    return;

                _x = value;
                OnPropertyChanged(s_xChanged);
            }
        }

        /// <summary>
        /// Gets or sets the y coordinate of the <see cref="VillageObject"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is not between 0 and <see cref="Village.Height"/>.</exception>
        public int Y
        {
            get
            {
                return _y;
            }
            set
            {
                if (value < 0 || value > Village.Height)
                    throw new ArgumentOutOfRangeException("value", "value must be between 0 and Village.Height.");

                if (_y == value)
                    return;

                _y = value;
                OnPropertyChanged(s_yChanged);
            }
        }

        /// <summary>
        /// Gets the <see cref="Data.AssetManager"/> of <see cref="Village"/>.
        /// </summary>
        protected AssetManager Assets => _village.AssetManager;

         // Kind ID of the VillageObject.
        internal abstract int KindID { get; }
        #endregion

        #region Methods
        #region Component System
        // Adds the specified component to the VillageObject.
        internal void AddComponent(LogicComponent component)
        {
            component._vilObject = this;
            _components[component.ComponentID] = component;
        }

        // Removes the component of the specified type from the VillageObject.
        // NOTE: This thing should not be called in normal cases. Unless pushing back to pool.
        internal bool RemoveComponent(int componentId)
        {
            var remove = _components[componentId] != null;
            if (remove)
            {
                LogicComponentPool.Push(_components[componentId]);
                _components[componentId] = null;
            }
            return remove;
        }

        // Returns the LogicComponent with the specified component ID.
        internal LogicComponent GetComponent(int componentId)
        {
            return _components[componentId];
        }

        /// <summary>
        /// Gets the instance of the specified <typeparamref name="TLogicComponent"/> attached to this
        /// <see cref="VillageObject"/>; returns null if no instance of the specified <typeparamref name="TLogicComponent"/>
        /// was found.
        /// </summary>
        /// 
        /// <typeparam name="TLogicComponent">Type of <see cref="LogicComponent"/> to retrieve.</typeparam>
        /// <returns>
        /// Instance of the specified <typeparamref name="TLogicComponent"/>; returns null if no instance of the 
        /// specified <typeparamref name="TLogicComponent"/> was found.
        /// </returns>
        public TLogicComponent GetComponent<TLogicComponent>() where TLogicComponent : LogicComponent
        {
            var type = typeof(TLogicComponent);
            for (int i = 0; i < _components.Length; i++)
            {
                if (_components[i] != null && _components[i].GetType() == type)
                    return (TLogicComponent)_components[i];
            }

            return null;
        }
        #endregion

        ///<summary>Raises the PropertyChanged event.</summary>
        protected void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null && IsPropertyChangedEnabled)
                PropertyChanged(this, args);
        }

        // Resets the VillageObject so that it can be reused.
        internal virtual void ResetVillageObject()
        {
            Interlocked.Increment(ref s_rested);
            _village = default(Village);
            _columnIndex = default(int);
            _x = default(int);
            _y = default(int);
        }

        internal virtual void Tick(int tick)
        {
            // Space
        }

        // Writes the current VillageObject to the JsonWriter.
        internal abstract void ToJsonWriter(JsonWriter writer);

        // Reads the current VillageObject to the JsonReader.
        internal abstract void FromJsonReader(JsonReader reader);

        // Pushes the VillageObject to the VillageObjectPool where it will
        // be reused when Village.FromJson is called.
        internal void PushToPool()
        {
            // Set village to null, so it can get picked up by the GC.
            _village = null;
            VillageObjectPool.Push(this);
            _count++;
        }

        internal void SetVillageInternal(Village village)
        {
            _village = village;
            _village.VillageObjects.Add(this);
        }
        #endregion
    }
}
