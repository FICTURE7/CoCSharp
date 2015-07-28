using CoCSharp.Data;
using Newtonsoft.Json;

namespace CoCSharp.Logic
{
    public abstract class VillageObject
    {
        public VillageObject()
        {
            ID = -1;
        }

        public VillageObject(int id)
        {
            ID = id;
        }

        [JsonProperty("data")]
        public int ID { get; set; }

        [JsonIgnore()]
        public virtual string Name { get; set; }

        [JsonProperty("x")]
        public virtual int X { get; set; }

        [JsonProperty("y")]
        public virtual int Y { get; set; }

        public virtual void FromDatabase(Database database) { }
    }
}
