using CoCSharp.Data;
using Newtonsoft.Json;

namespace CoCSharp.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class VillageObject
    {
        /// <summary>
        /// 
        /// </summary>
        public VillageObject()
        {
            ID = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public VillageObject(int id)
        {
            ID = id;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("data")]
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore()]
        public virtual string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("x")]
        public virtual int X { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("y")]
        public virtual int Y { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        public virtual void FromDatabase(Database database) { }
    }
}
