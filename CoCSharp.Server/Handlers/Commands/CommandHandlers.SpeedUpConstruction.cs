using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;
using CoCSharp.Server.Core;
using System.Diagnostics;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        private static void HandleSpeedUpConstructionCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            // Fabulous variable name.
            var sucCommand = (SpeedUpConstructionCommand)command;

            var villageObject = client.Avatar.Home.GetVillageObject(sucCommand.BuildableGameID);
            Debug.Assert(villageObject.ID == sucCommand.BuildableGameID);

            if (villageObject is Building)
            {
                var building = (Building)villageObject;
                if (building.IsConstructing)
                {
                    building.SpeedUpConstruction();
                }
                else
                {
                    // OutOfSync.
                }
            }
            else if (villageObject is Trap)
            {
                var trap = (Trap)villageObject;
                if (trap.IsConstructing)
                {
                    trap.SpeedUpConstruction();
                }
                else
                {
                    // OutOfSync.
                }
            }
            else
            {
                // Unknown SpeedUpable VillageObject.
            }
        }
    }
}
