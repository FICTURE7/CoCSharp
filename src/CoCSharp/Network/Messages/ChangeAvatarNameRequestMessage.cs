using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the client to the server to tell
    /// it that it has changed its avatar's name.
    /// </summary>
    public class ChangeAvatarNameRequestMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeAvatarNameRequestMessage"/> class.
        /// </summary>
        public ChangeAvatarNameRequestMessage()
        {
            // Space
        }

        /// <summary>
        /// New name of the avatar.
        /// </summary>
        public string NewName;

        /// <summary>
        /// Unknown byte 1.
        /// </summary>
        public byte Unknown1;

        /// <summary>
        /// Gets the ID of the <see cref="ChangeAvatarNameRequestMessage"/>.
        /// </summary>
        public override ushort Id { get { return 10212; } }

        /// <summary>
        /// Reads the <see cref="ChangeAvatarNameRequestMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="ChangeAvatarNameRequestMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            NewName = reader.ReadString();
            Unknown1 = reader.ReadByte();
        }

        /// <summary>
        /// Writes the <see cref="ChangeAvatarNameRequestMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="ChangeAvatarNameRequestMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(NewName);
            writer.Write(Unknown1);
        }
    }
}
