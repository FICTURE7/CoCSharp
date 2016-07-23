using CoCSharp.Network;
using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the server to the client to confirm
    /// the name change.
    /// </summary>
    public class ChangeAvatarNameResponseMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeAvatarNameResponseMessage"/> class.
        /// </summary>
        public ChangeAvatarNameResponseMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="ChangeAvatarNameResponseMessage"/>.
        /// </summary>
        public override ushort ID { get { return 24111; } }

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1; // 3

        /// <summary>
        /// New name confirmed by the server.
        /// </summary>
        public string NewName;

        /// <summary>
        /// Unknown integer 2.
        /// </summary>
        public int Unknown2; // 0
        /// <summary>
        /// Unknown integer 3.
        /// </summary>
        public int Unknown3; // 1
        /// <summary>
        /// Unknown integer 4.
        /// </summary>
        public int Unknown4; // -1

        /// <summary>
        /// Reads the <see cref="ChangeAvatarNameResponseMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="ChangeAvatarNameResponseMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Unknown1 = reader.ReadInt32();

            NewName = reader.ReadString();

            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadInt32();
            Unknown4 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="ChangeAvatarNameResponseMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="ChangeAvatarNameResponseMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Unknown1);

            writer.Write(NewName);

            writer.Write(Unknown2);
            writer.Write(Unknown3);
            writer.Write(Unknown4);
        }
    }
}
