using CoCSharp.Networking;
using CoCSharp.Networking.Messages.Commands;
using CoCSharp.Server.Core;
using System;

namespace CoCSharp.Server.Handlers
{
    public static partial class CommandHandlers
    {
        private static void HandleBuyResourcesCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var brCommand = (BuyResourcesCommand)command;
            var embeddedCommand = brCommand.Command;

            //TODO: Add amount to resource and all that jazz.
            FancyConsole.WriteLine("[&(darkgreen)Logic&(default)] Bought resource -> {1} for account &(darkcyan){0}&(default) \n\t\tAmount: {2}",
                  client.Avatar.Token, brCommand.ResourceDataID, brCommand.ResourceAmount);

            if (embeddedCommand == null)
                return;

            try
            {
                var handler = (CommandHandler)null;
                if (server.CommandHandlerDictionary.TryGetValue(embeddedCommand.ID, out handler))
                    handler(server, client, embeddedCommand);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred while handling embedded command: {0}\r\n\t{1}", brCommand.GetType().Name, ex);
            }
        }
    }
}
