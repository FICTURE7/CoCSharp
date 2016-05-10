using CoCSharp.Data.Slots;
using CoCSharp.Network;
using CoCSharp.Network.Messages;
using CoCSharp.Server.Core;
using System;

namespace CoCSharp.Server.Handlers
{
    public static class InGameMessageHandlers
    {
        // Minor performance improvement, just not to keep creating new KeepAliveResponse.
        private static KeepAliveResponseMessage _keepAliveRespond = new KeepAliveResponseMessage();

        private static void HandleKeepAliveRequestMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            client.NetworkManager.SendMessage(_keepAliveRespond);
        }

        private static void HandleAttackNpcMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            var anMessage = message as AttackNpcMessage;

            var npcVillage = server.NpcManager.LoadNpc(anMessage.NpcID);
            if (npcVillage == null)
                return; // TODO: Throw out of sync here or OwnHome?

            var avatar = new AvatarMessageComponent(client.Avatar)
            {
                TownHallLevel = 5,
                AllianceCastleLevel = 1,
                AllianceCastleTotalCapacity = 10,
                AllianceCastleUsedCapacity = 0
            };

            var ndMessage = new NpcDataMessage();
            ndMessage.AvatarData = avatar;
            ndMessage.NpcVillage = npcVillage;
            ndMessage.NpcID = anMessage.NpcID;

            FancyConsole.WriteLine("[&(darkmagenta)Attack&(default)] Account &(darkcyan){0}&(default) attacked NPC &(darkcyan){1}&(default).",
                client.Avatar.Token, anMessage.NpcID);

            client.NetworkManager.SendMessage(ndMessage);
        }

        private static void HandleAttackResultMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            var avatar = client.Avatar;
            var villageData = new VillageMessageComponent(avatar);

            var avatarData = new AvatarMessageComponent(avatar)
            {
                //TODO: Properly figure out townhall level and alliance castle stuff.
                TownHallLevel = 5,
                AllianceCastleLevel = 1,
                AllianceCastleTotalCapacity = 10,
                AllianceCastleUsedCapacity = 0,

                //TODO: Properly store them.
                ResourcesCapacity = new ResourceCapacitySlot[]
                {
                    new ResourceCapacitySlot(3000001, 5000),
                    new ResourceCapacitySlot(3000002, 5000)
                },

                ResourcesAmount = new ResourceAmountSlot[]
                {
                    new ResourceAmountSlot(3000001, 100000),
                    new ResourceAmountSlot(3000002, 100000)
                },

                NpcStars = server.NpcManager.CompleteNpcStarList
            };

            var ohdMessage = new OwnHomeDataMessage()
            {
                LastVisit = TimeSpan.FromSeconds(100), //TODO: Implement saving of LastVisit.
                Unknown1 = null,
                Timestamp = DateTime.UtcNow,
                OwnVillageData = villageData,
                OwnAvatarData = avatarData,
                Unknown4 = 1462629754000,
                Unknown5 = 1462629754000,
                Unknown6 = 1462631554000,
            };

            FancyConsole.WriteLine("[&(darkmagenta)Attack&(default)] Account &(darkcyan){0}&(default) returned home.",
                client.Avatar.Token);

            client.NetworkManager.SendMessage(ohdMessage);
        }

        private static void HandleCommandMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            var cmdMessage = message as CommandMessage;
            if (cmdMessage.Commands.Length > 0)
            {
                for (int i = 0; i < cmdMessage.Commands.Length; i++)
                {
                    var cmd = cmdMessage.Commands[i];

                    if (cmd == null)
                        break;

                    server.HandleCommand(client, cmd);
                }
                server.AvatarManager.SaveAvatar(client.Avatar);
            }
        }

        private static void HandleChatMessageClientMessageMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            var cmcMessage = message as ChatMessageClientMessage;
            var cmsMessage = new ChatMessageServerMessage();

            //TODO: Set alliance and all that jazz.

            cmsMessage.Name = client.Avatar.Name;
            cmsMessage.Message = cmcMessage.Message;

            server.SendMessageAll(cmsMessage);
        }

        public static void RegisterInGameMessageHandlers(CoCServer server)
        {
            server.RegisterMessageHandler(new CommandMessage(), HandleCommandMessage);
            server.RegisterMessageHandler(new KeepAliveRequestMessage(), HandleKeepAliveRequestMessage);
            server.RegisterMessageHandler(new AttackNpcMessage(), HandleAttackNpcMessage);
            server.RegisterMessageHandler(new AttackResultMessage(), HandleAttackResultMessage);
            server.RegisterMessageHandler(new ChatMessageClientMessage(), HandleChatMessageClientMessageMessage);
        }
    }
}
