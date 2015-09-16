using CoCSharp.Logic;
using CoCSharp.Networking;
using CoCSharp.Networking.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoCSharp.Server.Handlers
{
    public static class LoginPacketHandlers
    {
        public static void HandleLoginRequestPacket(CoCRemoteClient client, CoCServer server, IPacket packet)
        {
            var lrPacket = packet as LoginRequestPacket;
            client.NetworkManager.Seed = lrPacket.Seed;
            client.QueuePacket(new UpdateKeyPacket()
            {
                Key = CoCCrypto.CreateRandomByteArray(),
                ScramblerVersion = 1
            });

            if (lrPacket.UserID == 0 && lrPacket.UserToken == null)
            {
                client.Avatar = server.AvatarManager.NewAvatar;
                client.Home = server.DefaultVillage;
            }

            client.QueuePacket(new LoginSuccessPacket()
            {
                UserID = client.Avatar.ID,
                UserID1 = client.Avatar.ID,
                UserToken = client.Avatar.Token,
                FacebookID = null,
                GameCenterID = null,
                MajorVersion = 7,
                MinorVersion = 156,
                RevisionVersion = 5,
                ServerEnvironment = "prod",
                LoginCount = 0,
                PlayTime = new TimeSpan(0, 0, 0),
                Unknown1 = 0,
                FacebookAppID = "297484437009394",
                DateLastPlayed = DateTime.Now,
                DateJoined = DateTime.Now,
                Unknown2 = 0,
                GooglePlusID = null,
                CountryCode = "MU"
            });

            client.QueuePacket(new OwnHomeDataPacket()
            {
                LastVisit = TimeSpan.FromSeconds(0),
                Unknown1 = -1,
                Timestamp = DateTime.UtcNow,
                Unknown2 = 0,
                UserID = client.Avatar.ID,
                ShieldDuration = TimeSpan.FromSeconds(10),
                Unknown3 = 1200,
                Unknown4 = 60,
                Compressed = true,
                Home = client.Home,
                Avatar = client.Avatar,
                Unknown6 = 0,
                UserID1 = client.Avatar.ID,
                UserID2 = client.Avatar.ID,
                AllianceCastleLevel = -1,
                Unknown14 = 1200,
                Unknown15 = 60,
                Unknown19 = true,
                Unknown20 = 946720861000,
                Unknown21 = 1,
                Unknown25 = 1,
            });
        }

        public static void RegisterLoginPacketHandlers(CoCRemoteClient client)
        {
            client.RegisterPacketHandler(new LoginRequestPacket(), HandleLoginRequestPacket);
        }
    }
}
