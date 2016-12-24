namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the client to the server to tell it
    /// that an alliance was created.
    /// </summary>
    public class CreateAllianceMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAllianceMessage"/> class.
        /// </summary>
        public CreateAllianceMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="CreateAllianceMessage"/>.
        /// </summary>
        public override ushort Id { get { return 14301; } }

        /// <summary>
        /// Name of the alliance.
        /// </summary>
        public string Name;
        /// <summary>
        /// Description of the alliance.
        /// </summary>
        public string Description;
        /// <summary>
        /// Badge of the alliance.
        /// </summary>
        public int Badge;
        /// <summary>
        /// Invite type of the alliance.
        /// </summary>
        public int InviteType;
        /// <summary>
        /// Number of required trophy to join the alliance.
        /// </summary>
        public int RequiredTrophy;
        /// <summary>
        /// War frequency of the alliance.
        /// </summary>
        public int WarFrequency;
        /// <summary>
        /// Origin of the alliance.
        /// </summary>
        public int Origin;
        /// <summary>
        /// Value indicating if the war log of the alliance is
        /// public.
        /// </summary>
        public bool WarLogPublic;

        /// <summary>
        /// Reads the <see cref="CreateAllianceMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="CreateAllianceMessage"/>.
        /// </param>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Name = reader.ReadString();
            Description = reader.ReadString();
            Badge = reader.ReadInt32();
            InviteType = reader.ReadInt32();
            RequiredTrophy = reader.ReadInt32();
            WarFrequency = reader.ReadInt32();
            Origin = reader.ReadInt32();
            WarLogPublic = reader.ReadBoolean();
        }

        /// <summary>
        /// Writes the <see cref="CreateAllianceMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="CreateAllianceMessage"/>.
        /// </param>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Name);
            writer.Write(Description);
            writer.Write(Badge);
            writer.Write(InviteType);
            writer.Write(RequiredTrophy);
            writer.Write(WarFrequency);
            writer.Write(Origin);
            writer.Write(WarLogPublic);
        }
    }
}
