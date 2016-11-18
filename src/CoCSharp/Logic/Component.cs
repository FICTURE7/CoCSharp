using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a logical component to extend logic of <see cref="Logic.VillageObject"/>s.
    /// </summary>
    public abstract class Component : INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Component"/> class.
        /// </summary>
        protected Component()
        {
            // Space
        }

        internal VillageObject _parent;

        /// <summary>
        /// The event raised when a property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether to raise the <see cref="PropertyChanged"/> event.
        /// </summary>
        public bool IsPropertyChangedEnabled { get; set; }

        // Name of the component. E.g "units", "unit_prod", "unit_upg".
        /// <summary>
        /// Gets the name of the <see cref="Component"/>.
        /// </summary>
        protected internal abstract string ComponentName { get; }

        /// <summary>
        /// Gets the ID of the <see cref="Component"/>.
        /// </summary>
        /// 
        /// <remarks>
        /// Each type of <see cref="Component"/> must have a unique ID.
        /// </remarks>
        protected internal abstract int ComponentID { get; }

        /// <summary>
        /// Gets the <see cref="VillageObject"/> with which the <see cref="Component"/> is
        /// attached to.
        /// </summary>
        public VillageObject Parent => _parent;

        /// <summary>
        /// Performs a ticking logic on the specified game tick.
        /// </summary>
        /// <param name="ctick">Game tick.</param>
        protected internal abstract void Tick(int ctick);

        /// <summary>
        /// Writes the <see cref="Component"/> to the specified <see cref="JsonWriter"/>.
        /// </summary>
        /// <param name="writer"><see cref="JsonWriter"/> to write to.</param>
        protected internal abstract void ToJsonWriter(JsonWriter writer);

        /// <summary>
        /// Reads the <see cref="Component"/> from the specified <see cref="JsonReader"/>.
        /// </summary>
        /// <param name="reader"><see cref="JsonReader"/> to read from.</param>
        protected internal abstract void FromJsonReader(JsonReader reader);

        /// <summary>
        /// Resets the <see cref="Component"/>.
        /// </summary>
        protected internal virtual void ResetComponent()
        {
            _parent = default(VillageObject);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event with the specified event data
        /// if <see cref="IsPropertyChangedEnabled"/> is set to <c>true</c>.
        /// </summary>
        /// <param name="args">Event arguments.</param>
        protected void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (IsPropertyChangedEnabled && PropertyChanged != null)
                PropertyChanged(this, args);
        }
    }
}
