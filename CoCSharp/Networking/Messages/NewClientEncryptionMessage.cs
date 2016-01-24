namespace CoCSharp.Networking.Messages
{
    /// <summary>
    /// New message introduced in the latest update. It is the
    /// first message sent by the client to the server to initiate
    /// a login.
    /// </summary>
    public class NewClientEncryptionMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewClientEncryptionMessage"/> class.
        /// </summary>
        public NewClientEncryptionMessage()
        {
            // Space
        }

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;
        /// <summary>
        /// Unknown integer 2.
        /// </summary>
        public int Unknown2;
        /// <summary>
        /// Unknown integer 3.
        /// </summary>
        public int MajorVersion;

        /// <summary>
        /// Unknown integer 4.
        /// </summary>
        public int Unknown4;

        /// <summary>
        /// Unknown integer 5.
        /// </summary>
        public int MinorVersion;
        /// <summary>
        /// Hash string.
        /// </summary>
        public string Hash;

        /// <summary>
        /// Unknown integer 6.
        /// </summary>
        public int Unknown6;
        /// <summary>
        /// Unknown integer 7.
        /// </summary>
        public int Unknown7;

        /// <summary>
        /// Gets the ID of the <see cref="NewClientEncryptionMessage"/>.
        /// </summary>
        public override ushort ID { get { return 10100; } }

        /// <summary>
        /// Reads the <see cref="NewClientEncryptionMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="NewClientEncryptionMessage"/>.
        /// </param>
        public override void ReadMessage(MessageReader reader)
        {
            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            MajorVersion = reader.ReadInt32();
            Unknown4 = reader.ReadInt32();
            MinorVersion = reader.ReadInt32();
            Hash = reader.ReadString();
            Unknown6 = reader.ReadInt32();
            Unknown7 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="NewClientEncryptionMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="NewClientEncryptionMessage"/>.
        /// </param>
        public override void WriteMessage(MessageWriter writer)
        {
            writer.Write(Unknown1);
            writer.Write(Unknown2);
            writer.Write(MajorVersion);
            writer.Write(Unknown4);
            writer.Write(MinorVersion);
            writer.Write(Hash);
            writer.Write(Unknown6);
            writer.Write(Unknown7);
        }
    }
}
