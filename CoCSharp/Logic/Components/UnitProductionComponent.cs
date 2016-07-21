using System;
using Newtonsoft.Json;

namespace CoCSharp.Logic.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class UnitProductionComponent : LogicComponent
    {
        internal const int ID = 0;

        internal UnitProductionComponent()
        {
            // Space
        }

        internal override string ComponentName
        {
            get
            {
                return "unit_prod";
            }
        }

        internal override int ComponentID
        {
            get
            {
                return ID;
            }
        }

        internal override void Execute()
        {
            throw new NotImplementedException();
        }

        internal override void FromJsonReader(JsonReader reader)
        {
            throw new NotImplementedException();
        }

        internal override void ToJsonWriter(JsonWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
