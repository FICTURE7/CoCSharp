using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;
using CoCSharp.Server.Core;
using System.Diagnostics;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        private static void HandleClearObstacleCommand(CoCServer server, AvatarClient client, Command command)
        {
            var coCommand = command as ClearObstacleCommand;
            var obstacle = client.Home.GetObstacle(coCommand.ObstacleGameID);

            // Theirs is an issue where when the client removes an obstacle, the instance ID of the
            // Obstacle is desync because we're not updating them.
            // Might cause an issue with the Decoration.
            Debug.Assert(obstacle.ID == coCommand.ObstacleGameID);

            client.ResourcesAmount.GetSlot(GetResourceID(obstacle.Data.ClearResource)).Amount -= obstacle.Data.ClearCost;
            obstacle.BeginClearing();

            FancyConsole.WriteLine(LogFormats.Logic_Clearing_Started, client.Token, obstacle.X, obstacle.Y);
        }
    }
}
