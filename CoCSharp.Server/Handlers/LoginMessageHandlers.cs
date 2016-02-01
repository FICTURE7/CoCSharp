using CoCSharp.Data;
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
            var encryptionMessage = new EncryptionMessage()
            {
                ServerRandom = Crypto7.GenerateNonce(),
                ScramblerVersion = 1
            };

            var lrMessage = message as LoginRequestMessage;
            var lsMessage = new LoginSuccessMessage()
            {
                FacebookID = null,
                GameCenterID = null,
                MajorVersion = 7,
                MinorVersion = 200,
                RevisionVersion = 19,
                ServerEnvironment = "prod",
                LoginCount = 0,
                PlayTime = new TimeSpan(0, 0, 0), //TODO: Implement saving of playtime.
                Unknown1 = 0,
                FacebookAppID = "297484437009394", //TODO: Implement this into CoCSharp itself.
                DateLastPlayed = DateTime.Now, //TODO: Implement saving of date last played.
                DateJoined = DateTime.Now, //TODO: Implement saving of date joined.
                Unknown2 = 0,
                GooglePlusID = null,
                CountryCode = "EU"
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

            var avatarData = new AvatarData(avatar)
            {
                TownHallLevel = 5,
                AllianceCastleLevel = 1,
                AllianceCastleTotalCapacity = 10,
                AllianceCastleUsedCapacity = 0,

                ResourcesCapacity = new ResourceCapacitySlot[] 
                {
                    new ResourceCapacitySlot(3000001, 1000),
                    new ResourceCapacitySlot(3000002, 1000)
                },

                ResourcesAmount = new ResourceAmountSlot[] 
                {
                    new ResourceAmountSlot(3000001, 100),
                    new ResourceAmountSlot(3000002, 200)
                }
            };

            var ohdMessage = new OwnHomeDataMessage()
            {
                LastVisit = TimeSpan.FromSeconds(100),
                Unknown1 = -1,
                Timestamp = DateTime.UtcNow,
                OwnAvatarData = avatarData
            };

            client.NetworkManager.SendMessage(encryptionMessage);
            client.NetworkManager.SendMessage(lsMessage); // LoginSuccessMessage
            client.NetworkManager.SendMessage(ohdMessage); // OwnHomeDataMessage
        }

        public static void RegisterLoginMessageHandlers(CoCServer server)
        {
            server.RegisterMessageHandler(new LoginRequestMessage(), HandleLoginRequestMessage);
        }
    }
}
