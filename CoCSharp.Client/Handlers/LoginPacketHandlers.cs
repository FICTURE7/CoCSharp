using CoCSharp.Networking.Packets;
using SevenZip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCSharp.Client.Handlers
{
    public static class LoginPacketHandlers
    {
        public static void HandleLoginFailedPacket(CoCClient client, IPacket packet)
        {
            var lfpacket = packet as LoginFailed;
            File.WriteAllBytes("com_fingerprint", lfpacket.CompressedFingerprintJson);
        }

        public static void HandleUpdateKeyPacket(CoCClient client, IPacket packet)
        {
            var ukPacket = packet as UpdateKeyPacket;
            client.NetworkManager.UpdateChipers((ulong)client.Seed, ukPacket.Key);
        }

        public static void RegisterLoginPacketHandlers(CoCClient client)
        {
            client.RegisterPacketHandler(new UpdateKeyPacket(), HandleUpdateKeyPacket);
            client.RegisterPacketHandler(new LoginFailed(), HandleLoginFailedPacket);
        }
    }
}
