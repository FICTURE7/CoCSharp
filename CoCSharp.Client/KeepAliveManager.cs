using CoCSharp.Networking.Packets;
using System;
using System.Threading;

namespace CoCSharp.Client
{
    public class KeepAliveManager
    {
        public KeepAliveManager(CoCClient client)
        {
            Client = client;
            NextKeepAlive = DateTime.Now;
            LastKeepAlive = DateTime.MaxValue;
            KeepAliveThread = new Thread(UpdateKeepAlives);
        }

        public DateTime NextKeepAlive { get; private set; }
        public DateTime LastKeepAlive { get; private set; }

        private CoCClient Client { get; set; }
        private Thread KeepAliveThread { get; set; }
        private bool Running { get; set; }

        public void Start()
        {
            Running = true;
            KeepAliveThread.Start();
        }

        public void Stop()
        {
            Running = false;
            KeepAliveThread.Abort();
        }

        private void UpdateKeepAlives()
        {
            while (Running)
            {
                if (DateTime.Now >= NextKeepAlive)
                {
                    Client.QueuePacket(new KeepAliveRequestPacket());
                    LastKeepAlive = DateTime.Now;
                    NextKeepAlive = DateTime.Now.AddSeconds(5);
                }
            }
        }
    }
}
