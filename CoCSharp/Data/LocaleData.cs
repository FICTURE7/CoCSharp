namespace CoCSharp.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class LocaleData : CoCData
    {
        /// <summary>
        /// 
        /// </summary>
        public LocaleData()
        {
            // Space
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasEvenSpaceCharacters { get; set; }
        public string UsedSystemFont { get; set; }
        public string HelpshiftSDKLanguage { get; set; }
        public string HelpshiftSDKLanguageAndroid { get; set; }
        public int SortOrder { get; set; }
        public bool TestLanguage { get; set; }
        public string TestExcludes { get; set; }
    }
}
