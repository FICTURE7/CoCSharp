using CoCSharp.Network.Messages;
using System;
using System.Collections.Generic;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans clan (Alliance).
    /// </summary>
    public class Clan
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Clan"/>.
        /// </summary>
        public Clan()
        {
            _level = 1;
            _description = string.Empty;
            Members = new List<ClanMember>();
        }
        #endregion

        #region Fields & Properties
        /// <summary>
        /// Gets or sets the ID of the <see cref="Clan"/>.
        /// </summary>
        public long ID { get; set; }

        private string _name;
        /// <summary>
        /// Gets or sets the name of the <see cref="Clan"/>.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _name = value;
            }
        }

        private string _description;
        /// <summary>
        /// Gets or sets the description of the <see cref="Clan"/>.
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _description = value;
            }
        }

        private int _level;
        /// <summary>
        /// Gets or sets the level of the <see cref="Clan"/>.
        /// </summary>
        /// <remarks>
        /// Client seems to crash when level is less than 1.
        /// </remarks>
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                if (_level < 1)
                    throw new ArgumentOutOfRangeException("value", "value cannot be less 1.");

                _level = value;
            }
        }

        /// <summary>
        /// Gets or sets the badge of the <see cref="Clan"/>.
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
        /// Gets or sets the list <see cref="ClanMember"/> members in
        /// the <see cref="Clan"/>.
        /// </summary>
        public List<ClanMember> Members { get; set; }

        /// <summary>
        /// Gets a new <see cref="Network.Messages.AllianceDataResponseMessage"/>
        /// representing this <see cref="Clan"/> instance.
        /// </summary>
        public AllianceDataResponseMessage AllianceDataResponseMessage
        {
            get
            {
                var data = new AllianceDataResponseMessage();
                data.Clan = new ClanCompleteMessageComponent(this);
                data.Description = Description;
                data.Members = new ClanMemberMessageComponent[Members.Count];
                for (int i = 0; i < Members.Count; i++)
                    data.Members[i] = new ClanMemberMessageComponent(Members[i])
                    {
                        Unknown1 = 1
                    };
                return data;
            }
        }
        /// <summary>
        /// Gets a new <see cref="Network.Messages.AllianceFullEntryMessage"/>
        /// representing this <see cref="Clan"/> instance.
        /// </summary>
        public AllianceFullEntryMessage AllianceFullEntryMessage
        {
            get
            {
                var data = new AllianceFullEntryMessage();
                data.Clan = new ClanCompleteMessageComponent(this);
                data.Description = Description;
                return data;
            }
        }

        /// <summary>
        /// Finds a <see cref="ClanMember"/> with the same user ID as specified.
        /// </summary>
        /// <param name="id">ID which the <see cref="ClanMember"/> must have.</param>
        /// <returns><see cref="ClanMember"/> with the same user ID. If not found returns null.</returns>
        public ClanMember FindMember(long id)
        {
            for (int i = 0; i < Members.Count; i++)
            {
                var member = Members[i];
                if (member.ID == id)
                    return member;
            }
            return null;
        }

        /// <summary>
        /// Adds a <see cref="ClanMember"/> to the <see cref="Clan"/> from the specified
        /// <see cref="Avatar"/>. Returns <c>true</c> if succeeded; otherwise; false.
        /// </summary>
        /// <param name="avatar"><see cref="Avatar"/> to add to the <see cref="Clan"/>.</param>
        /// <returns>Returns <c>true</c> if succeeded; otherwise; false.</returns>
        /// 
        /// <remarks>
        /// Returns <c>false</c> if a <see cref="ClanMember"/> has the same ID as the specified <see cref="Avatar"/>.
        /// </remarks>
        public bool AddMember(Avatar avatar)
        {
            if (FindMember(avatar.ID) != null)
                return false;

            //TODO: Order by Trophy count.
            Members.Add(new ClanMember(avatar)
            {
                Role = ClanMemberRole.Member,
                Rank = Members.Count,
                PreviousRank = Members.Count
            });
            return true;
        }

        /// <summary>
        /// Removes a <see cref="ClanMember"/> with the same user ID as specified and returns <c>true</c>
        /// if succeeded; otherwise, <c>false</c>.
        /// </summary>
        /// <param name="id">ID which the <see cref="ClanMember"/> must have.</param>
        /// <returns><c>true</c> if succeeded; otherwise, <c>false</c>.</returns>
        public bool RemoveMember(long id)
        {
            for (int i = 0; i < Members.Count; i++)
            {
                if (Members[i].ID == id)
                {
                    Members.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
