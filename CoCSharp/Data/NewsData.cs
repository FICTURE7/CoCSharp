namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from news.csv. 
    /// </summary>
    public class NewsData : CoCData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="NewsData"/> class.
        /// </summary>
        public NewsData()
        {
            // Space
        }

        public string Name { get; set; }
        public int ID { get; set; } // hmmm
        public bool Enabled { get; set; }
        public bool EnabledIOS { get; set; }
        public bool EnabledAndroid { get; set; }
        public bool EnabledKunlun { get; set; }
        public string Type { get; set; }
        public bool ShowAsNew { get; set; }
        public string TID { get; set; }
        public string InfoTID { get; set; }
        public string ButtonTID { get; set; }
        public string ButtonURL { get; set; }
        public string NativeIOSURL { get; set; }
        public string IncludedLanguages { get; set; }
        public string ExcludedLanguages { get; set; }
        public string IncludedCountries { get; set; }
        public string ExcludedCountries { get; set; }
        public string IncludedLoginCountries { get; set; }
        public string ExcludedLoginCountries { get; set; }
        public bool CenterText { get; set; }
        public bool LoadResources { get; set; }
        public string ItemSWF { get; set; }
        public string ItemExportName { get; set; }
        public string IconSWF { get; set; }
        public string IconExportName { get; set; }
        public int IconFrame { get; set; }
        public bool AnimateIcon { get; set; }
        public bool CenterIcon { get; set; }
        public int MinTownHall { get; set; }
        public int MaxTownHall { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public int MaxDiamonds { get; set; }
        public bool ClickToDismiss { get; set; }
        public int AvatarIdModulo { get; set; }
        public int ModuloMin { get; set; }
        public int ModuloMax { get; set; }
        public bool Collapsed { get; set; }
        public string ActionType { get; set; }
        public string MinOS { get; set; }
        public string MaxOS { get; set; }

    }
}
