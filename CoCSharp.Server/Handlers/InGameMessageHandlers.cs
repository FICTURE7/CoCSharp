using CoCSharp.Network;
using CoCSharp.Network.Messages;
using CoCSharp.Server.Core;

namespace CoCSharp.Server.Handlers
{
    public static class InGameMessageHandlers
    {
        private static KeepAliveResponseMessage s_keepAliveRespond = new KeepAliveResponseMessage();

        private static void HandleKeepAliveRequestMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            client.NetworkManager.SendMessage(s_keepAliveRespond);
        }

        private static void HandleAttackNpcMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            var anMessage = message as AttackNpcMessage;

            var npcVillage = server.NpcManager.LoadNpc(anMessage.NpcID);
            if (npcVillage == null)
                return; // TODO: Throw out of sync here or OwnHome?

            var avatar = new AvatarMessageComponent(client.Avatar)
            {
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
            var ohdMessage = client.Avatar.OwnHomeDataMessage;

            FancyConsole.WriteLine("[&(darkmagenta)Attack&(default)] Account &(darkcyan){0}&(default) returned home.",
                client.Avatar.Token);

            client.NetworkManager.SendMessage(ohdMessage);
        }

        private static void HandleAvatarProfileRequestMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            var aprMessage = new AvatarProfileResponseMessage();
            aprMessage.Village = client.Avatar.Home;
            aprMessage.AvatarData = new AvatarMessageComponent(client.Avatar);

            //aprMessage.AvatarData.Unknown13 = 2; // League
            //aprMessage.AvatarData.Unknown14 = 13;
            //aprMessage.AvatarData.Unknown27 = 13;

            //aprMessage.AvatarData.AchievementProgress = new AchievementProgessSlot[]
            //{
            //    //new AchievementProgessSlot(23000021, 306),
            //    //new AchievementProgessSlot(23000022, 306),
            //    new AchievementProgessSlot(23000023, 306), // 23000021 to 23000023 -> all time best trophies.

            //    //new AchievementProgessSlot(23000060, 306), 
            //    //new AchievementProgessSlot(23000061, 306),
            //    new AchievementProgessSlot(23000062, 306) // 23000060 to 23000062 -> War Stars count.
            //};

            FancyConsole.WriteLine("[&(darkmagenta)Avatar&(default)] Profile &(darkcyan){0}&(default) was requested.",
                client.Avatar.Token);

            client.NetworkManager.SendMessage(aprMessage);
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

            cmsMessage.Level = client.Avatar.Level;
            cmsMessage.CurrentUserID = client.Avatar.ID;
            cmsMessage.UserID = client.Avatar.ID;
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
            server.RegisterMessageHandler(new AvatarProfileRequestMessage(), HandleAvatarProfileRequestMessage);
        }
    }
}
