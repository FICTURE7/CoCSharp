namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans decoration (deco).
    /// </summary>
    public class Decoration : VillageObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Decoration"/> class.
        /// </summary>
        public Decoration() : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Decoration"/> class
        /// with the specified data ID.
        /// </summary>
        /// <param name="dataID">Data ID of the <see cref="Decoration"/>.</param>
        public Decoration(int dataID) : base(dataID)
        {
            // Space
        }
    }
}
