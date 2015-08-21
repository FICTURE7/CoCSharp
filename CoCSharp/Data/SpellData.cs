namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from spells.csv.
    /// </summary>
    public class SpellData : CoCData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="SpellData"/> class.
        /// </summary>
        public SpellData()
        {
            // Space
        }

        public string Name { get; set; }
        public string TID { get; set; }
        public string InfoTID { get; set; }
        public bool DisableProduction { get; set; }
        public int SpellForgeLevel { get; set; }
        public int LaboratoryLevel { get; set; }
        public string TrainingResource { get; set; }
        public int TrainingCost { get; set; }
        public int HousingSpace { get; set; }
        public int TrainingTime { get; set; }
        public int DeployTimeMS { get; set; }
        public int ChargingTimeMS { get; set; }
        public int HitTimeMS { get; set; }
        public int UpgradeTimeH { get; set; }
        public string UpgradeResource { get; set; }
        public int UpgradeCost { get; set; }
        public int BoostTimeMS { get; set; }
        public int SpeedBoost { get; set; }
        public int SpeedBoost2 { get; set; }
        public int JumpHousingLimit { get; set; }
        public int JumpBoostMS { get; set; }
        public int DamageBoostPercent { get; set; }
        public int BuildingDamageBoostPercent { get; set; }
        public int Damage { get; set; }
        public int TroopDamagePermil { get; set; }
        public int BuildingDamagePermil { get; set; }
        public int ExecuteHealthPermil { get; set; }
        public int DamagePermilMin { get; set; }
        public int PreferredDamagePermilMin { get; set; }
        public int Radius { get; set; }
        public int NumberOfHits { get; set; }
        public int RandomRadius { get; set; }
        public int TimeBetweenHitsMS { get; set; }
        public string IconSWF { get; set; }
        public string IconExportName { get; set; }
        public string BigPicture { get; set; }
        public string PreDeployEffect { get; set; }
        public string DeployEffect { get; set; }
        public int DeployEffect2Delay { get; set; }
        public string DeployEffect2 { get; set; }
        public string ChargingEffect { get; set; }
        public string HitEffect { get; set; }
        public bool RandomRadiusAffectsOnlyGfx { get; set; }
        public int FreezeTimeMS { get; set; }
        public string SpawnObstacle { get; set; }
        public int NumObstacles { get; set; }
        public int StrengthWeight { get; set; }
        public int UnitOfType { get; set; }
        public bool TroopsOnly { get; set; }
        public string TargetInfoString { get; set; }
        public string PreferredTarget { get; set; }
        public int PreferredTargetDamageMod { get; set; }
        public bool BoostDefenders { get; set; }
        public int HeroDamageMultiplier { get; set; }
    }
}
