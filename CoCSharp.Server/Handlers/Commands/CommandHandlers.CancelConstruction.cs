using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        //TODO: Rename because its no necessarily a construction.
        private static void HandleCancelConstructionCommand(CoCServer server, AvatarClient client, Command command)
        {
            var ccCommand = (CancelConsturctionCommand)command;
            var villageObject = client.Home.GetVillageObject(ccCommand.VillageObjectID);
            if (villageObject is Trap)
            {
                var trap = (Trap)villageObject;
                if (trap.IsConstructing)
                {
                    trap.CancelConstruction();

                    client.ResourcesAmount.GetSlot(GetResourceID(trap.Data.BuildResource)).Amount += trap.NextUpgrade.BuildCost;
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

                    client.ResourcesAmount.GetSlot(GetResourceID(building.Data.BuildResource)).Amount += building.NextUpgrade.BuildCost;
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
