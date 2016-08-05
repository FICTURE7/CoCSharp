using CoCSharp.Network;
using CoCSharp.Network.Cryptography;
using CoCSharp.Network.Messages;
using System;
using System.Threading;

namespace CoCSharp.Server.Handlers
{
    public static class LoginMessageHandlers
    {
        private static void HandleLoginRequestMessage(CoCServer server, AvatarClient client, Message message)
        {
            //TODO: Send LoginFailed to old client versions.
            //TODO: Check if the client has sent SessionRequestMessage first.

            // client.SessionKey == null only when SessionRequestMessage is not sent.
            // Its more likely that its an old client.
            if (client.SessionKey == null)
            {
                var loginFailed = new LoginFailedMessage();
                client.SendMessage(loginFailed);
                client.Disconnect();
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

            client.ID = lrMessage.UserID;
            client.Token = lrMessage.UserToken;

            // Load avatar from db.
            // It will create a new avatar if ID == 0 && Token == null and return
            // true if it succeeded; otherwise a false.
            if (!client.Load())
            {
                var thread = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine("login: failed {0}:{1} thread {2}, already have entry with same id?", client.ID, client.Token, thread);

                var lfMessage = new LoginFailedMessage()
                {
                    Nonce = Crypto8.GenerateNonce(),
                    PublicKey = keyPair.PublicKey,

                    Message = "Issue with loading your account, please clear your app data and try again."
                };
                client.SendMessage(lfMessage);
                client.Disconnect();
                return;
            }
            else
            {
                var thread = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine("login: success {0}:{1} thread {2}", client.ID, client.Token, thread);

                client.Status |= ClientFlags.LoggedIn;
                lsMessage.UserID = client.ID;
                lsMessage.UserID1 = client.ID;
                lsMessage.UserToken = client.Token;
            }

            var ohdMessage = client.OwnHomeDataMessage;
            client.SendMessage(lsMessage); // LoginSuccessMessage
            client.SendMessage(ohdMessage); // OwnHomeDataMessage
            if (client.Alliance != null)
            {
                var clan = server.AllianceManager.LoadClan(client.Alliance.ID);
                client.SendMessage(clan.AllianceFullEntryMessage); // AllianceFullEntry
            }
        }

        private static void HandleHandshakeRequestMessage(CoCServer server, AvatarClient client, Message message)
        {
            client.SessionKey = Crypto8.GenerateNonce();
            var enMessage = new HandshakeSuccessMessage()
            {
                SessionKey = client.SessionKey
            };

            client.SendMessage(enMessage);
        }

        public static void RegisterLoginMessageHandlers(CoCServer server)
        {
            server.RegisterMessageHandler(new LoginRequestMessage(), HandleLoginRequestMessage);
            server.RegisterMessageHandler(new HandshakeRequestMessage(), HandleHandshakeRequestMessage);
        }
    }
}
