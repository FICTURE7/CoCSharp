using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Network.Messages;
using CoCSharp.Server.Core;
using System;
using System.Diagnostics;
using System.Threading;

namespace CoCSharp.Server
{
    public class Program
    {
        public static CoCServer Server { get; set; }

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
            FancyConsole.Enabled = false;
            Console.Title = "CoC# - Server";

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("starting server...");

            Server = new CoCServer();
            Server.Start();

            stopwatch.Stop();

            Console.WriteLine("done({0}ms): listening on *:9339", stopwatch.Elapsed.TotalMilliseconds);
            Console.WriteLine();

            //new Thread(() =>
            //{
            //    var count = 0;
            //    while (true)
            //    {
            //        var ava = new AvatarClient();
            //        ava.Server = Server;
            //        ava.SessionKey = new byte[24];
            //        var loginReq = new LoginRequestMessage();

            //        loginReq.UserToken = "asddf";
            //        loginReq.UserID = count;
            //        Server.HandleMessage(ava, loginReq);
            //        count++;
            //    }
            //}).Start();

            //new Thread(() =>
            //{
            //    var count = 0;
            //    while (true)
            //    {
            //        var ava = new AvatarClient();
            //        ava.Server = Server;
            //        ava.SessionKey = new byte[24];
            //        var loginReq = new LoginRequestMessage();

            //        loginReq.UserToken = "asdf";
            //        loginReq.UserID = count;
            //        Server.HandleMessage(ava, loginReq);
            //        count++;
            //    }
            //}).Start();


            while (true)
            {
                try
                {
                    AvatarManager.Flush();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("EXCEPTION: flushing avatars: {0}", ex.Message);
                    Console.WriteLine();
                }

                try
                {
                    AllianceManager.Flush();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("EXCEPTION: flushing alliances: {0}", ex.Message);
                    Console.WriteLine();
                }

                Thread.Sleep(100);
            }
        }
    }
}
