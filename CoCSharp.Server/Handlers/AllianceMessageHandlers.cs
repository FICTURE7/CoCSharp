using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Network.Messages;
using CoCSharp.Network.Messages.Commands;
using CoCSharp.Server.Core;
using System.Collections.Generic;

namespace CoCSharp.Server.Handlers
{
    public static class AllianceMessageHandlers
    {
        private static void HandleAllianceDataRequestMessage(CoCServer server, AvatarClient client, Message message)
        {
            var adreqMessage = (AllianceDataRequestMessage)message;
            var clan = server.AllianceManager.LoadClan(adreqMessage.ClanID);

            FancyConsole.WriteLine(LogFormats.Alliance_Data_Requested, clan.Name, client.Token); 
            client.NetworkManager.SendMessage(clan.AllianceDataResponseMessage);
        }

        private static void HandleJoinableAllianceListRequestMessage(CoCServer server, AvatarClient client, Message message)
        {
            var clansComponent = new List<CompleteClanMessageComponent>();
            var clans = server.AllianceManager.GetAllClan();
            foreach (var clan in clans)
            {
                if (clan.Members.Count == 0)
                    continue;

                var clanCom = new CompleteClanMessageComponent(clan);
                clansComponent.Add(clanCom);
            }

            client.NetworkManager.SendMessage(new JoinableAllianceListResponseMessage()
            {
                Clans = clansComponent.ToArray()
            });
        }

        private static void HandleCreateAllianceMessage(CoCServer server, AvatarClient client, Message message)
        {
            var caMessage = (CreateAllianceMessage)message;
            var clan = server.AllianceManager.CreateNewClan();
            clan.Name = caMessage.Name;
            clan.Description = caMessage.Description;
            clan.WarFrequency = caMessage.WarFrequency;
            clan.WarLogsPublic = caMessage.WarLogPublic;
            clan.Badge = caMessage.Badge;
            clan.InviteType = caMessage.InviteType;
            clan.Location = caMessage.Origin;
            clan.RequiredTrophies = caMessage.RequiredTrophy;

            var member = new ClanMember(client);
            member.Role = ClanMemberRole.Leader;
            member.Rank = 1;
            member.PreviousRank = 1;
            clan.Members.Add(member);

            FancyConsole.WriteLine(LogFormats.Alliance_Created, clan.Name, client.Token);
            server.AllianceManager.Queue(clan);

            var ajCommand = new AllianceJoinedCommand();
            ajCommand.ClanID = clan.ID;
            ajCommand.Name = clan.Name;
            ajCommand.Badge = clan.Badge;
            ajCommand.Level = clan.Level;
            ajCommand.Tick = -1;
            var ascMessage = new AvailableServerCommandMessage();
            ascMessage.Command = ajCommand;

            client.Alliance = clan;
            client.NetworkManager.SendMessage(ascMessage);
        }

        private static void HandleJoinAllianceMessage(CoCServer server, AvatarClient client, Message message)
        {
            var jaAlliance = (JoinAllianceMessage)message;
            var clan = server.AllianceManager.LoadClan(jaAlliance.ClanID);
            if (clan == null)
            {
                // Wut?
            }

            var member = new ClanMember(client);
            member.Role = ClanMemberRole.Member;
            member.Rank = clan.Members.Count;
            member.PreviousRank = clan.Members.Count;

            clan.Members.Add(member);

            FancyConsole.WriteLine("[&(darkblue)Alliance&(default)] Joined -> Account &(darkcyan){0}&(default) joined &(darkcyan){1}&(default).",
                client.Token, clan.Name);
            server.AllianceManager.Queue(clan);

            var ajCommand = new AllianceJoinedCommand();
            ajCommand.ClanID = clan.ID;
            ajCommand.Name = clan.Name;
            ajCommand.Badge = clan.Badge;
            ajCommand.Level = clan.Level;
            ajCommand.Tick = -1;
            var ascMessage = new AvailableServerCommandMessage();
            ascMessage.Command = ajCommand;

            client.Alliance = clan;
            client.NetworkManager.SendMessage(ascMessage);
        }

        private static void HandleLeaveAllianceMessage(CoCServer server, AvatarClient client, Message message)
        {
            var laMessage = (LeaveAllianceMessage)message;
            var clan = client.Alliance;
            var member = clan.FindMember(client.ID);

            if (!clan.RemoveMember(client.ID))
            {
                // Wut?
            }

            FancyConsole.WriteLine("[&(darkblue)Alliance&(default)] Left -> Account &(darkcyan){0}&(default) left &(darkcyan){1}&(default).",
                client.Token, clan.Name);

            if (clan.Members.Count == 0)
            {
                server.AllianceManager.Delete(clan);
                FancyConsole.WriteLine("[&(darkblue)Alliance&(default)] &(red)Deleted&(default) -> Clan &(darkcyan){0}&(default).",
                    clan.Name);
            }
            else if(member.Role == ClanMemberRole.Leader)
            {
                // Apparently the oldest CoLeader becomes the Leader and
                // if their is no CoLeaders the oldest Elder becomes the Leader and
                // if their is no Elder the oldest Member becomes the Leader.

                // Set the first co-leader in the list to be the leader.
                // or the first elder in the list if their are no co-leaders
                // or the first member in the list if their are no elder.
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
            }

            server.AllianceManager.Queue(clan);

            var alCommand = new AllianceLeftCommand();
            alCommand.ClanID = clan.ID;
            alCommand.Reason = 1;
            alCommand.Tick = -1;
            var ascMessage = new AvailableServerCommandMessage();
            ascMessage.Command = alCommand;

            client.Alliance = null;
            client.NetworkManager.SendMessage(ascMessage);
        }

        public static void RegisterAllianceMessageHandlers(CoCServer server)
        {
            server.RegisterMessageHandler(new AllianceDataRequestMessage(), HandleAllianceDataRequestMessage);

            server.RegisterMessageHandler(new JoinableAllianceListRequestMessage(), HandleJoinableAllianceListRequestMessage);

            server.RegisterMessageHandler(new CreateAllianceMessage(), HandleCreateAllianceMessage);

            server.RegisterMessageHandler(new JoinAllianceMessage(), HandleJoinAllianceMessage);
            server.RegisterMessageHandler(new LeaveAllianceMessage(), HandleLeaveAllianceMessage);
        }
    }
}
