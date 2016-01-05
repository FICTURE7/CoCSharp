namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans clan (Alliance).
    /// </summary>
    public class Clan
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Clan"/>.
        /// </summary>
        public Clan()
        {
            // Space
        }

        /// <summary>
        /// Gets or sets the ID of the <see cref="Clan"/>.
        /// </summary>
        public long ID { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the <see cref="Clan"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the level of the <see cref="Clan"/>.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the badge of the <see cref="Clan"/>.
        /// </summary>
        public int Badge { get; set; }

        /// <summary>
        /// Get or sets the role of the avatar.
        /// </summary>
        public int Role { get; set; }
    }
}
