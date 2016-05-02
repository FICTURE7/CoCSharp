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
            var data = server.DataManager.FindDecoration(bdCommand.DecorationDataID);
            var deco = new Decoration(bdCommand.X, bdCommand.Y);
            deco.Data = data;
            client.Avatar.Home.Decorations.Add(deco);

            FancyConsole.WriteLine(BoughtDecorationFormat, bdCommand.DecorationDataID, client.Avatar.Token, bdCommand.X, bdCommand.Y);
        }
    }
}
