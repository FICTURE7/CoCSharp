using CoCSharp.Data.Models;
using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;
using CoCSharp.Server.Core;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        private static void HandleClearObstacleCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var coCommand = command as ClearObstacleCommand;
            var token = new VillageObjectToken(server, client);
            var obstacle = client.Avatar.Home.GetObstacle(coCommand.ObstacleGameID);
            var data = server.AssetManager.SearchCsv<ObstacleData>(obstacle.GetDataID(), 0);

            obstacle.UserToken = token;
            obstacle.Data = data;
            obstacle.BeginClearing();
            obstacle.ClearingFinished += ClearingFinished;

            FancyConsole.WriteLine(StartedClearObstacleFormat, client.Avatar.Token, obstacle.X, obstacle.Y);
        }
    }
}
