using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Stream entry indicating a someone joined or left the clan.
    /// </summary>
    public class JoinedOrLeftAllianceStream : AllianceStreamEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JoinedOrLeftAllianceStream"/> class.
        /// </summary>
        public JoinedOrLeftAllianceStream()
        {
            //space
        }

        /// <summary>
        /// Defines if user joined or left.
        /// </summary>
        public int Action;
        /// <summary>
        /// User ID of user that joined or left.
        /// </summary>
        public long ActorUserId;
        /// <summary>
        /// Name of user that joined or left.
        /// </summary>
        public string ActorName;

        /// <summary>
        /// Get the ID of the <see cref="JoinedOrLeftAllianceStream"/>
        /// </summary>
        public override int Id => 4;

        /// <summary>
        /// Reads the <see cref="JoinedOrLeftAllianceStream"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="JoinedOrLeftAllianceStream"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadStreamEntry(MessageReader reader)
        {
            base.ReadStreamEntry(reader);

            Action = reader.ReadInt32();
            ActorUserId = reader.ReadInt64();
            ActorName = reader.ReadString();
        }

        /// <summary>
        /// Writes the <see cref="JoinedOrLeftAllianceStream"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="JoinedOrLeftAllianceStream"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteStreamEntry(MessageWriter writer)
        {
            base.WriteStreamEntry(writer);

            writer.Write(Action);
            writer.Write(ActorUserId);
            writer.Write(ActorName);
        }
    }
}
