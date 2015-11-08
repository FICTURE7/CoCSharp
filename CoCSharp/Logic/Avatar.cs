namespace CoCSharp.Logic
{
    public class Avatar
    {
        public Avatar()
        {
            ID = 0;
            Token = null;
        }

        public Avatar(long id)
        {
            ID = id;
        }

        public Avatar(long id, string token)
            : this(id)
        {
            Token = token;
        }

        public long ID { get; set; }
        public int Trophies { get; set; }
        public int TownHallLevel { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int Gems { get; set; }
        public int AttacksWon { get; set; }
        public int AttacksLost { get; set; }
        public int DefensesWon { get; set; }
        public int DefensesLost { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public Clan Clan { get; set; }
    }
}
