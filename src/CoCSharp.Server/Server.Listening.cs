using CoCSharp.Server.Api.Events.Server;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CoCSharp.Server
{
    public partial class Server
    {
        public volatile int TotalConnections;

        private volatile bool _stopAccept;
        private readonly Socket _listener;
        private readonly SocketAsyncEventArgsPool _acceptPool;

        private bool StartListener()
        {
            const int PORT = 9339;
            const int BACKLOG = 100;

            // Binds the _listener socket to 0.0.0.0:9339 and begins an AcceptAsync operation
            // on the _listener socket.
            var endpoint = new IPEndPoint(IPAddress.Any, PORT);
            try
            {
                Logs.Info($"Binding listening socket to {endpoint}...");
                _listener.Bind(endpoint);
                _listener.Listen(BACKLOG);

                StartAccept(null);
            }
            catch (SocketException ex)
            {
                Logs.Error("**Unable to start listener socket: " + ex);
                Logs.Error("**Socket Error: " + ex.SocketErrorCode);
                return false;
            }

            return true;
        }

        private void StopListener()
        {
            _stopAccept = true;
        }

        private void StartAccept(SocketAsyncEventArgs args)
        {
            if (_stopAccept)
                return;

            try
            {
                // If the args is null, we take one from the pool.
                if (args == null)
                {
                    args = _acceptPool.Pop();
                    // If the args is null, it means the pool is empty,
                    // we create a new instance to start accepting again.
                    if (args == null)
                        args = _acceptPool.CreateNew();
                }

                if (!_listener.AcceptAsync(args))
                    ProcessAccept(args);
            }
            catch (Exception ex)
            {
                Logs.Error("***Unable to start accept! " + ex);
            }
        }

        private void ProcessAccept(SocketAsyncEventArgs args)
        {
            var remoteSocket = (Socket)null;
            try
            {
                remoteSocket = args.AcceptSocket;
                if (_stopAccept)
                {
                    KillSocket(remoteSocket);
                }
                else
                {
                    var client = new Client(this, remoteSocket);
                    _clients.Add(client);

                    Interlocked.Increment(ref TotalConnections);
                    OnConnection(new ServerConnectionEventArgs(this, client));
                    // Clean AcceptSocket to accept even more.
                    args.AcceptSocket = null;
                }
            }
            catch (Exception ex)
            {
                Logs.Error("Unable to process accept! " + ex);
                KillSocket(remoteSocket);
            }
        }

        private void AcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            var remoteSocket = e.AcceptSocket;
            if (e.SocketError != SocketError.Success)
            {
                KillSocket(remoteSocket);
            }

            ProcessAccept(e);
            e.AcceptSocket = null;
            if (!_stopAccept)
                StartAccept(e);
        }

        private static void KillSocket(Socket socket)
        {
            if (socket == null)
                return;

            try { socket.Shutdown(SocketShutdown.Both); }
            catch { /* Swallow. */ }

            try { socket.Close(); }
            catch { /* Swallow. */ }
        }
    }
}
