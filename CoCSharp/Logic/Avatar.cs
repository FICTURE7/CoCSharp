using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Data.Slots;
using CoCSharp.Network.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans avatar.
    /// </summary>
    public class Avatar
    {
        #region Constants
        private static readonly PropertyChangedEventArgs s_namedChanged = new PropertyChangedEventArgs("Name");
        private static readonly PropertyChangedEventArgs s_isNamedChanged = new PropertyChangedEventArgs("IsNamed");
        private static readonly PropertyChangedEventArgs s_tokenChanged = new PropertyChangedEventArgs("Token");
        private static readonly PropertyChangedEventArgs s_idChanged = new PropertyChangedEventArgs("ID");
        private static readonly PropertyChangedEventArgs s_shieldEndTimeChanged = new PropertyChangedEventArgs("ShieldEndTime");
        private static readonly PropertyChangedEventArgs s_homeChanged = new PropertyChangedEventArgs("Home");
        private static readonly PropertyChangedEventArgs s_allianceChanged = new PropertyChangedEventArgs("Alliance");
        private static readonly PropertyChangedEventArgs s_leagueChanged = new PropertyChangedEventArgs("League");
        private static readonly PropertyChangedEventArgs s_levelChanged = new PropertyChangedEventArgs("Level");
        private static readonly PropertyChangedEventArgs s_experienceChanged = new PropertyChangedEventArgs("Experience");
        private static readonly PropertyChangedEventArgs s_gemsChanged = new PropertyChangedEventArgs("Gems");
        private static readonly PropertyChangedEventArgs s_freeGemsChanged = new PropertyChangedEventArgs("FreeGems");
        private static readonly PropertyChangedEventArgs s_trophiesChanged = new PropertyChangedEventArgs("Trophies");
        private static readonly PropertyChangedEventArgs s_attkWonChanged = new PropertyChangedEventArgs("AttacksWon");
        private static readonly PropertyChangedEventArgs s_attkLostChanged = new PropertyChangedEventArgs("AttacksLost");
        private static readonly PropertyChangedEventArgs s_defWonChanged = new PropertyChangedEventArgs("DefensesWon");
        private static readonly PropertyChangedEventArgs s_defLostChanged = new PropertyChangedEventArgs("DefensesLost");
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Avatar"/> class.
        /// </summary>
        public Avatar()
        {
            // If _level is less that 1 then the client crashes.
            _level = 1;
        }
        #endregion

        #region Fields & Properties
        /// <summary>
        /// Gets or sets a value indicating whether to raise the <see cref="PropertyChanged"/> event.
        /// </summary>
        public bool IsPropertyChangedEnabled { get; set; }

        /// <summary>
        /// The event raised when a property value has changed.
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs> PropertyChanged;

        private string _name;
        /// <summary>
        /// Gets or sets the username of the <see cref="Avatar"/>.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name == value)
                    return;

                OnPropertyChanged(s_namedChanged);
                _name = value;
            }
        }

        private bool _isNamed;
        /// <summary>
        /// Gets or sets whether the <see cref="Avatar"/> has been named.
        /// </summary>
        public bool IsNamed
        {
            get
            {
                return _isNamed;
            }
            set
            {
                if (_isNamed == value)
                    return;

                OnPropertyChanged(s_isNamedChanged);
                _isNamed = value;
            }
        }

        private string _token;
        /// <summary>
        /// Gets or sets the user token of the <see cref="Avatar"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is not a valid token.</exception>
        public string Token
        {
            get
            {
                return _token;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("value");
                if (!TokenUtils.CheckToken(value))
                    throw new ArgumentException("'" + value + "' is not a valid token.");
                if (_token == value)
                    return;

                OnPropertyChanged(s_tokenChanged);
                _token = value;
            }
        }

        // Also known as UserID.
        private long _id;
        /// <summary>
        /// Gets or sets the user ID of the <see cref="Avatar"/>.
        /// </summary>
        public long ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id == value)
                    return;

                OnPropertyChanged(s_idChanged);
                _id = value;
            }
        }

        /// <summary>
        /// Gets the shield duration of the <see cref="Avatar"/>.
        /// </summary>
        public TimeSpan ShieldDuration
        {
            get
            {
                var duration = DateTimeConverter.ToUnixTimestamp(ShieldEndTime) - DateTimeConverter.UnixUtcNow;

                if (duration < 0)
                    return TimeSpan.FromSeconds(0);

                return TimeSpan.FromSeconds(duration);
            }
        }

        private DateTime _shieldEndTime;
        /// <summary>
        /// Gets or sets the shield UTC end time of the <see cref="Avatar"/>.
        /// </summary>
        /// <exception cref="ArgumentException"><paramref name="value"/>.Kind is not <see cref="DateTimeKind.Utc"/>.</exception>
        public DateTime ShieldEndTime
        {
            get
            {
                return _shieldEndTime;
            }
            set
            {
                //if (value.Kind != DateTimeKind.Utc)
                //    throw new ArgumentException("value.Kind must be a kind of DateTimeKind.Utc.", "value");

                if (_shieldEndTime == value)
                    return;

                OnPropertyChanged(s_shieldEndTimeChanged);
                _shieldEndTime = value;
            }
        }

        private Village _home;
        /// <summary>
        /// Gets or sets the <see cref="Village"/> associated with this <see cref="Avatar"/>.
        /// </summary>
        public Village Home
        {
            get
            {
                return _home;
            }
            set
            {
                if (_home == value)
                    return;

                OnPropertyChanged(s_homeChanged);
                _home = value;
            }
        }

        private Clan _alliance;
        /// <summary>
        /// Gets or sets the <see cref="Clan"/> associated with this <see cref="Avatar"/>.
        /// </summary>
        public Clan Alliance
        {
            get
            {
                return _alliance;
            }
            set
            {
                if (_alliance == value)
                    return;

                OnPropertyChanged(s_allianceChanged);
                _alliance = value;
            }
        }

        private int _league;
        /// <summary>
        /// Gets or sets the league of the <see cref="Avatar"/>.
        /// </summary>
        public int League
        {
            get
            {
                return _league;
            }
            set
            {
                if (_league == value)
                    return;

                OnPropertyChanged(s_leagueChanged);
                _league = value;
            }
        }

        private int _level;
        /// <summary>
        /// Gets or sets the level of the <see cref="Avatar"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than 1.</exception>
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                if (_level == value)
                    return;

                // Clash of Clans client crashes when level is less than 1.
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", "value cannot be less than 1.");

                OnPropertyChanged(s_levelChanged);
                _level = value;
            }
        }

        private int _experience;
        /// <summary>
        /// Gets or sets the experience of the <see cref="Avatar"/>.
        /// </summary>
        public int Experience
        {
            get
            {
                return _experience;
            }
            set
            {
                if (_experience == value)
                    return;

                OnPropertyChanged(s_experienceChanged);
                _experience = value;
            }
        }

        private int _gems;
        /// <summary>
        /// Gets or sets the amount of gems of the <see cref="Avatar"/>.
        /// </summary>
        public int Gems
        {
            get
            {
                return _gems;
            }
            set
            {
                if (_gems == value)
                    return;

                OnPropertyChanged(s_gemsChanged);
                _gems = value;
            }
        }

        private int _freeGems;
        /// <summary>
        /// Gets or sets the amount of free gems of the <see cref="Avatar"/>.
        /// </summary>
        public int FreeGems
        {
            get
            {
                return _freeGems;
            }
            set
            {
                if (_freeGems == value)
                    return;

                OnPropertyChanged(s_freeGemsChanged);
                _freeGems = value;
            }
        }

        private int _trophies;
        /// <summary>
        /// Gets or sets the amount of trophies of the <see cref="Avatar"/>.
        /// </summary>
        public int Trophies
        {
            get
            {
                return _trophies;
            }
            set
            {
                if (_trophies == value)
                    return;

                OnPropertyChanged(s_trophiesChanged);
                _trophies = value;
            }
        }

        private int _attkWon;
        /// <summary>
        /// Gets or sets the number of attacks won by the <see cref="Avatar"/>.
        /// </summary>
        public int AttacksWon
        {
            get
            {
                return _attkWon;
            }
            set
            {
                if (_attkWon == value)
                    return;

                OnPropertyChanged(s_attkWonChanged);
                _attkWon = value;
            }
        }

        private int _attkLost;
        /// <summary>
        /// Gets or sets the number of attacks lost by the <see cref="Avatar"/>.
        /// </summary>
        public int AttacksLost
        {
            get
            {
                return _attkLost;
            }
            set
            {
                if (_attkLost == value)
                    return;

                OnPropertyChanged(s_attkLostChanged);
                _attkLost = value;
            }
        }

        private int _defWon;
        /// <summary>
        /// Gets or sets the number of defenses won by the <see cref="Avatar"/>.
        /// </summary>
        public int DefensesWon
        {
            get
            {
                return _defWon;
            }
            set
            {
                if (_defWon == value)
                    return;

                OnPropertyChanged(s_defWonChanged);
                _defWon = value;
            }
        }

        private int _defLost;
        /// <summary>
        /// Gets or sets the number of defenses lost by the <see cref="Avatar"/>.
        /// </summary>
        public int DefensesLost
        {
            get
            {
                return _defLost;
            }
            set
            {
                if (_defLost == value)
                    return;

                OnPropertyChanged(s_defLostChanged);           
                _defLost = value;
            }
        }

        /// <summary>
        /// Gets or sets the resources capacity.
        /// </summary>
        public ResourceCapacitySlot[] ResourcesCapacity { get; set; }

        /// <summary>
        /// Gets or sets the amount of resources available.
        /// </summary>
        public ResourceAmountSlot[] ResourcesAmount { get; set; }

        /// <summary>
        /// Gets or sets the units available.
        /// </summary>
        public UnitSlot[] Units { get; set; }

        /// <summary>
        /// Gets or sets the spells available.
        /// </summary>
        public SpellSlot[] Spells { get; set; }

        /// <summary>
        /// Gets or sets the units upgrades.
        /// </summary>
        public UnitUpgradeSlot[] UnitUpgrades { get; set; }

        /// <summary>
        /// Gets or sets the spells upgrades.
        /// </summary>
        public SpellUpgradeSlot[] SpellUpgrades { get; set; }

        /// <summary>
        /// Gets or sets the heroes upgrades.
        /// </summary>
        public HeroUpgradeSlot[] HeroUpgrades { get; set; }

        /// <summary>
        /// Gets or sets the heroes health.
        /// </summary>
        public HeroHealthSlot[] HeroHealths { get; set; }

        /// <summary>
        /// Gets or sets the heroes states.
        /// </summary>
        public HeroStateSlot[] HeroStates { get; set; }

        /// <summary>
        /// Gets or sets the alliance units.
        /// </summary>
        public AllianceUnitSlot[] AllianceUnits { get; set; }

        /// <summary>
        /// Get or sets the tutorial progress.
        /// </summary>
        public TutorialProgressSlot[] TutorialProgess { get; set; }

        /// <summary>
        /// Gets or sets the achievements state.
        /// </summary>
        public AchievementSlot[] Acheivements { get; set; }

        /// <summary>
        /// Gets or sets the achievements progress.
        /// </summary>
        public AchievementProgessSlot[] AcheivementProgress { get; set; }

        /// <summary>
        /// Gets or sets the NPC stars.
        /// </summary>
        public NpcStarSlot[] NpcStars { get; set; }

        /// <summary>
        /// Gets or sets the NPC gold.
        /// </summary>
        public NpcGoldSlot[] NpcGold { get; set; }

        /// <summary>
        /// Gets or sets the NPC elixir.
        /// </summary>
        public NpcElixirSlot[] NpcElixir { get; set; }

        /// <summary>
        /// Gets a new <see cref="Network.Messages.OwnHomeDataMessage"/> for the
        /// <see cref="Avatar"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="Home"/> is null.</exception>
        public OwnHomeDataMessage OwnHomeDataMessage
        {
            get
            {
                if (Home == null)
                    throw new InvalidOperationException("Home cannot be null.");

                var villageData = new VillageMessageComponent(this);
                var avatarData = new AvatarMessageComponent(this);
                var ohdMessage = new OwnHomeDataMessage()
                {
                    OwnVillageData = villageData,
                    OwnAvatarData = avatarData,
                    Unknown4 = 1462629754000,
                    Unknown5 = 1462629754000,
                    Unknown6 = 1462631554000,
                };

                return ohdMessage;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Updates all <see cref="Slot"/> array of the <see cref="Avatar"/> with the specified
        /// <see cref="AssetManager"/>.
        /// </summary>
        /// <param name="manager"><see cref="AssetManager"/> from which data will be used.</param>
        public void UpdateSlots(AssetManager manager)
        {
            // Update the resource capacity of the avatar according to
            // its Home Village object.

            //UpdateResourceCapacity(manager);
        }

        private void UpdateResourceCapacity(AssetManager manager)
        {
            var capacitySlot = new List<ResourceCapacitySlot>();

            // Total capacity.
            var gold = 0;
            var elixir = 0;
            var darkElixir = 0;
            var warGold = 0;
            var warElixir = 0;
            var warDarkElixir = 0;

            for (int i = 0; i < Home.Buildings.Count; i++)
            {
                var building = Home.Buildings[i];
                // Ignore the building if its locked (from village.json). E.g: Clan Castle level 0.
                if (building.IsLocked)
                    continue;

                // If the building does not have any data,
                // we ignore it.
                if (building.Data == null)
                    continue;

                var data = (BuildingData)building.Data;

                // Ignore the building if its locked (from building.csv). E.g: Clan Castle level 0.
                if (data.Locked)
                    continue;

                //if (data.MaxStoredGold > 0)
                gold += data.MaxStoredGold;
                //if (data.MaxStoredWarGold > 0)
                warGold += data.MaxStoredWarGold;
                //if (data.MaxStoredElixir > 0)
                elixir += data.MaxStoredElixir;
                //if (data.MaxStoredWarElixir > 0)
                warElixir += data.MaxStoredWarElixir;
                //if (data.MaxStoredDarkElixir > 0)
                darkElixir += data.MaxStoredDarkElixir;
                //if (data.MaxStoredWarDarkElixir > 0)
                warDarkElixir += data.MaxStoredWarDarkElixir;
            }

            if (gold > 0)
                capacitySlot.Add(new ResourceCapacitySlot(manager.SearchCsv<ResourceData>("TID_GOLD", 0).ID, gold));
            if (warGold > 0)
                capacitySlot.Add(new ResourceCapacitySlot(manager.SearchCsv<ResourceData>("TID_WAR_GOLD", 0).ID, warGold));
            if (elixir > 0)
                capacitySlot.Add(new ResourceCapacitySlot(manager.SearchCsv<ResourceData>("TID_ELIXIR", 0).ID, elixir));
            if (warElixir > 0)
                capacitySlot.Add(new ResourceCapacitySlot(manager.SearchCsv<ResourceData>("TID_WAR_ELIXIR", 0).ID, warElixir));
            if (darkElixir > 0)
                capacitySlot.Add(new ResourceCapacitySlot(manager.SearchCsv<ResourceData>("TID_DARK_ELIXIR", 0).ID, darkElixir));
            if (warDarkElixir > 0)
                capacitySlot.Add(new ResourceCapacitySlot(manager.SearchCsv<ResourceData>("TID_WAR_DARK_ELIXIR", 0).ID, warDarkElixir));

            ResourcesCapacity = capacitySlot.ToArray();
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event with the specified <see cref="PropertyChangedEventArgs"/>.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null && IsPropertyChangedEnabled)
                PropertyChanged(this, args);
        }
        #endregion
    }
}
