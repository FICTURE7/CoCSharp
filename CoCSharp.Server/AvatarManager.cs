using CoCSharp.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace CoCSharp.Server
{
    public class AvatarManager
    {
        public AvatarManager(CoCServer server)
        {
            NextPlayerID = 1;
            Server = server;
            Avatars = new List<Avatar>();
        }

        public long NextPlayerID { get; private set; }
        public List<Avatar> Avatars { get; set; }
        public Avatar NewAvatar
        {
            get
            {
                var token = (string)null;
                var buffer = new byte[20];
                _Random.NextBytes(buffer);
                token = BitConverter.ToString(_SHA1.ComputeHash(buffer)).Replace("-", "").ToLower();

                var avatar = new Avatar(NextPlayerID, token)
                {
                    Username = _NewNames[_Random.Next(_NewNames.Length - 1)],
                    Trophies = 0,
                    TownHallLevel = 0,
                    Level = 0,
                    Experience = 0,
                    Gems = 0,
                    AttacksWon = 0,
                    AttacksLost = 0,
                    DefensesWon = 0,
                    DefensesLost = 0,
                    Clan = null
                };
                NextPlayerID++;
                Avatars.Add(avatar);
                return avatar;
            }
        }

        private CoCServer Server { get; set; }

        private static SHA1 _SHA1 = SHA1.Create();
        private static Random _Random = new Random();
        private static string[] _NewNames = new string[] // use config file
        {
            "Ginger",
            "Albert",
            "Ronald",
            "Muffer",
            "Fuffer",
        };

        public Avatar FindAvatar(long id)
        {
            var avatar = FindAvatar(a => a.ID == id);
            if (avatar.Length < 1)
                throw new ArgumentException("No avatar with this ID was found.");
            if (avatar.Length > 1)
                throw new ArgumentException(avatar.Length + " avatars was found. And avatar ID must be unique.");
            return avatar[0];
        }

        public Avatar[] FindAvatar(string username)
        {
            return FindAvatar(avatar => avatar.Username == username);
        }

        public Avatar[] FindAvatar(Func<Avatar, bool> predicate)
        {
            return Avatars.Where(predicate).ToArray();
        }
    }
}
