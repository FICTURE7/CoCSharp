using CoCSharp.Logic;
using CoCSharp.Networking.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCSharp.Server.Handlers
{
    public static class LoginPacketHandlers
    {
        public static void HandleLoginRequestPacket(CoCRemoteClient client, CoCServer server, IPacket packet)
        {
            client.NetworkManager.Seed = ((LoginRequestPacket)packet).Seed;
            client.QueuePacket(new UpdateKeyPacket()
            {
                Key = new byte[] { 23, 32, 45, 13, 54, 43 } // should generate random.
            });
            client.QueuePacket(new LoginSuccessPacket()
            {
                UserID = 12312332,
                UserToken = "SOMETOKEN",
                ServerEnvironment = "prod",
                DateJoined = DateTime.Now,
                DateLastPlayed = DateTime.Now,
                FacebookAppID = "asdasd",
                FacebookID = "asdasd",
                GameCenterID = "asdasd",
                GooglePlusID = "asdasdsdad",
                LoginCount = 69,
                MajorVersion = 7,
                MinorVersion = 156,
                PlayTime = new TimeSpan(0, 0, 0),
                RevisionVersion = 0,
                CountryCode = "MU"
            });
            client.QueuePacket(new OwnHomeDataPacket()
            {
                LastVisit = TimeSpan.FromSeconds(0),
                Unknown1 = -1,
                Timestamp = DateTime.UtcNow,
                Unknown2 = 0,
                UserID1 = 12312332,
                ShieldDuration = TimeSpan.FromSeconds(10),
                Unknown3 = 1200,
                Unknown4 = 60,
                Compressed = true,
                Home = new Village(),
                Unknown6 = 0
            });
        }

        public static void RegisterLoginPacketHandlers(CoCRemoteClient client)
        {
            client.RegisterPacketHandler(new LoginRequestPacket(), HandleLoginRequestPacket);
        }
    }
}
