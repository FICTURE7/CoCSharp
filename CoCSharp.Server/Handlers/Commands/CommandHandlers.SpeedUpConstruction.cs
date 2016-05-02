using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;
using CoCSharp.Server.Core;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        private static void HandleSpeedUpConstructionCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            // Fabulous variable name.
            var sucCommand = (SpeedUpConstructionCommand)command;
            var buildable = client.Avatar.Home.GetVillageObject<Buildable>(sucCommand.BuildableGameID);
            if (!buildable.IsConstructing)
            {
                FancyConsole.WriteLine(BuildableNotInConstructionFormat, client.Avatar.Token, sucCommand.BuildableGameID);
                return;
            }
            buildable.SpeedUpConstruction();
        }
    }
}
