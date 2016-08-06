using CoCSharp.Logic;
using System;

namespace CoCSharp.Network.Messages.Commands
{
    /// <summary>
    ///  Command that is sent by the server to the client
    /// to tell it that it has receive a new rank.
    /// </summary>
    public class AllianceRoleUpdatedCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceRoleUpdatedCommand"/> class.
        /// </summary>
        public AllianceRoleUpdatedCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="AllianceRoleUpdatedCommand"/>.
        /// </summary>
        public override int ID { get { return 8; } }

        /// <summary>
        /// Clan ID which the client in.
        /// </summary>
        public long ClanID;
        /// <summary>
        /// Current tick.
        /// </summary>
        public int Tick;
        /// <summary>
        /// Role.
        /// </summary>
        public ClanMemberRole Role;
        /// <summary>
        /// Unknown Interger 1.
        /// </summary>
        public int Unknown1;

        /// Reads the <see cref="AllianceRoleUpdatedCommand"/> from the specified <see cref="MessageReader"/>.
        /// <summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AllianceRoleUpdatedCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            ClanID = reader.ReadInt64();
            Unknown1 = reader.ReadInt32();
            Role = (ClanMemberRole)reader.ReadInt32();
            Tick = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="AllianceRoleUpdatedCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AllianceRoleUpdatedCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(ClanID);
            writer.Write(Unknown1);
            writer.Write((int)Role);
            writer.Write(Tick);
        }
    }
}
