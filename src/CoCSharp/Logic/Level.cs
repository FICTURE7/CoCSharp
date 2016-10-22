namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a level.
    /// </summary>
    public class Level
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Level"/> class.
        /// </summary>
        public Level()
        {
            _village = new Village();
            _avatar = new Avatar();
        }
        #endregion

        #region Fields & Properties
        private Village _village;
        private Avatar _avatar;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Level"/> is owned
        /// by an NPC.
        /// </summary>
        public bool IsNpc { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Logic.Village"/> associated with the <see cref="Level"/>.
        /// </summary>
        public Village Village { get; set; }

        /// <summary>
        /// Gets the <see cref="Logic.Avatar"/> associated with the <see cref="Level"/>.
        /// </summary>
        public Avatar Avatar => _avatar;
        #endregion

        /// <summary>
        /// Ticks all <see cref="VillageObject"/> in the <see cref="Level"/>.
        /// </summary>
        public void Tick()
        {
            Village.Update();
        }
    }
}
