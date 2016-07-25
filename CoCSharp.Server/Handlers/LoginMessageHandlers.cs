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
            if (lrMessage.UserToken != null && !TokenUtils.CheckToken(lrMessage.UserToken))
            {
                var loginFailed = new LoginFailedMessage();
                client.NetworkManager.SendMessage(loginFailed);
                return;
            }

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

            client.ID = lrMessage.UserID;
            client.Token = lrMessage.UserToken;

            // Load avatar from db.
            // It will create a new avatar if ID == 0 && Token == null and return
            // true if it succeeded; otherwise a false.
            if (!client.Load())
            {
                FancyConsole.WriteLine("[&(blue)Login&(default)] Avatar &(darkcyan){0}&(default):{1} failed.",
                                           client.Token, client.ID);

                var loginFailed = new LoginFailedMessage();
                client.NetworkManager.SendMessage(loginFailed);
                //TODO: Disconnect client.
                return;
            }
            else
            {
                FancyConsole.WriteLine("[&(blue)Login&(default)] Avatar &(darkcyan){0}&(default):{1} success.",
                                           client.Token, client.ID);

                lsMessage.UserID = client.ID;
                lsMessage.UserID1 = client.ID;
                lsMessage.UserToken = client.Token;
            }

            var ohdMessage = client.OwnHomeDataMessage;
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
