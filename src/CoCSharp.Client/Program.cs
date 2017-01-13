using System;
using System.Net;

namespace CoCSharp.Client
{
    public static class Program
    {
        public static Client[] Clients { get; private set; }

        public static void Main(string[] args)
        {
            var endPoint = new IPEndPoint(IPAddress.Loopback, 9339);

            var client = new Client();
            client.Connect(endPoint);
            client.Login(0, null);

            //const int NUM_CLIENT = 50;
            //Clients = new Client[NUM_CLIENT];
            //for (int i = 0; i < NUM_CLIENT; i++)
            //{
            //    Clients[i] = new Client();
            //    Clients[i].Connect(endPoint);
            //}

            //for (int i = 0; i < NUM_CLIENT; i++)
            //{
            //    Clients[i].Login(0, null);
            //}

            //while (true)
            //{
            //    for (int i = 0; i < NUM_CLIENT; i++)
            //        Clients[i].Tick();

            //    Thread.Sleep(16);
            //}
            Console.ReadLine();
        }
    }
}
