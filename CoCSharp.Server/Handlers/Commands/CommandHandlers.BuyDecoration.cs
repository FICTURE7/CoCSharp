using CoCSharp.Data.Models;
using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;
using CoCSharp.Server.Core;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        private static void HandleBuyDecorationCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var bdCommand = (BuyDecorationCommand)command;
            var data = server.AssetManager.SearchCsv<DecorationData>(bdCommand.DecorationDataID, 0);
            var deco = new Decoration(client.Avatar.Home, bdCommand.X, bdCommand.Y);
            deco.Data = data;

            FancyConsole.WriteLine(BoughtDecorationFormat, bdCommand.DecorationDataID, client.Avatar.Token, bdCommand.X, bdCommand.Y);
        }
    }
}
