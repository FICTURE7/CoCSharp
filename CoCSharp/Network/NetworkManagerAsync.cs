using CoCSharp.Network.Cryptography;
using CoCSharp.Network.Messages;
using System;
using System.IO;
using System.Net.Sockets;

namespace CoCSharp.Network
{
    /// <summary>
    /// Implements methods to send and receive <see cref="Message"/> from <see cref="Socket"/> asynchronously
    /// using the <see cref="SocketAsyncEventArgs"/> model.
    /// </summary>
    public class NetworkManagerAsync : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkManagerAsync"/> class with the specified <see cref="Socket"/>.
        /// </summary>
        /// <param name="connection"><see cref="Socket"/> instance.</param>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="connection"/> is null.</exception>
        public NetworkManagerAsync(Socket connection)
            : this(connection, NetworkManagerAsyncSettings.DefaultSettings, new Crypto8(MessageDirection.Client, Crypto8.StandardKeyPair))
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkManagerAsync"/> class with the specified <see cref="Socket"/> 
        /// and <see cref="NetworkManagerAsyncSettings"/>.
        /// </summary>
        /// 
        /// <param name="connection"><see cref="Socket"/> instance.</param>
        /// <param name="settings">
        /// <see cref="NetworkManagerAsyncSettings"/> instance for better <see cref="SocketAsyncEventArgs"/>
        /// object management.
        /// </param>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="connection"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        public NetworkManagerAsync(Socket connection, NetworkManagerAsyncSettings settings)
            : this(connection, settings, new Crypto8(MessageDirection.Client, Crypto8.StandardKeyPair))
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkManagerAsync"/> class with the specified <see cref="Socket"/>
        /// and <see cref="NetworkManagerAsyncSettings"/> with the specified <see cref="Crypto8"/> that will be used to encrypt and decrypt messages.
        /// </summary>
        /// 
        /// <param name="connection"><see cref="Socket"/> instance.</param>
        /// <param name="settings">
        /// <see cref="NetworkManagerAsyncSettings"/> instance for better <see cref="SocketAsyncEventArgs"/>
        /// object management.
        /// </param>
        /// <param name="crypto"><see cref="Crypto8"/> instance that will be used to encrypt and decrypt messages.</param>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="connection"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="crypto"/> is null.</exception>
        public NetworkManagerAsync(Socket connection, NetworkManagerAsyncSettings settings, Crypto8 crypto)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (crypto == null)
                throw new ArgumentNullException("crypto");

            Connection = connection;
            Settings = settings;
            Crypto = crypto;
            Statistics = new NetworkManagerAsyncStatistics();

            _receivePool = Settings._receivePool;
            _sendPool = Settings._sendPool;

