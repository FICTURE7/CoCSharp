using CoCSharp.Networking.Cryptography;
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
        /// <param name="connection"><see cref="Socket"/> instance.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connection"/> is null.</exception>
        public NetworkManagerAsync(Socket connection) : this(connection, NetworkManagerAsyncSettings.DefaultSettings)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkManagerAsync"/> class
        /// with the specified <see cref="Socket"/> and <see cref="NetworkManagerAsyncSettings"/>.
        /// </summary>
        /// <param name="connection"><see cref="Socket"/> instance.</param>
        /// <param name="settings">
        /// <see cref="NetworkManagerAsyncSettings"/> instance for better <see cref="SocketAsyncEventArgs"/>
        /// object management.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="connection"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        public NetworkManagerAsync(Socket connection, NetworkManagerAsyncSettings settings)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            if (settings == null)
                throw new ArgumentNullException("settings");

            Connection = connection;
            Settings = settings;
            Statistics = new NetworkManagerAsyncStatistics();

            _crypto = new Crypto8();
            _receivePool = Settings.ReceivePool;
            _sendPool = Settings.SendPool;

            StartReceive(_receivePool.Pop());
        }

        private bool _disposed;
        private readonly Crypto8 _crypto;
        private readonly SocketAsyncEventArgsPool _receivePool;
        private readonly SocketAsyncEventArgsPool _sendPool;

        /// <summary>
        /// Gets the <see cref="Socket"/> that is used to send and receive
        /// data.
        /// </summary>
        public Socket Connection { get; private set; }

        /// <summary>
        /// Gets the <see cref="NetworkManagerAsyncSettings"/> being used the
        /// current <see cref="NetworkManagerAsync"/>.
        /// </summary>
        public NetworkManagerAsyncSettings Settings { get; private set; }

        /// <summary>
        /// Gets the <see cref="NetworkManagerAsyncStatistics"/> associated with
        /// the current <see cref="NetworkManagerAsync"/>.
        /// </summary>
        public NetworkManagerAsyncStatistics Statistics { get; private set; }

        /// <summary>
        /// Sends the specified message using the <see cref="Connection"/> socket.
        /// </summary>
        /// <param name="message"><see cref="Message"/> to send.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is null.</exception>
        /// <exception cref="InvalidMessageException"><paramref name="message"/> length greater than <see cref="Message.MaxSize"/>.</exception>
        public void SendMessage(Message message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            using (var deMessageWriter = new MessageWriter(new MemoryStream()))
            {
                message.WriteMessage(deMessageWriter);
                var body = ((MemoryStream)deMessageWriter.BaseStream).ToArray();
                _crypto.Encrypt(ref body);

                //if (message is LoginRequestMessage)
                //    _seed = ((LoginRequestMessage)message).Seed;
                //else if (message is EncryptionMessage)
                //    _crypto.UpdateCiphers(_seed, ((EncryptionMessage)message).ServerRandom);

                if (body.Length > Message.MaxSize)
                    throw new InvalidMessageException("Length of message is greater than Message.MaxSize.");

                using (var enMessageWriter = new MessageWriter(new MemoryStream()))
                {
                    var len = BitConverter.GetBytes(body.Length);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(len);

                    enMessageWriter.Write(message.ID);
                    enMessageWriter.Write(len, 1, 3); // message len
                    enMessageWriter.Write(message.Version);
                    enMessageWriter.Write(body); // encrypted body

                    var messageData = ((MemoryStream)enMessageWriter.BaseStream).ToArray();
                    var args = _sendPool.Pop();
                    var token = args.UserToken as MessageSendToken;
                    token.ID = message.ID;
                    token.Length = body.Length;
                    token.Version = message.Version;
                    token.Body = messageData;

                    StartSend(args);
                }
            }
        }

        public void UpdateKey(byte[] publicKey)
        {
            _crypto.UpdateKey(publicKey);
        }

        private void StartSend(SocketAsyncEventArgs args)
        {
            args.Completed += AsyncOperationCompleted;
            var token = args.UserToken as MessageSendToken;

            if (token.SendRemaining > 0) // if still have bytes to send
            {
                if (token.Body.Length > Settings.BufferSize) // if message larger than buffer size
                {
                    Buffer.BlockCopy(token.Body, token.SendOffset, args.Buffer, args.Offset, Settings.BufferSize);
                }
                else // else resize buffer count
                {
                    Buffer.BlockCopy(token.Body, token.SendOffset, args.Buffer, args.Offset, token.SendRemaining);
                    args.SetBuffer(args.Offset, token.SendRemaining);
                }
            }

            if (!Connection.SendAsync(args))
                ProcessSend(args);
        }

        private void ProcessSend(SocketAsyncEventArgs args)
        {
            var bytesToProcess = args.BytesTransferred;
            var token = args.UserToken as MessageSendToken;

            Statistics.TotalByteReceived += args.BytesTransferred;

            token.SendOffset += bytesToProcess;
            if (token.SendRemaining > 0) // if still have bytes to send
            {
                StartSend(args); // reuse same op
                return;
            }
            else // else reset and push back the args
            {
                token.Reset();
                args.SetBuffer(args.Offset, Settings.BufferSize); // just in case
                _sendPool.Push(args);
            }
        }

        private void StartReceive(SocketAsyncEventArgs args)
        {
            args.Completed += AsyncOperationCompleted;
            if (!Connection.ReceiveAsync(args))
                AsyncOperationCompleted(Connection, args);
        }

        private void ProcessReceive(SocketAsyncEventArgs args)
        {
            var bytesToProcess = args.BytesTransferred;
            var token = args.UserToken as MessageReceiveToken;

            Statistics.TotalByteReceived += args.BytesTransferred;

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
                        token.Offset = args.Offset;

                        StartReceive(args); // reuse same op
                        return;
                    }
                    else
                    {
                        Buffer.BlockCopy(args.Buffer, token.Offset, token.Header, token.HeaderOffset, token.HeaderRemaining);
                        bytesToProcess -= token.HeaderRemaining;
                        token.Offset += token.HeaderRemaining;
                        token.HeaderOffset += token.HeaderRemaining;
                        ProcessReceiveToken(token);
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
                        token.Offset = args.Offset;

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
                var messageEnBody = (byte[])null;
                var messageDeBody = (byte[])null;

                if (message is LoginRequestMessage) // pre decryption
                {
                    var publicKey = new byte[CoCKeyPair.KeyLength]; // copy key from message
                    Buffer.BlockCopy(token.Body, 0, publicKey, 0, CoCKeyPair.KeyLength);

                    _crypto.UpdateKey(publicKey);

                    messageEnBody = new byte[token.Length - CoCKeyPair.KeyLength]; // copy remaining bytes
                    Buffer.BlockCopy(token.Body, CoCKeyPair.KeyLength, messageEnBody, 0, token.Length - CoCKeyPair.KeyLength);

                    messageDeBody = (byte[])messageEnBody.Clone(); // cloning cuz we dont want a reference
                }
                else
                {
                    messageEnBody = token.Body;
                    messageDeBody = (byte[])token.Body.Clone();
                }

                var messageData = new byte[token.Length + Message.HeaderSize];

                Buffer.BlockCopy(token.Header, 0, messageData, 0, Message.HeaderSize);
                Buffer.BlockCopy(token.Body, 0, messageData, Message.HeaderSize, token.Length);

                if (!(message is NewServerEncryptionMessage || message is NewClientEncryptionMessage))
                {
                    _crypto.Decrypt(ref messageDeBody);
                    File.WriteAllBytes("lel", messageDeBody);
                }

                if (message is UnknownMessage)
                {
                    var unknownMessage = (UnknownMessage)message;
                    unknownMessage.Length = token.Length;
                    unknownMessage.Version = token.Version;
                    unknownMessage.DecryptedBytes = messageDeBody;
                    unknownMessage.EncryptedBytes = messageEnBody;

                    OnMessageReceived(new MessageReceivedEventArgs()
                    {
                        Message = message,
                        MessageData = messageData,
                        MessageFullyRead = true
                    });
                    token.Reset();
                    continue;
                }

                using (var reader = new MessageReader(new MemoryStream(messageDeBody)))
                {
                    var ex = (Exception)null;
                    try
                    {
                        message.ReadMessage(reader);

                        //if (message is LoginRequestMessage)
                        //    _seed = ((LoginRequestMessage)message).Seed;
                        //else if (message is EncryptionMessage)
                        //{
                        //    if (((EncryptionMessage)message).ScramblerVersion != 1)
                        //        throw new NotSupportedException("CoCSharp only supports scrambler version 1.");

                        //    //_crypto.UpdateCiphers(_seed, ((EncryptionMessage)message).ServerRandom);
                        //}


                    }
                    catch (Exception exception)
                    {
                        ex = exception;
                    }

                    OnMessageReceived(new MessageReceivedEventArgs()
                    {
                        Message = message,
                        MessageData = messageData,
                        MessageFullyRead = reader.BaseStream.Position == reader.BaseStream.Length,
                        Exception = ex
                    });
                }
                token.Reset();
            }

            token.Offset = args.Offset;
            _receivePool.Push(args);
            StartReceive(_receivePool.Pop());
        }

        private void ProcessReceiveToken(MessageReceiveToken token)
        {
            token.ID = (ushort)((token.Header[0] << 8) | (token.Header[1]));
            token.Length = (token.Header[2] << 16) | (token.Header[3] << 8) | (token.Header[4]);
            token.Version = (ushort)((token.Header[5] << 8) | (token.Header[6]));
            token.Body = new byte[token.Length];
        }

        private void AsyncOperationCompleted(object sender, SocketAsyncEventArgs args)
        {
            args.Completed -= AsyncOperationCompleted;
            if (args.SocketError == SocketError.OperationAborted || _disposed)
            {
                _receivePool.Push(args);
                return; // gently stop any operations
            }

            if (args.SocketError != SocketError.Success)
                throw new SocketException((int)args.SocketError); //TODO: Better handling, DisconnectedEventArgs

            switch (args.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(args);
                    break;

                case SocketAsyncOperation.Send:
                    ProcessSend(args);
                    break;

                default:
                    throw new InvalidOperationException("IMPOSSIBRU!");
            }
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="NetworkManagerAsync"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all unmanged resources and optionally releases managed resources
        /// used by the current instance of the <see cref="NetworkManagerAsync"/> class.
        /// </summary>
        /// <param name="disposing">Releases managed resources if set to <c>true</c>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    try
                    {
                        Connection.Shutdown(SocketShutdown.Both);
                    }
                    catch { }

                    Connection.Close();
                    //Settings.Dipose();
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
