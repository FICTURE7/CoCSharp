using CoCSharp.Client.API;
using System;
using CoCSharp.Client.API.Events;

namespace ExamplePlugin
{
    public class ExamplePlugin : Plugin
    {
        public ExamplePlugin()
        {  
            // Space
        }

        public override string Name { get { return "Example Plugin"; } }
        public override string Description { get { return "A plugin just for sample."; } }
        public override string Author { get { return "FICTURE7"; } }

        public override void OnLoad()
        {
            Client.ChatMessage += OnChatMessage;
            Client.Login += OnLogin;
            Console.WriteLine("Plugin loaded!", Name);
        }

        private void OnLogin(object sender, LoginEventArgs e)
        {
            Console.WriteLine("Logged in!");
        }

        private void OnChatMessage(object sender, ChatMessageEventArgs e)
        {
            Console.WriteLine("Got message!\n{0}", e.Message);
        }
    }
}
