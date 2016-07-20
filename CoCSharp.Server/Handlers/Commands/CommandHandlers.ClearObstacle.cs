using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;
using CoCSharp.Server.Core;
using System.Diagnostics;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        private static void HandleClearObstacleCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var coCommand = command as ClearObstacleCommand;
            var obstacle = client.Avatar.Home.GetObstacle(coCommand.ObstacleGameID);

            Debug.Assert(obstacle.ID == coCommand.ObstacleGameID);

            obstacle.UserToken = client;
            obstacle.BeginClearing();

            FancyConsole.WriteLine(StartedClearObstacleFormat, client.Avatar.Token, obstacle.X, obstacle.Y);
        }
    }
}
