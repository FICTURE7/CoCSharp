using CoCSharp.Network.Messages;
using CoCSharp.Server.Api;
using CoCSharp.Server.Api.Chatting;
using CoCSharp.Server.Chatting.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoCSharp.Server.Chatting
{
    public class ChatManager : IChatManager
    {
        public ChatManager()
        {
            _commands = new Dictionary<string, Type>();
            _commandTypes = new List<Type>();

            RegisterCommand(new HelpChatCommand());
            RegisterCommand(new ChangeLogChatCommand());
            RegisterCommand(new StatisticsChatCommand());
        }

        private List<Type> _commandTypes;
        private Dictionary<string, Type> _commands;

        public void Handle(IClient client, string message)
        {
            var text = message.Remove(0, 1); // Remove first '/' character.
            var commands = text.Split(' ');
            if (commands.Length > 0)
            {
                var commandName = commands[0];
                var commandType = (Type)null;

                if (!_commands.TryGetValue(commandName, out commandType))
                {
                    SendChatMessage(client, $"Unknown command '{commandName}'. Type '/help' for more information.");
                }
                else
                {
                    var args = commands.Take(1).ToArray(); // Remove the command name in the argument list.
                    var instance = (ChatCommand)Activator.CreateInstance(commandType);
                    instance.Manager = this;

                    instance.Execute(client, args);
                }
            }
            else
            {
                // Should never happen.
            }
        }

        public void SendChatMessage(IClient client, string message)
        {
            var level = client.Session.Level;
            var cmsMessage = new ChatMessageServerMessage
            {
                // To prevent the client from viewing profile and stuff.
                UserId = level.Avatar.Id,
                HomeId = level.Avatar.Id,

                League = 22,
                Name = "Command System",
                ExpLevels = 100,
                Message = message,
            };

            client.SendMessage(cmsMessage);
        }

        public IEnumerable<Type> GetCommandTypes()
        {
            return _commandTypes;
        }

        private void RegisterCommand(ChatCommand command)
        {
            var commandType = command.GetType();
            _commandTypes.Add(commandType);
            _commands.Add(command.Name, commandType);

            if (command.Alias != null)
            {
                for (int i = 0; i < command.Alias.Length; i++)
                    _commands.Add(command.Alias[i], command.GetType());
            }
        }
    }
}
