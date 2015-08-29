using CoCSharp.Data.Csv;
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

        public string Name { get; set; }
        public int AttackCost { get; set; }
        public int ResourceStorageLootPercentage { get; set; }
        public int DarkElixirStorageLootPercentage { get; set; }
        public int ResourceStorageLootCap { get; set; }
        public int DarkElixirStorageLootCap { get; set; }
        [CsvProperty("Troop Housing")]
        public int TroopHousing { get; set; }
        [CsvProperty("Elixir Storage")]
        public int ElixirStorage { get; set; }
        [CsvProperty("Gold Storage")]
        public int GoldStorage { get; set; }
        [CsvProperty("Elixir Pump")]
        public int ElixirPump { get; set; }
        [CsvProperty("Gold Mine")]
        public int GoldMine { get; set; }
        public int Barrack { get; set; }
        public int Cannon { get; set; }
        public int Wall { get; set; }
        [CsvProperty("Archer Tower")]
        public int ArcherTower { get; set; }
        [CsvProperty("Wizard Tower")]
        public int WizardTower { get; set; }
        [CsvProperty("Air Defense")]
        public int AirDefense { get; set; }
        public int Mortar { get; set; }
        [CsvProperty("Alliance Castle")]
        public int AllianceCastle { get; set; }
        public int Ejector { get; set; }
        public int Superbomb { get; set; }
        public int Mine { get; set; }
        [CsvProperty("Worker Building")]
        public int WorkerBuilding { get; set; }
        public int Laboratory { get; set; }
        [CsvProperty("Communications mast")]
        public int Communicationsmast { get; set; }
        [CsvProperty("Tesla Tower")]
        public int TeslaTower { get; set; }
        [CsvProperty("Spell Forge")]
        public int SpellForge { get; set; }
        [CsvProperty("Mini Spell Factory")]
        public int MiniSpellFactory { get; set; }
        public int Bow { get; set; }
        public int Halloweenbomb { get; set; }
        public int Slowbomb { get; set; }
        [CsvProperty("Hero Altar Barbarian King")]
        public int HeroAltarBarbarianKing { get; set; }
        [CsvProperty("Dark Elixir Pump")]
        public int DarkElixirPump { get; set; }
        [CsvProperty("Dark Elixir Storage")]
        public int DarkElixirStorage { get; set; }
        [CsvProperty("Hero Altar Archer Queen")]
        public int HeroAltarArcherQueen { get; set; }
        public int AirTrap { get; set; }
        public int MegaAirTrap { get; set; }
        [CsvProperty("Dark Elixir Barrack")]
        public int DarkElixirBarrack { get; set; }
        [CsvProperty("Dark Tower")]
        public int DarkTower { get; set; }
        public int SantaTrap { get; set; }
        public int StrengthMaxTroopTypes { get; set; }
        public int Totem { get; set; }
        public int Halloweenskels { get; set; }
        [CsvProperty("Air Blaster")]
        public int AirBlaster { get; set; }
    }
}
