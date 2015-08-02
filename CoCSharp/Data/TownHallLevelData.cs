namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from townhall_levels.csv.
    /// </summary>
    public class TownHallLevelData : CoCData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="TownHallLevelData"/> class.
        /// </summary>
        public TownHallLevelData()
        {
            // Space
        }

        //TODO: Add CsvPropertyAttribute to some of those properties.
        public string Name { get; set; }
        public int AttackCost { get; set; }
        public int ResourceStorageLootPercentage { get; set; }
        public int DarkElixirStorageLootPercentage { get; set; }
        public int ResourceStorageLootCap { get; set; }
        public int DarkElixirStorageLootCap { get; set; }
        public int TroopHousing { get; set; }
        public int ElixirStorage { get; set; }
        public int GoldStorage { get; set; }
        public int ElixirPump { get; set; }
        public int GoldMine { get; set; }
        public int Barrack { get; set; }
        public int Cannon { get; set; }
        public int Wall { get; set; }
        public int ArcherTower { get; set; }
        public int WizardTower { get; set; }
        public int AirDefense { get; set; }
        public int Mortar { get; set; }
        public int AllianceCastle { get; set; }
        public int Ejector { get; set; }
        public int Superbomb { get; set; }
        public int Mine { get; set; }
        public int WorkerBuilding { get; set; }
        public int Laboratory { get; set; }
        public int Communicationsmast { get; set; }
        public int TeslaTower { get; set; }
        public int SpellForge { get; set; }
        public int MiniSpellFactory { get; set; }
        public int Bow { get; set; }
        public int Halloweenbomb { get; set; }
        public int Slowbomb { get; set; }
        public int HeroAltarBarbarianKing { get; set; }
        public int DarkElixirPump { get; set; }
        public int DarkElixirStorage { get; set; }
        public int HeroAltarArcherQueen { get; set; }
        public int AirTrap { get; set; }
        public int MegaAirTrap { get; set; }
        public int DarkElixirBarrack { get; set; }
        public int DarkTower { get; set; }
        public int SantaTrap { get; set; }
        public int StrengthMaxTroopTypes { get; set; }
        public int Totem { get; set; }
        public int Halloweenskels { get; set; }
        public int AirBlaster { get; set; }
    }
}
