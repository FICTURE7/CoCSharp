using CoCSharp.Networking;
using CoCSharp.Networking.Messages;
using CoCSharp.Networking.Messages.Commands;
using System;

namespace CoCSharp.Server.Handlers
{
    public static class InGameMessageHandlers
    {
        private static void HandleKeepAliveRequestMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            client.NetworkManager.SendMessage(new KeepAliveResponseMessage());
        }

        private static void HandleCommandMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            var cmdMessage = message as CommandMessage;
            if (cmdMessage.Commands.Length > 0)
            {
                for (int i = 0; i < cmdMessage.Commands.Length; i++)
                {
                    var cmd = cmdMessage.Commands[i];
                    if (cmd is MoveBuildingCommand)
                    {
                        var mbCmd = cmd as MoveBuildingCommand;
                        Console.WriteLine("Moving building {0} to {1}, {2}", mbCmd.BuildingID, mbCmd.X, mbCmd.Y);
                        var index = mbCmd.BuildingID - 500000000;
                        client.Avatar.Home.Buildings[index].X = mbCmd.X;
                        client.Avatar.Home.Buildings[index].Y = mbCmd.Y;
                        server.AvatarManager.SaveAvatar(client.Avatar);
                    }
                }
            }
        }

        private static void HandleChatMessageClientMessageMessage(CoCServer server, CoCRemoteClient client, Message message)
        {
            var cmcMessage = message as ChatMessageClientMessage;
            var cmsMessage = new ChatMessageServerMessage();
            cmsMessage.Name = client.Avatar.Name;
            cmsMessage.Message = cmcMessage.Message;

            //TODO: Send to all users

            client.NetworkManager.SendMessage(cmsMessage);
        }

        public static void RegisterInGameMessageHandlers(CoCServer server)
        {
            server.RegisterMessageHandler(new CommandMessage(), HandleCommandMessage);
            server.RegisterMessageHandler(new KeepAliveRequestMessage(), HandleKeepAliveRequestMessage);
            server.RegisterMessageHandler(new ChatMessageClientMessage(), HandleChatMessageClientMessageMessage);
        }
    }
}
