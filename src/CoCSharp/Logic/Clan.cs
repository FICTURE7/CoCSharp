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
            _name = string.Empty;
            _description = string.Empty;
            _sync = new object();

            Members = new List<ClanMember>();
            Entries = new List<AllianceStreamEntry>();
        }
        #endregion

        #region Fields & Properties
        private string _name;
        private string _description;
        private int _level;

        // Clan instances should be thread safe, since multiple clients could
        // use the same instance.
        private readonly object _sync;

        /// <summary>
        /// Gets or sets the ID of the <see cref="Clan"/>.
        /// </summary>
        public long Id { get; set; }

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
                    throw new ArgumentNullException(nameof(value));

                _name = value;
            }
        }

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
                    throw new ArgumentNullException(nameof(value));

                _description = value;
            }
        }

        /// <summary>
        /// Gets or sets the level of the <see cref="Clan"/>.
        /// </summary>
        /// 
        /// <remarks>
        /// Client seems to crash when level is less than 1.
        /// </remarks>
        public int ExpLevels
        {
            get
            {
                return _level;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException(nameof(value), "value cannot be less 1.");

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
        /// Gets or sets the list of <see cref="AllianceStreamEntry"/> in the
        /// <see cref="Clan"/>.
        /// </summary>
        public List<AllianceStreamEntry> Entries { get; set; }

        /// <summary>
        /// Gets a new <see cref="AllianceDataResponseMessage"/>
        /// representing this <see cref="Clan"/> instance.
        /// </summary>
        public AllianceDataResponseMessage AllianceDataResponse
        {
            get
            {
                Update();

                var data = new AllianceDataResponseMessage();

                data.Clan = new ClanCompleteMessageComponent(this);
                data.Description = Description;
                data.Members = new ClanMemberMessageComponent[Members.Count];
                for (int i = 0; i < Members.Count; i++)
                {
                    data.Members[i] = new ClanMemberMessageComponent(Members[i])
                    {
                        Unknown4 = 1
                    };
                }

                return data;
            }
        }

        /// <summary>
        /// Gets a new <see cref="AllianceFullEntryMessage"/>
        /// representing this <see cref="Clan"/> instance.
        /// </summary>
        public AllianceFullEntryMessage AllianceFullEntry
        {
            get
            {
                Update();

                var data = new AllianceFullEntryMessage();
                data.Clan = new ClanCompleteMessageComponent(this);
                data.Description = Description;
                return data;
            }
        }

        /// <summary>
        /// Gets a new <see cref="AllianceStreamMessage"/> representing the <see cref="Clan.Entries"/> of this
        /// <see cref="Clan"/> instance.
        /// </summary>
        public AllianceStreamMessage AllianceStream
        {
            get
            {
                Update();

                var entries = Entries.ToArray();
                var stream = new AllianceStreamMessage
                {
                    Entries = entries
                };
                return stream;
            }
        }
        #endregion

        #region Methods
        public ChatAllianceStreamEntry Chat(long userId, string textMessage)
        {
            var member = Get(userId);
            if (member == null)
                return null;

            lock (_sync)
            {
                var caStreamEntry = new ChatAllianceStreamEntry
                {
                    EntryId = Entries.Count,

                    Unknown2 = 3,

                    HomeId = member.Id,
                    UserId = member.Id,
                    League = member.League,
                    ExpLevels = member.ExpLevels,
                    Name = member.Name,
                    Role = member.Role,

                    MessageText = textMessage,
                };

                // Add that to the clans so that it gets saves later on.
                // And sent again when the clan is loaded to the clients.
                Entries.Add(caStreamEntry);

                Update();
                return caStreamEntry;
            }
        }

        /// <summary>
        /// Finds a <see cref="ClanMember"/> with the same user ID as specified.
        /// </summary>
        /// <param name="userId">ID which the <see cref="ClanMember"/> must have.</param>
        /// <returns><see cref="ClanMember"/> with the same user ID. If not found returns null.</returns>
        public ClanMember Get(long userId)
        {
            lock (_sync)
            {
                for (int i = 0; i < Members.Count; i++)
                {
                    var member = Members[i];
                    if (member.Id == userId)
                        return member;
                }
                return null;
            }
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
        public JoinedOrLeftAllianceStream Join(Avatar avatar)
        {
            var member = Get(avatar.Id);
            if (member != null)
                return null;

            lock (_sync)
            {
                var jolStreamEntry = new JoinedOrLeftAllianceStream
                {
                    EntryId = Entries.Count,

                    Unknown2 = 3,

                    HomeId = avatar.Id,
                    UserId = avatar.Id,
                    League = avatar.League,
                    ExpLevels = avatar.ExpLevels,
                    Name = avatar.Name,
                    Role = ClanMemberRole.Member,

                    Action = 3, // Action = 3 => Joined, Action = 4 => Left.
                    ActorName = avatar.Name,
                    ActorUserId = avatar.Id,
                };

                var newMember = new ClanMember
                {
                    Id = avatar.Id,
                    Trophies = avatar.Trophies,
                    ExpLevels = avatar.ExpLevels,
                    Name = avatar.Name,
                    League = avatar.League,
                    Role = ClanMemberRole.Member,
                    Rank = Members.Count + 1,
                    PreviousRank = Members.Count + 1,
                    NewMember = true
                };

                Members.Add(newMember);

                Entries.Add(jolStreamEntry);
                Update();
                return jolStreamEntry;
            }
        }

        /// <summary>
        /// Removes a <see cref="ClanMember"/> with the same user ID as specified and returns <c>true</c>
        /// if succeeded; otherwise, <c>false</c>.
        /// </summary>
        /// <param name="userId">ID which the <see cref="ClanMember"/> must have.</param>
        /// <returns><c>true</c> if succeeded; otherwise, <c>false</c>.</returns>
        public JoinedOrLeftAllianceStream Leave(long userId)
        {
            lock (_sync)
            {
                for (int i = 0; i < Members.Count; i++)
                {
                    var member = Members[i];
                    if (member.Id == userId)
                    {
                        var jolStreamEntry = new JoinedOrLeftAllianceStream
                        {
                            EntryId = Entries.Count,

                            Unknown2 = 3,

                            HomeId = member.Id,
                            UserId = member.Id,
                            League = member.League,
                            ExpLevels = member.ExpLevels,
                            Name = member.Name,
                            Role = member.Role,

                            Action = 4, // Action = 3 => Joined, Action = 4 => Left.
                            ActorName = member.Name,
                            ActorUserId = member.Id,

                            SinceOccured = TimeSpan.FromSeconds(0)
                        };

                        Members.RemoveAt(i);

                        // Clamp the entries count to 100.
                        if (Entries.Count >= 100)
                            Entries.RemoveAt(0);

                        Entries.Add(jolStreamEntry);

                        Update();
                        return jolStreamEntry;
                    }
                }
            }
            return null;
        }

        public void Promote(long userId, ClanMemberRole newRole)
        {
            var member = Get(userId);

            // If a new member is promoted, he is not considered new anymore.
            if (member.NewMember)
                member.NewMember = false;

            member.Role = newRole;
        }

        public void Update()
        {
            var total = 0;
            // Insertion sorting of clan members by amount of trophies.
            for (int i = 1; i < Members.Count; i++)
            {
                var j = i;
                while (j > 0 && Members[j].Trophies > Members[j - 1].Trophies)
                {
                    // Swap [j] and [j -1].
                    var temp = Members[j - 1];
                    Members[j - 1] = Members[j];
                    Members[j] = temp;
                    j--;
                }
            }

            for (int i = 0; i < Members.Count; i++)
            {
                var member = Members[i];

                // Update the Rank and PreviousRank of member.
                var rank1 = i + 1;
                if (member.Rank != rank1)
                {
                    member.PreviousRank = member.Rank;
                    member.Rank = rank1;
                }

                // If its been more than 3 days since the player joined, he is not considered new anymore.
                if ((DateTime.UtcNow - member.DateJoined) >= TimeSpan.FromDays(3))
                    member.NewMember = false;

                // Calculate the number of clan trophies.
                var rank2 = member.Rank;
                var trophies = member.Trophies;
                var accountedTrophies = 0;
                if (rank2 >= 1 || rank2 <= 10)
                    accountedTrophies = (int)Math.Round(0.5 * trophies);
                else if (rank2 >= 11 || rank2 <= 20)
                    accountedTrophies = (int)Math.Round(0.25 * trophies);
                else if (rank2 >= 21 || rank2 <= 30)
                    accountedTrophies = (int)Math.Round(0.12 * trophies);
                else if (rank2 >= 31 || rank2 <= 40)
                    accountedTrophies = (int)Math.Round(0.10 * trophies);
                else if (rank2 >= 41 || rank2 <= 50)
                    accountedTrophies = (int)Math.Round(0.3 * trophies);

                total += accountedTrophies;
            }

            TotalTrophies = total;

            // Clamp the number of entries to 100 only.
            while (Entries.Count >= 100)
                Entries.RemoveAt(0);

            // Update the time since the entries were sent.
            for (int i = 0; i < Entries.Count; i++)
                Entries[i].Update();
        }
        #endregion
    }
}
