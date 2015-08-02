namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from heros.csv.
    /// </summary>
    public class HeroData : CoCData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="HeroData"/> class.
        /// </summary>
        public HeroData()
        {
            // Space
        }

        public string Name { get; set; }
        public string TID { get; set; }
        public string InfoTID { get; set; }
        public string SWF { get; set; }
        public int Speed { get; set; }
        public int Hitpoints { get; set; }
        public int UpgradeTimeH { get; set; }
        public string UpgradeResource { get; set; }
        public int UpgradeCost { get; set; }
        public int RequiredTownHallLevel { get; set; }
        public int AttackRange { get; set; }
        public int AttackSpeed { get; set; }
        public int Damage { get; set; }
        public int PreferedTargetDamageMod { get; set; }
        public int DamageRadius { get; set; }
        public string IconSWF { get; set; }
        public string IconExportName { get; set; }
        public string BigPicture { get; set; }
        public string BigPictureSWF { get; set; }
        public string SmallPicture { get; set; }
        public string SmallPictureSWF { get; set; }
        public string Projectile { get; set; }
        public string RageProjectile { get; set; }
        public string PreferedTargetBuilding { get; set; }
        public string DeployEffect { get; set; }
        public string AttackEffect { get; set; }
        public string HitEffect { get; set; }
        public bool IsFlying { get; set; }
        public bool AirTargets { get; set; }
        public bool GroundTargets { get; set; }
        public int AttackCount { get; set; }
        public string DieEffect { get; set; }
        public string Animation { get; set; }
        public int MaxSearchRadiusForDefender { get; set; }
        public int HousingSpace { get; set; }
        public string SpecialAbilityEffect { get; set; }
        public int RegenerationTimeMinutes { get; set; }
        public int TrainingTime { get; set; }
        public string TrainingResource { get; set; }
        public int TrainingCost { get; set; }
        public string CelebrateEffect { get; set; }
        public int SleepOffsetX { get; set; }
        public int SleepOffsetY { get; set; }
        public int PatrolRadius { get; set; }
        public string AbilityTriggerEffect { get; set; }
        public bool AbilityAffectsHero { get; set; }
        public string AbilityAffectsCharacter { get; set; }
        public int AbilityRadius { get; set; }
        public int AbilityTime { get; set; }
        public bool AbilityOnce { get; set; }
        public int AbilityCooldown { get; set; }
        public int AbilitySpeedBoost { get; set; }
        public int AbilitySpeedBoost2 { get; set; }
        public int AbilityDamageBoostPercent { get; set; }
        public string AbilitySummonTroop { get; set; }
        public int AbilitySummonTroopCount { get; set; }
        public bool AbilityStealth { get; set; }
        public int AbilityDamageBoostOffset { get; set; }
        public int AbilityHealthIncrease { get; set; }
        public string AbilityTID { get; set; }
        public string AbilityDescTID { get; set; }
        public string AbilityIcon { get; set; }
        public string AbilityBigPictureExportName { get; set; }
        public int AbilityDelay { get; set; }
        public int StrengthWeight { get; set; }
        public int StrengthWeight2 { get; set; }
        public int AlertRadius { get; set; }
    }
}
