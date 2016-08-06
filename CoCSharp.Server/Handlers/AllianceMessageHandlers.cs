using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Network.Messages;
using CoCSharp.Network.Messages.Commands;
using System;
using System.Collections.Generic;

namespace CoCSharp.Server.Handlers
{
    public static class AllianceMessageHandlers
    {
        private static void HandleAllianceDataRequestMessage(CoCServer server, AvatarClient client, Message message)
        {
            var adreqMessage = (AllianceDataRequestMessage)message;
            var clan = server.AllianceManager.LoadClan(adreqMessage.ClanID);
            client.SendMessage(clan.AllianceDataResponseMessage);
        }

        private static void HandleJoinableAllianceListRequestMessage(CoCServer server, AvatarClient client, Message message)
        {
            var clansComponent = new List<CompleteClanMessageComponent>();
            var clans = server.AllianceManager.GetAllClan();
            var count = 0;
            foreach (var clan in clans)
            {
                if (count == 64)
                    break;
                if (clan.Members.Count == 0)
                    continue;

                var clanCom = new CompleteClanMessageComponent(clan);
                clansComponent.Add(clanCom);
                count++;
            }

            client.SendMessage(new JoinableAllianceListResponseMessage()
            {
                Clans = clansComponent.ToArray()
            });
        }

        private static void HandleCreateAllianceMessage(CoCServer server, AvatarClient client, Message message)
        {
            var caMessage = (CreateAllianceMessage)message;
            var clan = server.AllianceManager.CreateNewClan();
            clan.Name = caMessage.Name;
            clan.Description = caMessage.Description == null ? string.Empty : caMessage.Description;
            clan.WarFrequency = caMessage.WarFrequency;
            clan.WarLogsPublic = caMessage.WarLogPublic;
            clan.Badge = caMessage.Badge;
            clan.InviteType = caMessage.InviteType;
            clan.Location = caMessage.Origin;
            clan.RequiredTrophies = caMessage.RequiredTrophy;

            var member = new ClanMember(client)
            {
                Role = ClanMemberRole.Leader,
                Rank = 1,
                PreviousRank = 1,
            };
            clan.Members.Add(member);
            client.Alliance = clan;
            server.AllianceManager.QueueSave(clan);

            Console.WriteLine("alliance: created a new alliance {0}", clan.Name);
            var ajCommand = new AllianceJoinedCommand()
            {
                ClanID = clan.ID,
                Name = clan.Name,
                Badge = clan.Badge,
                Level = clan.Level,
                Tick = -1
            };

            var roCommand = new AllianceRoleUpdatedCommand()
            {
                ClanID = clan.ID,
                Unknown1 = 2,
                Role = ClanMemberRole.Leader,
                Tick = -1
            };

            var ascMessage = new AvailableServerCommandMessage()
            {
                Command = ajCommand
            };

            var rocMessage = new AvailableServerCommandMessage()
            {
                Command = roCommand
            };

            client.SendMessage(ascMessage);
            client.SendMessage(rocMessage);
        }

        private static void HandleChangeAllianceSettingMessage(CoCServer server, AvatarClient client, Message message)
        {
            // Not yet finish because working on fixing Alliance Data
            var edAlliance = (ChangeAllianceSettingMessage)message;
            var clan = server.AllianceManager.LoadClan(client.Alliance.ID);
            if (clan == null)
            {
                Log.Warning("alliance manager returned a null clan; skipping");
                return;
            }

            clan.Description = edAlliance.Description;
            clan.Badge = edAlliance.Badge;
            clan.InviteType = edAlliance.InviteType;
            clan.RequiredTrophies = edAlliance.RequiredTrophies;
            clan.WarFrequency = edAlliance.WarFrequency;
            clan.Location = edAlliance.Location;
            clan.WarLogsPublic = edAlliance.WarLogsPublic;

            server.AllianceManager.QueueSave(clan);

            var csCommand = new AllianceSettingChangedCommand()
            {
                ClanID = clan.ID,
                Badge = clan.Badge,
                Level = clan.Level,
                Tick = -1
            };
            var ascMessage = new AvailableServerCommandMessage()
            {
                Command = csCommand
            };

            client.SendMessage(ascMessage);
        }

