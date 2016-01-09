using CoCSharp.Networking;
using CoCSharp.Networking.Messages;

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
                    if (cmd == null)
                        continue;

                    var handler = (CommandHandler)null;
                    if (server.CommandHandlers.TryGetValue(cmd.ID, out handler))
                        handler(server, client, cmd);
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

            for (int i = 0; i < server.Clients.Count; i++)
                server.Clients[i].NetworkManager.SendMessage(cmsMessage);
        }

        public static void RegisterInGameMessageHandlers(CoCServer server)
        {
            server.RegisterMessageHandler(new CommandMessage(), HandleCommandMessage);
            server.RegisterMessageHandler(new KeepAliveRequestMessage(), HandleKeepAliveRequestMessage);
            server.RegisterMessageHandler(new ChatMessageClientMessage(), HandleChatMessageClientMessageMessage);
        }
    }
}
