namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from billing_packages.csv.
    /// </summary>
    public class BillingPackageData : CoCData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="BillingPackageData"/> class.
        /// </summary>
        public BillingPackageData()
        {
            // Space
        }

        public string Name { get; set; }
        public string TID { get; set; }
        public bool Disabled { get; set; }
        public bool ExistsApple { get; set; }
        public bool ExistsAndroid { get; set; }
        public int Diamonds { get; set; }
        public int USD { get; set; }
        public string IconSWF { get; set; }
        public string IconExportName { get; set; }
        public string ShopItemExportName { get; set; }
        public int Order { get; set; }
        public bool RED { get; set; }
        public int RMB { get; set; }
        public bool KunlunOnly { get; set; }
        public int LenovoID { get; set; }
    }
}
