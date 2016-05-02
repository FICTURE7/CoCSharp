using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;
using CoCSharp.Server.Core;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        private static void HandleRearmTrapCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var rtCommand = (RearmTrapCommand)command;
            var trap = client.Avatar.Home.GetTrap(rtCommand.TrapGameID);

            //var data = (TrapData)null;

            //if (trap.Data == null)
            //    data = (TrapData)trap.Data;
            //else
            //    data = server.DataManager.FindTrap(trap.GetDataID(), trap.Level);

            if (trap.Broken == true)
            {
                trap.Broken = false;
                FancyConsole.WriteLine(RearmedTrapFormat, rtCommand.TrapGameID, client.Avatar.Token, trap.X, trap.Y);
            }
            //else
            //{
            //    // we're desync!
            //}
        }
    }
}
