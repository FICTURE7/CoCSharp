using CoCSharp.Logic;

namespace CoCSharp.Networking.Messages
{
    /// <summary>
    /// Represents an <see cref="Avatar"/>'s clan data sent
    /// in the networking protocol.
    /// </summary>
    public class ClanMessageData : MessageData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClanMessageData"/> class.
        /// </summary>
        public ClanMessageData()
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
