using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Logic;
using CoCSharp.Network.Messages.Commands;
using System;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        //TODO: Send an OutOfSyncMessage when things seems wrong.
        //TODO: Consume resources and all that fancy stuff.

        public static void RegisterCommandHandlers(Server server)
        {
            server.RegisterCommandHandler(new BuyBuildingCommand(), HandleBuyBuildingCommand);
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
        internal static void DoLogic(object sender, LogicEventArgs e)
        {
            if (e is ConstructionEventArgs<BuildingData>)
            {
                var args = (ConstructionEventArgs<BuildingData>)e;
                var building = (Building)args.Buildable;
                var client = (AvatarClient)args.UserToken;
                if (client == null)
                    return;

                if (e.Operation.HasFlag(LogicOperation.Cancel))
                {
                    //FancyConsole.WriteLine(LogFormats.Logic_Construction_Cancelled, client.Token, building.X, building.Y, building.Data.Level);
                }
                else
                {
                    Console.WriteLine("Hi mom!");
                    //FancyConsole.WriteLine(LogFormats.Logic_Construction_Finished, client.Token, building.X, building.Y, building.Data.Level);
                    var exp = LogicUtils.CalculateExperience(building.Data.BuildTime);
                    client.Experience += exp;
                }

                client.Save();
            }
        }
    }
}