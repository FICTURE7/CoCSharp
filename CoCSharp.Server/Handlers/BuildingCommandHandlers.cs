using CoCSharp.Networking;
using CoCSharp.Networking.Messages.Commands;
using CoCSharp.Logic;
using System;

namespace CoCSharp.Server.Handlers
{
    public delegate void CommandHandler(CoCServer server, CoCRemoteClient client, Command command);

    public static class BuildingCommandHandlers
    {
        private static CoCRemoteClient s_client;
        private static CoCServer s_server;

        private static void HandleBuyBuildingCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            s_client = client; //TODO: Find a more consitent way of doing stuff. Cause this is not multiclient
            s_server = server;

            var bbCmd = command as BuyBuildingCommand;
            var dataIndex = IDConverter.ToIndex(bbCmd.BuildingDataID);
            var dataID = bbCmd.BuildingDataID;
            Console.WriteLine("Buying new building {0} at {1}, {2}", dataID, bbCmd.X, bbCmd.Y);

            var building = new Building(dataID);
            building.Data = server.DataManager.FindBuilding(dataIndex, 0);

            building.X = bbCmd.X;
            building.Y = bbCmd.Y;
            building.BeginConstruct();
            building.ConstructionFinished += OnConstructed;

            client.Avatar.Home.Buildings.Add(building);
        }

        private static void HandleUpgradeBuildingCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            // Space
        }

        public static void RegisterBuildingCommandHandlers(CoCServer server)
        {
            server.RegisterCommandHandler(new BuyBuildingCommand(), HandleBuyBuildingCommand);
            server.RegisterCommandHandler(new UpgradeBuildingCommand(), HandleUpgradeBuildingCommand);
        }

        private static void OnConstructed(object sender, ConstructionFinishEventArgs e)
        {
            s_server.AvatarManager.SaveAvatar(s_client.Avatar);
        }
    }
}
