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

            var avatarVillage = avatar.Home;
            var assetManager = server.AssetManager;
            var npcManager = server.NpcManager;

            //Profiler.Start("Avatar.UpdateSlots");
            //avatar.UpdateSlots(assetManager);
            //Profiler.Stop("Avatar.UpdateSlots");
            avatar.ResourcesCapacity = new ResourceCapacitySlot[]
            {
                new ResourceCapacitySlot(3000000, 1000),
                new ResourceCapacitySlot(3000001, 1000),
            };

            avatar.ResourcesAmount = new ResourceAmountSlot[]
            {
                new ResourceAmountSlot(3000000, 1000),
                new ResourceAmountSlot(3000001, 1000),
            };
            avatar.NpcStars = npcManager.CompleteNpcStarList;

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
