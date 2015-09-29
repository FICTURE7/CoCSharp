using CoCSharp.Client.API;
using CoCSharp.Client.API.Events;
using CoCSharp.Networking.Packets;
using System;
using System.IO;

namespace CoCSharp.Client.Handlers
{
    public static class LoginPacketHandlers
    {
        public static void HandleLoginFailedPacket(ICoCClient client, IPacket packet)
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

        public static void HandleLoginSuccessPacket(ICoCClient client, IPacket packet)
        {
            var lsPacket = packet as LoginSuccessPacket;
            ((CoCClient)client).OnLogin(new LoginEventArgs(lsPacket));
        }

        public static void HandleUpdateKeyPacket(ICoCClient client, IPacket packet)
        {
            Console.WriteLine("Updated ciphers with new server key.");
        }

        public static void RegisterLoginPacketHandlers(CoCClient client)
        {
            client.RegisterDefaultPacketHandler(new LoginFailedPacket(), HandleLoginFailedPacket);
            client.RegisterDefaultPacketHandler(new LoginSuccessPacket(), HandleLoginSuccessPacket);
            client.RegisterDefaultPacketHandler(new UpdateKeyPacket(), HandleUpdateKeyPacket);
        }
    }
}
