using CoCSharp.Networking.Packets;
using System;
using System.Threading;

namespace CoCSharp.Client
{
    public class KeepAliveManager : IDisposable
    {
        private bool m_Disposed = false;

        public KeepAliveManager(CoCClient client)
        {
            Delay = 5;
            Client = client;
            NextKeepAlive = DateTime.Now;
            LastKeepAlive = DateTime.MaxValue;
            KeepAliveThread = new Thread(UpdateKeepAlives);
        }

        ~KeepAliveManager()
        {
            Dispose(false);
        }

        public int Delay { get; set; }
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
            try
            {
                while (Running)
                {
                    if (DateTime.Now >= NextKeepAlive)
                    {
                        Client.SendPacket(new KeepAliveRequestPacket());
                        LastKeepAlive = DateTime.Now;
                        NextKeepAlive = DateTime.Now.AddSeconds(Delay);
                    }
                    Thread.Sleep(Delay * 999);
                }
            }
            catch (ThreadAbortException)
            {
                // aborting
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (m_Disposed)
                return;

            if (disposing)
                Stop();

            m_Disposed = true;
        }
    }
}
