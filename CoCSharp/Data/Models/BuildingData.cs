using CoCSharp.Csv;
using System;

namespace CoCSharp.Data.Models
{
    /// <summary>
    /// Defines data from the logic/buildings.csv file.
    /// </summary>
    public class BuildingData : CsvData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingData"/> class.
        /// </summary>
        public BuildingData()
        {
            // Space
        }

        #region Fields & Properties
        internal override int BaseDataID
        {
            get { return 1000000; }
        }

        /// <summary>
        /// Gets or sets the Name of building.
        /// </summary>
        [CsvAlias("          ")]  // Its named like this in buildings.csv.
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the TID Instructor.
        /// </summary>
        [CsvAlias("TID_Instructor")]
        public string TIDInstructor { get; set; }
        /// <summary>
        /// Gets or sets the Instructor weight.
        /// </summary>
        public int InstructorWeight { get; set; }
        /// <summary>
        /// Gets or sets the Info TID.
        /// </summary>
        public string InfoTID { get; set; }
        /// <summary>
        /// Gets or sets the SWF asset location.
        /// </summary>
        public string SWF { get; set; }
        /// <summary>
        /// Gets or sets the Building class.
        /// </summary>
        public string BuildingClass { get; set; }
        /// <summary>
        /// Gets or sets the Export name.
        /// </summary>
        public string ExportName { get; set; }
        /// <summary>
        /// Gets or sets the Export name NPC.
        /// </summary>
        public string ExportNameNpc { get; set; }
        /// <summary>
        /// Gets or sets the Export name construction.
        /// </summary>
        public string ExportNameConstruction { get; set; }

        /// <summary>
        /// Gets or sets the build time.
        /// </summary>
        [CsvIgnore]
        public TimeSpan BuildTime
        {
            get { return new TimeSpan(BuildTimeD, BuildTimeH, BuildTimeM, BuildTimeS); }
            set
            {
                BuildTimeD = value.Days;
                BuildTimeH = value.Hours;
                BuildTimeM = value.Minutes;
                BuildTimeS = value.Seconds;
            }
        }
        private int BuildTimeD { get; set; }
        private int BuildTimeH { get; set; }
        private int BuildTimeM { get; set; }
        private int BuildTimeS { get; set; }

