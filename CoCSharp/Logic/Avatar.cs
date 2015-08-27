using CoCSharp.Networking;
using System;

namespace CoCSharp.Logic
{
    public class Avatar
    {
        public Avatar()
        {
            // Space
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
        public int TownhallLevel { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int Gems { get; set; }
        public int AttacksWon { get; set; }
        public int AttacksLost { get; set; }
        public int DefencesWon { get; set; }
        public int DefencesLost { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public Clan Clan { get; set; }
    }
}
