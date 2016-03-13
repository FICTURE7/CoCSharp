using CoCSharp.Logic;
using CoCSharp.Networking;
using CoCSharp.Networking.Messages.Commands;
using System;

namespace CoCSharp.Server.Handlers
{
    public delegate void CommandHandler(CoCServer server, CoCRemoteClient client, Command command);

    public static class VillageObjectCommandHandlers
    {
        private static void HandleClearObstacleCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var coCommand = command as ClearObstacleCommand;

            var token = new VillageObjectUserToken(server, client);
            var obstacle = client.Avatar.Home.GetVillageObject<Obstacle>(coCommand.ObstacleGameID);
            var data = server.DataManager.FindObstacle(obstacle.GetDataID());

            obstacle.UserToken = token;
            obstacle.Data = data;
            obstacle.BeginClearing();
            obstacle.ClearingFinished += ObstacleClearingFinished;

            Console.WriteLine("\t[Cl] -> BeingClearing() for account {0} \n\t\tat {1},{2} - ",
                              client.Avatar.Token, obstacle.X, obstacle.Y);
        }

        private static void ObstacleClearingFinished(object sender, ClearingFinishedEventArgs e)
        {
            // Remove this event handler from the buildable because it might re-register the same handler twice.
            e.ClearedObstacle.ClearingFinished -= ObstacleClearingFinished;

            var token = e.UserToken as VillageObjectUserToken;
            var client = token.Client;
            var server = token.Server;

            Console.Write("\t[Cl] -> BeingClearing() for account {0} \n\t\tat {1},{2} - ",
                          client.Avatar.Token, e.ClearedObstacle.X, e.ClearedObstacle.Y);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            if (!e.WasCancelled)
            {
                Console.WriteLine("Finished!");
                client.Avatar.Home.Obstacles.Remove(e.ClearedObstacle);
            }
            else
                Console.WriteLine("Cancelled!");
            Console.ResetColor();

            //TODO: Schedule saves instead.
            server.AvatarManager.SaveAvatar(client.Avatar);
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
            var villageObject = client.Avatar.Home.GetVillageObject(ccCommand.VillageObjectID);
            if (villageObject is Buildable)
            {
                var buildable = villageObject as Buildable;
                buildable.CancelConstruction();
            }
            else if (villageObject is Obstacle)
            {
                var obstacle = villageObject as Obstacle;
                obstacle.CancelClearing();
            }

            Console.WriteLine("\t[Co] -> CancelConstruction() for account {0} \n\t\tat {1},{2}",
                              client.Avatar.Token, villageObject.X, villageObject.Y);
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
                Console.WriteLine("Exception occurred while handling embedded command: {0}\r\n\t{1}", brCommand.GetType().Name, ex);
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
            server.RegisterCommandHandler(new ClearObstacleCommand(), HandleClearObstacleCommand);

            server.RegisterCommandHandler(new SpeedUpConstructionCommand(), HandleSpeedUpConstructionCommand);
            server.RegisterCommandHandler(new CancelConsturctionCommand(), HandleCancelConsturctionCommand);
            server.RegisterCommandHandler(new BuyResourcesCommand(), HandleBuyResourcesCommand);

            server.RegisterCommandHandler(new MoveVillageObjectCommand(), HandleMoveVillageObjectCommand);
            server.RegisterCommandHandler(new MoveMultipleVillageObjectCommand(), HandleMoveMultipleVillageObjectCommand);
        }
    }
}
