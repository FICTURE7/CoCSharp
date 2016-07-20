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
        private static void HandleUpgradeMultipleBuildableCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var umbCommand = (UpgradeMultipleBuildableCommand)command;

            for (int i = 0; i < umbCommand.BuildingsGameID.Length; i++)
            {
                var gameId = umbCommand.BuildingsGameID[i];
                var villageObject = client.Avatar.Home.GetVillageObject(gameId);

                Debug.Assert(villageObject.ID == gameId);

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
                        FancyConsole.WriteLine(BuildableAlreadyInConstructionFormat, client.Avatar.Token, gameId);
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
                        FancyConsole.WriteLine(BuildableAlreadyInConstructionFormat, client.Avatar.Token, gameId);
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
}
