using CoCSharp.Data.Slots;
using CoCSharp.Logic;
using CoCSharp.Networking;
using CoCSharp.Networking.Cryptography;
using CoCSharp.Networking.Messages;
using System;

namespace CoCSharp.Server.Handlers
{
    public delegate void MessageHandler(CoCServer server, CoCRemoteClient client, Message message);

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
                Console.WriteLine("Created new avatar with Token {0}, ID {1}", avatar.Token, avatar.ID);

                lsMessage.UserID = avatar.ID;
                lsMessage.UserID1 = avatar.ID;
                lsMessage.UserToken = avatar.Token;
            }
            else
            {
                if (!server.AvatarManager.LoadedAvatar.TryGetValue(lrMessage.UserToken, out avatar)) // unknown token and id
                {
                    avatar = server.AvatarManager.CreateNewAvatar(lrMessage.UserToken, lrMessage.UserID);
                    Console.WriteLine("Unknown avatar, Created new avatar with Token {0}, ID {1}", avatar.Token, avatar.ID);
                }
                else Console.WriteLine("Avatar with Token {0}, ID {1} logged in.", avatar.Token, avatar.ID);

                lsMessage.UserID = avatar.ID;
                lsMessage.UserID1 = avatar.ID;
                lsMessage.UserToken = avatar.Token;
            }

            server.AvatarManager.SaveAvatar(avatar);
            client.Avatar = avatar;

            var avatarData = new AvatarMessageData(avatar)
            {
                //TODO: Properly figure townhall level and alliance castle stuff.
                TownHallLevel = 5,
                AllianceCastleLevel = 1,
                AllianceCastleTotalCapacity = 10,
                AllianceCastleUsedCapacity = 0,

                //TODO: Properly store them and calculate them.
                ResourcesCapacity = new ResourceCapacitySlot[] 
                {
                    new ResourceCapacitySlot(3000001, 10000000),
                    new ResourceCapacitySlot(3000002, 10000000)
                },

                ResourcesAmount = new ResourceAmountSlot[] 
                {
                    new ResourceAmountSlot(3000001, 10000000),
                    new ResourceAmountSlot(3000002, 10000000)
                }
            };

            var ohdMessage = new OwnHomeDataMessage()
            {
                LastVisit = TimeSpan.FromSeconds(100), //TODO: Implement saving of LastVisit.
                Unknown1 = -1,
                Timestamp = DateTime.UtcNow,
                OwnAvatarData = avatarData
            };

            client.NetworkManager.SendMessage(lsMessage); // LoginSuccessMessage
            client.NetworkManager.SendMessage(ohdMessage); // OwnHomeDataMessage
        }

        public static void HandleNewClientEncryptionMessage(CoCServer server, CoCRemoteClient client, Message message)
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
            server.RegisterMessageHandler(new SessionRequestMessage(), HandleNewClientEncryptionMessage);
        }
    }
}
