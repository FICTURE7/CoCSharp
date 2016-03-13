using CoCSharp.Logic;
using CoCSharp.Networking;
using CoCSharp.Networking.Messages.Commands;
using System;

namespace CoCSharp.Server.Handlers
{
    public static class ConstructionCommandHandlers
    {
        private static void HandleBuyBuildingCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var bbCommand = command as BuyBuildingCommand;

            //TODO: Consume resource and all that jazz.

            // Look for the data in the loaded building data.
            var data = server.DataManager.FindBuilding(bbCommand.BuildingDataID, 0);

            // A token is needed for saving of the avatar
            // on event handling.
            var token = new VillageObjectUserToken(server, client);
            var building = new Building(bbCommand.X, bbCommand.Y, token);
            client.Avatar.Home.Buildings.Add(building);

            building.ConstructionFinished += BuyBuildingConstructionFinished;

            building.Data = data;
            building.BeginConstruction();

            Console.WriteLine("\t[Co] -> BeingConstruction() for account {0} \n\t\tat {1},{2} with lvl {3}",
                              client.Avatar.Token, bbCommand.X, bbCommand.Y, building.Level);
        }

        private static void HandleUpgradeBuildingCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var ubCommand = command as UpgradeBuildingCommand;

            //TODO: Consume resource and all that jazz.

            var token = new VillageObjectUserToken(server, client);
            var building = client.Avatar.Home.GetVillageObject<Buildable>(ubCommand.BuildingGameID);
            var data = server.DataManager.FindBuilding(building.GetDataID(), building.Level + 1);

            // Use upgraded CsvData.
            building.UserToken = token;
            building.Data = data;
            building.BeginConstruction();

            building.ConstructionFinished += UpgradeBuildingConstructionFinished;

            Console.WriteLine("\t[Up] -> BeingConstruction() for account {0} \n\t\tat {1},{2} with lvl {3}",
                              client.Avatar.Token, building.X, building.Y, building.Level);
        }

        private static void HandleUpgradeMultipleBuildingsCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var umbCommand = command as UpgradeMultipleBuildingsCommand;

            Console.WriteLine("\t[Up] -> Upgrading multiple Buildings count {0} for account {1}",
                              umbCommand.BuildingsGameID.Length, client.Avatar.Token);

            for (int i = 0; i < umbCommand.BuildingsGameID.Length; i++)
            {
                var gameId = umbCommand.BuildingsGameID[i];
                var token = new VillageObjectUserToken(server, client);
                var building = client.Avatar.Home.GetVillageObject<Buildable>(gameId);
                var data = server.DataManager.FindBuilding(building.GetDataID(), building.Level + 1);

                building.UserToken = token;
                building.Data = data;
                building.BeginConstruction();

                building.ConstructionFinished += UpgradeBuildingConstructionFinished;

                Console.WriteLine("\t\t[Mv] -> Upgrading Building {0} \n\t\t\tto {1},{2}",
                                  gameId, building.X, building.Y);
            }
        }

        private static void HandleBuyTrapCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var btCommand = command as BuyTrapCommand;

            //TODO: Consume resource and all that jazz.

            // Look for the data in the loaded trap data.
            var data = server.DataManager.FindTrap(btCommand.TrapDataID, 0);

            // A token is needed for saving of the avatar
            // on event handling.
            var token = new VillageObjectUserToken(server, client);
            var building = new Trap(btCommand.X, btCommand.Y, token);
            client.Avatar.Home.Traps.Add(building);

            building.ConstructionFinished += BuyBuildingConstructionFinished;

            building.Data = data;
            building.BeginConstruction();

            Console.WriteLine("\t[Co] -> BeingConstruction() for account {0} \n\t\tat {1},{2} with lvl {3}",
                              client.Avatar.Token, btCommand.X, btCommand.Y, building.Level);
        }

        #region Events

        private static void BuyBuildingConstructionFinished(object sender, ConstructionFinishEventArgs e)
        {
            // Remove this event handler from the buildable because it might re-register the same handler twice.
            e.BuildableConstructed.ConstructionFinished -= BuyBuildingConstructionFinished;

            var token = e.UserToken as VillageObjectUserToken;
            var client = token.Client;
            var server = token.Server;

            Console.Write("\t[Co] -> BeingConstruction() for account {0} \n\t\tat {1},{2} with lvl {3} - ",
                          client.Avatar.Token, e.BuildableConstructed.X, e.BuildableConstructed.Y, e.BuildableConstructed.Level);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            if (!e.WasCancelled)
                Console.WriteLine("Finished!");
            else
                Console.WriteLine("Cancelled!");
            Console.ResetColor();

            //TODO: Schedule saves instead.
            server.AvatarManager.SaveAvatar(client.Avatar);
        }

        private static void UpgradeBuildingConstructionFinished(object sender, ConstructionFinishEventArgs e)
        {
            // Remove this event handler from the buildable because it might re-register the same handler twice.
            e.BuildableConstructed.ConstructionFinished -= UpgradeBuildingConstructionFinished;

            var token = e.UserToken as VillageObjectUserToken;
            var client = token.Client;
            var server = token.Server;

            Console.Write("\t[Up] -> BeingConstruction() for account {0} \n\t\tat {1},{2} with lvl {3} - ",
                          client.Avatar.Token, e.BuildableConstructed.X, e.BuildableConstructed.Y, e.BuildableConstructed.Level);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            if (!e.WasCancelled)
                Console.WriteLine("Finished!");
            else
                Console.WriteLine("Cancelled!");
            Console.ResetColor();

            //TODO: Schedule saves instead.
            server.AvatarManager.SaveAvatar(client.Avatar);
        }

        #endregion

        public static void RegisterCommandHandlers(CoCServer server)
        {
            server.RegisterCommandHandler(new BuyBuildingCommand(), HandleBuyBuildingCommand);
            server.RegisterCommandHandler(new UpgradeBuildingCommand(), HandleUpgradeBuildingCommand);
            server.RegisterCommandHandler(new UpgradeMultipleBuildingsCommand(), HandleUpgradeMultipleBuildingsCommand);

            server.RegisterCommandHandler(new BuyTrapCommand(), HandleBuyTrapCommand);
        }
    }
}
