using System;
using System.Diagnostics;
using System.Threading;

namespace CoCSharp.Server
{
    public class Program
    {
        public static Server Server { get; set; }

        public static void Main(string[] args)
        {
            Console.WriteLine("starting server...");
            var sw = Stopwatch.StartNew();

            using (Server = new Server())
            {
                Server.Start();
                sw.Stop();

                Console.WriteLine("done! ({0})ms", sw.Elapsed.TotalMilliseconds);

                // Wasting the main thread, because we can.
                Thread.Sleep(Timeout.Infinite);
            }
        }
    }
}
