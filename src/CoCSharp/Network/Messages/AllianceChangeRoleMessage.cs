using CoCSharp.Logic;
using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the client to the server />
    /// was sent to provide data about an promoteion/demote alliance/clan.
    /// </summary>
    public class AllianceChangeRoleMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceChangeRoleMessage"/> class.
        /// </summary>
        public AllianceChangeRoleMessage()
        {
            //space
        }

        /// <summary>
        /// User ID of the player.
        /// That promoted/demoted
        /// </summary>
        public long UserId;
        /// <summary>
        /// Role that the player gain
        /// </summary>
        public ClanMemberRole Role;

        /// <summary>
        /// Gets the ID of the <see cref="AllianceChangeRoleMessage"/>.
        /// </summary>

        public override ushort Id { get { return 14306; } }

        /// <summary>
        /// Reads the <see cref="AllianceChangeRoleMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AllianceChangeRoleMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            UserId = reader.ReadInt64();
            Role = (ClanMemberRole)reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="AllianceChangeRoleMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AllianceChangeRoleMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(UserId);
            writer.Write((int)Role);
        }
    }
}