        /// <summary>
        /// Gets or sets the Build resource.
        /// </summary>
        public string BuildResource { get; set; }
        /// <summary>
        /// Gets or sets the Build cost.
        /// </summary>
        public int BuildCost { get; set; }
        /// <summary>
        /// Gets or sets the TownHall level unlocked.
        /// </summary>
        public int TownHallLevel { get; set; }
        /// <summary>
        /// Gets or sets the Width.
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Gets or sets the Height.
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Gets or sets the Icon asset location.
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// Gets or sets the Export name build animation.
        /// </summary>
        public string ExportNameBuildAnim { get; set; }
        /// <summary>
        /// Gets or sets the Maximum store Gold.
        /// </summary>
        public int MaxStoredGold { get; set; }
        /// <summary>
        /// Gets or sets the Maximum stored Elixir.
        /// </summary>
        public int MaxStoredElixir { get; set; }
        /// <summary>
        /// Gets or sets the Maximum stored DarkElixir.
        /// </summary>
        public int MaxStoredDarkElixir { get; set; }
        /// <summary>
        /// Gets or sets the Maximum stored WarGold.
        /// </summary>
        public int MaxStoredWarGold { get; set; }
        /// <summary>
        /// Gets or sets the Maximum stored WarElixir.
        /// </summary>
        public int MaxStoredWarElixir { get; set; }
        /// <summary>
        /// Gets or sets the Maximum stored WarDarkElixir.
        /// </summary>
        public int MaxStoredWarDarkElixir { get; set; }
        /// <summary>
        /// Gets or sets Bunker.
        /// </summary>
        public bool Bunker { get; set; }
        /// <summary>
        /// Gets or sets Housing space.
        /// </summary>
        public int HousingSpace { get; set; }
        /// <summary>
        /// Gets or sets Produces resource.
        /// </summary>
        public string ProducesResource { get; set; }
        /// <summary>
        /// Gets or sets Resource/Hour.
        /// </summary>
        public int ResourcePerHour { get; set; }
        /// <summary>
        /// Gets or sets Resource max.
        /// </summary>
        public int ResourceMax { get; set; }
        /// <summary>
        /// Gets or sets Resource icon limit.
        /// </summary>
        public int ResourceIconLimit { get; set; }
        /// <summary>
        /// Gets or sets Unit production.
        /// </summary>
        public int UnitProduction { get; set; }
        /// <summary>
        /// Gets or sets Upgrades units.
        /// </summary>
        public bool UpgradesUnits { get; set; }
        /// <summary>
        /// Gets or sets Units of type.
        /// </summary>
        public int ProducesUnitsOfType { get; set; }
        /// <summary>
        /// Gets or sets Boost cost.
        /// </summary>
        public int BoostCost { get; set; }
        /// <summary>
        /// Gets or sets Hitpoints.
        /// </summary>
        public int Hitpoints { get; set; }
        /// <summary>
        /// Gets or sets Regen time.
        /// </summary>
        public int RegenTime { get; set; }
        /// <summary>
        /// Gets or sets Attack range.
        /// </summary>
        public int AttackRange { get; set; }
        /// <summary>
        /// Gets or sets Alt attack mode.
        /// </summary>
        public bool AltAttackMode { get; set; }
        /// <summary>
        /// Gets or sets Alt attack range.
        /// </summary>
        public int AltAttackRange { get; set; }
        /// <summary>
        /// Gets or sets Prepare speed.
        /// </summary>
        public int PrepareSpeed { get; set; }
        /// <summary>
        /// Gets or sets Attack speed.
        /// </summary>
        public int AttackSpeed { get; set; }
        /// <summary>
        /// Gets or sets Cooldown override.
        /// </summary>
        public int CoolDownOverride { get; set; }
        /// <summary>
        /// Gets or sets damage.
        /// </summary>
        public int Damage { get; set; }
        /// <summary>
        /// Gets or sets Preferred target.
        /// </summary>
        public string PreferredTarget { get; set; }
        /// <summary>
        /// Gets or sets Preferred target damage mod.
        /// </summary>
        public int PreferredTargetDamageMod { get; set; }
        /// <summary>
        /// Gets or sets random hit position.
        /// </summary>
        public bool RandomHitPosition { get; set; }
        /// <summary>
        /// Gets or sets Destroy effect.
        /// </summary>
        public string DestroyEffect { get; set; }
        /// <summary>
        /// Gets or sets Attack effect.
        /// </summary>
        public string AttackEffect { get; set; }
        /// <summary>
        /// Gets or sets Attack effect 2.
        /// </summary>
        public string AttackEffect2 { get; set; }
        /// <summary>
        /// Gets or sets Hit effect.
        /// </summary>
        public string HitEffect { get; set; }
        /// <summary>
        /// Gets or sets Projectile.
        /// </summary>
        public string Projectile { get; set; }
        /// <summary>
        /// Gets or sets Export name damaged.
        /// </summary>
        public string ExportNameDamaged { get; set; }
        /// <summary>
        /// Gets or sets Building W.
        /// </summary>
        public int BuildingW { get; set; }
        /// <summary>
        /// Gets or sets Building H.
        /// </summary>
        public int BuildingH { get; set; }
        /// <summary>
        /// Gets or sets Export name base.
        /// </summary>
        public string ExportNameBase { get; set; }
        /// <summary>
        /// Gets or sets Export name base NPC.
        /// </summary>
        public string ExportNameBaseNpc { get; set; }
        /// <summary>
        /// Gets or sets Export Name base war.
        /// </summary>
        public string ExportNameBaseWar { get; set; }
        /// <summary>
        /// Gets or sets Air targets.
        /// </summary>
        public bool AirTargets { get; set; }
        /// <summary>
        /// Gets or sets Ground targets.
        /// </summary>
        public bool GroundTargets { get; set; }
        /// <summary>
        /// Gets or sets Alt air targets.
        /// </summary>
        public bool AltAirTargets { get; set; }
        /// <summary>
        /// Gets or sets Alt ground targets.
        /// </summary>
        public bool AltGroundTargets { get; set; }
        /// <summary>
        /// Gets or sets Alt multi targets.
        /// </summary>
        public bool AltMultiTargets { get; set; }
        /// <summary>
        /// Gets or sets Ammo count.
        /// </summary>
        public int AmmoCount { get; set; }
        /// <summary>
        /// Gets or sets Ammo resource.
        /// </summary>
        public string AmmoResource { get; set; }
        /// <summary>
        /// Gets or sets Ammo cost.
        /// </summary>
        public int AmmoCost { get; set; }
        /// <summary>
        /// Gets or sets Min attack range.
        /// </summary>
        public int MinAttackRange { get; set; }
        /// <summary>
        /// Gets or sets Damage radius.
        /// </summary>
        public int DamageRadius { get; set; }
        /// <summary>
        /// Gets or sets Push back.
        /// </summary>
        public int PushBack { get; set; }
        /// <summary>
        /// Gets or sets Wall corner pieces.
        /// </summary>
        public bool WallCornerPieces { get; set; }
        /// <summary>
        /// Gets or sets Load ammo effect.
        /// </summary>
        public string LoadAmmoEffect { get; set; }
        /// <summary>
        /// Gets or sets No ammo effect.
        /// </summary>
        public string NoAmmoEffect { get; set; }
        /// <summary>
        /// Gets or sets Toggle attack mode effect.
        /// </summary>
        public string ToggleAttackModeEffect { get; set; }
        /// <summary>
        /// Gets or sets Pick up effect.
        /// </summary>
        public string PickUpEffect { get; set; }
        /// <summary>
        /// Gets or sets Placing effect.
        /// </summary>
        public string PlacingEffect { get; set; }
        /// <summary>
        /// Gets or sets Can not sell last.
        /// </summary>
        public bool CanNotSellLast { get; set; }
        /// <summary>
        /// Gets or sets Defender character.
        /// </summary>
        public string DefenderCharacter { get; set; }
        /// <summary>
        /// Gets or sets Defender count.
        /// </summary>
        public int DefenderCount { get; set; }
        /// <summary>
        /// Gets or sets Defender Z.
        /// </summary>
        public int DefenderZ { get; set; }
        /// <summary>
        /// Gets or sets Destruction XP.
        /// </summary>
        public int DestructionXP { get; set; }
        /// <summary>
        /// Gets or sets Locked.
        /// </summary>
        public bool Locked { get; set; }
        /// <summary>
        /// Gets or sets Hidden.
        /// </summary>
        public bool Hidden { get; set; }
        /// <summary>
        /// Get or sets AOESpell.
        /// </summary>
        public string AOESpell { get; set; }
        /// <summary>
        /// Gets or sets AOESPell alternate.
        /// </summary>
        public string AOESpellAlternate { get; set; }
        /// <summary>
        /// Gets or sets Trigger radius.
        /// </summary>
        public int TriggerRadius { get; set; }
        /// <summary>
        /// Gets or sets Export name triggered.
        /// </summary>
        public string ExportNameTriggered { get; set; }
        /// <summary>
        /// Gets or sets Appear effect.
        /// </summary>
        public string AppearEffect { get; set; }
        /// <summary>
        /// Gets or sets Forges spells.
        /// </summary>
        public bool ForgesSpells { get; set; }
        /// <summary>
        /// Gets or sets Forges mini spells.
        /// </summary>
        public bool ForgesMiniSpells { get; set; }
        /// <summary>
        /// Gets or sets Is hero barrack.
        /// </summary>
        public bool IsHeroBarrack { get; set; }
        /// <summary>
        /// Gets or sets Hero type.
        /// </summary>
        public string HeroType { get; set; }
        /// <summary>
        /// Gets or sets Increasing damage.
        /// </summary>
        public bool IncreasingDamage { get; set; }
        /// <summary>
        /// Gets or sets Damage Lv2.
        /// </summary>
        public int DamageLv2 { get; set; }
        /// <summary>
        /// Gets or sets Damage Lv3.
        /// </summary>
        public int DamageLv3 { get; set; }
        /// <summary>
        /// Gets or sets Damage multi.
        /// </summary>
        public int DamageMulti { get; set; }
        /// <summary>
        /// Gets or sets Lv2 switch time.
        /// </summary>
        public int Lv2SwitchTime { get; set; }
        /// <summary>
        /// Gets or sets Lv3 switch time.
        /// </summary>
        public int Lv3SwitchTime { get; set; }
        /// <summary>
        /// Gets or sets Attack effect Lv2.
        /// </summary>
        public string AttackEffectLv2 { get; set; }
        /// <summary>
        /// Gets or sets Attack effect Lv3.
        /// </summary>
        public string AttackEffectLv3 { get; set; }
        /// <summary>
        /// Gets or sets Transition effect Lv2.
        /// </summary>
        public string TransitionEffectLv2 { get; set; }
        /// <summary>
        /// Gets or sets Transition effect Lv3.
        /// </summary>
        public string TransitionEffectLv3 { get; set; }
        /// <summary>
        /// Gets or sets Alt number multi-targets.
        /// </summary>
        public int AltNumMultiTargets { get; set; }
        /// <summary>
        /// Gets or sets Prevents healing.
        /// </summary>
        public bool PreventsHealing { get; set; }
        /// <summary>
        /// Gets or sets Strength weight.
        /// </summary>
        public int StrengthWeight { get; set; }
        /// <summary>
        /// Gets or sets Alternate pick new target delay.
        /// </summary>
        public int AlternatePickNewTargetDelay { get; set; }
        /// <summary>
        /// Gets or sets Alt build resource.
        /// </summary>
        public string AltBuildResource { get; set; }
        /// <summary>
        /// Gets or sets Speed mod.
        /// </summary>
        public int SpeedMod { get; set; }
        /// <summary>
        /// Gets or sets Status effect time.
        /// </summary>
        public int StatusEffectTime { get; set; }
        /// <summary>
        /// Gets or sets Shockwave push strength.
        /// </summary>
        public int ShockwavePushStrength { get; set; }
        /// <summary>
        /// Gets or sets Shockwave push arc length.
        /// </summary>
        public int ShockwaveArcLength { get; set; }
        /// <summary>
        /// Gets or sets Shockwave expand radius.
        /// </summary>
        public int ShockwaveExpandRadius { get; set; }
        /// <summary>
        /// Gets or sets Targeting cone angle.
        /// </summary>
        public int TargetingConeAngle { get; set; }
        /// <summary>
        /// Gets or sets Aim rotate step,
        /// </summary>
        public int AimRotateStep { get; set; }
        #endregion
    }
}
