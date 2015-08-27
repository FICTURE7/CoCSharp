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
                "gamea.clashofclans.com",
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
            if (e.ClanName == null)
            {
                Console.WriteLine("<{0}>: {1}", e.Username, e.Message);
            }
            else
            {
                Console.Write("<[");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write(e.ClanName);
                Console.ResetColor();
                Console.Write("]");
                Console.WriteLine("{0}>: {1}", e.Username, e.Message);
            }
        }
    }
}
