using CoCSharp.Networking;
using CoCSharp.Networking.Messages;
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
                    
                    // Should probably drop the client as well, because the network stream is very likely to be messed up.
                    if (cmd == null)
                        break;

                    try
                    {
                        var handler = (CommandHandler)null;
                        if (server.CommandHandlerDictionary.TryGetValue(cmd.ID, out handler))
                            handler(server, client, cmd);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Exception occurred while handling command: {0}\r\n\t{1}", cmd.GetType().Name, ex);
                    }
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
