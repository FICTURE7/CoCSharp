using System;
using CoCSharp.Data.Models;
using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        private static void HandleBuyBuildingCommand(Server server, AvatarClient client, Command command)
        {
            var bbCommand = (BuyBuildingCommand)command;
            var data = server.AssetManager.SearchCsv<BuildingData>(bbCommand.BuildingDataID, 0);
            var building = new Building(client.Home, data, bbCommand.X, bbCommand.Y, -1);

            //Console.WriteLine("bought new building! {0}", bbCommand.BuildingDataID);
        }
    }
}
