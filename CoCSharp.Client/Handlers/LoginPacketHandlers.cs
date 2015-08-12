using CoCSharp.Networking.Packets;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CoCSharp.Client.Handlers
{
    public static class LoginPacketHandlers
    {
        public static void HandleLoginFailedPacket(CoCClient client, IPacket packet)
        {
            var lfPacket = packet as LoginFailedPacket;
            Console.WriteLine("Failed to login, reason: {0}", lfPacket.FailureReason);
            switch (lfPacket.FailureReason)
            {
                case LoginFailedPacket.LoginFailureReason.OutdatedContent:
                    if (lfPacket.Fingerprint != null)
                    {
                        var fingerprintJson = lfPacket.Fingerprint.ToJson();
                        File.WriteAllText("fingerprint.json", fingerprintJson);

                        Console.WriteLine("Server expected hash: {0}", lfPacket.Fingerprint.Hash);
                    }
                    break;
            }
        }

        public static void HandleLoginSuccessPacket(CoCClient client, IPacket packet)
        {
            var lsPacket = packet as LoginSuccessPacket;
            Console.WriteLine("Successfully logged in!");
        }

        public static void HandleUpdateKeyPacket(CoCClient client, IPacket packet)
        {
            Console.WriteLine("Updating encryption on thread {0}", Thread.CurrentThread.ManagedThreadId);
            var ukPacket = packet as UpdateKeyPacket;
            client.NetworkManager.UpdateChipers((ulong)client.NetworkManager.Seed, ukPacket.Key);
            Console.WriteLine("Updated encryption on thread {0}", Thread.CurrentThread.ManagedThreadId);
        }

        public static void RegisterLoginPacketHandlers(CoCClient client)
        {
            client.RegisterPacketHandler(new LoginFailedPacket(), HandleLoginFailedPacket);
            client.RegisterPacketHandler(new LoginSuccessPacket(), HandleLoginSuccessPacket);
            client.RegisterPacketHandler(new UpdateKeyPacket(), HandleUpdateKeyPacket);
        }
    }
}
