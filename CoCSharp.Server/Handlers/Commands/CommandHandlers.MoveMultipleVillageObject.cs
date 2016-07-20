using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;
using CoCSharp.Server.Core;
using System.Diagnostics;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        private static void HandleMoveMultipleVillageObjectCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var mmvCommand = (MoveMultipleVillageObjectCommand)command;
            for (int i = 0; i < mmvCommand.MovesData.Length; i++)
            {
                var moveData = mmvCommand.MovesData[i];
                var gameId = moveData.VillageObjectGameID;
                var villageObject = client.Avatar.Home.GetVillageObject(gameId);

                Debug.Assert(villageObject.ID == gameId);

                villageObject.X = moveData.X;
                villageObject.Y = moveData.Y;

                FancyConsole.WriteLine(MoveVillageObjectFormat, gameId, client.Avatar.Token, villageObject.X, villageObject.Y);
            }
        }
    }
}
