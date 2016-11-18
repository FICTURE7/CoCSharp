using CoCSharp.Data.Models;
using CoCSharp.Data.Slots;
using CoCSharp.Logic;
using CoCSharp.Logic.Commands;
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
            _attackingDict = new Dictionary<long, AttackInfo>();

            _handlers = new Dictionary<int, Handler>();
            _handlers.Add(new HandshakeRequestMessage().ID, HandleHandshakeRequest);
            _handlers.Add(new LoginRequestMessage().ID, HandleLoginRequest);

            _handlers.Add(new KeepAliveRequestMessage().ID, HandleKeepAlive);
            _handlers.Add(new ChangeAvatarNameRequestMessage().ID, HandleChangeAvatarRequestName);

            _handlers.Add(new CommandMessage().ID, HandleCommand);

            _handlers.Add(new ChatMessageClientMessage().ID, HandleChatMessageClient);

            _handlers.Add(new ReturnHomeMessage().ID, HandleReturnHome);

            _handlers.Add(new VisitHomeMessage().ID, HandleVisitHome);

            _handlers.Add(new AvatarProfileRequestMessage().ID, HandleAvatarProfileRequest);
        }

        private struct AttackInfo
        {
            public Level Defender;
            public Level Attacker;

            public int Lost;
            public int Reward;
        }
        #endregion

        #region Fields & Properties
        private readonly Dictionary<long, AttackInfo> _attackingDict;
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
                Server.Log.Warn($"MessageHandler does not contain a handler for message with ID: {message.ID}");
        }

        private void HandleReturnHome(IClient client, Message message)
        {
            var rhMessage = (ReturnHomeMessage)message;
            var level = client.Level;
            var ohdMessage = level.OwnHomeData;

            if (level.State == 2)
            {
                var info = default(AttackInfo);
                if (!_attackingDict.TryGetValue(level.Avatar.ID, out info))
                {
                    Server.Log.Warn("Unable to obtain attack info about a client while its state was 2.");
                }
                else
                {
                    var defender = info.Defender;
                    var attacker = info.Attacker;

                    var lost = info.Lost;
                    var reward = info.Reward;

                    if (defender.Avatar.Trophies > 0)
                        defender.Avatar.Trophies -= lost;

                    level.Avatar.Trophies += reward;

                    // Save attacker and defender.
                    var defenderSave = new LevelSave(defender);
                    var attackerSave = new LevelSave(level);

                    Server.Db.SaveLevel(defenderSave);
                    Server.Db.SaveLevel(attackerSave);

                    _attackingDict.Remove(level.Avatar.ID);
                }
            }

            // State == 0, means were home.
            level.State = 0;

            client.SendMessage(ohdMessage);
        }

        private void HandleAvatarProfileRequest(IClient client, Message message)
        {
            var aprMessage = (AvatarProfileRequestMessage)message;
            var ownLevel = client.Level;
            if (aprMessage.UserID == client.Level.Avatar.ID)
            {
                var aprrMessage = ownLevel.AvatarProfileResponse;
                client.SendMessage(aprrMessage);
            }
            else
            {
                var save = Server.Db.LoadLevel(aprMessage.UserID);
                var level = save.ToLevel(Server.Assets);
                var aprrMessage = level.AvatarProfileResponse;
                client.SendMessage(aprrMessage);
            }
        }

        private void HandleVisitHome(IClient client, Message message)
        {
            var vhMessage = (VisitHomeMessage)message;
            var visitSave = Server.Db.LoadLevel(vhMessage.HomeID);

            // If we failed to retrieve the Level that was requested
            // we return the client home.
            if (visitSave == null)
            {
                Server.Log.Error($"Failed to retrieve LevelSave with ID {vhMessage.ID}");

                var ohdMessage = client.Level.OwnHomeData;
                client.SendMessage(ohdMessage);
            }
            else
            {
                // Turn the LevelSave into a Level.
                var visitLevel = visitSave.ToLevel(Server.Assets);
                var vhdMessage = visitLevel.VisitHomeData;
                vhdMessage.OwnAvatarData = new AvatarMessageComponent(client.Level);

                client.SendMessage(vhdMessage);
            }
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

            const string ERROR_ACC_RESOLVE = "Problem resolving account. Clear application data and try again.";

            // Determine if the client is requesting a new Level
            // to be associated with it.
            if (lrMessage.UserID == 0)
            {
                // If somehow a client had a client ID of 0 but had a non null user token.
                if (lrMessage.UserToken != null)
                {
                    var lfMessage = new LoginFailedMessage
                    {
                        Message = ERROR_ACC_RESOLVE
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
                        Message = ERROR_ACC_RESOLVE
                    };
                    client.SendMessage(lfMessage);
                    return;
                }
                else
                {
                    levelSave = Server.Db.LoadLevel(lrMessage.UserID);
                    if (levelSave == null)
                        levelSave = Server.Db.NewLevel(lrMessage.UserID, lrMessage.UserToken);

                    // Check if the loaded Level with the specified ID in LoginRequestMessage has the same Token
                    // as the specified Token in LoginRequestMessage.
                    if (levelSave.Token != lrMessage.UserToken)
                    {
                        var lfMessage = new LoginFailedMessage
                        {
                            Message = ERROR_ACC_RESOLVE
                        };
                        client.SendMessage(lfMessage);
                        return;
                    }

                    level = levelSave.ToLevel(Server.Assets);
                }
            }

            // Set the Level of the client.
            client.Level = level;

            var lsMessage = new LoginSuccessMessage
            {
                UserToken = levelSave.Token,
                UserID = levelSave.ID,
                UserID1 = levelSave.ID,

                FacebookID = null,
                GameCenterID = null,

                MajorVersion = 8,
                MinorVersion = 551,
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

            client.SendMessage(lsMessage);
            client.SendMessage(ohdMessage);
        }

        // All KeepAliveResponseMessage are the same.
        private static readonly KeepAliveResponseMessage s_response = new KeepAliveResponseMessage();
        private void HandleKeepAlive(IClient client, Message message)
        {
            client.LastKeepAliveTime = DateTime.UtcNow;
            client.KeepAliveExpireTime = client.LastKeepAliveTime.AddSeconds(30);

            client.SendMessage(s_response);
        }

        private void HandleCommand(IClient client, Message message)
        {
            var cmdMessage = (CommandMessage)message;

            var level = client.Level;
            // In case the level is being processed by more than 1 thread.
            lock (level)
            {
                if (cmdMessage.Commands.Length > 0)
                {
                    // To figure out if we need to save the level.
                    var needSave = false;

                    for (int i = 0; i < cmdMessage.Commands.Length; i++)
                    {
                        var command = cmdMessage.Commands[i];

                        // Stream of commands is broken,
                        // we can exit early.
                        if (command == null)
                            break;

                        // MatchmakingCommand is a weird one,
                        // so we handle it the weird way.
                        if (command is MatchmakingCommand)
                        {
                            // Tries to look for a RandomLevel which is not
                            // the same the requesting player's level 5 times.
                            var eneLevelSave = (ILevelSave)null;
                            var count = 0;
                            do
                            {
                                eneLevelSave = Server.Db.RandomLevel();
                                count++;
                            } while (eneLevelSave.ID == level.Avatar.ID && count <= 5);

                            // Return the client home if we were unable to find
                            // a random level which is not the same as its own home.
                            if (eneLevelSave == null || eneLevelSave.ID == level.Avatar.ID)
                            {
                                Server.Log.Warn("Unable to find a random LevelSave.");

                                var ohdMessage = level.OwnHomeData;
                                client.SendMessage(ohdMessage);
                            }
                            else
                            {
                                var eneLevel = eneLevelSave.ToLevel(Server.Assets);
                                var trophyDiff = Math.Abs(eneLevel.Avatar.Trophies - level.Avatar.Trophies);

                                // State 1 == searching for opponent.
                                level.State = 1;

                                // Reward Formula.
                                // --------------------
                                // f(x) = (5x^0.25) + 5
                                // Where x is difference in trophies and f(x)
                                // is the trophy gained.

                                // Amount of Trophies the attacking player is going to get.
                                var reward = (int)Math.Round(Math.Pow((5 * trophyDiff), 0.25) + 5d);

                                // Lost Formula.
                                // --------------------
                                // f(x) = (2x^0.35) + 5
                                // Where x is difference in trophies and f(x)
                                // is the trophy lost.

                                // Amount of Trophies the player being attacked is going to lose.
                                var lost = (int)Math.Round(Math.Pow((2 * trophyDiff), 0.35) + 5d);

                                // Register this thing in our "on going attacking dictionary".
                                var info = new AttackInfo
                                {
                                    Attacker = level,
                                    Defender = eneLevel,

                                    Lost = lost,
                                    Reward = reward
                                };
                                _attackingDict.Add(level.Avatar.ID, info);

                                // Send EnemyHomeDataMessage to player.
                                var ehdMessage = eneLevel.EnemyHomeData;
                                ehdMessage.OwnAvatarData = new AvatarMessageComponent(client.Level);

                                client.SendMessage(ehdMessage);
                            }
                        }
                        else
                        {
                            level.Tick(command.Tick);
                            command.Execute(level);
                        }

                        needSave = true;
                    }

                    // Save the Level to the database if
                    // it needs to be saved.
                    if (needSave)
                    {
                        var save = client.Save;
                        Server.Db.SaveLevel(save);
                    }
                }

                level.Tick(cmdMessage.Tick);
            }
        }

        private void HandleChatMessageClient(IClient client, Message message)
        {
            var cmcMessage = (ChatMessageClientMessage)message;
            var cmsMessage = new ChatMessageServerMessage
            {
                UserID = client.Level.Avatar.ID,
                HomeID = client.Level.Avatar.ID,
                League = client.Level.Avatar.League,
                Name = client.Level.Avatar.Name,
                ExpLevel = client.Level.Avatar.ExpLevel,
                Alliance = client.Level.Avatar.Alliance,
                Message = cmcMessage.Message,
            };

            // TODO: Might want to create Chat Rooms to prevent locking on the main client list.

            // Sends the message to all the clients.
            foreach (var c in Server.Clients)
                c.SendMessage(cmsMessage);
        }

        private void HandleChangeAvatarRequestName(IClient client, Message message)
        {
            var careqMessage = (ChangeAvatarNameRequestMessage)message;
            var level = client.Level;
            var avatar = level.Avatar;
            avatar.Name = careqMessage.NewName;
            avatar.IsNamed = true;

            // Update the TutorialProgress slots to skip the
            // "Get a name" stuff.
            var tutorialProgress = avatar.TutorialProgess;
            var count = tutorialProgress.Count;
            for (int i = count; i < count + 3; i++)
                tutorialProgress.Add(new TutorialProgressSlot(21000000 + i));

            avatar.UseResource("Gold", -900);
            avatar.UseResource("Elixir", -400);

            var ascMessage = new AvailableServerCommandMessage();
            var ancCommand = new AvatarNameChangedCommand();
            ancCommand.NewName = careqMessage.NewName;
            ancCommand.Unknown1 = 1;
            ancCommand.Unknown2 = -1; // -> Tick?

            ascMessage.Command = ancCommand;

            client.SendMessage(ascMessage);

            // Save the client.
            var save = client.Save;
            Server.Db.SaveLevel(save);
        }
        #endregion
    }
}
