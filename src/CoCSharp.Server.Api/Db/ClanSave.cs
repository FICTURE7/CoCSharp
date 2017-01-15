using CoCSharp.Logic;
using CoCSharp.Network.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoCSharp.Server.Api.Db
{
    /// <summary>
    /// Represents a <see cref="Clan"/> save and is a simple wrapper around <see cref="Clan"/>.
    /// </summary>
    public class ClanSave
    {
        // Simple object that can be converted into Clans and vice versa.
        // DbData -> ClanSave -> Clan.
        // DbData <- ClanSave <- Clan.

        #region Constructors
        internal ClanSave()
        {
            // Space
        }
        #endregion

        #region Fields & Properties
        // Instance that is going to get returned everytime ToClan is called.
        // This is to prevent instance duplication.
        private readonly Clan _clan;

        /// <summary>
        /// Gets or sets the time <see cref="ClanSave"/> was first saved.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the last time the <see cref="ClanSave"/> was saved.
        /// </summary>
        public DateTime DateLastSave { get; set; }

        /// <summary>
        /// Gets or sets the ID of the of the <see cref="ClanSave"/>.
        /// </summary>
        public long ClanId { get; set; }

        /// <summary>
        /// Gets or sets the name of the <see cref="ClanSave"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the <see cref="ClanSave"/>.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the experience level of the <see cref="ClanSave"/>.
        /// </summary>
        public int ExpLevels { get; set; }

        /// <summary>
        /// Gets or sets the badge.
        /// </summary>
        public int Badge { get; set; }

        /// <summary>
        /// Gets or sets the invite type of the clan.
        /// </summary>
        public int InviteType { get; set; }

        /// <summary>
        /// Gets or sets the total number of trophies.
        /// </summary>
        public int TotalTrophies { get; set; }

        /// <summary>
        /// Gets or sets the required number of trophies to join.
        /// </summary>
        public int RequiredTrophies { get; set; }

        /// <summary>
        /// Gets or sets the total number of wars won.
        /// </summary>
        public int WarsWon { get; set; }

        /// <summary>
        /// Gets or sets the total number of wars lost.
        /// </summary>
        public int WarsLost { get; set; }

        /// <summary>
        /// Gets or sets the total number of wars tried.
        /// </summary>
        public int WarsTried { get; set; }

        /// <summary>
        /// Gets or sets the language of the <see cref="Clan"/>.
        /// </summary>
        public int Language { get; set; }

        /// <summary>
        /// Gets or sets the war frequency of the <see cref="Clan"/>.
        /// </summary>
        public int WarFrequency { get; set; }

        /// <summary>
        /// Gets or sets the location of the <see cref="Clan"/>.
        /// </summary>
        public int Location { get; set; }

        /// <summary>
        /// Gets or sets the perk points of the
        /// <see cref="Clan"/>.
        /// </summary>
        public int PerkPoints { get; set; }

        /// <summary>
        /// Gets or sets the clan wars win streak
        /// of the <see cref="Clan"/>.
        /// </summary>
        public int WinStreak { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the
        /// war logs of the <see cref="Clan"/> is public.
        /// </summary>
        public bool WarLogsPublic { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IEnumerable{T}"/> of <see cref="ClanMember"/>.
        /// </summary>
        public IEnumerable<ClanMember> Members { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IEnumerable{T}"/> of <see cref="AllianceStreamEntry"/>.
        /// </summary>
        public IEnumerable<AllianceStreamEntry> Entries { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a <see cref="Clan"/> representing this <see cref="ClanSave"/> instance.
        /// </summary>
        /// <returns>A <see cref="Clan"/> representing this <see cref="ClanSave"/> instance.</returns>
        public Clan ToClan()
        {
            var c = new Clan();
            Overwrite(c);
            return c;
        }

        /// <summary>
        /// Loads the current <see cref="ClanSave"/> with the specified <see cref="Clan"/>.
        /// </summary>
        /// <param name="clan"><see cref="Clan"/> that the <see cref="ClanSave"/> is going to represent.</param>
        public void FromClan(Clan clan)
        {
            if (clan == null)
                throw new ArgumentNullException(nameof(clan));

            ClanId = clan.Id;
            Name = clan.Name;
            Description = clan.Description;
            ExpLevels = clan.ExpLevels;
            Badge = clan.Badge;
            InviteType = clan.InviteType;
            TotalTrophies = clan.TotalTrophies;
            RequiredTrophies = clan.RequiredTrophies;
            WarsWon = clan.WarsWon;
            WarsLost = clan.WarsLost;
            WarsTried = clan.WarsTried;
            Language = clan.Language;
            WarFrequency = clan.WarFrequency;
            Location = clan.Location;
            PerkPoints = clan.PerkPoints;
            WinStreak = clan.WinStreak;
            WarLogsPublic = clan.WarLogsPublic;
            Members = clan.Members;
            Entries = clan.Entries;
        }

        /// <summary>
        /// Overwrites the values in the specified <see cref="Clan"/> with the values in this <see cref="ClanSave"/> instance.
        /// </summary>
        /// <param name="clan"><see cref="Clan"/> to overwrite.</param>
        public void Overwrite(Clan clan)
        {
            if (clan == null)
                throw new ArgumentNullException(nameof(clan));

            clan.Id = ClanId;
            clan.Name = Name == null ? string.Empty : Name;
            clan.Description = Description == null ? string.Empty : Description;
            clan.ExpLevels = ExpLevels;
            clan.Badge = Badge;
            clan.InviteType = InviteType;
            clan.TotalTrophies = TotalTrophies;
            clan.RequiredTrophies = RequiredTrophies;
            clan.WarsWon = WarsWon;
            clan.WarsLost = WarsLost;
            clan.WarsTried = WarsTried;
            clan.Language = Language;
            clan.WarFrequency = WarFrequency;
            clan.Location = Location;
            clan.PerkPoints = PerkPoints;
            clan.WinStreak = WinStreak;
            clan.WarLogsPublic = WarLogsPublic;

            if (Members == null)
            {
                clan.Members = new List<ClanMember>();
            }
            else
            {
                if (Members is List<ClanMember>)
                    clan.Members = (List<ClanMember>)Members;
                else
                    clan.Members = Members.ToList();
            }

            if (Entries == null)
            {
                clan.Entries = new List<AllianceStreamEntry>();
            }
            else
            {
                if (Entries is List<ClanMember>)
                    clan.Entries = (List<AllianceStreamEntry>)Entries;
                else
                    clan.Entries = Entries.ToList();
            }
        }
        #endregion
    }
}
