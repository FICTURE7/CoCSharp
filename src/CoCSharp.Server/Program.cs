using CoCSharp.Network;
using CoCSharp.Network.Messages;
using CoCSharp.Server.Core;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CoCSharp.Server
{
    public class Program
    {
        public static Server Server { get; set; }

        public static AvatarManager AvatarManager
        {
            get
            {
                if (Server == null)
                    return null;

                return Server.AvatarManager;
            }
        }

        public static AllianceManager AllianceManager
        {
            get
            {
                if (Server == null)
                    return null;

                return Server.AllianceManager;
            }
        }

        public static void Main(string[] args)
        {
            Console.Title = "CoC# - Server";

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(
@"
_________        _________   _________.__                         
\_   ___ \  ____ \_   ___ \ /   _____/|  |__ _____ _____________  
/    \  \/ /  _ \/    \  \/ \_____  \ |  |  \\__  \\_  __ \____ \ 
\     \___(  <_> )     \____/        \|   Y  \/ __ \|  | \/  |_> >
 \______  /\____/ \______  /_______  /|___|  (____  /__|  |   __/ 
        \/               \/        \/      \/     \/      |__|    
");
            Console.ResetColor();
            Console.WriteLine("Starting CoC# - Server Emulator for Clash of Clans...");
            Console.WriteLine("Visit https://github.com/FICTURE7/CoCSharp/tree/server-dev for the latest news.\n");

            Server = new Server();
            Server.Start();

            stopwatch.Stop();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nServer loaded successfully in {0} ms. Listening on port 9339.", stopwatch.Elapsed.TotalMilliseconds);
            Console.ResetColor();
            Console.WriteLine();
            while (true)
            {
                try
                {
                    AvatarManager.Flush();
                }
                catch (Exception ex)
                {
                    Log.Exception("Flushing avatars", ex);
                }

                try
                {
                    AllianceManager.Flush();
                }
                catch (Exception ex)
                {
                    Log.Exception("Flushing alliances", ex);
                }

                Thread.Sleep(100);
            }
        }
    }
}
