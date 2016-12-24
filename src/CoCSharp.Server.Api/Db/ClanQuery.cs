namespace CoCSharp.Server.Api.Db
{
    /// <summary>
    /// Represents a clan search.
    /// </summary>
    public struct ClanQuery
    {
        /// <summary>
        /// Search string.
        /// </summary>
        public string TextSearch;
        /// <summary>
        /// War frequency of clan.
        /// </summary>
        public int? WarFrequency;
        /// <summary>
        /// Location of clan.
        /// </summary>
        public int? ClanLocation;
        /// <summary>
        /// Minimum number of members.
        /// </summary>
        public int MinimumMembers;
        /// <summary>
        /// Maximum number of members.
        /// </summary>
        public int MaximumMembers;
        /// <summary>
        /// Trophy limit.
        /// </summary>
        public int PerkPoints;
        /// <summary>
        /// Whether to return clans that the user can't join as well.
        /// </summary>
        public bool OnlyCanJoin;
        /// <summary>
        /// Minimum level of the clan.
        /// </summary>
        public int ExpLevels;
    }
}