            var args = _receivePool.Pop();
            if (args == null)
            {
                args = new SocketAsyncEventArgs();
                args.SetBuffer(new byte[65535], 0, 65535);
                MessageReceiveToken.Create(args);
            }
            StartReceive(args);
        }

        private bool _disposed;
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
        /// Gets the <see cref="Crypto8"/> being used with
        /// the current <see cref="NetworkManagerAsync"/>.
        /// </summary>
        public Crypto8 Crypto { get; private set; }

        /// <summary>
        /// Sends the specified message using the <see cref="Connection"/> socket.
        /// </summary>
        /// <param name="message"><see cref="Message"/> to send.</param>
        /// 
        /// <exception cref="ObjectDisposedException">The current instance of the <see cref="NetworkManagerAsync"/> is disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="message"/> is null.</exception>
        /// <exception cref="InvalidMessageException"><paramref name="message"/> length greater than <see cref="Message.MaxSize"/>.</exception>
        public void SendMessage(Message message)
        {
            //TODO: Custom write for LoginRequestMessage.

            if (_disposed)
                throw new ObjectDisposedException(null, "Cannot SendMessage because the NetworkManagerAsync object was disposed.");
            if (message == null)
                throw new ArgumentNullException("message");

            using (var deMessageWriter = new MessageWriter(new MemoryStream()))
            {
                message.WriteMessage(deMessageWriter);
                var body = ((MemoryStream)deMessageWriter.BaseStream).ToArray();

                if (body.Length > Message.MaxSize)
                    throw new InvalidMessageException("Length of message is greater than Message.MaxSize.");

                if (!(message is SessionSuccessMessage || message is SessionRequestMessage)) // ignore 10100 and 20100 for encryption
                    Crypto.Encrypt(ref body);

                if (message is LoginSuccessMessage)
                {
                    var lsMessage = message as LoginSuccessMessage;
                    Crypto.UpdateNonce(lsMessage.Nonce, UpdateNonceType.Encrypt);
                    Crypto.UpdateSharedKey(lsMessage.PublicKey);
                }

                using (var enMessageWriter = new MessageWriter(new MemoryStream()))
                {
                    var len = BitConverter.GetBytes(body.Length);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(len);

                    enMessageWriter.Write(message.ID);
                    enMessageWriter.Write(len, 1, 3); // message length
                    enMessageWriter.Write(message.Version);
                    enMessageWriter.Write(body); // encrypted body

                    var messageData = ((MemoryStream)enMessageWriter.BaseStream).ToArray();
                    var args = _sendPool.Pop();
                    if (args == null)
                    {
                        args = new SocketAsyncEventArgs();
                        args.SetBuffer(new byte[65535], 0, 65535);
                        MessageSendToken.Create(args);
                    }
                    var token = args.UserToken as MessageSendToken;
                    token.ID = message.ID;
                    token.Length = body.Length;
                    token.Version = message.Version;
                    token.Body = messageData;

                    StartSend(args);
                }
            }
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

                // After encryption (was added after the server/client encrypted the data).
                if (message is LoginRequestMessage) // we're the server
                {
                    // Copies clientKey(pk) from raw message -> token.Body[:32].
                    var publicKey = new byte[CoCKeyPair.KeyLength];
                    Buffer.BlockCopy(token.Body, 0, publicKey, 0, CoCKeyPair.KeyLength);

                    // Copies remaining bytes from raw message -> token.Body[32:]
                    messageEnBody = new byte[token.Length - CoCKeyPair.KeyLength];
                    Buffer.BlockCopy(token.Body, CoCKeyPair.KeyLength, messageEnBody, 0, messageEnBody.Length);

                    var lrMessage = message as LoginRequestMessage;
                    lrMessage.PublicKey = publicKey;

                    // Should never happen. As the Crypto8 was initiated as the client and
                    // LoginRequestMessage is sent to the server only.
                    if (Crypto.Direction == MessageDirection.Server)
                        Console.WriteLine("Vut Ze Fk m8!?");

                    // Updates with clientKey(pk). _cryptoState = InitialKey
                    Crypto.UpdateSharedKey(publicKey);
                }
                else
                {
                    messageEnBody = token.Body;
                }

                // Cloning the byte array because we don't want a reference.
                messageDeBody = (byte[])messageEnBody.Clone();

                // Full message data, that is message header including message body.
                var messageData = new byte[token.Length + Message.HeaderSize];
                Buffer.BlockCopy(token.Header, 0, messageData, 0, Message.HeaderSize);
                Buffer.BlockCopy(token.Body, 0, messageData, Message.HeaderSize, token.Length);

                // Ignore 10100 & 20100 for decryption.
                // TODO: Ignore message for decryption when Crypto._state == CryptoState.None.
                if (!(message is SessionSuccessMessage || message is SessionRequestMessage))
                {
                    try
                    {
                        Crypto.Decrypt(ref messageDeBody);
                    }
                    catch (Exception ex)
                    {
                        // Failed to decrypt the message.
                        OnMessageReceived(new MessageReceivedEventArgs()
                        {
                            Message = message,
                            MessageData = messageData,
                            MessageBody = messageDeBody,
                            Exception = ex
                        });
                        token.Reset();

                        // Could break early here because the keys are probably messed up.
                        continue;
                    }
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
                        MessageBody = messageDeBody,
                        MessageFullyRead = true
                    });
                    token.Reset();
                    continue;
                }

                using (var reader = new MessageReader(new MemoryStream(messageDeBody)))
                {
                    var exception = (Exception)null;

                    try { message.ReadMessage(reader); }
                    catch (Exception ex) { exception = ex; }

                    // Before encryption (was added before the server/client encrypted the data).
                    if (message is LoginRequestMessage) // We're the server.
                    {
                        var lrMessage = message as LoginRequestMessage;
                        Crypto.UpdateNonce(lrMessage.Nonce, UpdateNonceType.Decrypt); // update with snonce, decryptnonce = snonce
                        Crypto.UpdateNonce(lrMessage.Nonce, UpdateNonceType.Blake);
                    }
                    else if (message is LoginSuccessMessage) // We're the client.
                    {
                        var lsMessage = message as LoginSuccessMessage;
                        Crypto.UpdateNonce(lsMessage.Nonce, UpdateNonceType.Decrypt); // update with rnonce, decryptnonce = rnonce
                        Crypto.UpdateSharedKey(lsMessage.PublicKey); // Update crypto with k.
                    }
                    else if (message is LoginFailedMessage) // We're the client.
                    {
                        var lfMessage = message as LoginFailedMessage;
                        Crypto.UpdateNonce(lfMessage.Nonce, UpdateNonceType.Decrypt); // update with rnonce, decryptnonce = rnonce
                        Crypto.UpdateSharedKey(lfMessage.PublicKey); // Update crypto with k.
                    }

                    OnMessageReceived(new MessageReceivedEventArgs()
                    {
                        Message = message,
                        MessageData = messageData,
                        MessageBody = messageDeBody,
                        MessageFullyRead = reader.BaseStream.Position == reader.BaseStream.Length,
                        Exception = exception
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

            //TODO: Better handling, DisconnectedEventArgs
            if (args.SocketError != SocketError.Success)
                OnDisconnected(new DisconnectedEventArgs() { Error = args.SocketError });

            if (args.BytesTransferred == 0)
            {
                _receivePool.Push(args);
                OnDisconnected(new DisconnectedEventArgs());
                return;
            }

            switch (args.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(args);
                    break;

                case SocketAsyncOperation.Send:
                    ProcessSend(args);
                    break;

                default:
                    throw new InvalidOperationException("IMPOSSIBRU! Unexpected SocketAsyncOperation: " + args.LastOperation);
            }
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="NetworkManagerAsync"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases all unmanaged resources and optionally releases managed resources
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
                    //Settings.Dipose(); // Don't dispose because we might dispose other NetworkManagers.
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
            if (MessageReceived != null && !_disposed)
                MessageReceived(this, e);
        }

        /// <summary>
        /// The event raised when <see cref="Connection"/> socket got disconnected.
        /// </summary>
        public event EventHandler<DisconnectedEventArgs> Disconnected;
        /// <summary>
        /// Use this method to trigger the <see cref="Disconnected"/> event.
        /// </summary>
        /// <param name="e">The arguments.</param>
        protected virtual void OnDisconnected(DisconnectedEventArgs e)
        {
            if (Disconnected != null)
                Disconnected(this, e);
        }
    }
}
