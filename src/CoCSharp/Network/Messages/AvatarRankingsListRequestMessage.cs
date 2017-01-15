using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Description of AvatarRankingsListRequest.
    /// </summary>
    public class AvatarRankingsListRequestMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarRankingsListRequestMessage"/> class.
        /// </summary>
        public AvatarRankingsListRequestMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="AvatarRankingsListRequestMessage"/>.
        /// </summary>
        public override ushort Id => 14404;

        /// <summary>
        /// Unknown byte 1.
        /// </summary>
        public byte Unknown1;

        /// <summary>
        /// User ID.
        /// </summary>
        public long UserId;

        /// <summary>
        /// Reads the <see cref="AvatarRankingsListRequestMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AvatarRankingsListRequestMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Unknown1 = reader.ReadByte();

            UserId = reader.ReadInt64();
        }

        /// <summary>
        /// Writes the <see cref="AvatarRankingsListRequestMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AvatarRankingsListRequestMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Unknown1);

            writer.Write(UserId);
        }
    }
}
