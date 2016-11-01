using CoCSharp.Data;
using CoCSharp.Data.Slots;
using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Network.Cryptography;
using CoCSharp.Network.Messages;
using CoCSharp.Server.API;
using CoCSharp.Server.API.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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

            _handlers.Add(new KeepAliveRequestMessage().ID, HandleKeepAlive);
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
            var levelSave = (ILevelSave)null;
            var level = (Level)null;
            if (lrMessage.UserID == 0)
            {
                // If somehow a client had a client ID of 0 but had a non null user token.
                if (lrMessage.UserToken != null)
                {
                    var lfMessage = new LoginFailedMessage
                    {
                        Message = "Problem resolving account. Clear application data and try again."
                    };
                    client.SendMessage(lfMessage);
                    return;
                }
                else
                {
                    levelSave = Server.Db.NewLevel();
                    level = levelSave.ToLevel(Server.Assets);
                }
            }
            else
            {
                // If somehow a client did not have an ID but had a user token.
                if (lrMessage.UserToken == null)
                {
                    var lfMessage = new LoginFailedMessage
                    {
                        Message = "Problem resolving account. Clear application data and try again."
                    };
                    client.SendMessage(lfMessage);
                    return;
                }
                else
                {
                    levelSave = Server.Db.LoadLevel(lrMessage.UserID);
                    if (levelSave == null)
                        levelSave = Server.Db.NewLevel(lrMessage.UserID, lrMessage.UserToken);

                    if (levelSave.Token != lrMessage.UserToken)
                    {
                        var lfMessage = new LoginFailedMessage
                        {
                            Message = "Problem resolving account. Clear application data and try again."
                        };
                        client.SendMessage(lfMessage);
                        return;
                    }

                    level = levelSave.ToLevel(Server.Assets);
                }
            }

            client.Level = level;

            var lsMessage = new LoginSuccessMessage
            {
                UserToken = levelSave.Token,
                UserID = levelSave.ID,
                UserID1 = levelSave.ID,

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

            var ohdMessage = level.OwnHomeData;

            var keppo = new OwnHomeDataMessage();
            var keppo2 = new MessageReader(new MemoryStream(File.ReadAllBytes("24101")));
            keppo.ReadMessage(keppo2);

            client.SendMessage(lsMessage);
            client.SendMessage(ohdMessage);
        }

        private static readonly KeepAliveResponseMessage s_response = new KeepAliveResponseMessage();
        private void HandleKeepAlive(IClient client, Message message)
        {
            client.SendMessage(s_response);
        }
        #endregion
    }
}
