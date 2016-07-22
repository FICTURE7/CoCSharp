using CoCSharp.Data;
using CoCSharp.Data.Models;
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
            {
                client.NetworkManager.SendMessage(client.Avatar.OwnHomeDataMessage);
                return;
            }

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
            if (cmcMessage.Message[0] == '/')
            {
                var cmd = cmcMessage.Message.Substring(1);
                cmsMessage.Name = "Server";
                switch (cmd)
                {
                    case "addgems":
                        client.Avatar.Gems += 500;
                        client.Avatar.FreeGems += 500;

                        cmsMessage.Message = "Added 500 gems.";
                        client.NetworkManager.SendMessage(cmsMessage);

                        //var ohdMessage = client.Avatar.OwnHomeDataMessage;
                        client.NetworkManager.SendMessage(client.Avatar.OwnHomeDataMessage);
                        return;

                    case "clearobstacles":
                        var count = client.Avatar.Home.Obstacles.Count;
                        client.Avatar.Home.Obstacles.Clear();

                        //var ohdMessage = client.Avatar.OwnHomeDataMessage;
                        client.NetworkManager.SendMessage(client.Avatar.OwnHomeDataMessage);

                        cmsMessage.Message = "Cleared " + count + " obstacles.";
                        client.NetworkManager.SendMessage(cmsMessage);
                        return;

                    case "max":
                        var countBuilding = client.Avatar.Home.Buildings.Count;
                        for (int i = 0; i < countBuilding; i++)
                        {
                            var building = client.Avatar.Home.Buildings[i];
                            var collection = AssetManager.DefaultInstance.SearchCsv<BuildingData>(building.Data.ID);
                            var data = collection[collection.Count - 1];
                            if (building.IsConstructing)
                                building.CancelConstruction();

                            building.Data = data;
                        }

                        var countTraps = client.Avatar.Home.Traps.Count;
                        for (int i = 0; i < countTraps; i++)
                        {
                            var trap = client.Avatar.Home.Traps[i];
                            var collection = AssetManager.DefaultInstance.SearchCsv<TrapData>(trap.Data.ID);
                            var data = collection[collection.Count - 1];
                            if (trap.IsConstructing)
                                trap.CancelConstruction();

                            trap.Data = data;
                        }

                        cmsMessage.Message = "Maxed " + countBuilding + " buildings and " + countTraps + " traps.";
                        client.NetworkManager.SendMessage(cmsMessage);

                        client.NetworkManager.SendMessage(client.Avatar.OwnHomeDataMessage);
                        return;

                    default:
                        cmsMessage.Message = "Unknown command.";
                        client.NetworkManager.SendMessage(cmsMessage);
                        return;
                }
            }

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
