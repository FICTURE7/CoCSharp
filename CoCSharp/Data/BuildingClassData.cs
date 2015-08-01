namespace CoCSharp.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class BuildingClassData : CoCData
    {
        /// <summary>
        /// 
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
        public bool ShopCategoryDefense{ get; set; }
    }
}
