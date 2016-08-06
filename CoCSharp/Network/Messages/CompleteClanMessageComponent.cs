using CoCSharp.Logic;
using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Represents a <see cref="Clan"/>.
    /// </summary>
    public class CompleteClanMessageComponent : MessageComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompleteClanMessageComponent"/> class.
        /// </summary>
        public CompleteClanMessageComponent()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompleteClanMessageComponent"/> class from
        /// the specified <see cref="Clan"/>.
        /// </summary>
        /// <param name="clan"><see cref="Clan"/> from which the data will be set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="clan"/> is null.</exception>
        public CompleteClanMessageComponent(Clan clan)
        {
            if (clan == null)
                throw new ArgumentNullException("clan");

            ID = clan.ID;
            Name = clan.Name;
            Badge = clan.Badge;
            InviteType = clan.InviteType;
            MemberCount = clan.Members.Count;
            TotalTrophies = clan.TotalTrophies;
            RequiredTrophies = clan.RequiredTrophies;
            WarsWon = clan.WarsWon;
            WarsLost = clan.WarsLost;
            WarsTried = clan.WarsTried;
            Language = clan.Language;
            WarFrequency = clan.WarFrequency;
            Location = clan.Location;
            PerkPoints = clan.PerkPoints;
            Level = clan.Level;
            WinStreak = clan.WinStreak;
        }

        /// <summary>
        /// ID of the clan.
        /// </summary>
        public long ID;
        /// <summary>
        /// Name of the clan.
        /// </summary>
        public string Name;
        /// <summary>
        /// Badge of the clan.
        /// </summary>
        public int Badge;
        /// <summary>
        /// Invite type of the clan.
        /// </summary>
        public int InviteType;
        /// <summary>
        /// Member count of the clan.
        /// </summary>
        public int MemberCount;
        /// <summary>
        /// Total number of trophies.
        /// </summary>
        public int TotalTrophies;
        /// <summary>
        /// Required number of trophies to join.
        /// </summary>
        public int RequiredTrophies;
        /// <summary>
        /// Total number of wars won.
        /// </summary>
        public int WarsWon;
        /// <summary>
        /// Total number of wars lost.
        /// </summary>
        public int WarsLost;
        /// <summary>
        /// Total number of wars tried.
        /// </summary>
        public int WarsTried;
        /// <summary>
        /// Language of the clan.
        /// </summary>
        public int Language;
        /// <summary>
        /// War frequency of the clan.
        /// </summary>
        public int WarFrequency;
        /// <summary>
        /// Location of the clan.
        /// </summary>
        public int Location;
        /// <summary>
        /// Perk points of the clan.
        /// </summary>
        public int PerkPoints;
        /// <summary>
        /// Level of the clan.
        /// </summary>
        public int Level;
        /// <summary>
        /// Win streak of the clan.
        /// </summary>
        public int WinStreak;
        /// <summary>
        /// Value for indicating war logs of the <see cref="Clan"/> is  available for public.
        /// </summary>
        public bool WarLogsPublic;

        /// <summary>
        /// Reads the <see cref="CompleteClanMessageComponent"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="CompleteClanMessageComponent"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessageComponent(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            ID = reader.ReadInt64();
            Name = reader.ReadString();
            Badge = reader.ReadInt32();
            InviteType = reader.ReadInt32();
            MemberCount = reader.ReadInt32();
            TotalTrophies = reader.ReadInt32();
            RequiredTrophies = reader.ReadInt32();
            WarsWon = reader.ReadInt32();
            WarsLost = reader.ReadInt32();
            WarsTried = reader.ReadInt32();
            Language = reader.ReadInt32();
            WarFrequency = reader.ReadInt32();
            Location = reader.ReadInt32();
            PerkPoints = reader.ReadInt32();
            Level = reader.ReadInt32();
            WinStreak = reader.ReadInt32();

            WarLogsPublic = reader.ReadBoolean();
        }

        /// <summary>
        /// Writes the <see cref="CompleteClanMessageComponent"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="CompleteClanMessageComponent"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessageComponent(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(ID);
            writer.Write(Name);
            writer.Write(Badge);
            writer.Write(InviteType);
            writer.Write(MemberCount);
            writer.Write(TotalTrophies);
            writer.Write(RequiredTrophies);
            writer.Write(WarsWon);
            writer.Write(WarsLost);
            writer.Write(WarsTried);
            writer.Write(Language);
            writer.Write(WarFrequency);
            writer.Write(Location);
            writer.Write(PerkPoints);
            writer.Write(Level);
            writer.Write(WinStreak);

            writer.Write(WarLogsPublic);
        }
    }
}
