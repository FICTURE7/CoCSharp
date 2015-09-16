namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from building_classes.csv.
    /// </summary>
    public class BuildingClassData : CoCData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="BuildingClassData"/> class.
        /// </summary>
        public BuildingClassData()
        {
            // Space
        }

        public string Name { get; set; }
        public string TID { get; set; }
        public bool CanBuy { get; set; }
        public bool ShopCategoryResource { get; set; }
        public bool ShopCategoryArmy { get; set; }
        public bool ShopCategoryDefense { get; set; }
    }
}
