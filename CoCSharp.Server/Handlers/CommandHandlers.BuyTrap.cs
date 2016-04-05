using System;
using CoCSharp.Logic;
using CoCSharp.Networking;
using CoCSharp.Networking.Messages.Commands;
using CoCSharp.Server.Core;

namespace CoCSharp.Server.Handlers
{
    public static partial class CommandHandlers
    {
        public static void HandleBuyTrapCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var btCommand = (BuyTrapCommand)command;
            var token = new VillageObjectUserToken(server, client);
            var data = server.DataManager.FindBuilding(btCommand.TrapDataID, 0);
            var trap = new Trap(btCommand.X, btCommand.Y, token);

            trap.ConstructionFinished += ConstructionFinished;

            trap.Data = data;
            trap.BeginConstruction();
            client.Avatar.Home.Traps.Add(trap);

            FancyConsole.WriteLine(StartedConstructionFormat, client.Avatar.Token, btCommand.X, btCommand.Y, trap.Level);
        }
    }
}
