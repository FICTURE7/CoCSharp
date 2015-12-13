using CoCSharp.Networking.Messages;
using System;
using System.IO;
using System.Net.Sockets;

namespace CoCSharp.Networking
{
    /// <summary>
    /// Implements methods to read and write <see cref="Message"/> from <see cref="Socket"/> asynchronously
    /// using the <see cref="SocketAsyncEventArgs"/> model.
    /// </summary>
    public class NetworkManagerAsync : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkManagerAsync"/> class
        /// with the specified <see cref="Socket"/>.
        /// </summary>
        /// <param name="connection"></param>
        /// <exception cref="ArgumentNullException"><paramref name="connection"/> is null.</exception>
        public NetworkManagerAsync(Socket connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            Connection = connection;
            _seed = 0;
            _receivePool = new SocketAsyncEventArgsPool(5);
            _sendPool = new SocketAsyncEventArgsPool(5);
            _crypto = new CoCCrypto();

            SetPools();

            StartReceive(_receivePool.Pop());
        }

        private int _seed;
        private bool _disposed;
        private SocketAsyncEventArgsPool _receivePool;
        private SocketAsyncEventArgsPool _sendPool;
        private CoCCrypto _crypto;

        /// <summary>
        /// Gets the <see cref="Socket"/> that is used to send and receive
        /// data.
        /// </summary>
        public Socket Connection { get; private set; }

        /// <summary>
        /// Gets the seed that is used for encryption.
        /// </summary>
        public int Seed { get { return _seed; } }

        public void SendMessage(Message message)
        {
            throw new NotImplementedException();

            var length = 0;
            if (length > Message.MaxSize)
                throw new InvalidMessageException("Length of message is greater than Message.MaxSize.");
        }

        private void StartReceive(SocketAsyncEventArgs args)
        {
            if (!Connection.ReceiveAsync(args))
                AsyncOperationCompleted(Connection, args);
        }

