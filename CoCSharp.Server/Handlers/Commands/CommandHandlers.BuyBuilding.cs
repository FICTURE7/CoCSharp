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
            var token = new VillageObjectToken(server, client);
            var data = server.DataManager.FindBuilding(bbCommand.BuildingDataID, 0);
            var building = new Building(client.Avatar.Home, bbCommand.X, bbCommand.Y, token);

            building.ConstructionFinished += ConstructionFinished;
            building.Data = data;
            building.BeginConstruction();

            FancyConsole.WriteLine(StartedConstructionFormat, client.Avatar.Token, bbCommand.X, bbCommand.Y, building.Level);
        }
    }
}
