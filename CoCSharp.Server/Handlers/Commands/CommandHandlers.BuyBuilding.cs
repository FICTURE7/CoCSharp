using CoCSharp.Data.Models;
using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;
using CoCSharp.Server.Core;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        private static void HandleBuyBuildingCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var bbCommand = (BuyBuildingCommand)command;
            var data = server.AssetManager.SearchCsv<BuildingData>(bbCommand.BuildingDataID, 0);
            var building = new Building(client.Avatar.Home, data, bbCommand.X, bbCommand.Y);

            building.UserToken = client;
            building.ConstructionFinished += BuildingConstructionFinished;
            building.BeginConstruction();

            FancyConsole.WriteLine(StartedConstructionFormat, client.Avatar.Token, bbCommand.X, bbCommand.Y, building.Data.Level);
        }
    }
}
