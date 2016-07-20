using CoCSharp.Csv;
using CoCSharp.Data.Models;
using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;
using CoCSharp.Server.Core;
using System.Diagnostics;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        private static void HandleUpgradeBuildableCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var ubCommand = (UpgradeBuildableCommand)command;

            var villageObject = client.Avatar.Home.GetVillageObject(ubCommand.BuildableGameID);
            Debug.Assert(villageObject.ID == ubCommand.BuildableGameID);

            if (villageObject is Building)
            {
                var building = (Building)villageObject;
                if (!building.IsConstructing)
                {
                    building.UserToken = client;
                    building.BeginConstruction();
                    FancyConsole.WriteLine(StartedConstructionFormat, client.Avatar.Token, building.X, building.Y, building.Data.Level);
                }
                else
                {
                    FancyConsole.WriteLine(BuildableAlreadyInConstructionFormat, client.Avatar.Token, ubCommand.BuildableGameID);
                    // OutOfSync.
                }
            }
            else if (villageObject is Trap)
            {
                var trap = (Trap)villageObject;
                if (!trap.IsConstructing)
                {
                    trap.UserToken = client;
                    trap.BeginConstruction();
                    FancyConsole.WriteLine(StartedConstructionFormat, client.Avatar.Token, trap.X, trap.Y, trap.Data.Level);
                }
                else
                {
                    FancyConsole.WriteLine(BuildableAlreadyInConstructionFormat, client.Avatar.Token, ubCommand.BuildableGameID);
                    // OutOfSync.
                }
            }
            else
            {
                // Unknown Buildable object.
            }
        }
    }
}
