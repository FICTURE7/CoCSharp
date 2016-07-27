using System;
using Newtonsoft.Json;
using CoCSharp.Data.Models;
using System.Collections.Generic;

namespace CoCSharp.Logic.Components
{
    internal class ResourceStorageComponent : LogicComponent
    {
        internal const int ID = 4;

        internal ResourceStorageComponent() : base()
        {
            // Space
        }

        internal override int ComponentID
        {
            get
            {
                return ID;
            }
        }

        internal override string ComponentName
        {
            get
            {
                return "res_storage";
            }
        }

        public List<ResourceData> ResourceKinds { get; set; }

        internal override void Execute()
        {
            // Space
        }

        internal override void FromJsonReader(JsonReader reader)
        {
            throw new InvalidOperationException("ResourceStorageComponent cannot be read from JSON.");
        }

        internal override void ToJsonWriter(JsonWriter writer)
        {
            throw new InvalidOperationException("ResourceStorageComponent cannot be written to JSON.");
        }
    }
}
