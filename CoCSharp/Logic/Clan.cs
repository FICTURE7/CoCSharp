namespace CoCSharp.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public class Clan
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Clan"/> class.
        /// </summary>
        public Clan()
        {
            // Space
        }

        public long ID { get; set; }
        public string Name { get; set; }
        public int Unknown1 { get; set; }
        public int Type { get; set; } //invite only, closed, open
        public int Members { get; set; }
        public int Trophies { get; set; }
        public int Unknown2 { get; set; }
        public int WarsWin { get; set; }
        public int WarsLost { get; set; }
        public int WarsDraw { get; set; }
        public int Badge { get; set; }
        public int Unknown3 { get; set; }
        public int Unknown4 { get; set; }
        public int EP { get; set; }
        public int Level { get; set; }

    }
}
