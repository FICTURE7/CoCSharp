using CoCSharp.Data.Models;
using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;
using CoCSharp.Server.Core;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        private static void HandleBuyDecorationCommand(CoCServer server, AvatarClient client, Command command)
        {
            var bdCommand = (BuyDecorationCommand)command;
            var data = server.AssetManager.SearchCsv<DecorationData>(bdCommand.DecorationDataID, 0);
            var deco = new Decoration(client.Home, data, bdCommand.X, bdCommand.Y);

            //Debug.Assert(deco.ID == bdCommand.DecorationDataID);

            client.ResourcesAmount.GetSlot(GetResourceID(data.BuildResource)).Amount -= data.BuildCost;
            FancyConsole.WriteLine(LogFormats.Logic_Deco_Bought, bdCommand.DecorationDataID, client.Token, bdCommand.X, bdCommand.Y);
        }
    }
}
