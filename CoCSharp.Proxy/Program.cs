using CoCSharp.Networking.Cryptography;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CoCSharp.Proxy
{
    public class Program
    {
        public static CoCProxy Proxy { get; set; }

        public static void Main(string[] args)
        {
            Console.Title = "CoC# - Proxy";

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Directory.CreateDirectory("messages");
            Proxy = new CoCProxy();
            Proxy.Start(new IPEndPoint(IPAddress.Any, 9339));

            stopwatch.Stop();

            Console.WriteLine("Done({0}ms)! Listening on *:9339", stopwatch.Elapsed.TotalMilliseconds);
            Thread.Sleep(Timeout.Infinite);
        }

        public static void mm()
        {
            var crypt = new Crypto8();

            var m10101 = File.ReadAllBytes("messages\\therecv1");
            var len = 327;

            var clientKey = new byte[32];
            Buffer.BlockCopy(m10101, 7, clientKey, 0, 32); // get public key from m10101

            var serverKey = crypt.KeyPair.PublicKey;
            crypt.UpdateKey(clientKey);

            File.WriteAllBytes("sk", serverKey);
            File.WriteAllBytes("ck", clientKey);

            var body = new byte[len - 7 - 32];
            Buffer.BlockCopy(m10101, 7 + 32, body, 0, body.Length);

            crypt.Decrypt(ref body);

            File.WriteAllBytes("decrypted", body);
            Console.WriteLine("Done with encryption...");
            Console.ReadLine();
        }

        private static Socket _listener;

        public static void m()
        {
            _listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(new IPEndPoint(IPAddress.Any, 9339));
            _listener.Listen(10);

            var client = _listener.Accept();
            Console.WriteLine("Accepted new client: {0}", client.RemoteEndPoint);

            var server = new Socket(SocketType.Stream, ProtocolType.Tcp);
            server.Connect("gamea.clashofclans.com", 9339);
            Console.WriteLine("Created new connection to gamea.clashofclans.com");

            client.Send(File.ReadAllBytes("themsg"));
            var count = 0;
            while (true)
            {
                var fbuf = new byte[65535];
                client.Receive(fbuf);
                File.WriteAllBytes("messages\\therecv" + count, fbuf);
                count++;
            }
        }
    }
}
