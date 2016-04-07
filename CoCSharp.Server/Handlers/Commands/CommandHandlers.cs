using CoCSharp.Data;
using CoCSharp.Logic;
using CoCSharp.Networking.Messages.Commands;
using CoCSharp.Server.Core;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        private const string StartedConstructionFormat =
            "[&(darkgreen)Logic&(default)] Construction -> Started for account &(darkcyan){0}&(default) \n\t\tat {1},{2} with lvl {3}";
        private const string FinishedConstructionFormat =
            "[&(darkgreen)Logic&(default)] Construction -> &(green)Finished&(default) for account &(darkcyan){0}&(default) \n\t\tat {1},{2} with lvl {3}";
        private const string CancelledConstructionFormat =
            "[&(darkgreen)Logic&(default)] Construction -> &(darkyellow)Cancelled&(default) for account &(darkcyan){0}&(default) \n\t\tat {1},{2} with lvl {3}";
        private const string BuildableAlreadyInConstructionFormat =
            "[&(darkgreen)Logic&(default)] Construction -> &(red)Error:&(default) for account &(darkcyan){0}&(default) \n\t\talready in construction with ID {1}";
        private const string BuildableNotInConstructionFormat =
            "[&(darkgreen)Logic&(default)] Construction -> &(red)Error:&(default) for account &(darkcyan){0}&(default) \n\t\tnot in construction with ID {1}";

        private const string MoveVillageObjectFormat =
            "[&(darkgreen)Logic&(default)] Placement -> Moved &(darkcyan){0}&(default) for account &(darkcyan){1}&(default) \n\t\tat {2},{3}";

        private const string StartedClearObstacleFormat =
           "[&(darkgreen)Logic&(default)] Clearing -> Started for account &(darkcyan){0}&(default) \n\t\tat {1},{2}";
        private const string FinishedClearObstacleFormat =
           "[&(darkgreen)Logic&(default)] Clearing -> &(green)Finished&(default) for account &(darkcyan){0}&(default) \n\t\tat {1},{2}";
        private const string CancelledClearObstacleFormat =
           "[&(darkgreen)Logic&(default)] Clearing -> &(darkyellow)Cancelled&(default) for account &(darkcyan){0}&(default) \n\t\tat {1},{2}";

        private const string RearmedTrapFormat =
            "[&(darkgreen)Logic&(default)] Rearmed -> &(darkcyan){0}&(default) for account &(darkcyan){1}&(default) \n\t\tat {2},{3}";

        private const string BoughtDecorationFormat =
            "[&(darkgreen)Logic&(default)] Decoration -> Bought &(darkcyan){0}&(default) for account &(darkcyan){1}&(default) \n\t\tat {2},{3}";
        private const string SoldDecorationFormat =
            "[&(darkgreen)Logic&(default)] Decoration -> Sold &(darkcyan){0}&(default) for account &(darkcyan){1}&(default) \n\t\tat {2},{3}";

        //TODO: Send an OutOfSyncMessage when things seems wrong.
        //TODO: Consume resources and all that fancy stuff.

        public static void RegisterCommandHandlers(CoCServer server)
        {
            server.RegisterCommandHandler(new BuyBuildingCommand(), HandleBuyBuildingCommand);
            server.RegisterCommandHandler(new BuyTrapCommand(), HandleBuyTrapCommand);
            server.RegisterCommandHandler(new BuyDecorationCommand(), HandleBuyDecorationCommand);
            server.RegisterCommandHandler(new BuyResourcesCommand(), HandleBuyResourcesCommand);

            server.RegisterCommandHandler(new SellDecorationCommand(), HandleSellDecorationCommand);

            server.RegisterCommandHandler(new UpgradeBuildableCommand(), HandleUpgradeBuildableCommand);
            server.RegisterCommandHandler(new UpgradeMultipleBuildableCommand(), HandleUpgradeMultipleBuildableCommand);
            server.RegisterCommandHandler(new CancelConsturctionCommand(), HandleCancelConstructionCommand);
            server.RegisterCommandHandler(new SpeedUpConstructionCommand(), HandleSpeedUpConstructionCommand);

            server.RegisterCommandHandler(new MoveVillageObjectCommand(), HandleMoveVillageObjectCommand);
            server.RegisterCommandHandler(new MoveMultipleVillageObjectCommand(), HandleMoveMultipleVillageObjectCommand);

            server.RegisterCommandHandler(new ClearObstacleCommand(), HandleClearObstacleCommand);

            server.RegisterCommandHandler(new RearmTrapCommand(), HandleRearmTrapCommand);
        }

        private static void ConstructionFinished(object sender, ConstructionFinishEventArgs e)
        {
            e.BuildableConstructed.ConstructionFinished -= ConstructionFinished;
            var token = e.UserToken as VillageObjectUserToken;
            var buildable = e.BuildableConstructed;
            var client = token.Client;
            var server = token.Server;

            var exp = 0;
            if (buildable is Building)
            {
                var data = (BuildingData)buildable.Data;
                exp = LogicUtils.CalculateExperience(data.BuildTime);
            }
            else if (buildable is Trap)
            {
                var data = (TrapData)buildable.Data;
                exp = LogicUtils.CalculateExperience(data.BuildTime);
            }

            client.Avatar.Experience += exp;

            if (e.WasCancelled)
                FancyConsole.WriteLine(CancelledConstructionFormat, client.Avatar.Token, buildable.X, buildable.Y, buildable.Level);
            else
                FancyConsole.WriteLine(FinishedConstructionFormat, client.Avatar.Token, buildable.X, buildable.Y, buildable.Level);

            server.AvatarManager.SaveAvatar(client.Avatar);
        }

        private static void ClearingFinished(object sender, ClearingFinishedEventArgs e)
        {
            e.ClearedObstacle.ClearingFinished -= ClearingFinished;

            var token = e.UserToken as VillageObjectUserToken;
            var obstacle = e.ClearedObstacle;
            var client = token.Client;
            var server = token.Server;

            var data = (ObstacleData)e.ClearedObstacle.Data;
            var exp = LogicUtils.CalculateExperience(data.ClearTime);

            client.Avatar.Experience += exp;
            if (e.WasCancelled)
                FancyConsole.WriteLine(CancelledClearObstacleFormat, client.Avatar.Token, obstacle.X, obstacle.Y);
            else
                FancyConsole.WriteLine(FinishedClearObstacleFormat, client.Avatar.Token, obstacle.X, obstacle.Y);

            client.Avatar.Home.Obstacles.Remove(obstacle);
            server.AvatarManager.SaveAvatar(client.Avatar);
        }
    }
}