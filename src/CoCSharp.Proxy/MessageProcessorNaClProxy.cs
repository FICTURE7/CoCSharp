using System;
using CoCSharp.Network;
using CoCSharp.Network.Cryptography;
using System.Diagnostics;
using System.IO;
using CoCSharp.Network.Cryptography.NaCl;

namespace CoCSharp.Proxy
{
    public class MessageProcessorNaClProxy : MessageProcessor
    {
        // Key pair we're going to use to decrypt traffic coming from the client.
        public MessageProcessorNaClProxy(KeyPair keyPair)
        {
            if (keyPair == null)
                throw new ArgumentNullException(nameof(keyPair));

            _keyPair = keyPair;
            _lock = new object();
        }

        private object _lock;
        private Crypto8 _clientCrypto;
        private Crypto8 _serverCrypto;

        private int _state;
        private readonly KeyPair _keyPair;

        private byte[] _sessionKey;
        private byte[] _serverNonce;
        private byte[] _clientNonce;

        public int State => _state;
        public byte[] SessionKey => _sessionKey;
        public byte[] ServerNonce => _serverNonce;
        public byte[] ClientNonce => _clientNonce;
        public Crypto8 ServerCrypto
        {
            get
            {
                lock (_lock)
                {
                    return _serverCrypto;
                }
            }
        }
        public Crypto8 ClientCrypto
        {
            get
            {
                lock (_lock)
                {
                    return _clientCrypto;
                }
            }
        }
        public override CoCCrypto Crypto
        {
            get
            {
                throw new Exception("Use ServerCrypto or ClientCrypto instead.");
            }
        }

