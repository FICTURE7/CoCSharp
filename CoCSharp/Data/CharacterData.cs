using System;

namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from characters.csv.
    /// </summary>
    public class CharacterData : VillageObjectData
    {
        public int HousingSpace { get; set; }
        public int BarackLevel { get; set; }
        public int LaboratoryLevel { get; set; }
        public int Speed { get; set; }
    }
}
