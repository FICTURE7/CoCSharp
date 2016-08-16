using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Data.Slots;
using CoCSharp.Network.Messages;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans avatar.
    /// </summary>
    public class Avatar : INotifyPropertyChanged
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
            // If _level is less than 1 then the client crashes.
            _level = 1;

            ResourcesCapacity = new SlotCollection<ResourceCapacitySlot>();
            ResourcesAmount = new SlotCollection<ResourceAmountSlot>();
            Units = new SlotCollection<UnitSlot>();
            Spells = new SlotCollection<SpellSlot>();
            UnitUpgrades = new SlotCollection<UnitUpgradeSlot>();
            SpellUpgrades = new SlotCollection<SpellUpgradeSlot>();
            HeroUpgrades = new SlotCollection<HeroUpgradeSlot>();
            HeroHealths = new SlotCollection<HeroHealthSlot>();
            HeroStates = new SlotCollection<HeroStateSlot>();
            AllianceUnits = new SlotCollection<AllianceUnitSlot>();
            TutorialProgess = new SlotCollection<TutorialProgressSlot>();
            Acheivements = new SlotCollection<AchievementSlot>();
            AcheivementProgress = new SlotCollection<AchievementProgessSlot>();
            NpcStars = new SlotCollection<NpcStarSlot>();
            NpcGold = new SlotCollection<NpcGoldSlot>();
            NpcElixir = new SlotCollection<NpcElixirSlot>();
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
        public event PropertyChangedEventHandler PropertyChanged;

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

                _name = value;
                OnPropertyChanged(s_namedChanged);
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

                _isNamed = value;
                OnPropertyChanged(s_isNamedChanged);
            }
        }

        private string _token;
        /// <summary>
        /// Gets or sets the user token of the <see cref="Avatar"/>.
        /// </summary>
        public string Token
        {
            get
            {
                return _token;
            }
            set
            {
                if (_token == value)
                    return;

                _token = value;
                OnPropertyChanged(s_tokenChanged);
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

                _id = value;
                OnPropertyChanged(s_idChanged);
            }
        }

        /// <summary>
        /// Gets the shield duration of the <see cref="Avatar"/>.
        /// </summary>
        public TimeSpan ShieldDuration
        {
            get
            {
                var duration = TimeUtils.ToUnixTimestamp(ShieldEndTime) - TimeUtils.UnixUtcNow;

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
                if (_shieldEndTime == value)
                    return;

                _shieldEndTime = value;
                OnPropertyChanged(s_shieldEndTimeChanged);
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

                _home = value;
                OnPropertyChanged(s_homeChanged);
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

                _alliance = value;
                OnPropertyChanged(s_allianceChanged);
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

                _league = value;
                OnPropertyChanged(s_leagueChanged);
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

                _level = value;
                OnPropertyChanged(s_levelChanged);
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

                _experience = value;
                OnPropertyChanged(s_experienceChanged);
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

                _gems = value;
                OnPropertyChanged(s_gemsChanged);
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

                _freeGems = value;
                OnPropertyChanged(s_freeGemsChanged);
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

                _trophies = value;
                OnPropertyChanged(s_trophiesChanged);
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

                _attkWon = value;
                OnPropertyChanged(s_attkWonChanged);
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

                _attkLost = value;
                OnPropertyChanged(s_attkLostChanged);
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

                _defWon = value;
                OnPropertyChanged(s_defWonChanged);
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

                _defLost = value;
                OnPropertyChanged(s_defLostChanged);
            }
        }

        /// <summary>
        /// Gets or sets the resources capacity.
        /// </summary>
        public SlotCollection<ResourceCapacitySlot> ResourcesCapacity { get; set; }

        /// <summary>
        /// Gets or sets the amount of resources available.
        /// </summary>
        public SlotCollection<ResourceAmountSlot> ResourcesAmount { get; set; }

        /// <summary>
        /// Gets or sets the units available.
        /// </summary>
        public SlotCollection<UnitSlot> Units { get; set; }

        /// <summary>
        /// Gets or sets the spells available.
        /// </summary>
        public SlotCollection<SpellSlot> Spells { get; set; }

        /// <summary>
        /// Gets or sets the units upgrades.
        /// </summary>
        public SlotCollection<UnitUpgradeSlot> UnitUpgrades { get; set; }

        /// <summary>
        /// Gets or sets the spells upgrades.
        /// </summary>
        public SlotCollection<SpellUpgradeSlot> SpellUpgrades { get; set; }

        /// <summary>
        /// Gets or sets the heroes upgrades.
        /// </summary>
        public SlotCollection<HeroUpgradeSlot> HeroUpgrades { get; set; }

        /// <summary>
        /// Gets or sets the heroes health.
        /// </summary>
        public SlotCollection<HeroHealthSlot> HeroHealths { get; set; }

        /// <summary>
        /// Gets or sets the heroes states.
        /// </summary>
        public SlotCollection<HeroStateSlot> HeroStates { get; set; }

        /// <summary>
        /// Gets or sets the alliance units.
        /// </summary>
        public SlotCollection<AllianceUnitSlot> AllianceUnits { get; set; }

        /// <summary>
        /// Get or sets the tutorial progress.
        /// </summary>
        public SlotCollection<TutorialProgressSlot> TutorialProgess { get; set; }

        /// <summary>
        /// Gets or sets the achievements state.
        /// </summary>
        public SlotCollection<AchievementSlot> Acheivements { get; set; }

        /// <summary>
        /// Gets or sets the achievements progress.
        /// </summary>
        public SlotCollection<AchievementProgessSlot> AcheivementProgress { get; set; }

        /// <summary>
        /// Gets or sets the NPC stars.
        /// </summary>
        public SlotCollection<NpcStarSlot> NpcStars { get; set; }

        /// <summary>
        /// Gets or sets the NPC gold.
        /// </summary>
        public SlotCollection<NpcGoldSlot> NpcGold { get; set; }

        /// <summary>
        /// Gets or sets the NPC elixir.
        /// </summary>
        public SlotCollection<NpcElixirSlot> NpcElixir { get; set; }

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

                UpdateSlots();

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
        /// Updates all <see cref="SlotCollection{TSot}"/> of the <see cref="Avatar"/> with <see cref="Home"/>'s <see cref="Village.AssetManager"/>
        /// instance.
        /// </summary>
        /// 
        /// <exception cref="InvalidOperationException"><see cref="Home"/> is null.</exception>
        public void UpdateSlots()
        {
            if (Home == null)
                throw new InvalidOperationException("Home cannot be null.");

            Debug.Assert(Home.AssetManager != null);
            // Update the resource capacity of the avatar according to
            // its Home Village object.
            var manager = Home.AssetManager;
            ResourcesCapacity.Clear();

            // Total capacity.
            var gold = 0;
            var elixir = 0;
            var darkElixir = 0;
            var warGold = 0;
            var warElixir = 0;
            var warDarkElixir = 0;

            foreach (var building in Home.Buildings)
            {
                // Ignore the building if its locked (from village.json). E.g: Clan Castle level 0.
                if (building.IsLocked)
                    continue;

                var data = building.Data;
                if (data == null)
                    continue;

                // Ignore the building if its locked (from building.csv). E.g: Clan Castle level 0.
                if (data.Locked)
                    continue;

                gold += data.MaxStoredGold;
                warGold += data.MaxStoredWarGold;
                elixir += data.MaxStoredElixir;
                warElixir += data.MaxStoredWarElixir;
                darkElixir += data.MaxStoredDarkElixir;
                warDarkElixir += data.MaxStoredWarDarkElixir;
            }

            ResourcesCapacity.Add(new ResourceCapacitySlot(manager.SearchCsv<ResourceData>("TID_GOLD", 0).ID, gold));
            if (warGold > 0)
                ResourcesCapacity.Add(new ResourceCapacitySlot(manager.SearchCsv<ResourceData>("TID_WAR_GOLD", 0).ID, warGold));

            ResourcesCapacity.Add(new ResourceCapacitySlot(manager.SearchCsv<ResourceData>("TID_ELIXIR", 0).ID, elixir));
            if (warElixir > 0)
                ResourcesCapacity.Add(new ResourceCapacitySlot(manager.SearchCsv<ResourceData>("TID_WAR_ELIXIR", 0).ID, warElixir));

            // Dark Elixir is unlocked at townhall 7.
            if (darkElixir > 0 || Home.TownHall.Level >= 7)
                ResourcesCapacity.Add(new ResourceCapacitySlot(manager.SearchCsv<ResourceData>("TID_DARK_ELIXIR", 0).ID, darkElixir));
            if (warDarkElixir > 0)
                ResourcesCapacity.Add(new ResourceCapacitySlot(manager.SearchCsv<ResourceData>("TID_WAR_DARK_ELIXIR", 0).ID, warDarkElixir));
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event with the specified <see cref="PropertyChangedEventArgs"/>.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (IsPropertyChangedEnabled && PropertyChanged != null)
                PropertyChanged(this, args);
        }
        #endregion
    }
}
