using CoCSharp.Logic;
using CoCSharp.Networking;
using CoCSharp.Networking.Messages.Commands;
using CoCSharp.Server.Core;

namespace CoCSharp.Server.Handlers
{
    public static partial class CommandHandlers
    {
        private static void HandleUpgradeMultipleBuildableCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var umbCommand = (UpgradeMultipleBuildableCommand)command;

            for (int i = 0; i < umbCommand.BuildingsGameID.Length; i++)
            {
                var gameId = umbCommand.BuildingsGameID[i];
                var token = new VillageObjectUserToken(server, client);
                var buildable = client.Avatar.Home.GetVillageObject<Buildable>(gameId);
                var data = server.DataManager.FindBuilding(buildable.GetDataID(), buildable.Level + 1);

                buildable.UserToken = token;
                buildable.Data = data;
                buildable.BeginConstruction();

                buildable.ConstructionFinished += ConstructionFinished;

                FancyConsole.WriteLine(StartedConstructionFormat, client.Avatar.Token, buildable.X, buildable.Y, buildable.Level);
            }
        }
    }
}
