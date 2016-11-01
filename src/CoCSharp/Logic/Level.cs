using CoCSharp.Network.Messages;

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
            _avatar = new Avatar();
        }
        #endregion

        #region Fields & Properties
        private readonly Avatar _avatar;

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

        public OwnHomeDataMessage OwnHomeData
        {
            get
            {
                var villageData = new VillageMessageComponent(this);
                var avatarData = new AvatarMessageComponent(this);
                var ohdMessage = new OwnHomeDataMessage()
                {
                    OwnVillageData = villageData,
                    OwnAvatarData = avatarData,
                    Unkonwn4 = 1462629754000,
                    Unknown5 = 1462629754000,
                    Unknown6 = 1462631554000,
                };
                return ohdMessage;
            }                
        }
        #endregion

        #region Methods
        /// <summary>
        /// Ticks all <see cref="VillageObject"/> in the <see cref="Level"/>.
        /// </summary>
        public void Tick()
        {
            Village.Update();
        }
        #endregion
    }
}
