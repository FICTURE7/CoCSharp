using Newtonsoft.Json;

namespace CoCSharp.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public class Decoration : VillageObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Decoration"/> class with
        /// the specified ID.
        /// </summary>
        /// <param name="id">Sets the ID of this <see cref="Decoration"/> class.</param>
        public Decoration(int id, Village village)
            : base(id, village)
        {
            // Space
        }
    }
}
