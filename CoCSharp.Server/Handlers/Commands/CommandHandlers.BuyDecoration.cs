using CoCSharp.Data.Models;
using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;
using CoCSharp.Server.Core;
using System.Diagnostics;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        private static void HandleBuyDecorationCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var bdCommand = (BuyDecorationCommand)command;
            var data = server.AssetManager.SearchCsv<DecorationData>(bdCommand.DecorationDataID, 0);
            var deco = new Decoration(client.Avatar.Home, data, bdCommand.X, bdCommand.Y);

            Debug.Assert(deco.ID == bdCommand.DecorationDataID);

            FancyConsole.WriteLine(BoughtDecorationFormat, bdCommand.DecorationDataID, client.Avatar.Token, bdCommand.X, bdCommand.Y);
        }
    }
}
