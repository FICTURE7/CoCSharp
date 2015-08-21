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
            client.Seed = ((LoginRequestPacket)packet).Seed;
            client.QueuePacket(new UpdateKeyPacket()
            {
                Key = new byte[] { 23, 32, 45, 13, 54, 43 }
            });
            client.QueuePacket(new LoginSuccessPacket()
            {
                UserID = 12312332,
                UserToken = "SOMETOKEN",
                ServerEnvironment = "prod",
                DateJoined = "123123",
                DateLastPlayed = "123123",
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
        }

        public static void RegisterLoginPacketHandlers(CoCRemoteClient client)
        {
            client.RegisterPacketHandler(new LoginRequestPacket(), HandleLoginRequestPacket);
        }
    }
}
