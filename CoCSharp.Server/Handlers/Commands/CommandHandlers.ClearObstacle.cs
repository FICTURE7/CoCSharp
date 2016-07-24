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
            var obstacle = client.Home.GetObstacle(coCommand.ObstacleGameID);

            // Theirs is an issue where when the client removes an obstacle, the instance ID of the
            // Obstacle is desync because we're not updating them.
            // Might cause an issue with the Decoration.
            Debug.Assert(obstacle.ID == coCommand.ObstacleGameID);

            obstacle.BeginClearing();

            FancyConsole.WriteLine(StartedClearObstacleFormat, client.Token, obstacle.X, obstacle.Y);
        }
    }
}
