using CoCSharp.Network;
using CoCSharp.Network.Messages.Commands;
using CoCSharp.Server.Core;
using System.Diagnostics;

namespace CoCSharp.Server.Handlers.Commands
{
    public static partial class CommandHandlers
    {
        private static void HandleRearmTrapCommand(CoCServer server, CoCRemoteClient client, Command command)
        {
            var rtCommand = (RearmTrapCommand)command;
            var trap = client.Avatar.Home.GetTrap(rtCommand.TrapGameID);

            Debug.Assert(trap.ID == rtCommand.TrapGameID);

            if (trap.Broken == true)
            {
                trap.Broken = false;
                FancyConsole.WriteLine(RearmedTrapFormat, rtCommand.TrapGameID, client.Avatar.Token, trap.X, trap.Y);
            }
        }
    }
}
