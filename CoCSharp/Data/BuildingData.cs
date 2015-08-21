using CoCSharp.Data.Csv;

namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from buildings.csv.
    /// </summary>
    public class BuildingData : CoCData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingData"/> class.
        /// </summary>
        public BuildingData()
        {
            // Space
        }

        [CsvProperty("          ")]  // its named like this in buildings.csv
        public string Name { get; set; }
        [CsvProperty("TID_Instructor")]
        public string TIDInstructor { get; set; }
        public int InstructorWeight { get; set; }
        public string TID { get; set; }
        public string InfoTID { get; set; }
        public string SWF { get; set; }
        public string BuildingClass { get; set; }
        public string ExportName { get; set; }
        public string ExportNameNpc { get; set; }
        public string ExportNameConstruction { get; set; }
        public int BuildTimeD { get; set; }
        public int BuildTimeH { get; set; }
        public int BuildTimeM { get; set; }
        public int BuildTimeS { get; set; }
        public string BuildResource { get; set; }
        public int BuildCost { get; set; }
        public int TownHallLevel { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Icon { get; set; }
        public string ExportNameBuildAnim { get; set; }
        public int MaxStoredGold { get; set; }
        public int MaxStoredElixir { get; set; }
        public int MaxStoredDarkElixir { get; set; }
        public int MaxStoredWarGold { get; set; }
        public int MaxStoredWarElixir { get; set; }
        public int MaxStoredWarDarkElixir { get; set; }
        public bool Bunker { get; set; }
        public int HousingSpace { get; set; }
        public string ProducesResource { get; set; }
        public int ResourcePerHour { get; set; }
        public int ResourceMax { get; set; }
        public int ResourceIconLimit { get; set; }
        public int UnitProduction { get; set; }
        public bool UpgradesUnits { get; set; }
        public int ProducesUnitsOfType { get; set; }
        public int BoostCost { get; set; }
        public int Hitpoints { get; set; }
        public int RegenTime { get; set; }
        public int AttackRange { get; set; }
        public bool AltAttackMode { get; set; }
        public int AltAttackRange { get; set; }
        public int PrepareSpeed { get; set; }
        public int AttackSpeed { get; set; }
        public int CoolDownOverride { get; set; }
        public int Damage { get; set; }
        public string PreferredTarget { get; set; }
        public int PreferredTargetDamageMod { get; set; }
        public bool RandomHitPosition { get; set; }
        public string DestroyEffect { get; set; }
        public string AttackEffect { get; set; }
        public string AttackEffect2 { get; set; }
        public string HitEffect { get; set; }
        public string Projectile { get; set; }
        public string ExportNameDamaged { get; set; }
        public int BuildingW { get; set; }
        public int BuildingH { get; set; }
        public string ExportNameBase { get; set; }
        public string ExportNameBaseNpc { get; set; }
        public string ExportNameBaseWar { get; set; }
        public bool AirTargets { get; set; }
        public bool GroundTargets { get; set; }
        public bool AltAirTargets { get; set; }
        public bool AltGroundTargets { get; set; }
        public bool AltMultiTargets { get; set; }
        public int AmmoCount { get; set; }
        public string AmmoResource { get; set; }
        public int AmmoCost { get; set; }
        public int MinAttackRange { get; set; }
        public int DamageRadius { get; set; }
        public bool PushBack { get; set; }
        public bool WallCornerPieces { get; set; }
        public string LoadAmmoEffect { get; set; }
        public string NoAmmoEffect { get; set; }
        public string ToggleAttackModeEffect { get; set; }
        public string PickUpEffect { get; set; }
        public string PlacingEffect { get; set; }
        public bool CanNotSellLast { get; set; }
        public string DefenderCharacter { get; set; }
        public int DefenderCount { get; set; }
        public int DefenderZ { get; set; }
        public int DestructionXP { get; set; }
        public bool Locked { get; set; }
        public bool Hidden { get; set; }
        public string AOESpell { get; set; }
        public string AOESpellAlternate { get; set; }
        public int TriggerRadius { get; set; }
        public string ExportNameTriggered { get; set; }
        public string AppearEffect { get; set; }
        public bool ForgesSpells { get; set; }
        public bool ForgesMiniSpells { get; set; }
        public bool IsHeroBarrack { get; set; }
        public string HeroType { get; set; }
        public bool IncreasingDamage { get; set; }
        public int DamageLv2 { get; set; }
        public int DamageLv3 { get; set; }
        public int DamageMulti { get; set; }
        public int Lv2SwitchTime { get; set; }
        public int Lv3SwitchTime { get; set; }
        public string AttackEffectLv2 { get; set; }
        public string AttackEffectLv3 { get; set; }
        public string TransitionEffectLv2 { get; set; }
        public string TransitionEffectLv3 { get; set; }
        public int AltNumMultiTargets { get; set; }
        public bool PreventsHealing { get; set; }
        public int StrengthWeight { get; set; }
        public int AlternatePickNewTargetDelay { get; set; }
        public string AltBuildResource { get; set; }
        public int SpeedMod { get; set; }
        public int StatusEffectTime { get; set; }
        public int ShockwavePushStrength { get; set; }
        public int ShockwaveArcLength { get; set; }
        public int ShockwaveExpandRadius { get; set; }
        public int TargetingConeAngle { get; set; }
        public int AimRotateStep { get; set; }
    }
}
