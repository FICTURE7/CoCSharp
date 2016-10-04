namespace CoCSharp.Logic
{
    /// <summary>
    /// Defines clan member roles.
    /// </summary>
    public enum ClanMemberRole : int
    {
        /// <summary>
        /// A member of the clan.
        /// </summary>
        Member = 1,

        /// <summary>
        /// Leader of the clan.
        /// </summary>
        Leader = 2,

        /// <summary>
        /// An elder of the clan.
        /// </summary>
        Elder = 3,

        /// <summary>
        /// A co-leader of the clan.
        /// </summary>
        CoLeader = 4
    };
}
