using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Logic;
using CoCSharp.Server.Core;
using System;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        //TODO: Send an OutOfSyncMessage when things seems wrong.
        //TODO: Consume resources and all that fancy stuff.

        public static void RegisterCommandHandlers(CoCServer server)
        {
            //server.RegisterCommandHandler(new BuyBuildingCommand(), HandleBuyBuildingCommand);
            //server.RegisterCommandHandler(new BuyTrapCommand(), HandleBuyTrapCommand);
            //server.RegisterCommandHandler(new BuyDecorationCommand(), HandleBuyDecorationCommand);
            //server.RegisterCommandHandler(new BuyResourcesCommand(), HandleBuyResourcesCommand);

            //server.RegisterCommandHandler(new SellDecorationCommand(), HandleSellDecorationCommand);

            //server.RegisterCommandHandler(new UnlockBuildingCommand(), HandleUnlockBuildingCommand);
            //server.RegisterCommandHandler(new UpgradeBuildableCommand(), HandleUpgradeBuildableCommand);
            //server.RegisterCommandHandler(new UpgradeMultipleBuildableCommand(), HandleUpgradeMultipleBuildableCommand);
            //server.RegisterCommandHandler(new CancelConsturctionCommand(), HandleCancelConstructionCommand);
            //server.RegisterCommandHandler(new SpeedUpConstructionCommand(), HandleSpeedUpConstructionCommand);

            //server.RegisterCommandHandler(new MoveVillageObjectCommand(), HandleMoveVillageObjectCommand);
            //server.RegisterCommandHandler(new MoveMultipleVillageObjectCommand(), HandleMoveMultipleVillageObjectCommand);

            //server.RegisterCommandHandler(new ClearObstacleCommand(), HandleClearObstacleCommand);

            //server.RegisterCommandHandler(new RearmTrapCommand(), HandleRearmTrapCommand);

            //server.RegisterCommandHandler(new MatchmakingCommand(), HandleMatchmakingCommand);
        }

        // Doing this because a lot of stuff made incorrectly.
        private static int GetResourceID(string name)
        {
            switch (name)
            {
                case "Gold":
                    return AssetManager.DefaultInstance.SearchCsv<ResourceData>("TID_GOLD").ID;

                case "Elixir":
                    return AssetManager.DefaultInstance.SearchCsv<ResourceData>("TID_ELIXIR").ID;

                case "DarkElixir":
                    return AssetManager.DefaultInstance.SearchCsv<ResourceData>("TID_DARK_ELIXIR").ID;

                case "Diamonds":
                    return AssetManager.DefaultInstance.SearchCsv<ResourceData>("TID_DIAMONDS").ID;

                default:
                    return -1;
            }
        }

        public static void BuildingConstructionFinished(object sender, ConstructionFinishedEventArgs<BuildingData> e)
        {
#if !DEBUG
            try
            {
#endif
            var building = (Building)e.BuildableConstructed;
            var client = (AvatarClient)e.UserToken;

            if (e.WasCancelled)
            {
                FancyConsole.WriteLine(LogFormats.Logic_Construction_Cancelled, client.Token, building.X, building.Y, building.Data.Level);
            }
            else
            {
                FancyConsole.WriteLine(LogFormats.Logic_Construction_Finished, client.Token, building.X, building.Y, building.Data.Level);
                var exp = LogicUtils.CalculateExperience(building.Data.BuildTime);
                client.Experience += exp;
            }

            client.Save();
#if !DEBUG
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred while handling Building construction finished; {0} -> {1}", ex.GetType().Name, ex.Message);
            }
#endif
        }

        public static void TrapConstructionFinished(object sende, ConstructionFinishedEventArgs<TrapData> e)
        {
#if !DEBUG
            try
            {
#endif
            var trap = (Trap)e.BuildableConstructed;
            var client = (AvatarClient)e.UserToken;

            if (e.WasCancelled)
            {
                FancyConsole.WriteLine(LogFormats.Logic_Construction_Cancelled, client.Token, trap.X, trap.Y, trap.Data.Level);
            }
            else
            {
                FancyConsole.WriteLine(LogFormats.Logic_Construction_Finished, client.Token, trap.X, trap.Y, trap.Data.Level);

                var exp = LogicUtils.CalculateExperience(trap.Data.BuildTime);
                client.Experience += exp;
            }

            client.Save();
#if !DEBUG
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred while handling Trap construction finished; {0} -> {1}", ex.GetType().Name, ex.Message);
            }
#endif
        }

        public static void ObstacleClearingFinished(object sender, ClearingFinishedEventArgs e)
        {
#if !DEBUG
            try
            {
#endif
            var obstacle = e.ClearedObstacle;
            var client = (AvatarClient)e.UserToken;

            if (e.WasCancelled)
            {
                FancyConsole.WriteLine(LogFormats.Logic_Clearing_Cancelled, client.Token, obstacle.X, obstacle.Y);
            }
            else
            {
                FancyConsole.WriteLine(LogFormats.Logic_Clearing_Finished, client.Token, obstacle.X, obstacle.Y);
                var exp = LogicUtils.CalculateExperience(obstacle.Data.ClearTime);
                client.Experience += exp;
            }

            client.Save();
#if !DEBUG
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred while handling Obstacle clearing finished; {0} -> {1}", ex.GetType().Name, ex.Message);
            }
#endif
        }
    }
}