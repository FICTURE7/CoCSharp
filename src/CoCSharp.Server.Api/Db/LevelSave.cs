using CoCSharp.Data;
using CoCSharp.Data.Slots;
using CoCSharp.Logic;
using System;
using System.Collections.Generic;

namespace CoCSharp.Server.Api.Db
{
    // Simple object that can be converted into Levels and vice versa.
    // DbData -> LevelSave -> Level.
    // DbData <- LevelSave <- Level.

    /// <summary>
    /// Represents a <see cref="Level"/> save and is a simple wrapper around <see cref="Level"/>.
    /// </summary>
    public class LevelSave
    {
        #region Constructors
        internal LevelSave()
        {
            // Space
        }
        #endregion

        #region Fields & Properties
        /// <summary>
        /// Gets or sets the time <see cref="LevelSave"/> was first saved.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the last time the <see cref="LevelSave"/> was saved.
        /// </summary>
        public DateTime DateLastSave { get; set; }

        /// <summary>
        /// Gets or sets the amount of time the <see cref="LevelSave"/> was online.
        /// </summary>
        public TimeSpan PlayTime { get; set; }

        /// <summary>
        /// Gets or sets the amount of time the <see cref="LevelSave"/> was logged in.
        /// </summary>
        public int LoginCount { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Village"/> json.
        /// </summary>
        public string LevelJson { get; set; }

        /// <summary>
        /// Gets or sets the ID of the <see cref="LevelSave"/>.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the clan ID of the <see cref="LevelSave"/>.
        /// </summary>
        public long? ClanId { get; set; }

        /// <summary>
        /// Gets or sets the token of the <see cref="LevelSave"/>.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the name of the <see cref="LevelSave"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="LevelSave"/> was named.
        /// </summary>
        public bool IsNamed { get; set; }

        /// <summary>
        /// Gets or sets the number of trophies.
        /// </summary>
        public int Trophies { get; set; }

        /// <summary>
        /// Gets or sets the league.
        /// </summary>
        public int League { get; set; }

        /// <summary>
        /// Gets or sets the experience point.
        /// </summary>
        public int ExpPoints { get; set; }

        /// <summary>
        /// Gets or sets the experience level.
        /// </summary>
        public int ExpLevels { get; set; }

        /// <summary>
        /// Gets or sets the amount of gems owned.
        /// </summary>
        public int Gems { get; set; }

        /// <summary>
        /// Gets or sets the amount of free gems.
        /// </summary>
        public int FreeGems { get; set; }

        /// <summary>
        /// Gets or sets the amount of attacks won.
        /// </summary>
        public int AttacksWon { get; set; }

        /// <summary>
        /// Gets or sets the amount of attacks lost.
        /// </summary>
        public int AttacksLost { get; set; }

        /// <summary>
        /// Gets or sets the amount of defenses won.
        /// </summary>
        public int DefensesWon { get; set; }

        /// <summary>
        /// Gets or sets the amount of defenses lost.
        /// </summary>
        public int DefensesLost { get; set; }

        /// <summary>
        /// Gets or sets the capacity of each resources.
        /// </summary>
        public IEnumerable<ResourceCapacitySlot> ResourcesCapacity { get; set; }

        /// <summary>
        /// Gets or sets the amount of each resources.
        /// </summary>
        public IEnumerable<ResourceAmountSlot> ResourcesAmount { get; set; }

        /// <summary>
        /// Gets or sets the amount of units available.
        /// </summary>
        public IEnumerable<UnitSlot> Units { get; set; }

        /// <summary>
        /// Gets or sets the amount of spells available.
        /// </summary>
        public IEnumerable<SpellSlot> Spells { get; set; }

        /// <summary>
        /// Gets or sets unit upgrades.
        /// </summary>
        public IEnumerable<UnitUpgradeSlot> UnitUpgrades { get; set; }

        /// <summary>
        /// Gets or sets spells upgrades.
        /// </summary>
        public IEnumerable<SpellUpgradeSlot> SpellUpgrades { get; set; }

        /// <summary>
        /// Gets or sets heroes upgrades.
        /// </summary>
        public IEnumerable<HeroUpgradeSlot> HeroUpgrades { get; set; }

        /// <summary>
        /// Gets or sets heroes healths.
        /// </summary>
        public IEnumerable<HeroHealthSlot> HeroHealths { get; set; }

        /// <summary>
        /// Gets or sets heroes states.
        /// </summary>
        public IEnumerable<HeroStateSlot> HeroStates { get; set; }

        /// <summary>
        /// Gets or sets alliance units.
        /// </summary>
        public IEnumerable<AllianceUnitSlot> AllianceUnits { get; set; }

        /// <summary>
        /// Gets or sets the tutorial progress.
        /// </summary>
        public IEnumerable<TutorialProgressSlot> TutorialProgress { get; set; }

        /// <summary>
        /// Gets or sets achievements states.
        /// </summary>
        public IEnumerable<AchievementSlot> Achievements { get; set; }

        /// <summary>
        /// Gets or sets achievement progress.
        /// </summary>
        public IEnumerable<AchievementProgessSlot> AchievementProgress { get; set; }

        /// <summary>
        /// Gets or sets the NPC stars.
        /// </summary>
        public IEnumerable<NpcStarSlot> NpcStars { get; set; }

        /// <summary>
        /// Gets or sets the NPC gold.
        /// </summary>
        public IEnumerable<NpcGoldSlot> NpcGold { get; set; }

        /// <summary>
        /// Gets or sets the NPC elixir.
        /// </summary>
        public IEnumerable<NpcElixirSlot> NpcElixir { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Overwrites the values in the specified <see cref="Level"/> with the values in this <see cref="LevelSave"/> instance.
        /// </summary>
        /// <param name="level"><see cref="Level"/> to overwrite.</param>
        public void Overwrite(Level level)
        {
            if (level == null)
                throw new ArgumentNullException(nameof(level));

            level.Village = Village.FromJson(LevelJson, level);
            level.DateCreated = DateCreated;
            level.DateLastSave = DateLastSave;
            level.PlayTime = PlayTime;
            level.LoginCount = LoginCount;

            level.Token = Token;
            level.Avatar.Id = UserId;
            level.Avatar.Name = Name;
            level.Avatar.IsNamed = IsNamed;
            level.Avatar.League = League;
            level.Avatar.Trophies = Trophies;
            level.Avatar.ExpPoints = ExpPoints;
            level.Avatar.ExpLevels = ExpLevels;
            level.Avatar.Gems = Gems;
            level.Avatar.FreeGems = FreeGems;
            level.Avatar.AttacksWon = AttacksWon;
            level.Avatar.AttacksLost = AttacksLost;
            level.Avatar.DefensesWon = DefensesWon;
            level.Avatar.DefensesLost = DefensesLost;

            Replace(level.Avatar.ResourcesCapacity, ResourcesCapacity);
            Replace(level.Avatar.ResourcesAmount, ResourcesAmount);
            Replace(level.Avatar.Units, Units);
            Replace(level.Avatar.Spells, Spells);
            Replace(level.Avatar.UnitUpgrades, UnitUpgrades);
            Replace(level.Avatar.SpellUpgrades, SpellUpgrades);
            Replace(level.Avatar.HeroUpgrades, HeroUpgrades);
            Replace(level.Avatar.HeroHealths, HeroHealths);
            Replace(level.Avatar.HeroStates, HeroStates);
            Replace(level.Avatar.AllianceUnits, AllianceUnits);
            Replace(level.Avatar.TutorialProgess, TutorialProgress);
            Replace(level.Avatar.Achievements, Achievements);
            Replace(level.Avatar.AchievementProgress, AchievementProgress);
            Replace(level.Avatar.NpcStars, NpcStars);
            Replace(level.Avatar.NpcGold, NpcGold);
            Replace(level.Avatar.NpcElixir, NpcElixir);
        }

        /// <summary>
        /// Returns a <see cref="Level"/> representing this <see cref="LevelSave"/> instance.
        /// </summary>
        /// <returns>A <see cref="Level"/> representing this <see cref="LevelSave"/> instance.</returns>
        public Level ToLevel(AssetManager assets)
        {
            var level = new Level(assets);
            Overwrite(level);
            return level;
        }

        /// <summary>
        /// Loads the current <see cref="LevelSave"/> with the specified <see cref="Level"/>.
        /// </summary>
        /// <param name="level"><see cref="Level"/> that the <see cref="LevelSave"/> is going to represent.</param>
        public void FromLevel(Level level)
        {
            if (level == null)
                throw new ArgumentNullException(nameof(level));

            LevelJson = level.Village.ToJson();
            DateCreated = level.DateCreated;
            DateLastSave = level.DateLastSave;
            PlayTime = level.PlayTime;
            LoginCount = level.LoginCount;

            Token = level.Token;
            UserId = level.Avatar.Id;
            ClanId = level.Avatar.Alliance?.Id;
            Name = level.Avatar.Name;
            IsNamed = level.Avatar.IsNamed;
            League = level.Avatar.League;
            Trophies = level.Avatar.Trophies;
            ExpPoints = level.Avatar.ExpPoints;
            ExpLevels = level.Avatar.ExpLevels;
            Gems = level.Avatar.Gems;
            FreeGems = level.Avatar.FreeGems;
            AttacksWon = level.Avatar.AttacksWon;
            AttacksLost = level.Avatar.AttacksLost;
            DefensesWon = level.Avatar.DefensesWon;
            DefensesLost = level.Avatar.DefensesLost;

            ResourcesCapacity = level.Avatar.ResourcesCapacity;
            ResourcesAmount = level.Avatar.ResourcesAmount;
            Units = level.Avatar.Units;
            Spells = level.Avatar.Spells;
            UnitUpgrades = level.Avatar.UnitUpgrades;
            SpellUpgrades = level.Avatar.SpellUpgrades;
            HeroUpgrades = level.Avatar.HeroUpgrades;
            HeroHealths = level.Avatar.HeroHealths;
            HeroStates = level.Avatar.HeroStates;
            AllianceUnits = level.Avatar.AllianceUnits;
            TutorialProgress = level.Avatar.TutorialProgess;
            Achievements = level.Avatar.Achievements;
            AchievementProgress = level.Avatar.AchievementProgress;
            NpcStars = level.Avatar.NpcStars;
            NpcGold = level.Avatar.NpcGold;
            NpcElixir = level.Avatar.NpcElixir;
        }

        private static void Replace<T>(ICollection<T> dst, IEnumerable<T> src)
        {
            if (src == null)
                return;

            dst.Clear();
            foreach (var val in src)
                dst.Add(val);
        }
        #endregion
    }
}
