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
    [DebuggerDisplay("{DebuggerDisplayString}")]
    public abstract class VillageObject : INotifyPropertyChanged
    {
        #region Constants
        // The size of the LogicComponent array.
        private const int ComponentArraySize = 8;

        internal static int s_totalReseted = 0;
        internal static int s_totalCreated = 0;

        private static readonly PropertyChangedEventArgs s_xChanged = new PropertyChangedEventArgs(nameof(X));
        private static readonly PropertyChangedEventArgs s_yChanged = new PropertyChangedEventArgs(nameof(Y));
        #endregion

        #region Constructors
        // Constructor used to load the VillageObject from a JsonTextReader.
        internal VillageObject()
        {
            Interlocked.Increment(ref s_totalCreated);
            _components = new Component[ComponentArraySize];

            //Debug.WriteLine("Created new instance of VillageObject for JsonReading.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VillageObject"/> class with the specified
        /// <see cref="Logic.Village"/> instance.
        /// </summary>
        /// 
        /// <param name="village">
        /// <see cref="Logic.Village"/> instance which owns this <see cref="VillageObject"/>.
        /// </param>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="village"/> is null.</exception>
        protected VillageObject(Village village)
        {
            if (village == null)
                throw new ArgumentNullException(nameof(village));

            Interlocked.Increment(ref s_totalCreated);

            _components = new Component[ComponentArraySize];
            SetVillageInternal(village);
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
        private readonly Component[] _components;
        // Amount of time the VillageObject was pushed back to the pool.
        private int _pushCount;
        // Amount of time the VillageObject was rest.
        internal int _restCount;
        // Column index of the VillageObject in its VillageObjectCollection.
        internal int _columnIndex;

        /// <summary>
        /// Gets the string which <see cref="DebuggerDisplayAttribute"/> will use.
        /// </summary>
        protected virtual string DebuggerDisplayString => $"ID = {Id}";

        /// <summary>
        /// The event raised when a property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether to raise the <see cref="PropertyChanged"/> event.
        /// </summary>
        public bool IsPropertyChangedEnabled { get; set; }

        /// <summary>
        /// Gets the array of <see cref="Component"/> attached to this <see cref="VillageObject"/>.
        /// </summary>
        protected internal Component[] Components => _components;

        /// <summary>
        /// Gets the <see cref="Logic.Level"/> in which <see cref="Village"/> is in.
        /// </summary>
        public Level Level => _village.Level;

        /// <summary>
        /// Gets the <see cref="Logic.Village"/> in which the current <see cref="VillageObject"/> is in.
        /// </summary>
        public Village Village => _village;

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
        public int Id => Thread.VolatileRead(ref _columnIndex) + ((500 + KindId) * InternalConstants.IdBase);

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
                    throw new ArgumentOutOfRangeException(nameof(value), "value must be non-negative and Village.Width.");

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
                    throw new ArgumentOutOfRangeException(nameof(value), "value must be non-negative and Village.Height.");

                if (_y == value)
                    return;

                _y = value;
                OnPropertyChanged(s_yChanged);
            }
        }

        /// <summary>
        /// Gets the <see cref="AssetManager"/> of <see cref="Level"/>.
        /// </summary>
        protected AssetManager Assets => _village.Assets;

        // Kind ID of the VillageObject.
        internal abstract int KindId { get; }
        #endregion

        #region Methods
        //TODO: Rework component system.
        #region Component System
        // Adds the specified component to the VillageObject.
        internal void AddComponent(Component component)
        {
            component._parent = this;
            _components[component.ComponentID] = component;
        }

        // Removes the component of the specified type from the VillageObject.
        // NOTE: This thing should not be called in normal cases. Unless pushing back to pool.
        internal bool RemoveComponent(int componentId)
        {
            var remove = _components[componentId] != null;
            if (remove)
            {
                ComponentPool.Push(_components[componentId]);
                _components[componentId] = null;
            }
            return remove;
        }

        // Returns the LogicComponent with the specified component ID.
        internal Component GetComponent(int componentId)
        {
            return _components[componentId];
        }

        /// <summary>
        /// Gets the instance of the specified <typeparamref name="TLogicComponent"/> attached to this
        /// <see cref="VillageObject"/>; returns null if no instance of the specified <typeparamref name="TLogicComponent"/>
        /// was found.
        /// </summary>
        /// 
        /// <typeparam name="TLogicComponent">Type of <see cref="Component"/> to retrieve.</typeparam>
        /// <returns>
        /// Instance of the specified <typeparamref name="TLogicComponent"/>; returns null if no instance of the 
        /// specified <typeparamref name="TLogicComponent"/> was found.
        /// </returns>
        public TLogicComponent GetComponent<TLogicComponent>() where TLogicComponent : Component
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

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event with the specified <see cref="PropertyChangedEventArgs"/>.
        /// </summary>
        /// <param name="args"><see cref="PropertyChangedEventArgs"/> to fire the <see cref="PropertyChanged"/> event.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null && IsPropertyChangedEnabled)
                PropertyChanged(this, args);
        }

        /// <summary>
        /// Resets the <see cref="VillageObject"/>'s fields.
        /// </summary>
        protected internal virtual void ResetVillageObject()
        {
            _restCount++;
            Interlocked.Increment(ref s_totalReseted);

            // Pushes all its LogicComponent back to the pool.
            for (int i = 0; i < ComponentArraySize; i++)
                ComponentPool.Push(_components[i]);

            _village = default(Village);
            _columnIndex = default(int);
            _x = default(int);
            _y = default(int);
        }

        /// <summary>
        /// Ticks the <see cref="VillageObject"/> with the specified game tick at with which 
        /// the <see cref="VillageObject"/> will do all calculations.
        /// </summary>
        /// <param name="ctick">Game tick with which the <see cref="VillageObject"/> will do all calculations.</param>
        protected internal virtual void Tick(int ctick)
        {
            // Space.
        }

        /// <summary>
        /// Writes the <see cref="VillageObject"/> to the specified <see cref="JsonWriter"/>.
        /// </summary>
        /// <param name="writer"><see cref="JsonWriter"/> to write to.</param>
        protected internal abstract void ToJsonWriter(JsonWriter writer);

        /// <summary>
        /// Reads the <see cref="VillageObject"/> from the specified <see cref="JsonReader"/>.
        /// </summary>
        /// <param name="reader"><see cref="JsonReader"/> to read from.</param>
        protected internal abstract void FromJsonReader(JsonReader reader);

        // Pushes the VillageObject to the VillageObjectPool where it will
        // be reused when Village.FromJson is called.
        internal void PushToPool()
        {
            // Don't wait for ResetVillageObject to be called to set the _village to null,
            // so the _village can get picked up by the GC.
            _village = null;

            // Push the object back to the pool.
            VillageObjectPool.Push(this);
            _pushCount++;
        }

        internal void SetVillageInternal(Village village)
        {
            Debug.Assert(village != null, "Tried to set a VillageObject's village to null.");

            _village = village;
            // Adding it to the VillageObjectCollection to 
            // update the GameID of the object.
            _village.VillageObjects.Add(this);
        }
        #endregion
    }
}
