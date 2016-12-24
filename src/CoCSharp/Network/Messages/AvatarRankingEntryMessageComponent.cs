using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Description of AvatarRankingEntryMessageComponent.
    /// </summary>
    public class AvatarRankingEntryMessageComponent : MessageComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarRankingEntryMessageComponent"/> class.
        /// </summary>
        public AvatarRankingEntryMessageComponent()
        {
            // Space
        }

        /// <summary>
        /// User ID of avatar.
        /// </summary>
        public long UserId;
        /// <summary>
        /// User name of avatar.
        /// </summary>
        public string UserName;
        /// <summary>
        /// Rank of avatar.
        /// </summary>
        public int Rank;
        /// <summary>
        /// Trophies of avatar.
        /// </summary>
        public int Trophies;
        /// <summary>
        /// Previous rank of avatar.
        /// </summary>
        public int PreviousRank;
        /// <summary>
        /// Experience level of avatar.
        /// </summary>
        public int ExpLevels;
        /// <summary>
        /// Attacks won.
        /// </summary>
        public int AttacksWon;
        /// <summary>
        /// Attacks lost.
        /// </summary>
        public int AttacksLost;
        /// <summary>
        /// Defenses won.
        /// </summary>
        public int DefensesWon;
        /// <summary>
        /// Defenses lost.
        /// </summary>
        public int DefensesLost;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;

        /// <summary>
        /// Country code.
        /// </summary>
        public string CountryCode;
        /// <summary>
        /// Home ID, potentially another user ID.
        /// </summary>
        public long HomeId;

        /// <summary>
        /// Unknown integer 2.
        /// </summary>
        public int Unknown2;
        /// <summary>
        /// Unknown integer 3.
        /// </summary>
        public int Unknown3;

        /// <summary>
        /// Clan of avatar.
        /// </summary>
        public ClanMessageComponent Clan;

        /// <summary>
        /// Reads the <see cref="AvatarRankingEntryMessageComponent"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AvatarRankingEntryMessageComponent"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessageComponent(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            UserId = reader.ReadInt64();
            UserName = reader.ReadString();
            Rank = reader.ReadInt32();
            Trophies = reader.ReadInt32();
            PreviousRank = reader.ReadInt32();
            ExpLevels = reader.ReadInt32();
            AttacksWon = reader.ReadInt32();
            AttacksLost = reader.ReadInt32();
            DefensesWon = reader.ReadInt32();
            DefensesLost = reader.ReadInt32();

            Unknown1 = reader.ReadInt32();

            CountryCode = reader.ReadString();
            HomeId = reader.ReadInt64();

            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadInt32();

            if (reader.ReadBoolean())
            {
                Clan = new ClanMessageComponent
                {
                    Id = reader.ReadInt64(),
                    Name = reader.ReadString(),
                    Unknown2 = reader.ReadInt32()
                };
            }
        }

        /// <summary>
        /// Writes the <see cref="AvatarRankingEntryMessageComponent"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AvatarRankingEntryMessageComponent"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessageComponent(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(UserId);
            writer.Write(UserName);
            writer.Write(Rank);
            writer.Write(Trophies);
            writer.Write(PreviousRank);
            writer.Write(ExpLevels);
            writer.Write(AttacksWon);
            writer.Write(AttacksLost);
            writer.Write(DefensesWon);
            writer.Write(DefensesLost);

            writer.Write(Unknown1);

            writer.Write(CountryCode);
            writer.Write(HomeId);

            writer.Write(Unknown2);
            writer.Write(Unknown3);
            if (Clan != null)
            {
                writer.Write(true);
                writer.Write(Clan.Id);
                writer.Write(Clan.Name);
                writer.Write((int)Clan.Unknown2);
            }
            else
            {
                writer.Write(false);
            }
        }
    }
}
