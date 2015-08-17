using System;
using System.Net;

namespace CoCSharp.Client
{
    public class Program
    {
        public static CoCClient Client { get; set; }

        public static void Main(string[] args)
        {
#if DEBUG
            args = new string[]
            {
                "52.24.108.178",
                "9339"
            };
#endif
            Client = new CoCClient();
            Client.ChatMessage += OnChatMessage;
            var ipAddresses = Dns.GetHostAddresses(args[0]);
            var port = int.Parse(args[1]);
            var endPoint = new IPEndPoint(ipAddresses[0], port);
            Console.WriteLine("Connecting to {0}...", endPoint.Address);
            Client.Connect(endPoint);

            while (true)
            {
                var message = Console.ReadLine();
                Client.SendChatMessage(message);
            }
        }

        private static void OnChatMessage(object sender, Events.ChatMessageEventArgs e)
        {
            var message = e.ClanName == null ?
                          string.Format("<{0}>: {1}", e.Username, e.Message) :
                          string.Format("<[{0}]{1}>: {2}", e.ClanName, e.Username, e.Message);
            Console.WriteLine(message);
        }
    }
}
