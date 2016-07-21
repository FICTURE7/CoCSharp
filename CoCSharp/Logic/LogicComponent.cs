using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a logical component to extend logic of <see cref="Logic.VillageObject"/>s.
    /// </summary>
    [DebuggerDisplay("Name = {ComponentName}")]
    public abstract class LogicComponent
    {
        internal LogicComponent()
        {
            // Space
        }

        // Name of the component. E.g "units", "unit_prod", "unit_upg".
        internal abstract string ComponentName { get; }
        // ID of the component. For faster searches of component.
        internal abstract int ComponentID { get; }

        /// <summary>
        /// The event raised when a property value has changed.
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs> PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether to raise the <see cref="PropertyChanged"/> event.
        /// </summary>
        public bool IsPropertyChangedEnabled { get; set; }

        internal VillageObject _vilObject;
        /// <summary>
        /// Gets the <see cref="Logic.VillageObject"/> with which the <see cref="LogicComponent"/> is
        /// attached to.
        /// </summary>
        public VillageObject VillageObject
        {
            get
            {
                return _vilObject;
            }
        }

        // Execute logic of the LogicComponent.
        internal abstract void Execute();

        // Writes the current LogicComponent to the JsonWriter.
        internal abstract void ToJsonWriter(JsonWriter writer);

        // Reads the current LogicComponent to the JsonReader.
        internal abstract void FromJsonReader(JsonReader reader);

        internal void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (IsPropertyChangedEnabled && PropertyChanged != null)
                PropertyChanged(this, args);
        }
    }
}
