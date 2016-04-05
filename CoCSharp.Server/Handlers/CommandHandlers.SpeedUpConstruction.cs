using CoCSharp.Logic;
using CoCSharp.Networking;
using CoCSharp.Networking.Messages.Commands;

namespace CoCSharp.Server.Handlers
{
    public static partial class CommandHandlers
    {
        private static void HandleSpeedUpConstructionCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            // Fabulous variable name.
            var sucCommand = (SpeedUpConstructionCommand)command;
            var buildable = client.Avatar.Home.GetVillageObject<Buildable>(sucCommand.VillageObjectID);
            buildable.SpeedUpConstruction();
        }
    }
}
