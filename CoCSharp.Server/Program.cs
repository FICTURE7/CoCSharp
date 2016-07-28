using System;
using System.Diagnostics;
using System.Threading;
using CoCSharp.Logic;

namespace CoCSharp.Server
{
    public class Program
    {
        public static CoCServer Server { get; set; }

        public static void Main(string[] args)
        {
            Console.Title = "CoC# - Server";

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("Starting server...");

            Server = new CoCServer();
            Server.Start();

            stopwatch.Stop();

            Console.WriteLine("Done({0}ms)! Listening on *:9339", stopwatch.Elapsed.TotalMilliseconds);

            var avatarManager = Server.AvatarManager;
            while (true)
            {
                if (!avatarManager.QueuedAvatars.IsEmpty)
                {
                    var avatar = (Avatar)null;
                    if (avatarManager.QueuedAvatars.TryPop(out avatar))
                    {
                        avatarManager.SaveAvatar(avatar);
                        Debug.WriteLine("Saved avatar " + avatar.Token, "Saving");
                    }
                }

                Thread.Sleep(100);
            }
        }
    }
}
