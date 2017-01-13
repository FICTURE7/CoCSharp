using CoCSharp.Logic;
using CoCSharp.Logic.Commands;
using CoCSharp.Network;
using CoCSharp.Network.Cryptography;
using CoCSharp.Network.Messages;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace CoCSharp.Client
{
    public class Client : IDisposable
    {
        private static NetworkManagerAsyncSettings s_settings = new NetworkManagerAsyncSettings(64, 64);
        private static Random s_rand = new Random();

        public static bool ShouldReturnHome => s_rand.Next(0, 250) == 0;
        public static bool ShouldChat => s_rand.Next(0, 500) == 0;
        public static bool ShouldAttack => s_rand.Next(0, 500) == 0;
        public static bool ShouldProfile => s_rand.Next(0, 500) == 0;

        public Client()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public NetworkManagerAsync NetworkManager => _networkManager;

        private bool _disposed;
        private readonly Socket _socket;

        private LoginRequestMessage _lrMessage;
        private NetworkManagerAsync _networkManager;
        private int _tick;
        private int _state;

        public void Connect(EndPoint endPoint)
        {
            if (endPoint == null)
                throw new ArgumentNullException(nameof(endPoint));

            _socket.Connect(endPoint);
            _networkManager = new NetworkManagerAsync(_socket, s_settings, new MessageProcessorNaCl(Crypto8.GenerateKeyPair(), Crypto8.SupercellPublicKey));
            _networkManager.MessageReceived += OnMessageReceived;
            _networkManager.Disconnected += OnDisconnected;
        }

        public void Login(long userId, string userToken)
        {
            var handshake = new HandshakeRequestMessage
            {
                AppStore = 2,
                Build = 709,
                DeviceType = 2,
                Hash = "2f2c3464104feb771097b42ebf4dfe871bd56062",
                KeyVersion = 20,
                MajorVersion = 8,
                MinorVersion = 0,
                Protocol = 1,
            };

            _lrMessage = new LoginRequestMessage
            {
                // To make a new account
                UserId = userId,
                UserToken = userToken,

                MajorVersion = 8,
                MinorVersion = 709,
                ContentVersion = 0,
                LocaleKey = 2000000,
                Language = "en",
                AdvertisingGuid = "",
                OSVersion = "4.4.2",
                IsAdvertisingTrackingEnabled = true,
                MasterHash = "2f2c3464104feb771097b42ebf4dfe871bd56062",
                FacebookDistributionID = "",
                VendorGUID = "",
                ClientVersion = "8.709.2",
                Seed = new Random().Next()
            };

            _networkManager.SendMessage(handshake);
            _state = 1;
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (e.Message == null)
            {
                Console.WriteLine("Unable to retrieve message.");
            }
            else
            {
                if (e.Message is HandshakeSuccessMessage)
                {
                    Debug.Assert(_state == 1);

                    _networkManager.SendMessage(_lrMessage);
                    _state = 2;
                }
                else if (e.Message is LoginSuccessMessage)
                {
                    Debug.Assert(_state == 2);

                    Console.WriteLine("Logged in successfully.");
                    _state = 3;
                }
                else if (e.Message is LoginFailedMessage)
                {
                    Console.WriteLine("Login failed.");
                }
                else if (e.Message is ChatMessageServerMessage)
                {
                    if (ShouldProfile)
                    {
                        var k = e.Message as ChatMessageServerMessage;
                        _networkManager.SendMessage(new AvatarProfileRequestMessage
                        {
                            UserId = k.UserId,
                            HomeId = k.HomeId
                        });
                    }
                }
                else if (e.Message is EnemyHomeDataMessage)
                {
                    Console.WriteLine("Got stuff to attack.");

                    _state = 4;
                }
                else if (e.Message is OwnHomeDataMessage)
                {
                    //Console.WriteLine(((OwnHomeDataMessage)e.Message).OwnVillageData.VillageJson);
                }
            }
        }

        private void OnDisconnected(object sender, DisconnectedEventArgs e)
        {
            Console.WriteLine("Disconnected!");
        }

        public void Tick()
        {
            if (_state >= 3)
            {
                var commands = new Command[0];
                if (_state == 4)
                {
                    if (ShouldReturnHome)
                    {
                        _networkManager.SendMessage(new ReturnHomeMessage());
                    }
                }

                if (ShouldAttack)
                {
                    commands = new Command[]
                    {
                        new MatchmakingCommand()
                    };

                    _networkManager.SendMessage(new CommandMessage
                    {
                        Commands = commands,
                        Tick = _tick
                    });

                    Console.WriteLine("Attacking stuff!");
                }
                else if (_tick % 150 == 0)
                {
                    _networkManager.SendMessage(new CommandMessage
                    {
                        Commands = commands,
                        Tick = _tick
                    });
                }

                if (ShouldChat)
                {
                    var msg = new ChatMessageClientMessage
                    {
                        TextMessage = "Hi mom",
                    };

                    _networkManager.SendMessage(msg);
                }
            }

            if (_state >= 2)
            {
                if (_tick % 300 == 0)
                {
                    _networkManager.SendMessage(new KeepAliveRequestMessage());
                }
            }

            _tick++;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (_networkManager == null)
                {
                    _socket.Dispose();
                }
                else
                {
                    _networkManager.Dispose();
                }
            }

            _disposed = true;
        }
    }
}
