using CoCSharp.Client.API.Events;
using System;
using System.Net;

namespace CoCSharp.Client
{
    public class Program
    {
        public static CoCClient Client { get; set; }
        public static ClientConfiguration Configuration { get; set; }

        public static void Main(string[] args)
        {
#if DEBUG
            args = new string[]
            {
                "gamea.clashofclans.com",
                "9339"
            };
#endif
            var port = -1;
            var address = (IPAddress)null;
            if (!TryGetPort(args, out port))
                return;
            if (!TryGetIPAddress(args, out address))
                return;

            Configuration = ClientConfiguration.LoadConfiguration("clientConfig.xml");
            Client = new CoCClient();
            Client.ChatMessage += OnChatMessage;
            Client.Avatar.ID = Configuration.UserID;
            Client.Avatar.Token = Configuration.UserToken;

            Console.WriteLine("Connecting to {0}:{1}...", address, port);
            Client.Connect(new IPEndPoint(address, port));

            while (true)
            {
                var message = Console.ReadLine();
                Client.SendChatMessage(message); //TODO: Handle stuff interms of commands.
            }
        }

        private static void OnChatMessage(object sender, ChatMessageEventArgs e)
        {
            //TODO: Give flexibilty for plugin dev to remove this.
            if (e.ClanName == null) Console.WriteLine("<[Lvl:{3}]{1}>: {2}", e.Username, e.Message, e.Packet.Level);
            else
            {
                Console.Write("<[");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write(e.ClanName);
                Console.ResetColor();
                Console.Write("][Lvl:{0}]", e.Packet.Level);
                Console.WriteLine("{0}>: {1}", e.Username, e.Message);
            }
        }

        private static bool TryGetPort(string[] args, out int port)
        {
            var lport = -1;
            if (int.TryParse(args[1], out lport))
            {
                port = lport;
                return true;
            }
            Console.WriteLine("Error: Invalid port number '{0}'", args[1]);
            port = lport;
            return false;
        }

        private static bool TryGetIPAddress(string[] args, out IPAddress address)
        {
            var laddress = (IPAddress)null;
            if (IPAddress.TryParse(args[0], out laddress))
            {
                address = laddress;
                return true;
            }
            Console.WriteLine("Trying to resolve Dns...");
            if (TryResolve(args[0], out laddress))
            {
                address = laddress;
                return true;
            }
            Console.WriteLine("Error: Failed to resolve Dns...");
            address = laddress;
            return false;
        }

        private static bool TryResolve(string hostAddr, out IPAddress address)
        {
            try
            {
                address = Dns.GetHostAddresses(hostAddr)[0];
                return true;
            }
            catch
            {
                address = null;
                return false;
            }
        }
    }
}
