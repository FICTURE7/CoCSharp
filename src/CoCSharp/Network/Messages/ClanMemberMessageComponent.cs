using CoCSharp.Logic;
using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Represents a member of a clan.
    /// </summary>
    public class ClanMemberMessageComponent : MessageComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClanMemberMessageComponent"/> class.
        /// </summary>
        public ClanMemberMessageComponent()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClanMemberMessageComponent"/> class with
        /// the specified <see cref="ClanMember"/>.
        /// </summary>
        /// <param name="member"><see cref="ClanMember"/> from which to initialize this <see cref="ClanMemberMessageComponent"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="member"/> is null.</exception>
        public ClanMemberMessageComponent(ClanMember member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            Id = member.Id;
            Name = member.Name;
            Role = member.Role;
            Level = member.ExpLevels;
            LeagueLevel = member.League;
            Trophies = member.Trophies;
            TroopsDonated = member.TroopsDonated;
            TroopsReceived = member.TroopsReceived;
            Rank = member.Rank;
            PreviousRank = member.PreviousRank;
            NewMember = member.NewMember;
            WarCoolDown = member.WarCooldown;
            WarPreference = member.WarPreference;
            Id2 = member.Id;
        }

        /// <summary>
        /// User ID of the member.
        /// </summary>
        public long Id;
        /// <summary>
        /// Name of the member.
        /// </summary>
        public string Name;
        /// <summary>
        /// Role of the member.
        /// </summary>
        public ClanMemberRole Role;
        /// <summary>
        /// Level of the member.
        /// </summary>
        public int Level;
        /// <summary>
        /// Level of the league of the member.
        /// </summary>
        public int LeagueLevel;
        /// <summary>
        /// Trophies of the member.
        /// </summary>
        public int Trophies;
        /// <summary>
        /// Number of troops donated.
        /// </summary>
        public int TroopsDonated;
        /// <summary>
        /// Number of troops received.
        /// </summary>
        public int TroopsReceived;
        /// <summary>
        /// Rank of the member.
        /// </summary>
        public int Rank;
        /// <summary>
        /// Previous rank of the member.
        /// </summary>
        public int PreviousRank;
        /// <summary>
        /// Value indicating if the member is a new member.
        /// </summary>
        public bool NewMember;
        /// <summary>
        /// War cool down of the member.
        /// </summary>
        public int WarCoolDown;
        /// <summary>
        /// War preference of member.
        /// </summary>
        public int WarPreference;

        /// <summary>
        /// Unknown byte 1.
        /// </summary>
        public byte Unknown1;
        /// <summary>
        /// Unknown byte 2.
        /// </summary>
        public byte Unknown2;
        /// <summary>
        /// Unknown byte 3.
        /// </summary>
        public byte Unknown3;
        /// <summary>
        /// Unknown byte 4.
        /// </summary>
        public byte Unknown4;

        /// <summary>
        /// User ID of the member.
        /// </summary>
        public long Id2;

        /// <summary>
        /// Reads the <see cref="ClanCompleteMessageComponent"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="ClanCompleteMessageComponent"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessageComponent(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Id = reader.ReadInt64();
            Name = reader.ReadString();
            Role = (ClanMemberRole)reader.ReadInt32();
            Level = reader.ReadInt32();
            LeagueLevel = reader.ReadInt32();
            Trophies = reader.ReadInt32();
            TroopsDonated = reader.ReadInt32();
            TroopsReceived = reader.ReadInt32();
            Rank = reader.ReadInt32();
            PreviousRank = reader.ReadInt32();
            NewMember = reader.ReadBoolean();
            WarCoolDown = reader.ReadInt32();
            WarPreference = reader.ReadInt32();

            Unknown1 = reader.ReadByte();
            Unknown2 = reader.ReadByte();
            Unknown3 = reader.ReadByte();
            Unknown4 = reader.ReadByte();
            if (Unknown4 == 1)
                Id2 = reader.ReadInt64();
        }

        /// <summary>
        /// Writes the <see cref="ClanCompleteMessageComponent"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="ClanCompleteMessageComponent"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessageComponent(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Id);
            writer.Write(Name);
            writer.Write((int)Role);
            writer.Write(Level);
            writer.Write(LeagueLevel);
            writer.Write(Trophies);
            writer.Write(TroopsDonated);
            writer.Write(TroopsReceived);
            writer.Write(Rank);
            writer.Write(PreviousRank);
            writer.Write(NewMember);
            writer.Write(WarCoolDown);
            writer.Write(WarPreference);

            // Apparently its ?LONG
            writer.Write(Unknown1);
            writer.Write(Unknown2);
            writer.Write(Unknown3);
            writer.Write(Unknown4);
            if (Unknown4 == 1)
                writer.Write(Id2);
        }
    }
}
