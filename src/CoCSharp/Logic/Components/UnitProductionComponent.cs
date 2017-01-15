using System;
using Newtonsoft.Json;
using System.Collections;

namespace CoCSharp.Logic.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class UnitProductionComponent : Component
    {
        internal const int ID = 0;

        internal UnitProductionComponent() : base()
        {
            _k = new Queue();
        }

        public bool _isSpell;
        private readonly Queue _k;

        /// <summary>
        /// Gets the <see cref="Queue"/> of character to produce.
        /// </summary>
        public Queue ProductionQueue => _k;

        /// <summary>
        /// Gets a value indicating whether the <see cref="UnitProductionComponent"/> is a spell production component.
        /// </summary>
        public bool IsSpell => _isSpell;

        /// <summary/>
        protected internal override string ComponentName => "unit_prod";

        /// <summary/>
        protected internal override int ComponentID => ID;

        /// <summary/>
        protected internal override void ResetComponent()
        {
            _k.Clear();
            _isSpell = default(bool);
        }

        /// <summary/>
        protected internal override void Tick(int ctick)
        {

        }

        /// <summary/>
        protected internal override void FromJsonReader(JsonReader reader)
        {

        }

        /// <summary/>
        protected internal override void ToJsonWriter(JsonWriter writer)
        {
            var type = _isSpell ? 1 : 0;
            writer.WritePropertyName("unit_type");
            writer.WriteValue(type);

            writer.WritePropertyName("m");
            writer.WriteValue(1);

            //writer.WriteStartArray();
            ////foreach (var k in _k)
            ////{
            ////    if (_isSpell)
            ////    {
            ////    }
            ////    else
            ////    {

            ////    }
            ////}

            //writer.WriteStartObject();

            //writer.WriteEndObject();

            //writer.WriteEndArray();
        }
    }
}
