using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        //TODO: Rename because its no necessarily a construction.
        private static void HandleCancelConstructionCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var ccCommand = (CancelConsturctionCommand)command;
            var villageObject = client.Avatar.Home.GetVillageObject(ccCommand.VillageObjectID);
            if (villageObject is Trap)
            {
                var trap = (Trap)villageObject;
                if (trap.IsConstructing)
                {
                    trap.CancelConstruction();
                }
                else
                {
                    // OutOfSync.
                }
            }
            else if (villageObject is Building)
            {
                var building = (Building)villageObject;
                if (building.IsConstructing)
                {
                    building.CancelConstruction();
                }
                else
                {
                    // OutOfSync.
                }
            }
            else if (villageObject is Obstacle)
            {
                var obstacle = (Obstacle)villageObject;
                if (obstacle.IsClearing)
                {
                    obstacle.CancelClearing();
                }
                else
                {
                    // OutOfSync.
                }
            }
            else
            {
                // Unknown Cancellable VillageObject. 
            }
        }
    }
}
