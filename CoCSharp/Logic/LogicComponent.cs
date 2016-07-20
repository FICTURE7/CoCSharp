using Newtonsoft.Json;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a logical component.
    /// </summary>
    public abstract class LogicComponent
    {
        internal LogicComponent()
        {
            // Space
        }

        internal VillageObject _vilObject;        

        // Name of the component. E.g "units", "unit_prod", "unit_upg".
        internal abstract string ComponentName { get; }

        // Execute logic of the LogicComponent.
        internal abstract void Execute();

        // Writes the current LogicComponent to the JsonWriter.
        internal abstract void ToJsonWriter(JsonWriter writer);

        // Reads the current LogicComponent to the JsonReader.
        internal abstract void FromJsonReader(JsonReader reader);
    }
}
