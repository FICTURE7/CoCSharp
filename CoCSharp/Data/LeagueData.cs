namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from leagues.csv.
    /// </summary>
    public class LeagueData : CoCData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="LeagueData"/> class.
        /// </summary>
        public LeagueData()
        {
            // Space
        }

        public string Name { get; set; }
        public string TID { get; set; }
        public string TIDShort { get; set; }
        public string IconSWF { get; set; }
        public string IconExportName { get; set; }
        public string LeagueBannerIcon { get; set; }
        public string LeagueBannerIconNum { get; set; }
        public string LeagueBannerIconHUD { get; set; }
        public int GoldReward { get; set; }
        public int ElixirReward { get; set; }
        public int DarkElixirReward { get; set; }
        public int PlacementLimitLow { get; set; }
        public int PlacementLimitHigh { get; set; }
        public int DemoteLimit { get; set; }
        public int PromoteLimit { get; set; }
        public int BucketPlacementRangeLow { get; set; }
        public int BucketPlacementRangeHigh { get; set; }
        public int BucketPlacementSoftLimit { get; set; }
        public int BucketPlacementHardLimit { get; set; }
        public bool IgnoredByServer { get; set; }
        public bool DemoteEnabled { get; set; }
        public bool PromoteEnabled { get; set; }
        public int AllocateAmount { get; set; }
        public int SaverCount { get; set; }
    }
}
