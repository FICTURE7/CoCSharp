using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the client to the server to 
    /// begin a new session.
    /// </summary>
    public class HandshakeRequestMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandshakeRequestMessage"/> class.
        /// </summary>
        public HandshakeRequestMessage()
        {
            // Space
        }

        /// <summary>
        /// Protocol.
        /// </summary>
        public int Protocol;
        /// <summary>
        /// Key version.
        /// </summary>
        public int KeyVersion;
        /// <summary>
        /// Major version.
        /// </summary>
        public int MajorVersion;
        /// <summary>
        /// Minor version.
        /// </summary>
        public int MinorVersion;
        /// <summary>
        /// Build.
        /// </summary>
        public int Build;
        /// <summary>
        /// Hash string.
        /// </summary>
        public string Hash;
        /// <summary>
        /// Device type.
        /// </summary>
        public int DeviceType;
        /// <summary>
        /// AppStore.
        /// </summary>
        public int AppStore;

        /// <summary>
        /// Gets the ID of the <see cref="HandshakeRequestMessage"/>.
        /// </summary>
        public override ushort Id { get { return 10100; } }

        /// <summary>
        /// Reads the <see cref="HandshakeRequestMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="HandshakeRequestMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Protocol = reader.ReadInt32();
            KeyVersion = reader.ReadInt32();
            MajorVersion = reader.ReadInt32();
            MinorVersion = reader.ReadInt32();
            Build = reader.ReadInt32();
            Hash = reader.ReadString();
            DeviceType = reader.ReadInt32();
            AppStore = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="HandshakeRequestMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="HandshakeRequestMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Protocol);
            writer.Write(KeyVersion);
            writer.Write(MajorVersion);
            writer.Write(MinorVersion);
            writer.Write(Build);
            writer.Write(Hash);
            writer.Write(DeviceType);
            writer.Write(AppStore);
        }
    }
}
