namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from alliance_levels.csv.
    /// </summary>
    public class AllianceLevelData : CoCData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="AllianceLevelData"/> class.
        /// </summary>
        public AllianceLevelData()
        {
            // Space
        }

        public string Name { get; set; }
        public int ExpPoints { get; set; }
        public bool IsVisible { get; set; }
        public int TroopRequestCooldown { get; set; }
        public int TroopDonationLimit { get; set; }
        public int TroopDonationRefund { get; set; }
        public int TroopDonationUpgrade { get; set; }
        public int WarLootCapacityPercent { get; set; }
        public int WarLootMultiplierPercent { get; set; }
        public int BadgeLevel { get; set; }
        public string BannerSWF { get; set; }
    }
}
