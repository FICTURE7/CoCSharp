using CoCSharp.Data.Models;
using CoCSharp.Logic;
using CoCSharp.Network.Messages.Commands;
using CoCSharp.Server.Core;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        #region Constants
        //TODO: Implement all those strings in a class or in a resource file.
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
        #endregion

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

        public static void BuildingConstructionFinished(object sender, ConstructionFinishedEventArgs<BuildingData> e)
        {
            var building = (Building)e.BuildableConstructed;
            var client = (CoCRemoteClient)e.UserToken;

            // Can happen when the buildings were scheduled automatically on load.
            if (client == null)
            {
                return;
            }

            if (e.WasCancelled)
            {

                FancyConsole.WriteLine(CancelledConstructionFormat, client.Avatar.Token, building.X, building.Y, building.Data.Level);
            }
            else
            {
                FancyConsole.WriteLine(FinishedConstructionFormat, client.Avatar.Token, building.X, building.Y, building.Data.Level);

                var exp = LogicUtils.CalculateExperience(building.Data.BuildTime);
                client.Avatar.Experience += exp;
            }

            client.Server.AvatarManager.SaveAvatar(client.Avatar);
        }

        public static void TrapConstructionFinished(object sende, ConstructionFinishedEventArgs<TrapData> e)
        {
            var trap = (Trap)e.BuildableConstructed;
            var client = (CoCRemoteClient)e.UserToken;

            if (e.WasCancelled)
            {
                FancyConsole.WriteLine(CancelledConstructionFormat, client.Avatar.Token, trap.X, trap.Y, trap.Data.Level);
            }
            else
            {
                FancyConsole.WriteLine(FinishedConstructionFormat, client.Avatar.Token, trap.X, trap.Y, trap.Data.Level);

                var exp = LogicUtils.CalculateExperience(trap.Data.BuildTime);
                client.Avatar.Experience += exp;
            }

            client.Server.AvatarManager.SaveAvatar(client.Avatar);
        }

        public static void ObstacleClearingFinished(object sender, ClearingFinishedEventArgs e)
        {
            var obstacle = e.ClearedObstacle;
            var client = (CoCRemoteClient)e.UserToken;

            if (e.WasCancelled)
            {
                FancyConsole.WriteLine(CancelledClearObstacleFormat, client.Avatar.Token, obstacle.X, obstacle.Y);
            }
            else
            {
                FancyConsole.WriteLine(FinishedClearObstacleFormat, client.Avatar.Token, obstacle.X, obstacle.Y);
                var exp = LogicUtils.CalculateExperience(obstacle.Data.ClearTime);
                client.Avatar.Experience += exp;
            }

            client.Server.AvatarManager.SaveAvatar(client.Avatar);
        }
    }
}