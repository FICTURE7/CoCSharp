using CoCSharp.Logic;
using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Represents an <see cref="Avatar"/>'s clan data sent
    /// in the networking protocol.
    /// </summary>
    public class ClanMessageComponent : MessageComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClanMessageComponent"/> class.
        /// </summary>
        public ClanMessageComponent()
        {
            // Space
        }

        /// <summary>
        /// Clan ID.
        /// </summary>
        public long Id;
        /// <summary>
        /// Clan name.
        /// </summary>
        public string Name;
        /// <summary>
        /// Clan badge data.
        /// </summary>
        public int Badge;
        /// <summary>
        /// Clan member role.
        /// </summary>
        public ClanMemberRole Role;
        /// <summary>
        /// Clan level.
        /// </summary>
        public int Level;

        // Unknown1 & Unknown2 does not actually belong to this component but AvatarMessageComponent.

        /// <summary>
        /// Unknown byte 1.
        /// </summary>
        public byte Unknown1;

        /// <summary>
        /// Unknown long 2.
        /// </summary>
        public long Unknown2;
    }
}
