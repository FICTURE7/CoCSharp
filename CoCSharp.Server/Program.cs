using CoCSharp.Csv;
using CoCSharp.Data;
using CoCSharp.Server.Core;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CoCSharp.Server
{
    public class Program
    {
        public static CoCServer Server { get; set; }

        public static void m(string[] args)
        {
            Console.Title = "CoC# - Server";
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Server = new CoCServer();
            Server.Start();

            stopwatch.Stop();

            Console.WriteLine("Done({0}ms)! Listening on *:9339", stopwatch.Elapsed.TotalMilliseconds);
            Thread.Sleep(Timeout.Infinite);
        }

        public static void m()
        {
            var stopwatch = new Stopwatch();
            var avatarManager = new AvatarManager();

            stopwatch.Start();
            for (int i = 0; i < 100; i++)
            {
                var avatar = avatarManager.CreateNewAvatar();
                Console.WriteLine("Created new avatar with id {0} and token {1}", avatar.ID, avatar.Token);
            }
            stopwatch.Stop();
            Console.WriteLine("Done in {0}ms", stopwatch.Elapsed.TotalMilliseconds);

            Console.WriteLine("Saving avatars");
            stopwatch.Restart();
            avatarManager.SaveAllAvatars();
            stopwatch.Stop();

            Console.WriteLine("Done in {0}ms", stopwatch.Elapsed.TotalMilliseconds);
            Console.ReadLine();
        }

        public static void mm()
        {
            var stopwatch = new Stopwatch();
            var name = string.Empty;

            stopwatch.Start();
            var saveFile = new SaveFile(@"C:\cygwin\home\Ramdass\GitHub\CoCSharp\CoCSharp.Server\bin\Debug\Avatars\7hs9x359qwq78hf45enn8zruw6sdnk8t4tcp7zdm\Patrik.txt");
            name = saveFile.GetKey("Name");
            name = saveFile.GetKey("Token");
            stopwatch.Stop();

            Console.WriteLine("Done in {0}ms", stopwatch.Elapsed.TotalMilliseconds);
            Console.ReadLine();
        }


        public static void Main()
        {
            var stopwatch = new Stopwatch();
            var name = string.Empty;

            stopwatch.Start();

            var table = new CsvTable(Path.Combine(CoCServerPaths.Content, "buildings.csv"));
            var buildings = CsvConvert.Deserialize<BuildingData>(table);

            stopwatch.Stop();

            Console.WriteLine("Done in {0}ms", stopwatch.Elapsed.TotalMilliseconds);
            Console.ReadLine();
        }
    }
}
