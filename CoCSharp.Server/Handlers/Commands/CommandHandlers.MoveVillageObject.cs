using CoCSharp.Networking;
using CoCSharp.Networking.Messages.Commands;
using CoCSharp.Server.Core;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        private static void HandleMoveVillageObjectCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var mvCommand = (MoveVillageObjectCommand)command;
            var gameId = mvCommand.MoveData.VillageObjectGameID;
            var villageObject = client.Avatar.Home.GetVillageObject(gameId);
            villageObject.X = mvCommand.MoveData.X;
            villageObject.Y = mvCommand.MoveData.Y;

            FancyConsole.WriteLine(MoveVillageObjectFormat, gameId, client.Avatar.Token, villageObject.X, villageObject.Y);
        }
    }
}
