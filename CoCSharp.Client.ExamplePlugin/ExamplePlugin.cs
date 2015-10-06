using CoCSharp.Client.API;
using CoCSharp.Client.API.Events;
using System;
using System.Collections.Generic;

namespace CoCSharp.Client.ExamplePlugin
{
    public class ExamplePlugin : Plugin
    {
        public ExamplePlugin()
        {
            PlayerHId = new List<string>();
            MessagePrefixes = new List<string>();
            MessagePrefixes.Add("Hi");
            MessagePrefixes.Add("Hello");
            MessagePrefixes.Add("How you doing?");
        }

        public override string Name { get { return "Example Plugin"; } }
        public override string Description { get { return "A plugin that sends Hi to everyone :]"; } }
        public override string Author { get { return "FICTURE7"; } }

        public string MessagePrefix
        {
            get { return MessagePrefixes[m_Random.Next(MessagePrefixes.Count - 1)]; }
        }
        public List<string> MessagePrefixes { get; set; }
        public List<string> PlayerHId { get; set; }

        private Random m_Random = new Random();

        public override void OnLoad()
        {
            Client.ChatMessage += OnChatMessage;
        }

        private void OnChatMessage(object sender, ChatMessageEventArgs e)
        {
            if (!PlayerHId.Contains(e.Username) && e.Username != Client.Avatar.Username)
            {
                Client.SendChatMessage(string.Format("{0}, {1}.", MessagePrefix, e.Username));
                PlayerHId.Add(e.Username);
            }
        }
    }
}
