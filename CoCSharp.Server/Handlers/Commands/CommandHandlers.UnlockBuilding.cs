using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;
using CoCSharp.Server.Core;
using System.Diagnostics;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        private static void HandleUnlockBuildingCommand(CoCServer server, AvatarClient client, Command command)
        {
            var ubCommand = (UnlockBuildingCommand)command;
            var building = client.Home.GetBuilding(ubCommand.BuildingID);

            Debug.Assert(building.ID == ubCommand.BuildingID);

            if (building.IsLocked)
            {
                FancyConsole.WriteLine(LogFormats.Logic_Unlocked_Building, building.ID, client.Token, building.X, building.Y);
                building.IsLocked = false;
            }
            else
            {
                // Out Of Sync.
            }
        }
    }
}
