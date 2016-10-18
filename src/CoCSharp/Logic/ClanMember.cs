namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a member of a <see cref="Clan"/>.
    /// </summary>
    public class ClanMember
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClanMember"/> class.
        /// </summary>
        public ClanMember()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClanMember"/> class
        /// with the specified <see cref="Avatar"/>.
        /// </summary>
        /// <param name="avatar"><see cref="Avatar"/> with which to initialize the <see cref="ClanMember"/> object.</param>
        public ClanMember(Avatar avatar)
        {
            ID = avatar.ID;
            Name = avatar.Name;
            Level = avatar.ExpLevel;
            LeagueLevel = avatar.League;
            Trophies = avatar.Trophies;
        }

        /// <summary>
        /// Gets or sets the User ID of the member.
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Gets or sets the Name of the member.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Role of the member.
        /// </summary>
        public ClanMemberRole Role { get; set; }

        /// <summary>
        /// Gets or sets the Level of the member.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the Level of the league of the member.
        /// </summary>
        public int LeagueLevel { get; set; }

        /// <summary>
        /// Gets or sets the Trophies of the member.
        /// </summary>
        public int Trophies { get; set; }

        /// <summary>
        /// Number of troops donated.
        /// </summary>
        public int TroopsDonated { get; set; }

        /// <summary>
        /// Gets or sets the Number of troops received.
        /// </summary>
        public int TroopsReceived { get; set; }

        /// <summary>
        /// Gets or sets the Rank of the member.
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// Gets or sets the Previous rank of the member.
        /// </summary>
        public int PreviousRank { get; set; }

        /// <summary>
        /// Gets or sets the Value indicating if the member is a new member.
        /// </summary>
        public bool NewMember { get; set; }

        /// <summary>
        /// Gets or sets the war cool down of the member.
        /// </summary>
        public int WarCoolDown { get; set; }

        /// <summary>
        /// Gets or sets the war preference of the member.
        /// </summary>
        public int WarPreference { get; set; }
    }
}
