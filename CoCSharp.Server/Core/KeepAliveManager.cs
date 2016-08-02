using System;
using System.Diagnostics;
using System.Threading;

namespace CoCSharp.Server.Core
{
    public class KeepAliveManager
    {
        public KeepAliveManager(CoCServer server)
        {
            if (server == null)
                throw new ArgumentNullException("server");

            _server = server;

            _keepAliveThread = new Thread(UpdateKeepAlive);
            _keepAliveThread.Name = "KeepAlive Thread";
            _keepAliveThread.Priority = ThreadPriority.BelowNormal;
        }

        public bool Started { get; set; }

        private readonly CoCServer _server;
        private readonly Thread _keepAliveThread;

        public void Start()
        {
            if (Started)
                throw new InvalidOperationException("KeepAliveManager already started.");

            Started = true;
            _keepAliveThread.Start();
        }

        public void Stop()
        {
            if (!Started)
                throw new InvalidOperationException("KeepAliveManager did not start yet.");

            Started = false;
            _keepAliveThread.Join();
        }

        private void UpdateKeepAlive()
        {
            while (Started)
            {
                try
                {
                    // If the client does not send a KeepAliveRequestMessage within an interval of 30 seconds
                    // we assume its disconnected.

                    for (int i = 0; i < _server.Clients.Count; i++)
                    {
                        var client = _server.Clients[i];
                        if (DateTime.UtcNow >= client.ExpirationKeepAlive)
                        {
                            client.Disconnect();
                            Debug.WriteLine("Disconnecting client because it did not send a KeepAliveRequestMessage in time.", "KeepAlive");
                        }
                    }

                    Thread.Sleep(500);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Exception while updating keep alive: {0}", ex.Message);
                }
            }
        }
    }
}
