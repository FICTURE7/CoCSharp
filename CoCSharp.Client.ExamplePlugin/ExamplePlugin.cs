using CoCSharp.Client.API;
using CoCSharp.Client.API.Events;
using System.Collections.Generic;

namespace CoCSharp.Client.ExamplePlugin
{
    public class ExamplePlugin : Plugin
    {
        public ExamplePlugin()
        {
            PlayerHId = new List<string>();
        }

        public override string Name { get { return "Example Plugin"; } }
        public override string Description { get { return "A plugin that sends Hi to everyone :]"; } }
        public override string Author { get { return "FICTURE7"; } }

        public List<string> PlayerHId { get; set; }

        public override void OnLoad()
        {
            Client.ChatMessage += OnChatMessage;
        }

        private void OnChatMessage(object sender, ChatMessageEventArgs e)
        {
            if (!PlayerHId.Contains(e.Username) || e.Username != Client.Avatar.Username)
            {
                Client.SendChatMessage(string.Format("Hi, {0}", e.Username));
                PlayerHId.Add(e.Username);
            }
        }
    }
}
