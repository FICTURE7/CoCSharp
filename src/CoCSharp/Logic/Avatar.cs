using CoCSharp.Csv;
using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Data.Slots;
using System;
using System.ComponentModel;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans avatar.
    /// </summary>
    public class Avatar : INotifyPropertyChanged
    {
        #region Constants
        private static readonly PropertyChangedEventArgs s_namedChanged = new PropertyChangedEventArgs(nameof(Name));
        private static readonly PropertyChangedEventArgs s_isNamedChanged = new PropertyChangedEventArgs(nameof(IsNamed));
        private static readonly PropertyChangedEventArgs s_idChanged = new PropertyChangedEventArgs(nameof(Id));
        private static readonly PropertyChangedEventArgs s_shieldEndTimeChanged = new PropertyChangedEventArgs(nameof(ShieldEndTime));
        private static readonly PropertyChangedEventArgs s_allianceChanged = new PropertyChangedEventArgs(nameof(Alliance));
        private static readonly PropertyChangedEventArgs s_leagueChanged = new PropertyChangedEventArgs(nameof(League));
        private static readonly PropertyChangedEventArgs s_expLevelChanged = new PropertyChangedEventArgs(nameof(ExpLevels));
        private static readonly PropertyChangedEventArgs s_expPointsChanged = new PropertyChangedEventArgs(nameof(ExpPoints));
        private static readonly PropertyChangedEventArgs s_gemsChanged = new PropertyChangedEventArgs(nameof(Gems));
        private static readonly PropertyChangedEventArgs s_freeGemsChanged = new PropertyChangedEventArgs(nameof(FreeGems));
        private static readonly PropertyChangedEventArgs s_trophiesChanged = new PropertyChangedEventArgs(nameof(Trophies));
        private static readonly PropertyChangedEventArgs s_attkWonChanged = new PropertyChangedEventArgs(nameof(AttacksWon));
        private static readonly PropertyChangedEventArgs s_attkLostChanged = new PropertyChangedEventArgs(nameof(AttacksLost));
        private static readonly PropertyChangedEventArgs s_defWonChanged = new PropertyChangedEventArgs(nameof(DefensesWon));
        private static readonly PropertyChangedEventArgs s_defLostChanged = new PropertyChangedEventArgs(nameof(DefensesLost));
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Avatar"/> class with the specified <see cref="Level"/>.
        /// </summary>
        /// <param name="level"><see cref="Level"/> which owns this <see cref="Avatar"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="level"/> is null.</exception>
        public Avatar(Level level)
        {
            if (level == null)
                throw new ArgumentNullException(nameof(level));

            _level = level;
            // If _level is less than 1 then the client crashes.
            _expLevel = 1;
            // If _name is null then the client crashes.
            _name = "unnamed";

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
            Achievements = new SlotCollection<AchievementSlot>();
            AchievementProgress = new SlotCollection<AchievementProgessSlot>();
            NpcStars = new SlotCollection<NpcStarSlot>();
            NpcGold = new SlotCollection<NpcGoldSlot>();
            NpcElixir = new SlotCollection<NpcElixirSlot>();
        }
        #endregion

        #region Fields & Properties
        private Level _level;

        private string _name;
        private bool _isNamed;

        // Also known as UserID.
        private long _id;

        private DateTime _shieldEndTime;

        private Clan _alliance;

        private int _expLevel;
        private int _expPoints;

        private int _league;

        private int _gems;
        private int _freeGems;

        private int _trophies;
        private int _attkWon;
        private int _attkLost;
        private int _defWon;
        private int _defLost;

        /// <summary>
        /// Gets or sets a value indicating whether to raise the <see cref="PropertyChanged"/> event.
        /// </summary>
        public bool IsPropertyChangedEnabled { get; set; }

        /// <summary>
        /// The event raised when a property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the <see cref="Logic.Level"/> which owns this <see cref="Avatar"/>.
        /// </summary>
        public Level Level => _level;

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
                // Client crashes if name is null.
                if (_name == null)
                    throw new ArgumentNullException(nameof(value));

                if (_name == value)
                    return;

                _name = value;
                OnPropertyChanged(s_namedChanged);
            }
        }

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

        /// <summary>
        /// Gets or sets the user ID of the <see cref="Avatar"/>.
        /// </summary>
        public long Id
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

        /// <summary>
        /// Gets or sets the level of the <see cref="Avatar"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than 1.</exception>
        public int ExpLevels
        {
            get
            {
                return _expLevel;
            }
            set
            {
                if (_expLevel == value)
                    return;

                // Clash of Clans client crashes when level is less than 1.
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value", "value cannot be less than 1.");

                _expLevel = value;
                OnPropertyChanged(s_expLevelChanged);
            }
        }

        /// <summary>
        /// Gets or sets the experience of the <see cref="Avatar"/>.
        /// </summary>
        public int ExpPoints
        {
            get
            {
                return _expPoints;
            }
            set
            {
                if (_expPoints == value)
                    return;

                _expPoints = value;
                OnPropertyChanged(s_expPointsChanged);
            }
        }

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
        public SlotCollection<AchievementSlot> Achievements { get; set; }

        /// <summary>
        /// Gets or sets the achievements progress.
        /// </summary>
        public SlotCollection<AchievementProgessSlot> AchievementProgress { get; set; }

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
        #endregion

        #region Methods
        public void UseResource(string resourceName, int amount)
        {
            var assets = _level.Assets;
            var resourceTable = assets.Get<CsvDataTable<ResourceData>>();
            var resourceId = resourceTable.Rows[resourceName].Id;

            var slot = ResourcesAmount.GetSlot(resourceId);
            if (slot == null)
            {
                ResourcesAmount.Add(new ResourceAmountSlot(resourceId, -amount));
            }
            else
            {
                if (amount > slot.Amount)
                    Level.Logs.Log("Resource transaction caused balance to switch to negative.");

                // Do some maths.
                slot.Amount -= amount;

                const string GEMS_RESOURCE_NAME = "Diamonds";
                // Not the neatest of ways, but it works.
                if (resourceName == GEMS_RESOURCE_NAME)
                {
                    Gems -= amount;
                    FreeGems -= amount;
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event with the specified <see cref="PropertyChangedEventArgs"/>.
        /// </summary>
        /// <param name="args">The data.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (IsPropertyChangedEnabled && PropertyChanged != null)
                PropertyChanged(this, args);
        }
        #endregion
    }
}
