using CoCSharp.Logic;
using CoCSharp.Networking;
using CoCSharp.Networking.Messages.Commands;
using System;

namespace CoCSharp.Server.Handlers
{
    public delegate void CommandHandler(CoCServer server, CoCRemoteClient client, Command command);

    public static class CommandHandlers
    {
        private static void HandleBuyBuildingCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var bbCommand = command as BuyBuildingCommand;

            //TODO: Consume resource and all that jazz.

            // Look for the data in the loaded building data.
            var data = server.DataManager.FindBuilding(bbCommand.BuildingDataID, 0);

            // A token is needed for saving of the avatar
            // on event handling.
            var token = new BuildableUserToken(server, client);
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

            var token = new BuildableUserToken(server, client);
            var building = client.Avatar.Home.GetVillageObject<Building>(ubCommand.BuildingGameID);
            var data = server.DataManager.FindBuilding(building.GetDataID(), building.Level + 1);

            // Use upgraded CsvData.
            building.UserToken = token;
            building.Data = data;
            building.BeginConstruction();

            building.ConstructionFinished += UpgradeBuildingConstructionFinished;

            Console.WriteLine("\t[Up] -> BeingConstruction() for account {0} \n\t\tat {1},{2} with lvl {3}",
                              client.Avatar.Token, building.X, building.Y, building.Level);
        }

        private static void HandleSpeedUpConstructionCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var sucCommand = command as SpeedUpConstructionCommand; // interesting name indeed xD

            //TODO: Consume gems and all that jazz.

            // Gets a buildable object with the same gameId give by the sucCommand.
            var buildable = client.Avatar.Home.GetVillageObject<Buildable>(sucCommand.VillageObjectID);

            // NOTE: SpeedUpConstruction will fire up the UpgradeBuildingConstructionFinished or BuyBuildingConstructionFinished
            // event because it was speed up right.
            buildable.SpeedUpConstruction();

            Console.WriteLine("\t[Co] -> SpeedUpConstruction() for account {0} \n\t\tat {1},{2} with lvl {3}",
                              client.Avatar.Token, buildable.X, buildable.Y, buildable.Level);
        }

        private static void HandleCancelConsturctionCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var ccCommand = command as CancelConsturctionCommand;

            //TODO: Add 50% of price to resource and all that jazz.

            // Gets a buildable object with the same gameId give by the sucCommand.
            var buildable = client.Avatar.Home.GetVillageObject<Buildable>(ccCommand.VillageObjectID);
            buildable.EndConstruction();

            Console.WriteLine("\t[Co] -> EndConstruction() for account {0} \n\t\tat {1},{2} with lvl {3}",
                              client.Avatar.Token, buildable.X, buildable.Y, buildable.Level);
        }

        private static void HandleBuyResourcesCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var brCommand = command as BuyResourcesCommand;
            var embeddedCommand = brCommand.Command;

            //TODO: Add amount to resource and all that jazz.
            Console.WriteLine("\t[Re] -> BoughtResource for account {0} \n\t\tID {1} amount {2}",
                  client.Avatar.Token, brCommand.ResourceDataID, brCommand.ResourceAmount);

            if (embeddedCommand == null)
                return;

            try
            {
                var handler = (CommandHandler)null;
                if (server.CommandHandlers.TryGetValue(embeddedCommand.ID, out handler))
                    handler(server, client, embeddedCommand);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured while handling embedded command: {0}\r\n\t{1}", brCommand.GetType().Name, ex);
            }
        }

        private static void HandleMoveVillageObjectCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var mvCommand = command as MoveVillageObjectCommand;

            var villageObject = client.Avatar.Home.GetVillageObject(mvCommand.MoveData.VillageObjectGameID);
            villageObject.X = mvCommand.MoveData.X;
            villageObject.Y = mvCommand.MoveData.Y;

            Console.WriteLine("\t[Mv] -> Moved VillageObject {0} for account {1} \n\t\tto {2},{3}",
                              mvCommand.MoveData.VillageObjectGameID, client.Avatar.Token, villageObject.X, villageObject.Y);
        }

        private static void HandleMoveMultipleVillageObjectCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var mvCommand = command as MoveMultipleVillageObjectCommand;

            Console.WriteLine("\t[Mv] -> Moving multiple VillageObjects count {0} for account {1}",
                              mvCommand.MovesData.Length, client.Avatar.Token);

            for (int i = 0; i < mvCommand.MovesData.Length; i++)
            {
                var villageObject = client.Avatar.Home.GetVillageObject(mvCommand.MovesData[i].VillageObjectGameID);
                villageObject.X = mvCommand.MovesData[i].X;
                villageObject.Y = mvCommand.MovesData[i].Y;

                Console.WriteLine("\t\t[Mv] -> Moved VillageObject {0} \n\t\t\tto {1},{2}",
                                  mvCommand.MovesData[i].VillageObjectGameID, villageObject.X, villageObject.Y);
            }
        }

        public static void RegisterCommandHandlers(CoCServer server)
        {
            server.RegisterCommandHandler(new BuyBuildingCommand(), HandleBuyBuildingCommand);
            server.RegisterCommandHandler(new UpgradeBuildingCommand(), HandleUpgradeBuildingCommand);
            server.RegisterCommandHandler(new SpeedUpConstructionCommand(), HandleSpeedUpConstructionCommand);
            server.RegisterCommandHandler(new CancelConsturctionCommand(), HandleCancelConsturctionCommand);
            server.RegisterCommandHandler(new BuyResourcesCommand(), HandleBuyResourcesCommand);

            server.RegisterCommandHandler(new MoveVillageObjectCommand(), HandleMoveVillageObjectCommand);
            server.RegisterCommandHandler(new MoveMultipleVillageObjectCommand(), HandleMoveMultipleVillageObjectCommand);
        }

        private static void BuyBuildingConstructionFinished(object sender, ConstructionFinishEventArgs e)
        {
            // Remove this event handler from the buildable because it might reregister the same handler twice.
            e.BuildableConstructed.ConstructionFinished -= BuyBuildingConstructionFinished;

            var token = e.UserToken as BuildableUserToken;
            var client = token.Client;
            var server = token.Server;

            Console.Write("\t[Co] -> BeingConstruction() for account {0} \n\t\tat {1},{2} with lvl {3} - ",
                              client.Avatar.Token, e.BuildableConstructed.X, e.BuildableConstructed.Y, e.BuildableConstructed.Level);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Finished!");
            Console.ResetColor();

            //TODO: Schedule saves instead.
            server.AvatarManager.SaveAvatar(client.Avatar);
        }

        private static void UpgradeBuildingConstructionFinished(object sender, ConstructionFinishEventArgs e)
        {
            // Remove this event handler from the buildable because it might reregister the same handler twice.
            e.BuildableConstructed.ConstructionFinished -= UpgradeBuildingConstructionFinished;

            var token = e.UserToken as BuildableUserToken;
            var client = token.Client;
            var server = token.Server;

            Console.WriteLine("\t[Up] -> BeingConstruction() for account {0} \n\t\tat {1},{2} with lvl {3} - ",
                              client.Avatar.Token, e.BuildableConstructed.X, e.BuildableConstructed.Y, e.BuildableConstructed.Level);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Finished!");
            Console.ResetColor();

            //TODO: Schedule saves instead.
            server.AvatarManager.SaveAvatar(client.Avatar);
        }
    }
}
