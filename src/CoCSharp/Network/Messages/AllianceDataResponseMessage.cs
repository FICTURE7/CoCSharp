using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the server to the client after the <see cref="AllianceDataRequestMessage"/>
    /// was sent to provide data about an alliance/clan.
    /// </summary>
    public class AllianceDataResponseMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceDataResponseMessage"/> class.
        /// </summary>
        public AllianceDataResponseMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="AllianceDataResponseMessage"/>.
        /// </summary>
        public override ushort ID { get { return 24301; } }

        /// <summary>
        /// Clan data.
        /// </summary>
        public CompleteClanMessageComponent Clan;
        /// <summary>
        /// Description of the clan.
        /// </summary>
        public string Description;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;

        /// <summary>
        /// ID of the war the clan is currently in.
        /// 0 If not in war.
        /// </summary>
        public long WarID;
        /// <summary>
        /// Members in the clan.
        /// </summary>
        public ClanMemberMessageComponent[] Members;

        /// <summary>
        /// Reads the <see cref="AllianceDataResponseMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AllianceDataResponseMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Clan = new CompleteClanMessageComponent();
            Clan.ReadMessageComponent(reader);
            Description = reader.ReadString();

            Unknown1 = reader.ReadInt32();

            if (reader.ReadBoolean())
                WarID = reader.ReadInt64();

            var count = reader.ReadInt32();
            Members = new ClanMemberMessageComponent[count];
            for (int i = 0; i < count; i++)
            {
                var member = new ClanMemberMessageComponent();
                member.ReadMessageComponent(reader);
                Members[i] = member;
            }
        }

        /// <summary>
        /// Writes the <see cref="AllianceDataResponseMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AllianceDataResponseMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            if (Clan == null)
                throw new InvalidOperationException("Clan cannot be null.");

            Clan.WriteMessageComponent(writer);
            writer.Write(Description);

            writer.Write(Unknown1);

            var inWar = WarID != 0;
            writer.Write(inWar);
            if (inWar)
                writer.Write(WarID);

            if (Members == null)
            {
                writer.Write(0);
                return;
            }

            var count = Members.Length;
            writer.Write(count);
            for (int i = 0; i < count; i++)
            {
                var member = Members[i];
                if (member == null)
                    throw new InvalidOperationException("Encountered null ClanMemberMesasgeComponent.");
                member.WriteMessageComponent(writer);
            }
        }
    }
}
