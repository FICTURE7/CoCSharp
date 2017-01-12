using CoCSharp.Server.Api;
using CoCSharp.Server.Api.Chatting;
using System;
using System.Text;

namespace CoCSharp.Server.Chatting.Commands
{
    public class HelpChatCommand : ChatCommand
    {
        private static readonly string[] s_alias = new string[] { "h" };

        public override string Name => "help";
        public override string[] Alias => s_alias;
        public override string Description => "Displays this help message";

        public override void Execute(IClient client, params string[] args)
        {
            var helpMessage = new StringBuilder();
            helpMessage.AppendLine("List of available commands: ");

            foreach (var t in Manager.GetCommandTypes())
            {
                var instance = (ChatCommand)Activator.CreateInstance(t);
                helpMessage.AppendFormat("/{0}", instance.Name);

                var alias = instance.Alias;
                if (alias != null && alias.Length > 0)
                {
                    helpMessage.Append(",");
                    for (int i = 0; i < instance.Alias.Length; i++)
                    {
                        var format = i == instance.Alias.Length - 1 ? "/{0}" : "/{0}";
                        helpMessage.AppendFormat(format, instance.Alias[i]);
                    }
                    helpMessage.AppendLine().AppendLine(" - " + instance.Description);
                }
                else
                {
                    helpMessage.AppendLine(" - " + instance.Description);
                }

                helpMessage.AppendLine();
            }

            Manager.SendChatMessage(client, helpMessage.ToString());
        }
    }
}