        public override Message ProcessIncoming(MessageHeader header, BufferStream stream, ref byte[] raw, ref byte[] plaintext)
        {
            var messageDirection = Message.GetMessageDirection(header.Id);
            // Message instance that we will return to the caller.
            var message = MessageFactory.Create(header.Id);
            var chiper = new byte[header.Length];
            stream.Read(chiper, 0, header.Length);

            // Unencrypted byte array.
            plaintext = null;
            lock (_lock)
            {
                // Handshaking.
                if (_state <= 1)
                {
                    // Handshakes are sent unencrypted.
                    // First message by both ends is always sent unencrypted.
                    plaintext = (byte[])chiper.Clone();

                    // Use the first message direction to configure the processor.
                    if (_state == 0)
                        _clientCrypto = new Crypto8(GetOppositeDirection(messageDirection), _keyPair);
                    if (_state == 1)
                        _serverCrypto = new Crypto8(GetOppositeDirection(messageDirection), Crypto8.GenerateKeyPair());
                }

                if (_state == 4)
                {
                    _clientCrypto.UpdateNonce((byte[])_serverNonce.Clone(), UpdateNonceType.Encrypt);
                    _clientCrypto.UpdateNonce((byte[])_clientNonce.Clone(), UpdateNonceType.Decrypt);
                    _clientCrypto.UpdateSharedKey(_serverCrypto.SharedKey);
                }

                // _state == 1 means we are the server.
                // Usually processing 10101 - LoginRequestMessage.
                if (_state == 2)
                {
                    _serverCrypto.UpdateSharedKey(Crypto8.SupercellPublicKey);

                    // -> Pre-Encryption.
                    // Copies the public key appended to the beginning of the message.
                    var publicKey = new byte[KeyPair.KeyLength];
                    Buffer.BlockCopy(chiper, 0, publicKey, 0, KeyPair.KeyLength);

                    Debug.WriteLine($"Public-Key from {header.Id}: {ToHexString(publicKey)}");

                    // Copies the remaining bytes into the plaintext buffer
                    var plaintextLen = header.Length - KeyPair.KeyLength;
                    plaintext = new byte[plaintextLen];
                    Buffer.BlockCopy(chiper, KeyPair.KeyLength, plaintext, 0, plaintextLen);

                    // Crypto8 will take publicKey & _keyPair.PublicKey and generate a blake2b nonce
                    _clientCrypto.UpdateSharedKey(publicKey);
                    // Then use _keyPair.PrivateKey, publicKey and nonce to decrypt.
                    _clientCrypto.Decrypt(ref plaintext);

                    // -> Post-Encryption.
                    _sessionKey = new byte[KeyPair.NonceLength];
                    _clientNonce = new byte[KeyPair.NonceLength];

                    // Copy the SessionKey and the ClientNonce.
                    Buffer.BlockCopy(plaintext, 0, _sessionKey, 0, KeyPair.NonceLength);
                    Buffer.BlockCopy(plaintext, KeyPair.NonceLength, _clientNonce, 0, KeyPair.NonceLength);

                    Debug.WriteLine($"Session-key from {header.Id}: {ToHexString(_sessionKey)}");
                    Debug.WriteLine($"Client-nonce from {header.Id}: {ToHexString(_clientNonce)}");

                    var actualMessage = new byte[plaintext.Length - (KeyPair.NonceLength * 2)];
                    Buffer.BlockCopy(plaintext, KeyPair.NonceLength * 2, actualMessage, 0, actualMessage.Length);

                    plaintext = actualMessage;
                }
                else if (_state == 3)
                {
                    _clientCrypto.UpdateNonce(_clientNonce, UpdateNonceType.Blake);
                    _serverCrypto.UpdateNonce(_clientNonce, UpdateNonceType.Blake);
                    _serverCrypto.Decrypt(ref chiper);

                    // Post-Encryption 
                    // Copies the public key appended to the beginning of the message.
                    _serverNonce = new byte[KeyPair.NonceLength];
                    Buffer.BlockCopy(chiper, 0, _serverNonce, 0, KeyPair.NonceLength);

                    var publicKey = new byte[KeyPair.KeyLength];
                    Buffer.BlockCopy(chiper, KeyPair.NonceLength, publicKey, 0, KeyPair.KeyLength);

                    _serverCrypto.UpdateNonce((byte[])_serverNonce.Clone(), UpdateNonceType.Decrypt);
                    _serverCrypto.UpdateNonce((byte[])_clientNonce.Clone(), UpdateNonceType.Encrypt);
                    _serverCrypto.UpdateSharedKey(publicKey);

                    Debug.WriteLine($"Server-Nonce from {header.Id}: {ToHexString(_serverNonce)}");
                    Debug.WriteLine($"New Public-Key from {header.Id}: {ToHexString(publicKey)}");

                    // Copies the remaining bytes into the plaintext buffer.
                    var plaintextLen = chiper.Length - KeyPair.KeyLength - KeyPair.NonceLength;
                    plaintext = new byte[plaintextLen];

                    Buffer.BlockCopy(chiper, KeyPair.KeyLength + KeyPair.NonceLength, plaintext, 0, plaintextLen);
                }
                else if (_state > 3)
                {
                    if (messageDirection == MessageDirection.Client)
                        _serverCrypto.Decrypt(ref chiper);

                    if (messageDirection == MessageDirection.Server)
                        _clientCrypto.Decrypt(ref chiper);

                    plaintext = chiper;
                }

                _state++;
            }
            try
            {
                using (var reader = new MessageReader(new MemoryStream(plaintext)))
                    message.ReadMessage(reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ex: " + ex);
            }
            return message;
        }

        public override byte[] ProcessOutgoing(Message message)
        {
            throw new NotImplementedException();
        }

        private static MessageDirection GetOppositeDirection(MessageDirection dir)
        {
            if (dir == MessageDirection.Client)
                return MessageDirection.Server;

            if (dir == MessageDirection.Server)
                return MessageDirection.Client;

            throw new Exception();
        }

        private static string ToHexString(byte[] bytes)
        {
            var str = string.Empty;
            for (int i = 0; i < bytes.Length; i++)
                str += bytes[i].ToString("x2");
            return str;
        }

    }
}
