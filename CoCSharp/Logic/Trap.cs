using Newtonsoft.Json;
using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public class Trap : VillageObject
    {
        /// <summary>
        /// Initailizes a new instance of the <see cref="Trap"/> class with the specified
        /// ID and level.
        /// </summary>
        /// <param name="id">Sets the ID of this <see cref="Trap"/> class.</param>
        /// <param name="level">Sets the ID of this <see cref="Trap"/> class.</param>
        public Trap(int id, int level, Village village)
            : base(id, village)
        {
            Level = level;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("need_repair")]
        public bool Broken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("lvl")]
        public int Level { get; set; }
    }
}
