using CoCSharp.Csv;
using System;

namespace CoCSharp.Data.Models
{
    /// <summary>
    /// Defines data from the logic/traps.csv file.
    /// </summary>
    public class TrapData : CsvData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrapData"/> class.
        /// </summary>
        public TrapData()
        {
            // Space
        }

        internal override int BaseDataID
        {
            get { return 12000000; }
        }

        // NOTE: This was generated from the traps.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets the Name of the trap.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the Info TID.
        /// </summary>
        public string InfoTID { get; set; }
        /// <summary>
        /// Gets or sets SWF asset location.
        /// </summary>
        public string SWF { get; set; }
        /// <summary>
        /// Gets or sets Export name.
        /// </summary>
        public string ExportName { get; set; }
        /// <summary>
        /// Gets or sets Export name air.
        /// </summary>
        public string ExportNameAir { get; set; }
        /// <summary>
        /// Gets or sets Export name build animation.
        /// </summary>
        public string ExportNameBuildAnim { get; set; }
        /// <summary>
        /// Gets or sets Export name build animation air.
        /// </summary>
        public string ExportNameBuildAnimAir { get; set; }
        /// <summary>
        /// Gets or sets Export name broken.
        /// </summary>
        public string ExportNameBroken { get; set; }
        /// <summary>
        /// Gets or sets Export name broken air.
        /// </summary>
        public string ExportNameBrokenAir { get; set; }
        /// <summary>
        /// Gets or sets Big picture.
        /// </summary>
        public string BigPicture { get; set; }
        /// <summary>
        /// Gets or sets Big picture SWF.
        /// </summary>
        public string BigPictureSWF { get; set; }
        /// <summary>
        /// Gets or sets Effect broken.
        /// </summary>
        public string EffectBroken { get; set; }
        /// <summary>
        /// Gets or sets Damage.
        /// </summary>
        public int Damage { get; set; }
        /// <summary>
        /// Gets or sets Damage radius.
        /// </summary>
        public int DamageRadius { get; set; }
        /// <summary>
        /// Gets or sets Trigger radius.
        /// </summary>
        public int TriggerRadius { get; set; }
        /// <summary>
        /// Gets or sets Width.
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Gets or sets Height.
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Gets or sets Effect.
        /// </summary>
        public string Effect { get; set; }
        /// <summary>
        /// Gets or sets Effect 2.
        /// </summary>
        public string Effect2 { get; set; }
        /// <summary>
        /// Gets or sets Damage effect.
        /// </summary>
        public string DamageEffect { get; set; }
        /// <summary>
        /// Gets or sets Passable.
        /// </summary>
        public bool Passable { get; set; }
        /// <summary>
        /// Gets or sets Build resource.
        /// </summary>
        public string BuildResource { get; set; }

        /// <summary>
        /// Gets or sets the build time.
        /// </summary>
        [CsvIgnore]
        public TimeSpan BuildTime
        {
            get { return new TimeSpan(BuildTimeD, BuildTimeH, BuildTimeM, 0); }
            set
            {
                BuildTimeD = value.Days;
                BuildTimeH = value.Hours;
                BuildTimeM = value.Minutes;
            }
        }
        private int BuildTimeD { get; set; }
        private int BuildTimeH { get; set; }
        private int BuildTimeM { get; set; }

        /// <summary>
        /// Gets or sets Build cost.
        /// </summary>
        public int BuildCost { get; set; }
        /// <summary>
        /// Gets or sets Rearm cost.
        /// </summary>
        public int RearmCost { get; set; }
        /// <summary>
        /// Gets or sets Town hall level.
        /// </summary>
        public int TownHallLevel { get; set; }
        /// <summary>
        /// Gets or sets Eject victims.
        /// </summary>
        public bool EjectVictims { get; set; }
        /// <summary>
        /// Gets or sets Min trigger housing limit.
        /// </summary>
        public int MinTriggerHousingLimit { get; set; }
        /// <summary>
        /// Gets or sets Eject housing limit.
        /// </summary>
        public int EjectHousingLimit { get; set; }
        /// <summary>
        /// Gets or sets Export name triggered.
        /// </summary>
        public string ExportNameTriggered { get; set; }
        /// <summary>
        /// Gets or sets Export name triggered air.
        /// </summary>
        public string ExportNameTriggeredAir { get; set; }
        /// <summary>
        /// Gets or sets Action frame.
        /// </summary>
        public int ActionFrame { get; set; }
        /// <summary>
        /// Gets or sets Pick up effect.
        /// </summary>
        public string PickUpEffect { get; set; }
        /// <summary>
        /// Gets or sets Placing effect.
        /// </summary>
        public string PlacingEffect { get; set; }
        /// <summary>
        /// Gets or sets Appear effect.
        /// </summary>
        public string AppearEffect { get; set; }
        /// <summary>
        /// Gets or sets Toggle attack mode effect.
        /// </summary>
        public string ToggleAttackModeEffect { get; set; }
        /// <summary>
        /// Gets or sets Duration ms.
        /// </summary>
        public int DurationMS { get; set; }
        /// <summary>
        /// Gets or sets Speed mod.
        /// </summary>
        public int SpeedMod { get; set; }
        /// <summary>
        /// Gets or sets Damage mod.
        /// </summary>
        public int DamageMod { get; set; }
        /// <summary>
        /// Gets or sets Air trigger.
        /// </summary>
        public bool AirTrigger { get; set; }
        /// <summary>
        /// Gets or sets Ground trigger.
        /// </summary>
        public bool GroundTrigger { get; set; }
        /// <summary>
        /// Gets or sets Healer trigger.
        /// </summary>
        public bool HealerTrigger { get; set; }
        /// <summary>
        /// Gets or sets Hit delay ms.
        /// </summary>
        public int HitDelayMS { get; set; }
        /// <summary>
        /// Gets or sets Hit count.
        /// </summary>
        public int HitCnt { get; set; }
        /// <summary>
        /// Gets or sets Projectile.
        /// </summary>
        public string Projectile { get; set; }
        /// <summary>
        /// Gets or sets Spell.
        /// </summary>
        public string Spell { get; set; }
        /// <summary>
        /// Gets or sets Strength weight.
        /// </summary>
        public int StrengthWeight { get; set; }
        /// <summary>
        /// Gets or sets Preferred target damage mod.
        /// </summary>
        public int PreferredTargetDamageMod { get; set; }
        /// <summary>
        /// Gets or sets Preferred target.
        /// </summary>
        public string PreferredTarget { get; set; }
        /// <summary>
        /// Gets or sets Spawned char ground.
        /// </summary>
        public string SpawnedCharGround { get; set; }
        /// <summary>
        /// Gets or sets Spawned char air.
        /// </summary>
        public string SpawnedCharAir { get; set; }
        /// <summary>
        /// Gets or sets Number spawns.
        /// </summary>
        public int NumSpawns { get; set; }
        /// <summary>
        /// Gets or sets Spawn initial delay ms.
        /// </summary>
        public int SpawnInitialDelayMs { get; set; }
        /// <summary>
        /// Gets or sets Time between spawns ms.
        /// </summary>
        public int TimeBetweenSpawnsMs { get; set; }
    }
}
