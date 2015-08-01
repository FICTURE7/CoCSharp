namespace CoCSharp.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourceData : CoCData
    {
        /// <summary>
        /// 
        /// </summary>
        public ResourceData()
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
