using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the client to the server to request
    /// for the data of an alliance/clan.
    /// </summary>
    public class AllianceDataRequestMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceDataRequestMessage"/> class.
        /// </summary>
        public AllianceDataRequestMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="AllianceDataRequestMessage"/>.
        /// </summary>
        public override ushort Id { get { return 14302; } }

        /// <summary>
        /// ID of the clan whose data was requested.
        /// </summary>
        public long ClanId;

        /// <summary>
        /// Reads the <see cref="AllianceDataRequestMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AllianceDataRequestMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            ClanId = reader.ReadInt64();
        }

        /// <summary>
        /// Writes the <see cref="AllianceDataRequestMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AllianceDataRequestMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(ClanId);
        }
    }
}
