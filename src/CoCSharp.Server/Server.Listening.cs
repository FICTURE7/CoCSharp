using CoCSharp.Server.API.Events.Server;
using System;
using System.Net;
using System.Net.Sockets;

namespace CoCSharp.Server
{
    public partial class Server
    {
        private readonly Socket _listener;
        private readonly SocketAsyncEventArgsPool _acceptPool;

        private void StartListener()
        {
            if (_disposed)
                return;

            const int PORT = 9339;
            const int BACKLOG = 100;

            // Binds the _listener socket to 0.0.0.0:9339 and begins an AcceptAsync operation
            // on the _listener socket.
            var endpoint = new IPEndPoint(IPAddress.Any, PORT);
            try
            {
                _listener.Bind(endpoint);
                _listener.Listen(BACKLOG);

                StartAccept(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                Environment.Exit(1);
            }
        }

        private void StartAccept(SocketAsyncEventArgs args)
        {
            if (_disposed)
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
                Console.WriteLine("Unable to start accept! " + ex);
            }
        }

        private void ProcessAccept(SocketAsyncEventArgs args)
        {
            if (_disposed)
                return;

            try
            {
                var remoteSocket = args.AcceptSocket;
                var client = new Client(this, remoteSocket);
                _clients.Add(client);

                OnConnection(new ServerConnectionEventArgs(this, client));
                // Clean AcceptSocket to accept even more.
                args.AcceptSocket = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to process accept! " + ex);
            }
            finally
            {
                StartAccept(args);
            }
        }

        private void AcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (_disposed)
                return;

            if (e.SocketError != SocketError.Success)
            {
                e.AcceptSocket.Close();
                e.AcceptSocket = null;
                StartAccept(e);
                return;
            }

            ProcessAccept(e);
        }
    }
}
