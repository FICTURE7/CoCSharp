using CoCSharp.Logic;
using CoCSharp.Networking;

namespace CoCSharp.Data
{
    /// <summary>
    /// Represents an <see cref="Avatar"/>'s clan data sent
    /// in the networking protocol.
    /// </summary>
    public class ClanData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClanData"/> class.
        /// </summary>
        public ClanData()
        {
            // Space
        }

        /// <summary>
        /// Clan ID.
        /// </summary>
        public long ID;
        /// <summary>
        /// Clan name.
        /// </summary>
        public string Name;
        /// <summary>
        /// Clan badge data.
        /// </summary>
        public int Badge;
        /// <summary>
        /// <see cref="Avatar"/> clan role.
        /// </summary>
        public int Role;
        /// <summary>
        /// Clan level.
        /// </summary>
        public int Level;
        /// <summary>
        /// Unknown byte 1.
        /// </summary>
        public byte Unknown1;
    }
}
