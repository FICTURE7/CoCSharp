using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the server to the client after login
    /// was sent to provide data about an alliance/clan.
    /// </summary>
    public class AllianceFullEntryMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceFullEntryMessage"/> class.
        /// </summary>
        public AllianceFullEntryMessage()
        {
            //space
        }

        /// <summary>
        /// Clan data.
        /// </summary>
        public ClanCompleteMessageComponent Clan;
        /// <summary>
        /// Description of the clan.
        /// </summary>
        public string Description;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;
        /// <summary>
        /// Unknown integer 2.
        /// </summary>
        public int Unknown2;
        /// <summary>
        /// ID of the war the clan is currently in.
        /// 0 If not in war.
        /// </summary>
        public long WarId;

        /// <summary>
        /// Gets the ID of the <see cref="AllianceFullEntryMessage"/>.
        /// </summary>
        public override ushort Id { get { return 24324; } }

        /// <summary>
        /// Reads the <see cref="AllianceFullEntryMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AllianceFullEntryMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Description = reader.ReadString();
            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();

            if (reader.ReadBoolean())
                WarId = reader.ReadInt64();

            Clan = new ClanCompleteMessageComponent();
            Clan.ReadMessageComponent(reader);
        }

        /// <summary>
        /// Writes the <see cref="AllianceFullEntryMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AllianceFullEntryMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            if (Clan == null)
                throw new InvalidOperationException("Clan cannot be null.");

            writer.Write(Description);
            writer.Write(Unknown1);
            writer.Write(Unknown2);

            var inWar = WarId != 0;
            writer.Write(inWar);
            if (inWar)
                writer.Write(WarId);

            Clan.WriteMessageComponent(writer);
        }
    }
}
