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
            //TODO: Send LoginFailed to old client versions.
            //TODO: Check if the client has sent SessionRequestMessage first.

            // client.SessionKey == null only when SessionRequestMessage is not sent.
            // Its more likely that its an old client.
            if (client.SessionKey == null)
            {
                var loginFailed = new LoginFailedMessage();
                client.NetworkManager.SendMessage(loginFailed);
                return;
            }

            var lrMessage = (LoginRequestMessage)message;
            var keyPair = Crypto8.GenerateKeyPair();
            var lsMessage = new LoginSuccessMessage()
            {
                // NetworkManagerAsync will use this nonce.
                Nonce = Crypto8.GenerateNonce(),
                PublicKey = keyPair.PublicKey,

                FacebookID = null,
                GameCenterID = null,

                MajorVersion = 8,
                MinorVersion = 212,
                RevisionVersion = 0,

                ServerEnvironment = "prod",
                LoginCount = 0, // TODO: Implement saving of LoginCount.
                PlayTime = new TimeSpan(0, 0, 0), //TODO: Implement saving of PlayTime.
                Unknown1 = 0,
                FacebookAppID = "297484437009394", //TODO: Implement this into CoCSharp itself.
                DateLastPlayed = DateTime.Now, //TODO: Implement saving of DateLastPlayed.
                DateJoined = DateTime.Now, //TODO: Implement saving of DateJoined.
                Unknown2 = 0,
                GooglePlusID = null,
                CountryCode = "OI" //TODO: Return Country code of IP.
            };

            var avatar = (Avatar)null;
            var avatarManager = server.AvatarManager;
            // When UserID == 0 and UserToken == null, it means we should create a new account.
            if (lrMessage.UserID == 0 && lrMessage.UserToken == null)
            {
                avatar = avatarManager.CreateNewAvatar();
                FancyConsole.WriteLine("[&(blue)Login&(default)] Created new avatar &(darkcyan){0}&(default):{1} success.",
                                       avatar.Token, avatar.ID);

                lsMessage.UserID = avatar.ID;
                lsMessage.UserID1 = avatar.ID;
                lsMessage.UserToken = avatar.Token;
            }
            else
            {
                // If the account with the specified UserToken does not exists,
                // we try to create one with the UserToken and UserID.
                if (!avatarManager.Exists(lrMessage.UserToken))
                {
                    avatar = avatarManager.CreateNewAvatar(lrMessage.UserToken, lrMessage.UserID);
                    if (avatar == null)
                    {
                        // Should send a LoginFailedMessage telling the client to clear its data.
                        var loginFailed = new LoginFailedMessage();
                        client.NetworkManager.SendMessage(loginFailed);
                        //TODO: Disconnect client.
                        return;
                    }

                    FancyConsole.WriteLine("[&(blue)Login&(default)] Unknown avatar -> Created new avatar with &(darkcyan){0}&(default):{1} success.",
                                           avatar.Token, avatar.ID);
                    avatarManager.SaveAvatar(avatar);
                }
                else
                {
                    avatar = avatarManager.LoadAvatar(lrMessage.UserToken);
                    if (avatar == null)
                    {
                        // Should send a LoginFailedMessage telling the client to clear its data.
                        return;
                    }

                    FancyConsole.WriteLine("[&(blue)Login&(default)] Avatar &(darkcyan){0}&(default):{1} success.",
                                           avatar.Token, avatar.ID);
                }

                lsMessage.UserID = avatar.ID;
                lsMessage.UserID1 = avatar.ID;
                lsMessage.UserToken = avatar.Token;
            }

            var home = avatar.Home;
            var dataManager = server.AssetManager;
            var npcManager = server.NpcManager;

            avatar.NpcStars = npcManager.CompleteNpcStarList;
            //avatar.ResourcesCapacity = home.GetResourceCapacity(dataManager.ResourcesData);
            avatar.ResourcesAmount = new ResourceAmountSlot[]
            {
                new ResourceAmountSlot(3000001, 100000),
                new ResourceAmountSlot(3000002, 100000)
            };

            avatar.Units = new UnitSlot[]
            {
                new UnitSlot(4000000, 999),
                new UnitSlot(4000001, 999),
                new UnitSlot(4000002, 999),
                new UnitSlot(4000003, 999),
                new UnitSlot(4000004, 999),
                new UnitSlot(4000005, 999),
                new UnitSlot(4000006, 999),
                new UnitSlot(4000007, 999),
                new UnitSlot(4000008, 999),
                new UnitSlot(4000009, 999),
            };

            avatar.UnitUpgrades = new UnitUpgradeSlot[]
            {
                new UnitUpgradeSlot(4000000, 6),
                new UnitUpgradeSlot(4000001, 6),
                new UnitUpgradeSlot(4000002, 6),
                new UnitUpgradeSlot(4000003, 6),
                new UnitUpgradeSlot(4000004, 5),
                new UnitUpgradeSlot(4000005, 5),
                new UnitUpgradeSlot(4000006, 5),
                new UnitUpgradeSlot(4000007, 3),
                new UnitUpgradeSlot(4000008, 4),
                new UnitUpgradeSlot(4000009, 4),
            };

            var ohdMessage = avatar.OwnHomeDataMessage;
            client.Avatar = avatar;
            client.NetworkManager.SendMessage(lsMessage); // LoginSuccessMessage
            client.NetworkManager.SendMessage(ohdMessage); // OwnHomeDataMessage
        }

        public static void HandleSessionRequestMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            client.SessionKey = Crypto8.GenerateNonce();
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
