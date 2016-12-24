using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the client to the server to request for an alliance search.
    /// </summary>
    public class AllianceSearchRequestMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceSearchRequestMessage"/> class.
        /// </summary>
        public AllianceSearchRequestMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="AllianceSearchRequestMessage"/>.
        /// </summary>
        public override ushort Id => 14324;

        /// <summary>
        /// Search string.
        /// </summary>
        public string TextSearch;
        /// <summary>
        /// War frequency of clan.
        /// </summary>
        public int WarFrequency;
        /// <summary>
        /// Location of clan.
        /// </summary>
        public int ClanLocation;
        /// <summary>
        /// Minimum number of members.
        /// </summary>
        public int MinimumMembers;
        /// <summary>
        /// Maximum number of members.
        /// </summary>
        public int MaximumMembers;
        /// <summary>
        /// Trophy limit.
        /// </summary>
        public int TrophyLimit;
        /// <summary>
        /// Whether to return clans that the user can't join as well.
        /// </summary>
        public bool OnlyCanJoin;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;

        /// <summary>
        /// Minimum levels.
        /// </summary>
        public int ExpLevels;

        /// <summary>
        /// Reads the <see cref="AllianceSearchRequestMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AllianceSearchRequestMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            TextSearch = reader.ReadString();
            WarFrequency = reader.ReadInt32();
            ClanLocation = reader.ReadInt32();
            MinimumMembers = reader.ReadInt32();
            MaximumMembers = reader.ReadInt32();
            TrophyLimit = reader.ReadInt32();
            OnlyCanJoin = reader.ReadBoolean();

            Unknown1 = reader.ReadInt32();

            ExpLevels = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="AllianceSearchRequestMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AllianceSearchRequestMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(TextSearch);
            writer.Write(WarFrequency);
            writer.Write(ClanLocation);
            writer.Write(MinimumMembers);
            writer.Write(MaximumMembers);
            writer.Write(TrophyLimit);
            writer.Write(OnlyCanJoin);

            writer.Write(Unknown1);

            writer.Write(ExpLevels);
        }
    }
}