        private static void HandleAllianceRoleUpdateMessage(CoCServer server, AvatarClient client, Message message)
        {
            var caAlliance = (AllianceChangeRoleMessage)message;
            var clan = server.AllianceManager.LoadClan(client.Alliance.ID);
            var promoteduser = clan.FindMember(caAlliance.UserID);

            if (clan == null)
            {
                Log.Warning("alliance manager returned a null clan; skipping");
                return;
            }
             
            if (promoteduser == null)
            {
                Log.Warning("promoted/demoted user not exist in clan; skipping");
                return;
            }

            if (clan.FindMember(client.ID).Role == ClanMemberRole.Leader || clan.FindMember(client.ID).Role == ClanMemberRole.CoLeader)
            {
                promoteduser.Role = caAlliance.Role;

                if (caAlliance.Role == ClanMemberRole.Leader)
                {
                    clan.FindMember(client.ID).Role = ClanMemberRole.CoLeader;

                    var csbCommand = new AllianceRoleUpdatedCommand()
                    {
                        ClanID = clan.ID,
                        Role = ClanMemberRole.CoLeader,
                        Unknown1 = (int)ClanMemberRole.CoLeader,
                        Tick = -1
                    };
                    var ascbMessage = new AvailableServerCommandMessage()
                    {
                        Command = csbCommand
                    };
                    client.SendMessage(ascbMessage);
                }

                var csCommand = new AllianceRoleUpdatedCommand()
                {
                    ClanID = clan.ID,
                    Role = caAlliance.Role,
                    Unknown1 = (int)caAlliance.Role,
                    Tick = -1
                };

                var ascMessage = new AvailableServerCommandMessage()
                {
                    Command = csCommand
                };

                var cssMessage = new AllianceChangeRoleOkMessage()
                {
                    UserID = caAlliance.UserID,
                    Role = caAlliance.Role
                };

                var user = server.Clients.Find(a => a.ID == caAlliance.UserID);
                if (user != null)
                    user.SendMessage(ascMessage);

                client.SendMessage(cssMessage);
                server.AllianceManager.QueueSave(clan);
            }
        }

        private static void HandleJoinAllianceMessage(CoCServer server, AvatarClient client, Message message)
        {
            var jaAlliance = (JoinAllianceMessage)message;
            var clan = server.AllianceManager.LoadClan(jaAlliance.ClanID);
            if (clan == null)
            {
                Log.Warning("alliance manager returned a null clan; skipping");
                return;
            }

            if (!clan.AddMember(client))
            {
                Log.Warning("client tried to join a clan its already in; skipping");
                return;
            }
            server.AllianceManager.QueueSave(clan);

            var ajCommand = new AllianceJoinedCommand()
            {
                ClanID = clan.ID,
                Name = clan.Name,
                Badge = clan.Badge,
                Level = clan.Level,
                Tick = -1
            };
            var ascMessage = new AvailableServerCommandMessage()
            {
                Command = ajCommand
            };

            client.Alliance = clan;
            client.SendMessage(ascMessage);
        }

        private static void HandleLeaveAllianceMessage(CoCServer server, AvatarClient client, Message message)
        {
            var laMessage = (LeaveAllianceMessage)message;
            var clan = client.Alliance;
            var clientMember = clan.FindMember(client.ID);
            if (clientMember == null)
            {
                Log.Warning("an avatar tried to leave a clan it is not in; skipping");
                return;
            }

            clan.RemoveMember(client.ID);

            if (clan.Members.Count == 0)
            {
                server.AllianceManager.QueueDelete(clan);
            }
            else if (clientMember.Role == ClanMemberRole.Leader)
            {
                // Apparently the oldest CoLeader becomes the Leader and
                // if there are no CoLeaders the oldest Elder becomes the Leader and
                // if there are no Elders the oldest Member becomes the Leader.

                // Set the first co-leader in the list to be the leader.
                // or the first elder in the list if there are no co-leaders
                // or the first member in the list if there are no elders.
                var newLeader = clan.Members[0];

                var firstCoLeader = (ClanMember)null;
                var firstElder = (ClanMember)null;
                var firstMember = (ClanMember)null;
                for (int i = 0; i < clan.Members.Count; i++)
                {
                    var member = clan.Members[i];
                    if (member.Role == ClanMemberRole.CoLeader)
                    {
                        firstCoLeader = member;
                        break;
                    }

                    if (firstElder == null && member.Role == ClanMemberRole.Elder)
                    {
                        firstElder = member;
                        continue;
                    }

                    if (firstMember == null && member.Role == ClanMemberRole.Member)
                    {
                        firstMember = member;
                        continue;
                    }
                }

                if (firstCoLeader != null)
                    newLeader = firstCoLeader;
                else if (firstElder != null)
                    newLeader = firstElder;
                else if (firstMember != null)
                    newLeader = firstMember;

                newLeader.Role = ClanMemberRole.Leader;
                server.AllianceManager.QueueSave(clan);
            }

            var alCommand = new AllianceLeftCommand()
            {
                ClanID = clan.ID,
                Reason = 1,
                Tick = -1
            };
            var ascMessage = new AvailableServerCommandMessage()
            {
                Command = alCommand
            };

            client.Alliance = null;
            client.SendMessage(ascMessage);
        }

        public static void RegisterAllianceMessageHandlers(CoCServer server)
        {
            server.RegisterMessageHandler(new AllianceDataRequestMessage(), HandleAllianceDataRequestMessage);

            server.RegisterMessageHandler(new JoinableAllianceListRequestMessage(), HandleJoinableAllianceListRequestMessage);

            server.RegisterMessageHandler(new CreateAllianceMessage(), HandleCreateAllianceMessage);

            server.RegisterMessageHandler(new JoinAllianceMessage(), HandleJoinAllianceMessage);
            server.RegisterMessageHandler(new LeaveAllianceMessage(), HandleLeaveAllianceMessage);

            server.RegisterMessageHandler(new ChangeAllianceSettingMessage(), HandleChangeAllianceSettingMessage);
            server.RegisterMessageHandler(new AllianceChangeRoleMessage(), HandleAllianceRoleUpdateMessage);
        }
    }
}
