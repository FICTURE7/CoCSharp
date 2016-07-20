using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;
using CoCSharp.Data.Models;
using CoCSharp.Server.Core;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        private static void HandleBuyTrapCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var btCommand = (BuyTrapCommand)command;
            var data = server.AssetManager.SearchCsv<TrapData>(btCommand.TrapDataID, 0);
            var trap = new Trap(client.Avatar.Home, data, btCommand.X, btCommand.Y);

            trap.UserToken = client;
            trap.ConstructionFinished += TrapConstructionFinished;
            trap.BeginConstruction();

            FancyConsole.WriteLine(StartedConstructionFormat, client.Avatar.Token, btCommand.X, btCommand.Y, trap.Data.Level);
        }
    }
}
