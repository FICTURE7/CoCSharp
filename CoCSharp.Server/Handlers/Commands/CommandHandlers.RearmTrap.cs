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
            var trap = client.Home.GetTrap(rtCommand.TrapGameID);

            Debug.Assert(trap.ID == rtCommand.TrapGameID);

            if (trap.Broken == true)
            {
                trap.Broken = false;
                client.ResourcesAmount.GetSlot(GetResourceID(trap.Data.BuildResource)).Amount -= trap.Data.RearmCost;
                FancyConsole.WriteLine(RearmedTrapFormat, rtCommand.TrapGameID, client.Token, trap.X, trap.Y);
            }
        }
    }
}
