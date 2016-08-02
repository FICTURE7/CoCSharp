using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;
using CoCSharp.Server.Core;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        private static void HandleBuyResourcesCommand(CoCServer server, AvatarClient client, Command command)
        {
            var brCommand = (BuyResourcesCommand)command;
            var embeddedCommand = brCommand.Command;

            client.ResourcesAmount.GetSlot(brCommand.ResourceDataID).Amount += brCommand.ResourceAmount;
            FancyConsole.WriteLine(LogFormats.Logic_Resources_Bought, client.Token, brCommand.ResourceDataID, brCommand.ResourceAmount);

            if (embeddedCommand == null)
                return;

            server.HandleCommand(client, embeddedCommand);
        }
    }
}
