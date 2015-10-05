using CoCSharp.Networking.Packets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoCSharp.Networking
{
    public static class PacketFactory
    {
        static PacketFactory()
        {
            m_PacketDictionary.Add(new LoginRequestPacket().ID, typeof(LoginRequestPacket)); // 10101
            m_PacketDictionary.Add(new KeepAliveRequestPacket().ID, typeof(KeepAliveRequestPacket)); // 10108
            m_PacketDictionary.Add(new SetDeviceTokenPacket().ID, typeof(SetDeviceTokenPacket)); // 10113
            m_PacketDictionary.Add(new ChangeAvatarNamePacket().ID, typeof(ChangeAvatarNamePacket)); // 10212
            m_PacketDictionary.Add(new BindFacebookAccountPacket().ID, typeof(BindFacebookAccountPacket)); // 14201
            m_PacketDictionary.Add(new AllianceChatMessageClientPacket().ID, typeof(AllianceChatMessageClientPacket)); // 14315
            m_PacketDictionary.Add(new AvatarProfileRequestPacket().ID, typeof(AvatarProfileRequestPacket)); // 14325
            m_PacketDictionary.Add(new ChatMessageClientPacket().ID, typeof(ChatMessageClientPacket)); // 14715

            m_PacketDictionary.Add(new UpdateKeyPacket().ID, typeof(UpdateKeyPacket)); // 20000
            m_PacketDictionary.Add(new LoginFailedPacket().ID, typeof(LoginFailedPacket)); // 20103
            m_PacketDictionary.Add(new LoginSuccessPacket().ID, typeof(LoginSuccessPacket)); // 20104
            m_PacketDictionary.Add(new KeepAliveResponsePacket().ID, typeof(KeepAliveResponsePacket)); // 20108
            m_PacketDictionary.Add(new OwnHomeDataPacket().ID, typeof(OwnHomeDataPacket)); // 24101
            m_PacketDictionary.Add(new AllianceChatMessageServerPacket().ID, typeof(AllianceChatMessageServerPacket)); // 24312
            m_PacketDictionary.Add(new ChatMessageServerPacket().ID, typeof(ChatMessageServerPacket)); // 24715
        }

        private static Dictionary<ushort, Type> m_PacketDictionary = new Dictionary<ushort, Type>();

        public static IPacket Create(ushort id)
        {
            var packetType = (Type)null;
            if (!m_PacketDictionary.TryGetValue(id, out packetType))
                return new UnknownPacket();
            return (IPacket)Activator.CreateInstance(packetType);
        }

        public static T Create<T>() where T : IPacket
        {
            var packetType = typeof(T);
            return (T)Activator.CreateInstance(packetType);
        }
    }
}
