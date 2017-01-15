using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the client to the server
    /// to tell it that it joined an alliance.
    /// </summary>
    public class JoinAllianceMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JoinAllianceMessage"/> class.
        /// </summary>
        public JoinAllianceMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="JoinAllianceMessage"/>.
        /// </summary>
        public override ushort Id { get { return 14305; } }

        /// <summary>
        /// ID of the clan the client joined.
        /// </summary>
        public long ClanId;

        /// <summary>
        /// Reads the <see cref="JoinAllianceMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="JoinAllianceMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            ClanId = reader.ReadInt64();
        }

        /// <summary>
        /// Writes the <see cref="JoinAllianceMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="JoinAllianceMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(ClanId);
        }
    }
}
