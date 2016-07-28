using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;
using System.Diagnostics;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        private static void HandleUnlockBuildingCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var ubCommand = (UnlockBuildingCommand)command;
            var building = client.Home.GetBuilding(ubCommand.BuildingID);

            Debug.Assert(building.ID == ubCommand.BuildingID);

            if (building.IsLocked)
            {
                building.IsLocked = false;
            }
            else
            {
                // Out Of Sync.
            }
        }
    }
}
