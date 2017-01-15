using CoCSharp.Server.Api;
using CoCSharp.Server.Api.Chatting;
using System.IO;

namespace CoCSharp.Server.Chatting.Commands
{
    public class ChangeLogChatCommand : ChatCommand
    {
        public override string Name => "changelog";
        public override string Description => "Displays the change log.";

        private static string s_changeLog = "Change log: \n" + File.ReadAllText("contents/changelog");

        public override void Execute(IClient client, params string[] args)
        {
            Manager.SendChatMessage(client, s_changeLog);
        }
    }
}
