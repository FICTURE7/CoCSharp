using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;
using CoCSharp.Server.Core;
using System.Diagnostics;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        private static void HandleSellDecorationCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var sdCommand = (SellDecorationCommand)command;
            var deco = client.Home.GetDecoration(sdCommand.DecorationGameID);

            Debug.Assert(deco.ID == sdCommand.DecorationGameID);

            //TODO: Improve API to allow removal of VillageObject with there game IDs.
            client.Home.Decorations.Remove(deco);

            FancyConsole.WriteLine(SoldDecorationFormat, sdCommand.DecorationGameID, client.Token, deco.X, deco.Y);
        }
    }
}
