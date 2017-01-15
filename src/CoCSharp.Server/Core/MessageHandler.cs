using CoCSharp.Data.Models;
using CoCSharp.Data.Slots;
using CoCSharp.Logic;
using CoCSharp.Logic.Commands;
using CoCSharp.Network;
using CoCSharp.Network.Messages;
using CoCSharp.Server.Api;
using CoCSharp.Server.Api.Chatting;
using CoCSharp.Server.Api.Core;
using CoCSharp.Server.Api.Core.Factories;
using CoCSharp.Server.Api.Db;
using CoCSharp.Server.Chatting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoCSharp.Server.Core
{
    public class MessageHandler : IMessageHandler
    {
        // All KeepAliveResponseMessage are the same.
        private static readonly KeepAliveResponseMessage s_response = new KeepAliveResponseMessage();

        private delegate Task HandlerAsync(IClient client, Message message, CancellationToken cancellationToken);

        #region Constructors
        public MessageHandler(IServer server)
        {
            if (server == null)
                throw new ArgumentNullException(nameof(server));

            _server = server;

            // Initialize our chat manager to handle chat.
            _chat = new ChatManager();
            _attackingDict = new ConcurrentDictionary<long, AttackInfo>();

            _handlers = new Dictionary<int, HandlerAsync>();
            _handlers.Add(new HandshakeRequestMessage().Id, HandleHandshakeRequest);
            _handlers.Add(new LoginRequestMessage().Id, HandleLoginRequest);
            _handlers.Add(new KeepAliveRequestMessage().Id, HandleKeepAlive);
            _handlers.Add(new ChangeAvatarNameRequestMessage().Id, HandleChangeAvatarRequestName);
            _handlers.Add(new CommandMessage().Id, HandleCommand);
            _handlers.Add(new ChatMessageClientMessage().Id, HandleChatMessageClient);
            _handlers.Add(new ReturnHomeMessage().Id, HandleReturnHome);
            _handlers.Add(new VisitHomeMessage().Id, HandleVisitHome);
            _handlers.Add(new AvatarProfileRequestMessage().Id, HandleAvatarProfileRequest);

            _handlers.Add(new AllianceDataRequestMessage().Id, HandleAllianceDataRequest);
            _handlers.Add(new AllianceSearchRequestMessage().Id, HandleAllianceSearchRequest);
            _handlers.Add(new JoinAllianceMessage().Id, HandleJoinAlliance);
            _handlers.Add(new JoinableAllianceListRequestMessage().Id, HandleJoinableAllianceListRequest);
            _handlers.Add(new LeaveAllianceMessage().Id, HandleLeaveAlliance);
            _handlers.Add(new CreateAllianceMessage().Id, HandleCreateAlliance);
            _handlers.Add(new AllianceChatMessage().Id, HandleAllianceChat);

            // Determine if we're on the maintenance mode from the server configuration.
            var maintenaceString = Server.Configuration["maintenance"];
            if (maintenaceString == null)
                _maintenance = false;
            else
                _maintenance = Convert.ToBoolean(maintenaceString);
        }

        // TODO: Remove this, instead use Sessions to keep track of what the client is up to.
        private struct AttackInfo
        {
            public Level Defender;
            public Level Attacker;

            public int Lost;
            public int Reward;
        }
        #endregion

        #region Fields & Properties
        private static readonly string s_welcome = File.ReadAllText("contents/welcome");

        private readonly ConcurrentDictionary<long, AttackInfo> _attackingDict;
        private readonly Dictionary<int, HandlerAsync> _handlers;
        private readonly IChatManager _chat;
        private readonly IServer _server;
        private readonly bool _maintenance;

        public IServer Server => _server;
        public IChatManager ChatManager => _chat;
        #endregion

        #region Methods
        public async Task HandleAsync(IClient client, Message message)
        {
            using (var source = new CancellationTokenSource())
            {
                var token = source.Token;

                Debug.Assert(client != null, "A null Client was passed to the MessageHandler.");
                Debug.Assert(message != null, "A null Message was passed to the MessageHandler.");

                var handler = (HandlerAsync)null;
                // Look for the specific message handler in our dictionary of handlers.
                if (_handlers.TryGetValue(message.Id, out handler))
                {
                    const int PROCESS_TIME = 4000;
                    //source.CancelAfter(PROCESS_TIME);
                    try
                    {
                        // Handle the message with the message handler.
                        await handler(client, message, token);
                    }
                    catch (OperationCanceledException)
                    {
                        //TODO: Add some cancel handlers?
                        Server.Logs.Warn($"Processing of {message.GetType().Name} was canceled.");
                    }
                }
                else
                {
                    Server.Logs.Warn($"MessageHandler does not contain a handler for message with ID: {message.Id}");
                }
            }
        }

        private async Task HandleReturnHome(IClient client, Message message, CancellationToken cancellationToken)
        {
            var rhMessage = (ReturnHomeMessage)message;
            var level = client.Session.Level;
            var ohdMessage = level.OwnHomeData;

            if (level.State == 2)
            {
                var info = default(AttackInfo);
                if (!_attackingDict.TryGetValue(level.Avatar.Id, out info))
                {
                    Server.Logs.Warn("Unable to obtain attack info about a client while its state was 2.");
                }
                else
                {
                    var defender = info.Defender;
                    var attacker = info.Attacker;

                    var lost = info.Lost;
                    var reward = info.Reward;

                    // Make sure we don't get negative trophies count.
                    if (defender.Avatar.Trophies >= lost)
                        defender.Avatar.Trophies -= lost;
                    else
                        defender.Avatar.Trophies = 0;

                    level.Avatar.Trophies += reward;
                    var atk = default(AttackInfo);
                    if (!_attackingDict.TryRemove(level.Avatar.Id, out atk))
                        Server.Logs.Error("Unable to remove attack-info from dictionary.");

                    var factory = Server.Factories.GetFactory<LevelSaveFactory>();
                    var defenderSave = factory.Create();
                    var attackerSave = factory.Create();

                    // Save attacker and defender.
                    defenderSave.FromLevel(defender);
                    attackerSave.FromLevel(level);

                    await Task.WhenAll(Server.Db.SaveLevelAsync(defenderSave, cancellationToken), 
                                       Server.Db.SaveLevelAsync(attackerSave, cancellationToken))
                              .ConfigureAwait(false);
                }
            }

            // State == 0, means were home.
            level.State = 0;

            client.SendMessage(ohdMessage);
        }

        private async Task HandleAvatarProfileRequest(IClient client, Message message, CancellationToken cancellationToken)
        {
            var aprMessage = (AvatarProfileRequestMessage)message;
            var ownLevel = client.Session.Level;

            // If the requested profile is the profile of the client,
            // no need to look it up in the database.
            if (aprMessage.UserId == ownLevel.Avatar.Id)
            {
                var aprrMessage = ownLevel.AvatarProfileResponse;
                client.SendMessage(aprrMessage);
            }
            else
            {
                //var save = await Server.Db.LoadLevelAsync(aprMessage.UserId);
                //var level = save.ToLevel(Server.Assets);
                var level = await Server.Levels.GetLevelAsync(aprMessage.UserId, cancellationToken);
                var aprrMessage = level.AvatarProfileResponse;

                client.SendMessage(aprrMessage);
            }
        }

        private async Task HandleVisitHome(IClient client, Message message, CancellationToken cancellationToken)
        {
            var vhMessage = (VisitHomeMessage)message;
            //var visitSave = await Server.Db.LoadLevelAsync(vhMessage.HomeId);
            var visitLevel = await Server.Levels.GetLevelAsync(vhMessage.HomeId, cancellationToken);

            // If we failed to retrieve the Level that was requested
            // we return the client home.
            if (visitLevel == null)
            {
                Server.Logs.Error($"Failed to retrieve LevelSave with ID {vhMessage.HomeId}");

                var ohdMessage = client.Session.Level.OwnHomeData;
                client.SendMessage(ohdMessage);
            }
            else
            {
                // Turn the LevelSave into a Level.
                var vhdMessage = visitLevel.VisitHomeData;
                vhdMessage.OwnAvatarData = new AvatarMessageComponent(client.Session.Level);

                client.SendMessage(vhdMessage);
            }
        }

        private Task HandleHandshakeRequest(IClient client, Message message, CancellationToken cancellationToken)
        {
            var hsMessage = new HandshakeSuccessMessage
            {
                SessionKey = client.Session.Id
            };

            client.SendMessage(hsMessage);
            return Task.FromResult<object>(null);
        }

        private async Task HandleLoginRequest(IClient client, Message message, CancellationToken cancellationToken)
        {
            var lrMessage = (LoginRequestMessage)message;
            var level = (Level)null;

            const string ERROR_ACC_RESOLVE = "Problem resolving account. Clear application data and try again.";
            const string MARKET_URL = "market://details?id=com.supercell.clashofclans";
            const int MAJ_VER = 8;
            const int MIN_VER = 709;

            // If the configuration says we're in maintenance we
            // tell the client the server is maintenance.
            if (_maintenance)
            {
                var lfMessage = new LoginFailedMessage
                {
                    Reason = LoginFailureReason.Maintenance,
                };
                client.SendMessage(lfMessage);
                return;
            }

            // Make sure the client is running on the version we want it to run.
            if (lrMessage.MajorVersion != MAJ_VER || lrMessage.MinorVersion != MIN_VER)
            {
                var lfMessage = new LoginFailedMessage
                {
                    Reason = LoginFailureReason.OutdatedVersion,
                };
                client.SendMessage(lfMessage);
            }
            // If the client master-hash is not the same as the server master-hash,
            // we ask the client to download the new assets.
            else if (lrMessage.MasterHash != Server.Assets.Fingerprint.MasterHash)
            {
                var lfMessage = new LoginFailedMessage
                {
                    Reason = LoginFailureReason.OutdatedContent,
                    FingerprintJson = Server.Assets.Fingerprint.ToJson(),
                    MarketUrl = MARKET_URL,
                    ContentUrl = Server.Configuration.ContentUrl,
                };
                client.SendMessage(lfMessage);
            }
            else
            {
                try
                {
                    // Determine if the client is requesting a new Level
                    // to be associated with it.
                    if (lrMessage.UserId == 0)
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
                        // Otherwise the client had a client ID of 0 and a null user token.
                        // Meaning its a requesting a new account.
                        else
                        {
                            level = await Server.Levels.NewLevelAsync(cancellationToken);
                        }
                    }
                    else
                    {
                        // If somehow a client had a user ID but not a user token.
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
                            level = await Server.Levels.GetLevelAsync(lrMessage.UserId, cancellationToken);

                            // If there is no level in the db with the same user id as specified, we create
                            // a level with the same user id and token as specified.
                            if (level == null)
                                level = await Server.Levels.NewLevelAsync(lrMessage.UserId, lrMessage.UserToken, cancellationToken);

                            if (level.Token != lrMessage.UserToken)
                            {
                                var lfMessage = new LoginFailedMessage
                                {
                                    Message = ERROR_ACC_RESOLVE
                                };
                                client.SendMessage(lfMessage);
                                return;
                            }
                        }
                    }
                }
                catch
                {
                    var lfMessage = new LoginFailedMessage
                    {
                        Message = ERROR_ACC_RESOLVE // Sorta lie since its not related to account resolving.
                    };
                    client.SendMessage(lfMessage);
                    throw;
                }

                // Client has been successfully logged in!
                client.Session.Login(level);

                const string ENV_PROD = "prod";
                const string FB_APP_ID = "297484437009394";

                var lsMessage = new LoginSuccessMessage
                {
                    UserToken = level.Token,
                    UserId = level.Avatar.Id,
                    HomeId = level.Avatar.Id,

                    FacebookId = null,
                    GameCenterId = null,

                    MajorVersion = MAJ_VER,
                    MinorVersion = MIN_VER,
                    RevisionVersion = 0,

                    ServerEnvironment = ENV_PROD,
                    LoginCount = level.LoginCount,
                    PlayTime = level.PlayTime,
                    FacebookAppId = FB_APP_ID,
                    DateLastSave = level.DateLastSave,
                    DateCreated = level.DateCreated,

                    GooglePlusId = null,
                    CountryCode = "EN" //TODO: Return Country code of IP.
                };

                var ohdMessage = level.OwnHomeData;

                client.SendMessage(lsMessage);
                client.SendMessage(ohdMessage);

                // If the Level forms part of a clan, we send the AllianceStream message of the alliance,
                // So that the client knows when stuff was sent.
                if (level.Avatar.Alliance != null)
                {
                    var asMessage = level.Avatar.Alliance.AllianceStream;
                    client.SendMessage(asMessage);
                }
            }
        }

        private Task HandleKeepAlive(IClient client, Message message, CancellationToken cancellationToken)
        {
            client.LastKeepAliveTime = DateTime.UtcNow;
            client.KeepAliveExpireTime = client.LastKeepAliveTime.AddSeconds(30);

            client.SendMessage(s_response);
            return Task.FromResult<object>(null);
        }

        private async Task HandleJoinableAllianceListRequest(IClient client, Message message, CancellationToken cancellationToken)
        {
            // Sends the first 64 clans to the client.
            var clans = new List<ClanCompleteMessageComponent>();
            var joinableClans = await Server.Db.SearchClansAsync(client.Session.Level, cancellationToken).ConfigureAwait(false);
            if (joinableClans != null)
            {
                foreach (var c in joinableClans)
                {
                    var clan = c.ToClan();
                    clans.Add(new ClanCompleteMessageComponent(clan));
                }
            }

            var jarrMessage = new JoinableAllianceListResponseMessage
            {
                Clans = clans.ToArray()
            };
            client.SendMessage(jarrMessage);
        }

        private async Task HandleAllianceSearchRequest(IClient client, Message message, CancellationToken cancellationToken)
        {
            var asrMessage = (AllianceSearchRequestMessage)message;
            var level = client.Session.Level;

            var clans = new List<ClanCompleteMessageComponent>();
            var search = new ClanQuery
            {
                TextSearch = asrMessage.TextSearch,
                WarFrequency = asrMessage.WarFrequency == 0 ? (int?)null : asrMessage.WarFrequency,
                ClanLocation = asrMessage.ClanLocation == 0 ? (int?)null : asrMessage.ClanLocation,
                MinimumMembers = asrMessage.MinimumMembers,
                MaximumMembers = asrMessage.MaximumMembers,
                PerkPoints = asrMessage.TrophyLimit,
                OnlyCanJoin = asrMessage.OnlyCanJoin,
                ExpLevels = asrMessage.ExpLevels
            };

            var searchClans = await Server.Db.SearchClansAsync(level, search, cancellationToken);
            if (searchClans != null)
            {
                foreach (var c in searchClans)
                {
                    var clan = c.ToClan();
                    clans.Add(new ClanCompleteMessageComponent(clan));
                }
            }

            var asrrMessage = new AllianceSearchResponseMessage
            {
                TextSearch = asrMessage.TextSearch,
                Clans = clans.ToArray()
            };

            client.SendMessage(asrrMessage);
        }

        private async Task HandleAllianceDataRequest(IClient client, Message message, CancellationToken cancellationToken)
        {
            var adrMessage = (AllianceDataRequestMessage)message;
            var clan = await Server.Clans.GetClanAsync(adrMessage.ClanId, cancellationToken);

            if (clan == null)
            {
                Server.Logs.Error($"Failed to retrieve ClanSave with ID {adrMessage.ClanId}");

                var ohdMessage = client.Session.Level.OwnHomeData;
                client.SendMessage(ohdMessage);
            }
            else
            {
                var adrrMessage = clan.AllianceDataResponse;
                client.SendMessage(adrrMessage);
            }
        }

        private async Task HandleAllianceChat(IClient client, Message message, CancellationToken cancellationToken)
        {
            var acMessage = (AllianceChatMessage)message;
            var level = client.Session.Level;
            var clan = level.Avatar.Alliance;

            if (clan == null)
            {
                Server.Logs.Warn("A client tried to send a chat message to a clan, but himself is not in a clan.");
            }
            else
            {
                var member = clan.Get(level.Avatar.Id);
                if (member == null)
                {
                    Server.Logs.Warn("A client tried to sent a chat message to a clan, but wasn't not in the clan.");
                }
                else
                {
                    var caStreamEntry = clan.Chat(level.Avatar.Id, acMessage.MessageText);
                    Debug.Assert(caStreamEntry != null);

                    var clanSave = Server.Factories.GetFactory<ClanSaveFactory>().Create();
                    clanSave.FromClan(clan);

                    await Server.Db.SaveClanAsync(clanSave, cancellationToken);

                    //TODO: Handle this stuff with the ChatManager.

                    // Send this message to all the clients online who are in the clans.
                    foreach (var c in Server.Clients)
                    {
                        // Send an alliance stream event to client.
                        var aseMessage = new AllianceStreamEventMessage
                        {
                            Entry = caStreamEntry
                        };

                        if (c?.Session.Level?.Avatar?.Alliance != null)
                        {
                            var cclan = c.Session.Level.Avatar.Alliance;
                            if (cclan.Id == clan.Id)
                                c.SendMessage(aseMessage);
                        }
                    }
                }
            }
        }

        private async Task HandleJoinAlliance(IClient client, Message message, CancellationToken cancellationToken)
        {
            var jaMessage = (JoinAllianceMessage)message;
            var clan = await Server.Clans.GetClanAsync(jaMessage.ClanId, cancellationToken);
            var level = client.Session.Level;

            if (clan == null)
            {
                Server.Logs.Warn("Client tried to join a clan that does not exists.");
            }
            else
            {
                var jolStreamEntry = clan.Join(level.Avatar);
                if (jolStreamEntry == null)
                {
                    Server.Logs.Warn("Client tried to join his own clan");
                }
                else
                {
                    client.Session.Level.Avatar.Alliance = clan;

                    var asMessage = clan.AllianceStream;
                    var ascMessage = new AvailableServerCommandMessage
                    {
                        Command = new AllianceJoinedCommand
                        {
                            ClanId = clan.Id,
                            Name = clan.Name,
                            Badge = clan.Badge,
                            ExpLevels = clan.ExpLevels,

                            Tick = -1
                        }
                    };

                    client.SendMessage(ascMessage);
                    client.SendMessage(asMessage);

                    await Server.Db.SaveLevelAsync(client.Save, cancellationToken);

                    // Send this message to all the clients online who are in the clans.
                    foreach (var c in Server.Clients)
                    {
                        // Send an alliance stream event to client.
                        var aseMessage = new AllianceStreamEventMessage
                        {
                            Entry = jolStreamEntry
                        };

                        if (c?.Session.Level?.Avatar?.Alliance != null)
                        {
                            var cclan = c.Session.Level.Avatar.Alliance;
                            if (cclan.Id == clan.Id)
                            {
                                c.SendMessage(aseMessage);
                            }
                        }
                    }
                }
            }
        }

        private async Task HandleLeaveAlliance(IClient client, Message message, CancellationToken cancellationToken)
        {
            var level = client.Session.Level;
            var clan = level.Avatar.Alliance;

            if (clan == null)
            {
                Server.Logs.Warn("A client tried to leave a clan, but himself is not in a clan.");
            }
            else
            {
                //TODO: Look for new leader in case leader leaves.
                var member = clan.Get(level.Avatar.Id);

                var jolStreamEntry = clan.Leave(level.Avatar.Id);
                if (jolStreamEntry == null)
                {
                    Server.Logs.Warn("A client tried to leave a clan, but wasn't not in the clan.");
                }
                else
                {
                    // Set avatar to null so that when it saves with ClanId = 0.
                    level.Avatar.Alliance = null;

                    var ascCommand = new AvailableServerCommandMessage
                    {
                        Command = new AllianceLeftCommand
                        {
                            ClanId = clan.Id,
                            Reason = 1,
                            Tick = -1
                        }
                    };
                    client.SendMessage(ascCommand);

                    // Set clan_id to NULL so that the FOREIGN KEY constraint does not fail.
                    var levelSave = client.Save;
                    await Server.Db.SaveLevelAsync(levelSave, cancellationToken);

                    var clanSave = Server.Factories.GetFactory<ClanSaveFactory>().Create();
                    clanSave.FromClan(clan);

                    await Server.Db.SaveClanAsync(clanSave, cancellationToken);

                    // Send this message to all the clients online who are in the clans.
                    foreach (var c in Server.Clients)
                    {
                        // Send an alliance stream event to client.
                        var aseMessage = new AllianceStreamEventMessage
                        {
                            Entry = jolStreamEntry
                        };

                        if (c?.Session.Level?.Avatar?.Alliance != null)
                        {
                            var cclan = c.Session.Level.Avatar.Alliance;
                            if (cclan.Id == clan.Id)
                            {
                                c.SendMessage(aseMessage);
                            }
                        }
                    }
                }
            }
        }

        private async Task HandleCreateAlliance(IClient client, Message message, CancellationToken cancellationToken)
        {
            var caMessage = (CreateAllianceMessage)message;

            var level = client.Session.Level;

            // Look up what it is gonna cost and how its gonna cost
            // from the /logic/globals.csv table.
            var globals = Server.Assets.DataTables.GetTable<GlobalData>();
            var resource = globals.Rows["ALLIANCE_CREATE_RESOURCE"][0].TextValue;
            var cost = globals.Rows["ALLIANCE_CREATE_COST"][0].NumberValue;
            level.Avatar.UseResource(resource, cost);

            // Request the db for a new clan.
            var clanSave = await Server.Db.NewClanAsync(cancellationToken);
            var clan = clanSave.ToClan();

            // Set the values as specified in the message.
            clan.Name = caMessage.Name;
            clan.Description = caMessage.Description;
            clan.Badge = caMessage.Badge;
            clan.InviteType = caMessage.InviteType;
            clan.RequiredTrophies = caMessage.RequiredTrophy;
            clan.WarFrequency = caMessage.WarFrequency;
            clan.Location = caMessage.Origin;
            clan.RequiredTrophies = caMessage.RequiredTrophy;

            // Add client that created the clan as a member of the clan
            // with role as the leader.
            var member = new ClanMember(client.Session.Level)
            {
                Role = ClanMemberRole.Leader,
                Rank = 1,
                PreviousRank = 1,
            };
            clan.Members.Add(member);

            // Set the clan of the creator to the created clan.
            level.Avatar.Alliance = clan;

            // Save the level which contains a ref to the ID of the clan.
            var levelSave = client.Save;
            await Server.Db.SaveLevelAsync(levelSave, cancellationToken);

            clanSave.FromClan(clan);
            // Save the clan to the db.
            await Server.Db.SaveClanAsync(clanSave, cancellationToken);

            // Let the client know he was added to the clan.
            var ascCommand1 = new AvailableServerCommandMessage
            {
                Command = new AllianceJoinedCommand
                {
                    ClanId = clan.Id,
                    Name = clan.Name,
                    Badge = clan.Badge,
                    ExpLevels = clan.ExpLevels,

                    Tick = -1,
                }
            };

            // Let the client know he is the leader of the clan.
            var ascCommand2 = new AvailableServerCommandMessage
            {
                Command = new AllianceRoleUpdatedCommand
                {
                    ClanId = clan.Id,
                    Role = member.Role,

                    Tick = -1
                }
            };

            client.SendMessage(ascCommand1);
            client.SendMessage(ascCommand2);
        }

        private async Task HandleCommand(IClient client, Message message, CancellationToken cancellationToken)
        {
            var cmdMessage = (CommandMessage)message;

            var level = client.Session.Level;
            var cclient = client as Client;
            if (cclient != null && cmdMessage.Tick >= 50 && !cclient._canChat)
            {
                ChatManager.Handle(client, "/changelog");
                ChatManager.SendChatMessage(client, s_welcome);
                cclient._canChat = true;
            }

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
                        var eneLevel = (Level)null;
                        var count = 0;
                        do
                        {
                            eneLevel = await Server.Levels.GetRandomLevelAsync(cancellationToken);
                            count++;
                        } while (eneLevel.Avatar.Id == level.Avatar.Id && count <= 5);

                        // Return the client home if we were unable to find
                        // a random level which is not the same as its own home.
                        if (eneLevel.Avatar.Id == level.Avatar.Id)
                        {
                            Server.Logs.Warn("Unable to find a random LevelSave.");

                            var ohdMessage = level.OwnHomeData;
                            client.SendMessage(ohdMessage);
                        }
                        else
                        {
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

                            if (!_attackingDict.ContainsKey(level.Avatar.Id))
                            {
                                if (!_attackingDict.TryAdd(level.Avatar.Id, info))
                                    Server.Logs.Error("Unable to add attack-info to dictionary.");
                            }
                            else
                            {
                                _attackingDict[level.Avatar.Id] = info;
                            }

                            // Send EnemyHomeDataMessage to player.
                            var ehdMessage = eneLevel.EnemyHomeData;
                            ehdMessage.OwnAvatarData = new AvatarMessageComponent(client.Session.Level);

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
                    await Server.Db.SaveLevelAsync(save, cancellationToken);
                }
            }

            level.Tick(cmdMessage.Tick);
        }

        private Task HandleChatMessageClient(IClient client, Message message, CancellationToken cancellationToken)
        {
            var senderLevel = client.Session.Level;
            var cmcMessage = (ChatMessageClientMessage)message;

            if (cmcMessage.TextMessage.StartsWith("/"))
            {
                ChatManager.Handle(client, cmcMessage.TextMessage);
            }
            else
            {
                if (cmcMessage.TextMessage.ToLower().Contains("savegame"))
                    ChatManager.SendChatMessage(client, "Did you know? 'savegame' isn't a command and does nothing? ^^");

                var cmsMessage = new ChatMessageServerMessage
                {
                    UserId = senderLevel.Avatar.Id,
                    HomeId = senderLevel.Avatar.Id,
                    League = senderLevel.Avatar.League,
                    Name = senderLevel.Avatar.Name,
                    ExpLevels = senderLevel.Avatar.ExpLevels,
                    Alliance = senderLevel.Avatar.Alliance,
                    Message = cmcMessage.TextMessage,
                };

                // TODO: Might want to create Chat Rooms to reduce network usage.

                // Sends the message to all the clients.
                foreach (var c in Server.Clients)
                    c.SendMessage(cmsMessage);
            }

            return Task.FromResult<object>(null);
        }

        private async Task HandleChangeAvatarRequestName(IClient client, Message message, CancellationToken cancellationToken)
        {
            var careqMessage = (ChangeAvatarNameRequestMessage)message;
            var level = client.Session.Level;
            var avatar = level.Avatar;
            avatar.Name = careqMessage.NewName;
            avatar.IsNamed = true;

            // Update the TutorialProgress slots to skip the
            // "Get a name" stuff.
            var tutorialProgress = avatar.TutorialProgess;
            var count = tutorialProgress.Count;
            for (int i = count; i < count + 3; i++)
                tutorialProgress.Add(new TutorialProgressSlot(21000000 + i));

            // 900 Gold and 400 Elixir is given after the tutorial ends.
            avatar.UseResource("Gold", -900);
            avatar.UseResource("Elixir", -400);

            var ascMessage = new AvailableServerCommandMessage
            {
                Command = new AvatarNameChangedCommand
                {
                    NewName = careqMessage.NewName,
                    Unknown1 = 1,
                    Unknown2 = -1 // -> Tick?
                }
            };

            client.SendMessage(ascMessage);

            // Save the client.
            var save = client.Save;
            await Server.Db.SaveLevelAsync(save, cancellationToken);
        }
        #endregion
    }
}
