using CoCSharp.Csv;

namespace CoCSharp.Data.Models
{
    /// <summary>
    /// Represents a data from logic/characters.csv
    /// </summary>
    public class CharacterData : CsvData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterData"/> class.
        /// </summary>
        public CharacterData()
        {
            // Space
        }

        internal override int KindId => 4;

        // NOTE: This was generated from the decomp_characters.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets TID.
        /// </summary>
        public string TID { get; set; }
        /// <summary>
        /// Gets or sets Info TID.
        /// </summary>
        public string InfoTID { get; set; }
        /// <summary>
        /// Gets or sets SWF.
        /// </summary>
        public string SWF { get; set; }
        /// <summary>
        /// Gets or sets Housing space.
        /// </summary>
        public int HousingSpace { get; set; }
        /// <summary>
        /// Gets or sets Barrack level.
        /// </summary>
        public int BarrackLevel { get; set; }
        /// <summary>
        /// Gets or sets Laboratory level.
        /// </summary>
        public int LaboratoryLevel { get; set; }
        /// <summary>
        /// Gets or sets Speed.
        /// </summary>
        public int Speed { get; set; }
        /// <summary>
        /// Gets or sets Hitpoints.
        /// </summary>
        public int Hitpoints { get; set; }
        /// <summary>
        /// Gets or sets Training time.
        /// </summary>
        public int TrainingTime { get; set; }
        /// <summary>
        /// Gets or sets Training resource.
        /// </summary>
        public string TrainingResource { get; set; }
        /// <summary>
        /// Gets or sets Training cost.
        /// </summary>
        public int TrainingCost { get; set; }
        /// <summary>
        /// Gets or sets Upgrade time h.
        /// </summary>
        public int UpgradeTimeH { get; set; }
        /// <summary>
        /// Gets or sets Upgrade resource.
        /// </summary>
        public string UpgradeResource { get; set; }
        /// <summary>
        /// Gets or sets Upgrade cost.
        /// </summary>
        public int UpgradeCost { get; set; }
        /// <summary>
        /// Gets or sets Donate cost.
        /// </summary>
        public int DonateCost { get; set; }
        /// <summary>
        /// Gets or sets Attack range.
        /// </summary>
        public int AttackRange { get; set; }
        /// <summary>
        /// Gets or sets Attack speed.
        /// </summary>
        public int AttackSpeed { get; set; }
        /// <summary>
        /// Gets or sets D p s.
        /// </summary>
        public int DPS { get; set; }
        /// <summary>
        /// Gets or sets Prefered target damage mod.
        /// </summary>
        public int PreferedTargetDamageMod { get; set; }
        /// <summary>
        /// Gets or sets Damage radius.
        /// </summary>
        public int DamageRadius { get; set; }
        /// <summary>
        /// Gets or sets Area damage ignores walls.
        /// </summary>
        public bool AreaDamageIgnoresWalls { get; set; }
        /// <summary>
        /// Gets or sets Self as aoe center.
        /// </summary>
        public bool SelfAsAoeCenter { get; set; }
        /// <summary>
        /// Gets or sets Icon s w f.
        /// </summary>
        public string IconSWF { get; set; }
        /// <summary>
        /// Gets or sets Icon export name.
        /// </summary>
        public string IconExportName { get; set; }
        /// <summary>
        /// Gets or sets Big picture.
        /// </summary>
        public string BigPicture { get; set; }
        /// <summary>
        /// Gets or sets Big picture s w f.
        /// </summary>
        public string BigPictureSWF { get; set; }
        /// <summary>
        /// Gets or sets Projectile.
        /// </summary>
        public string Projectile { get; set; }
        /// <summary>
        /// Gets or sets Prefered target building.
        /// </summary>
        public string PreferedTargetBuilding { get; set; }
        /// <summary>
        /// Gets or sets Prefered target building class.
        /// </summary>
        public string PreferedTargetBuildingClass { get; set; }
        /// <summary>
        /// Gets or sets Deploy effect.
        /// </summary>
        public string DeployEffect { get; set; }
        /// <summary>
        /// Gets or sets Attack effect.
        /// </summary>
        public string AttackEffect { get; set; }
        /// <summary>
        /// Gets or sets Hit effect.
        /// </summary>
        public string HitEffect { get; set; }
        /// <summary>
        /// Gets or sets Hit effect2.
        /// </summary>
        public string HitEffect2 { get; set; }
        /// <summary>
        /// Gets or sets Is flying.
        /// </summary>
        public bool IsFlying { get; set; }
        /// <summary>
        /// Gets or sets Air targets.
        /// </summary>
        public bool AirTargets { get; set; }
        /// <summary>
        /// Gets or sets Ground targets.
        /// </summary>
        public bool GroundTargets { get; set; }
        /// <summary>
        /// Gets or sets Attack count.
        /// </summary>
        public int AttackCount { get; set; }
        /// <summary>
        /// Gets or sets Die effect.
        /// </summary>
        public string DieEffect { get; set; }
        /// <summary>
        /// Gets or sets Animation.
        /// </summary>
        public string Animation { get; set; }
        /// <summary>
        /// Gets or sets Unit of type.
        /// </summary>
        public int UnitOfType { get; set; }
        /// <summary>
        /// Gets or sets Is jumper.
        /// </summary>
        public bool IsJumper { get; set; }
        /// <summary>
        /// Gets or sets Movement offset amount.
        /// </summary>
        public int MovementOffsetAmount { get; set; }
        /// <summary>
        /// Gets or sets Movement offset speed.
        /// </summary>
        public int MovementOffsetSpeed { get; set; }
        /// <summary>
        /// Gets or sets Tomb stone.
        /// </summary>
        public string TombStone { get; set; }
        /// <summary>
        /// Gets or sets Die damage.
        /// </summary>
        public int DieDamage { get; set; }
        /// <summary>
        /// Gets or sets Die damage radius.
        /// </summary>
        public int DieDamageRadius { get; set; }
        /// <summary>
        /// Gets or sets Die damage effect.
        /// </summary>
        public string DieDamageEffect { get; set; }
        /// <summary>
        /// Gets or sets Die damage delay.
        /// </summary>
        public int DieDamageDelay { get; set; }
        /// <summary>
        /// Gets or sets Disable production.
        /// </summary>
        public bool DisableProduction { get; set; }
        /// <summary>
        /// Gets or sets Secondary troop.
        /// </summary>
        public string SecondaryTroop { get; set; }
        /// <summary>
        /// Gets or sets Is secondary troop.
        /// </summary>
        public bool IsSecondaryTroop { get; set; }
        /// <summary>
        /// Gets or sets Secondary troop cnt.
        /// </summary>
        public int SecondaryTroopCnt { get; set; }
        /// <summary>
        /// Gets or sets Secondary spawn dist.
        /// </summary>
        public int SecondarySpawnDist { get; set; }
        /// <summary>
        /// Gets or sets Randomize sec spawn dist.
        /// </summary>
        public bool RandomizeSecSpawnDist { get; set; }
        /// <summary>
        /// Gets or sets Pick new target after pushback.
        /// </summary>
        public bool PickNewTargetAfterPushback { get; set; }
        /// <summary>
        /// Gets or sets Pushback speed.
        /// </summary>
        public int PushbackSpeed { get; set; }
        /// <summary>
        /// Gets or sets Summon troop.
        /// </summary>
        public string SummonTroop { get; set; }
        /// <summary>
        /// Gets or sets Summon troop count.
        /// </summary>
        public int SummonTroopCount { get; set; }
        /// <summary>
        /// Gets or sets Summon cooldown.
        /// </summary>
        public int SummonCooldown { get; set; }
        /// <summary>
        /// Gets or sets Summon effect.
        /// </summary>
        public string SummonEffect { get; set; }
        /// <summary>
        /// Gets or sets Summon limit.
        /// </summary>
        public int SummonLimit { get; set; }
        /// <summary>
        /// Gets or sets Spawn idle.
        /// </summary>
        public int SpawnIdle { get; set; }
        /// <summary>
        /// Gets or sets Strength weight.
        /// </summary>
        public int StrengthWeight { get; set; }
        /// <summary>
        /// Gets or sets Child troop.
        /// </summary>
        public string ChildTroop { get; set; }
        /// <summary>
        /// Gets or sets Child troop count.
        /// </summary>
        public int ChildTroopCount { get; set; }
        /// <summary>
        /// Gets or sets Speed decrease per child troop lost.
        /// </summary>
        public int SpeedDecreasePerChildTroopLost { get; set; }
        /// <summary>
        /// Gets or sets Child troop0  x.
        /// </summary>
        public int ChildTroop0_X { get; set; }
        /// <summary>
        /// Gets or sets Child troop0  y.
        /// </summary>
        public int ChildTroop0_Y { get; set; }
        /// <summary>
        /// Gets or sets Child troop1  x.
        /// </summary>
        public int ChildTroop1_X { get; set; }
        /// <summary>
        /// Gets or sets Child troop1  y.
        /// </summary>
        public int ChildTroop1_Y { get; set; }
        /// <summary>
        /// Gets or sets Child troop2  x.
        /// </summary>
        public int ChildTroop2_X { get; set; }
        /// <summary>
        /// Gets or sets Child troop2  y.
        /// </summary>
        public int ChildTroop2_Y { get; set; }
        /// <summary>
        /// Gets or sets Attack multiple buildings.
        /// </summary>
        public bool AttackMultipleBuildings { get; set; }
        /// <summary>
        /// Gets or sets Increasing damage.
        /// </summary>
        public bool IncreasingDamage { get; set; }
        /// <summary>
        /// Gets or sets D p s lv2.
        /// </summary>
        public int DPSLv2 { get; set; }
        /// <summary>
        /// Gets or sets D p s lv3.
        /// </summary>
        public int DPSLv3 { get; set; }
        /// <summary>
        /// Gets or sets Lv2 switch time.
        /// </summary>
        public int Lv2SwitchTime { get; set; }
        /// <summary>
        /// Gets or sets Lv3 switch time.
        /// </summary>
        public int Lv3SwitchTime { get; set; }
        /// <summary>
        /// Gets or sets Attack effect lv2.
        /// </summary>
        public string AttackEffectLv2 { get; set; }
        /// <summary>
        /// Gets or sets Attack effect lv3.
        /// </summary>
        public string AttackEffectLv3 { get; set; }
        /// <summary>
        /// Gets or sets Attack effect lv4.
        /// </summary>
        public string AttackEffectLv4 { get; set; }
        /// <summary>
        /// Gets or sets Transition effect lv2.
        /// </summary>
        public string TransitionEffectLv2 { get; set; }
        /// <summary>
        /// Gets or sets Transition effect lv3.
        /// </summary>
        public string TransitionEffectLv3 { get; set; }
        /// <summary>
        /// Gets or sets Transition effect lv4.
        /// </summary>
        public string TransitionEffectLv4 { get; set; }
        /// <summary>
        /// Gets or sets Hit effect offset.
        /// </summary>
        public int HitEffectOffset { get; set; }
        /// <summary>
        /// Gets or sets Targeted effect offset.
        /// </summary>
        public int TargetedEffectOffset { get; set; }
        /// <summary>
        /// Gets or sets Secondary spawn offset.
        /// </summary>
        public int SecondarySpawnOffset { get; set; }
        /// <summary>
        /// Gets or sets Custom defender icon.
        /// </summary>
        public string CustomDefenderIcon { get; set; }
        /// <summary>
        /// Gets or sets Special movement mod.
        /// </summary>
        public int SpecialMovementMod { get; set; }
        /// <summary>
        /// Gets or sets Invisibility radius.
        /// </summary>
        public int InvisibilityRadius { get; set; }
        /// <summary>
        /// Gets or sets Health reduction per second.
        /// </summary>
        public int HealthReductionPerSecond { get; set; }
        /// <summary>
        /// Gets or sets Auto merge distance.
        /// </summary>
        public int AutoMergeDistance { get; set; }
        /// <summary>
        /// Gets or sets Auto merge group size.
        /// </summary>
        public int AutoMergeGroupSize { get; set; }
        /// <summary>
        /// Gets or sets Is underground.
        /// </summary>
        public bool IsUnderground { get; set; }
        /// <summary>
        /// Gets or sets Projectile bounces.
        /// </summary>
        public int ProjectileBounces { get; set; }
        /// <summary>
        /// Gets or sets Friendly group weight.
        /// </summary>
        public int FriendlyGroupWeight { get; set; }
        /// <summary>
        /// Gets or sets Enemy group weight.
        /// </summary>
        public int EnemyGroupWeight { get; set; }
        /// <summary>
        /// Gets or sets New target attack delay.
        /// </summary>
        public int NewTargetAttackDelay { get; set; }
        /// <summary>
        /// Gets or sets Triggers traps.
        /// </summary>
        public bool TriggersTraps { get; set; }
        /// <summary>
        /// Gets or sets Chain shooting distance.
        /// </summary>
        public int ChainShootingDistance { get; set; }
        /// <summary>
        /// Gets or sets Pre attack effect.
        /// </summary>
        public string PreAttackEffect { get; set; }
        /// <summary>
        /// Gets or sets Move trail effect.
        /// </summary>
        public string MoveTrailEffect { get; set; }
        /// <summary>
        /// Gets or sets Becomes targetable effect.
        /// </summary>
        public string BecomesTargetableEffect { get; set; }
        /// <summary>
        /// Gets or sets Boosted if alone.
        /// </summary>
        public bool BoostedIfAlone { get; set; }
        /// <summary>
        /// Gets or sets Boost radius.
        /// </summary>
        public int BoostRadius { get; set; }
        /// <summary>
        /// Gets or sets Boost dmg perfect.
        /// </summary>
        public int BoostDmgPerfect { get; set; }
        /// <summary>
        /// Gets or sets Boost attack speed.
        /// </summary>
        public int BoostAttackSpeed { get; set; }
        /// <summary>
        /// Gets or sets Hide effect.
        /// </summary>
        public string HideEffect { get; set; }
    }
}
