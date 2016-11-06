using CoCSharp.Data;
using CoCSharp.Data.Slots;
using CoCSharp.Logic;
using System;
using System.Collections.Generic;

namespace CoCSharp.Server.API.Core
{
    /// <summary>
    /// Represents a <see cref="Level"/> save.
    /// </summary>
    public interface ILevelSave
    {
        #region Fields & Properties
        /// <summary>
        /// Gets or sets the <see cref="Village"/> json.
        /// </summary>
        string VillageJson { get; set; }

        /// <summary>
        /// Gets or sets the last time the <see cref="ILevelSave"/> was saved.
        /// </summary>
        DateTime LastSave { get; set; }

        /// <summary>
        /// Gets or sets the ID of the <see cref="ILevelSave"/>.
        /// </summary>
        long ID { get; set; }

        /// <summary>
        /// Gets or sets the token of the <see cref="ILevelSave"/>.
        /// </summary>
        string Token { get; set; }

        /// <summary>
        /// Gets or sets the name of the <see cref="ILevelSave"/>.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="ILevelSave"/> was named.
        /// </summary>
        bool IsNamed { get; set; }

        /// <summary>
        /// Gets or sets the number of trophies.
        /// </summary>
        int Trophies { get; set; }

        /// <summary>
        /// Gets or sets the league.
        /// </summary>
        int League { get; set; }

        /// <summary>
        /// Gets or sets the experience point.
        /// </summary>
        int ExpPoints { get; set; }

        /// <summary>
        /// Gets or sets the experience level.
        /// </summary>
        int ExpLevel { get; set; }

        /// <summary>
        /// Gets or sets the amount of gems owned.
        /// </summary>
        int Gems { get; set; }

        /// <summary>
        /// Gets or sets the amount of free gems.
        /// </summary>
        int FreeGems { get; set; }

        /// <summary>
        /// Gets or sets the amount of attacks won.
        /// </summary>
        int AttacksWon { get; set; }

        /// <summary>
        /// Gets or sets the amount of attacks lost.
        /// </summary>
        int AttacksLost { get; set; }

        /// <summary>
        /// Gets or sets the amount of defenses won.
        /// </summary>
        int DefensesWon { get; set; }

        /// <summary>
        /// Gets or sets the amount of defenses lost.
        /// </summary>
        int DefensesLost { get; set; }

        /// <summary>
        /// Gets or sets the capacity of each resources.
        /// </summary>
        IEnumerable<ResourceCapacitySlot> ResourcesCapacity { get; set; }

        /// <summary>
        /// Gets or sets the amount of each resources.
        /// </summary>
        IEnumerable<ResourceAmountSlot> ResourcesAmount { get; set; }

        /// <summary>
        /// Gets or sets the amount of units available.
        /// </summary>
        IEnumerable<UnitSlot> Units { get; set; }

        /// <summary>
        /// Gets or sets the amount of spells available.
        /// </summary>
        IEnumerable<SpellSlot> Spells { get; set; }

        /// <summary>
        /// Gets or sets unit upgrades.
        /// </summary>
        IEnumerable<UnitUpgradeSlot> UnitUpgrades { get; set; }

        /// <summary>
        /// Gets or sets spells upgrades.
        /// </summary>
        IEnumerable<SpellUpgradeSlot> SpellUpgrades { get; set; }

        /// <summary>
        /// Gets or sets heroes upgrades.
        /// </summary>
        IEnumerable<HeroUpgradeSlot> HeroUpgrades { get; set; }

        /// <summary>
        /// Gets or sets heroes healths.
        /// </summary>
        IEnumerable<HeroHealthSlot> HeroHealths { get; set; }

        /// <summary>
        /// Gets or sets heroes states.
        /// </summary>
        IEnumerable<HeroStateSlot> HeroStates { get; set; }

        /// <summary>
        /// Gets or sets alliance units.
        /// </summary>
        IEnumerable<AllianceUnitSlot> AllianceUnits { get; set; }

        /// <summary>
        /// Gets or sets achievements states.
        /// </summary>
        IEnumerable<AchievementSlot> Achievements { get; set; }

        /// <summary>
        /// Gets or sets achievement progress.
        /// </summary>
        IEnumerable<AchievementProgessSlot> AchievementProgress { get; set; }

        /// <summary>
        /// Gets or sets the NPC stars.
        /// </summary>
        IEnumerable<NpcStarSlot> NpcStars { get; set; }

        /// <summary>
        /// Gets or sets the NPC gold.
        /// </summary>
        IEnumerable<NpcGoldSlot> NpcGold { get; set; }

        /// <summary>
        /// Gets or sets the NPC elixir.
        /// </summary>
        IEnumerable<NpcElixirSlot> NpcElixir { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a <see cref="Level"/> representing this <see cref="ILevelSave"/> instance.
        /// </summary>
        /// <returns>A <see cref="Level"/> representing this <see cref="ILevelSave"/> instance.</returns>
        Level ToLevel(AssetManager assets);

        /// <summary>
        /// Returns an <see cref="ILevelSave"/> representing the specified <see cref="Level"/>.
        /// </summary>
        /// <param name="level"><see cref="Level"/> that the <see cref="ILevelSave"/> is going to represent.</param>
        void FromLevel(Level level);

        /// <summary>
        /// Overwrites the values in the specified <see cref="Level"/> with the values in this <see cref="ILevelSave"/> instance.
        /// </summary>
        /// <param name="level"><see cref="Level"/> to overwrite.</param>
        void Overwrite(Level level);
        #endregion
    }
}
