using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Network.Cryptography;
using CoCSharp.Network.Messages;
using CoCSharp.Server.API;
using CoCSharp.Server.API.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoCSharp.Server.Core
{
    public class MessageHandler : IMessageHandler
    {
        private delegate void Handler(IClient client, Message message);

        #region Constructors
        public MessageHandler(IServer server)
        {
            if (server == null)
                throw new ArgumentNullException(nameof(server));

            _server = server;
            _handlers = new Dictionary<int, Handler>();
            _handlers.Add(new HandshakeRequestMessage().ID, HandleHandshakeRequest);
            _handlers.Add(new LoginRequestMessage().ID, HandleLoginRequest);
        }
        #endregion

        #region Fields & Properties
        private readonly Dictionary<int, Handler> _handlers;
        private readonly IServer _server;

        public IServer Server => _server;
        #endregion

        #region Methods
        public void Handle(IClient client, Message message)
        {
            Debug.Assert(client != null);
            Debug.Assert(message != null);

            var handler = (Handler)null;
            if (_handlers.TryGetValue(message.ID, out handler))
                handler(client, message);
            else
                Server.Log.Warn($"MessageHandler was unable to handle message with ID {message.ID}");
        }

        private void HandleHandshakeRequest(IClient client, Message message)
        {
            client.SessionKey = Crypto8.GenerateNonce();
            var hsMessage = new HandshakeSuccessMessage
            {
                SessionKey = client.SessionKey
            };

            client.SendMessage(hsMessage);
        }

        private void HandleLoginRequest(IClient client, Message message)
        {
            var lrMessage = (LoginRequestMessage)message;
            var levelSave = Server.Db.LoadLevel(lrMessage.ID);
            var level = (Level)null;
            if (levelSave == null)
                levelSave = Server.Db.NewLevel();

            level = levelSave.ToLevel(Server.Assets);
            client.Level = level;

            var key = Crypto8.GenerateKeyPair();
            var lsMessage = new LoginSuccessMessage
            {
                // NetworkManagerAsync will use this nonce.
                //Nonce = Crypto8.GenerateNonce(),
                //PublicKey = key.PublicKey,

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

            //TODO: Send OwnHomeDataMessage and all that good stuff.
        }
        #endregion
    }
}