        private void ProcessReceive(SocketAsyncEventArgs args)
        {
            var bytesToProcess = args.BytesTransferred;
            var token = (MessageToken)args.UserToken;

            while (bytesToProcess != 0)
            {
                // copy header from buffer
                if (Message.HeaderSize != token.HeaderOffset) // if we dont have the header yet
                {
                    if (bytesToProcess < token.HeaderRemaining) // if we dont have the header in a single receive op
                    {
                        //Console.WriteLine("Reusing args: {0}", token.TokenID);

                        Buffer.BlockCopy(args.Buffer, token.Offset, token.Header, token.HeaderOffset, bytesToProcess);
                        token.Offset += bytesToProcess;
                        token.HeaderOffset += bytesToProcess;
                        bytesToProcess = 0;
                        token.Offset = 0;

                        StartReceive(args);
                        return;
                    }
                    else
                    {
                        Buffer.BlockCopy(args.Buffer, token.Offset, token.Header, token.HeaderOffset, token.HeaderRemaining);
                        bytesToProcess -= token.HeaderRemaining;
                        token.Offset += token.HeaderRemaining;
                        token.HeaderOffset += token.HeaderRemaining;
                        ProcessToken(token);
                    }
                }

                // copy body from buffer
                if (token.Length != token.BodyOffset) // if we dont have the body yet
                {
                    if (bytesToProcess < token.BodyRemaining) // if we dont have the body in a single receive op
                    {
                        //Console.WriteLine("Reusing args: {0}", token.TokenID);

                        Buffer.BlockCopy(args.Buffer, token.Offset, token.Body, token.BodyOffset, bytesToProcess);
                        token.Offset += bytesToProcess;
                        token.BodyOffset += bytesToProcess;
                        bytesToProcess = 0;
                        token.Offset = 0;

                        StartReceive(args);
                        return;
                    }
                    else
                    {
                        Buffer.BlockCopy(args.Buffer, token.Offset, token.Body, token.BodyOffset, token.BodyRemaining);
                        bytesToProcess -= token.BodyRemaining;
                        token.Offset += token.BodyRemaining;
                        token.BodyOffset += token.BodyRemaining;
                    }
                }

                var message = MessageFactory.Create(token.ID);
                var messageEnData = token.Body;
                var messageDeData = (byte[])token.Body.Clone();

                if (!(message is NewClientEncryptionMessage))
                    _crypto.Decrypt(messageDeData);

                if (message is UnknownMessage)
                {
                    var unknownMessage = (UnknownMessage)message;
                    unknownMessage.Length = token.Length;
                    unknownMessage.Version = token.Version;
                    unknownMessage.DecryptedBytes = messageDeData;
                    unknownMessage.EncryptedBytes = messageEnData;

                    OnMessageReceived(new MessageReceivedEventArgs()
                    {
                        Message = message,
                        MessageBody = messageEnData,
                        MessageHeader = token.Header
                    });
                    token.Reset();
                    continue;
                }

                using (var reader = new MessageReader(new MemoryStream(messageDeData)))
                {
                    try
                    {
                        message.ReadMessage(reader);
                        if (message is EncryptionMessage) // outdated
                        {
                            var encryptionMessage = (EncryptionMessage)message;
                            _crypto.UpdateCiphers(_seed, encryptionMessage.ServerRandom);
                        }
                        else if (message is LoginRequestMessage) // cant be read
                        {
                            var loginRequestMessage = (LoginRequestMessage)message;
                            _seed = loginRequestMessage.Seed;
                        }

                        OnMessageReceived(new MessageReceivedEventArgs()
                        {
                            Message = message,
                            MessageBody = messageEnData,
                            MessageHeader = token.Header
                        });
                    }
                    catch (Exception ex)
                    {
                        OnMessageReceived(new MessageReceivedEventArgs()
                        {
                            Message = message,
                            MessageBody = messageEnData,
                            MessageHeader = token.Header,
                            Exception = ex
                        });
                    }

                    //TODO: Better handling.
                    if (reader.BaseStream.Position < reader.BaseStream.Length)
                        Console.WriteLine("Warning: Did not fully read {0}.", message.GetType().Name);
                }
                token.Reset();
            }
            token.Offset = 0;
            _receivePool.Push(args);
            StartReceive(_receivePool.Pop());
        }

        private void ProcessToken(MessageToken token)
        {
            token.ID = (ushort)((token.Header[0] << 8) | (token.Header[1]));
            token.Length = (token.Header[2] << 16) | (token.Header[3] << 8) | (token.Header[4]);
            token.Version = (ushort)((token.Header[5] << 8) | (token.Header[6]));
            token.Body = new byte[token.Length];
        }

        private void AsyncOperationCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError != SocketError.Success)
                throw new SocketException((int)args.SocketError); // disconnected event

            switch (args.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(args);
                    break;

                default:
                    throw new Exception("wut");
            }
        }

        private void SetPools()
        {
            for (int i = 0; i < _receivePool.Capacity; i++)
            {
                var args = new SocketAsyncEventArgs();
                args.Completed += AsyncOperationCompleted;
                args.SetBuffer(new byte[65535], 0, 65535);
                MessageToken.Create(args);
                _receivePool.Push(args);
            }

            for (int i = 0; i < _sendPool.Capacity; i++)
            {
                var args = new SocketAsyncEventArgs();
                args.Completed += AsyncOperationCompleted;
                args.SetBuffer(new byte[65535], 0, 65535);
                MessageToken.Create(args);
                _sendPool.Push(args);
            }
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="NetworkManagerAsync"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Connection.Dispose();
                    _receivePool.Dispose();
                    _sendPool.Dispose();
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// The event raised when a <see cref="Message"/> is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        /// <summary>
        /// Use this method to trigger the <see cref="MessageReceived"/> event.
        /// </summary>
        /// <param name="e">The arguments.</param>
        protected virtual void OnMessageReceived(MessageReceivedEventArgs e)
        {
            if (MessageReceived != null)
                MessageReceived(this, e);
        }
    }
}
