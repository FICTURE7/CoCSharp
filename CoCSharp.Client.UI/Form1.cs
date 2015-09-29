using CoCSharp.Client.API.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoCSharp.Client.UI
{
    public partial class Form1 : Form
    {
        public static CoCClient Client { get; set; }
        public static ClientConfiguration Configuration { get; set; }

        public Form1()
        {
            InitializeComponent();


            var args = new string[]
            {
                "gamea.clashofclans.com",
                "9339"
            };

            var port = -1;
            var address = (IPAddress)null;
            if (!TryGetPort(args, out port))
                return;
            if (!TryGetIPAddress(args, out address))
                return;

            Configuration = ClientConfiguration.LoadConfiguration("clientConfig.xml");
            Client = new CoCClient();
            Client.Login += OnLogin;
            Client.ChatMessage += OnChatMessage;
            Client.Avatar.ID = Configuration.UserID;
            Client.Avatar.Token = Configuration.UserToken;

            Log(string.Format("Connecting to {0}:{1}...", address, port));
            Client.Connect(new IPEndPoint(address, port));

            //while (true)
            //{
            //    var command = Console.ReadLine();
            //    if (command[0] == '/')
            //        Console.WriteLine("TODO: Handle command.");
            //    else
            //        Client.SendChatMessage(command);
            //}

        }

        private void OnLogin(object sender, LoginEventArgs e)
        {
            Log("Successfully logged in!");
        }

        private void OnChatMessage(object sender, ChatMessageEventArgs e)
        {
            //TODO: Give flexibilty for plugin dev to remove this.
            if (e.ClanName == null) Console.WriteLine("<[Lvl:{0}]{1}>: {2}", e.Packet.Level, e.Username, e.Message);
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

        private bool TryGetPort(string[] args, out int port)
        {
            var lport = -1;
            if (int.TryParse(args[1], out lport))
            {
                port = lport;
                return true;
            }
            Log(string.Format("Error: Invalid port number '{0}'", args[1]));
            port = lport;
            return false;
        }

        private bool TryGetIPAddress(string[] args, out IPAddress address)
        {
            var laddress = (IPAddress)null;
            if (IPAddress.TryParse(args[0], out laddress))
            {
                address = laddress;
                return true;
            }
            Log("Trying to resolve Dns...");
            if (TryResolve(args[0], out laddress))
            {
                address = laddress;
                return true;
            }
            Log("Error: Failed to resolve Dns...");
            address = laddress;
            return false;
        }

        private bool TryResolve(string hostAddr, out IPAddress address)
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


        private void Log(string msg)
        {

            txtLog.Text += msg;
            txtLog.Text += Environment.NewLine;


        }

    }
}
