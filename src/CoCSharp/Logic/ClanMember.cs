using System;

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
        /// with the specified <see cref="Level"/>.
        /// </summary>
        /// <param name="level"><see cref="Level"/> with which to initialize the <see cref="ClanMember"/> object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="level"/> is null.</exception>
        public ClanMember(Level level)
        {
            if (level == null)
                throw new ArgumentNullException(nameof(level));

            Id = level.Avatar.Id;
            Name = level.Avatar.Name;
            ExpLevels = level.Avatar.ExpLevels;
            League = level.Avatar.League;
            Trophies = level.Avatar.Trophies;
        }

        /// <summary>
        /// Gets or sets the user ID of the member.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the member.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the role of the member.
        /// </summary>
        public ClanMemberRole Role { get; set; }

        /// <summary>
        /// Gets or sets the experience level of the member.
        /// </summary>
        public int ExpLevels { get; set; }

        /// <summary>
        /// Gets or sets the league of the member.
        /// </summary>
        public int League { get; set; }

        /// <summary>
        /// Gets or sets the number trophies of the member.
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
        public int WarCooldown { get; set; }

        /// <summary>
        /// Gets or sets the war preference of the member.
        /// </summary>
        public int WarPreference { get; set; }

        /// <summary>
        /// Gets or sets the time of when the player joined the clan.
        /// </summary>
        public DateTime DateJoined { get; set; }
    }
}
