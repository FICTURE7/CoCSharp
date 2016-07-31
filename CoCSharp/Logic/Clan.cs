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
        /// <summary>
        ///Invite type of the clan
        /// </summary>
        public int InviteType { get; set; }
         /// <summary>
         /// Member count of the clan.
      /// </summary>
         public int MemberCount { get; set; }
        /// <summary>
         /// Total number of trophies.
         /// </summary>
         public int TotalTrophies { get; set; }
         /// <summary>
         /// Required number of trophies to join.
         /// </summary>
         public int RequiredTrophies { get; set; }
         /// <summary>
         /// Total number of wars won.
         /// </summary>
         public int WarsWon { get; set; }
         /// <summary>
         /// Total number of wars lost.
         /// </summary>
         public int WarsLost { get; set; }
         /// <summary>
         /// Total number of wars tried.
         /// </summary>
         public int WarsTried { get; set; }
         /// <summary>
         /// Language of the clan.
         /// </summary>
         public int Language { get; set; }
         /// <summary>
         /// War frequency of the clan.
         /// </summary>
         public int WarFrequency { get; set; }
         /// <summary>
         /// Location of the clan.
         /// </summary>
       public int Location { get; set; }
      /// <summary>
      /// PerkPoints of the clan
       /// </summary>
        public int PerkPoints { get; set; }
       /// <summary>
       /// Clan wars win streak
       /// </summary>
        public int WinStreak { get; set; }
        /// <summary>
        /// Is public or not
        /// </summary>
        public bool WarLogsPublic { get; set; }
    }
}
