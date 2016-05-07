using CoCSharp.Data.Slots;
using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Network.Cryptography;
using CoCSharp.Network.Messages;
using CoCSharp.Server.Core;
using System;

namespace CoCSharp.Server.Handlers
{
    public static class LoginMessageHandlers
    {
        private static void HandleLoginRequestMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            //TODO: Implement LoginFailed to old client versions.

            var lrMessage = message as LoginRequestMessage;
            var keyPair = Crypto8.GenerateKeyPair();
            var lsMessage = new LoginSuccessMessage()
            {
                Nonce = Crypto8.GenerateNonce(),
                PublicKey = keyPair.PublicKey,
                FacebookID = null,
                GameCenterID = null,
                MajorVersion = 8,
                MinorVersion = 112,
                RevisionVersion = 0,
                ServerEnvironment = "prod",
                LoginCount = 0,
                PlayTime = new TimeSpan(0, 0, 0), //TODO: Implement saving of playtime.
                Unknown1 = 0,
                FacebookAppID = "297484437009394", //TODO: Implement this into CoCSharp itself.
                DateLastPlayed = DateTime.Now, //TODO: Implement saving of date last played.
                DateJoined = DateTime.Now, //TODO: Implement saving of date joined.
                Unknown2 = 0,
                GooglePlusID = null,
                CountryCode = "OI"
            };

            var avatar = (Avatar)null;
            if (lrMessage.UserID == 0 && lrMessage.UserToken == null) // new account
            {
                avatar = server.AvatarManager.CreateNewAvatar();
                FancyConsole.WriteLine("[&(blue)Login&(default)] Created new avatar &(darkcyan){0}&(default):{1} success.", 
                                       avatar.Token, avatar.ID);

                lsMessage.UserID = avatar.ID;
                lsMessage.UserID1 = avatar.ID;
                lsMessage.UserToken = avatar.Token;
            }
            else
            {
                // Create a new account if it does not exist.
                if (!server.AvatarManager.Exists(lrMessage.UserToken))
                {
                    avatar = server.AvatarManager.CreateNewAvatar(lrMessage.UserToken, lrMessage.UserID);
                    FancyConsole.WriteLine("[&(blue)Login&(default)] Unknown avatar -> Created new avatar with &(darkcyan){0}&(default):{1} success.",
                                           avatar.Token, avatar.ID);
                }
                else
                {
                    avatar = server.AvatarManager.LoadAvatar(lrMessage.UserToken);
                    FancyConsole.WriteLine("[&(blue)Login&(default)] Avatar &(darkcyan){0}&(default):{1} success.",
                                           avatar.Token, avatar.ID);
                }

                lsMessage.UserID = avatar.ID;
                lsMessage.UserID1 = avatar.ID;
                lsMessage.UserToken = avatar.Token;
            }

            //server.AvatarManager.SaveAvatar(avatar);
            client.Avatar = avatar;

            var villageData = new VillageMessageComponent()
            {
                HomeID = avatar.ID,
                ShieldDuration = avatar.ShieldDuration,
                Unknown2 = 1800,
                Unknown3 = 69119,
                Unknown4 = 1200,
                Unknown5 = 60,
                Home = avatar.Home,
            };

            var avatarData = new AvatarMessageComponent(avatar)
            {
                //TODO: Properly figure out townhall level and alliance castle stuff.
                TownHallLevel = 5,
                AllianceCastleLevel = 1,
                AllianceCastleTotalCapacity = 10,
                AllianceCastleUsedCapacity = 0,

                //TODO: Properly store them.
                ResourcesCapacity = new ResourceCapacitySlot[]
                {
                    new ResourceCapacitySlot(3000001, 5000),
                    new ResourceCapacitySlot(3000002, 5000)
                },

                ResourcesAmount = new ResourceAmountSlot[]
                {
                    new ResourceAmountSlot(3000001, 100000),
                    new ResourceAmountSlot(3000002, 100000)
                }
            };

            var ohdMessage = new OwnHomeDataMessage()
            {
                LastVisit = TimeSpan.FromSeconds(100), //TODO: Implement saving of LastVisit.
                Unknown1 = null,
                Timestamp = DateTime.UtcNow,
                OwnVillageData = villageData,
                OwnAvatarData = avatarData,
                Unknown4 = 1462629754000,
                Unknown5 = 1462629754000,
                Unknown6 = 1462631554000,
            };

            client.NetworkManager.SendMessage(lsMessage); // LoginSuccessMessage
            client.NetworkManager.SendMessage(ohdMessage); // OwnHomeDataMessage
        }

        public static void HandleSessionRequestMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            var enMessage = new SessionSuccessMessage()
            {
                SessionKey = client.SessionKey
            };

            client.NetworkManager.SendMessage(enMessage);
        }

        public static void RegisterLoginMessageHandlers(CoCServer server)
        {
            server.RegisterMessageHandler(new LoginRequestMessage(), HandleLoginRequestMessage);
            server.RegisterMessageHandler(new SessionRequestMessage(), HandleSessionRequestMessage);
        }
    }
}
