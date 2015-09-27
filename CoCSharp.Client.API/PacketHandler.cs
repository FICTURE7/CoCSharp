using CoCSharp.Networking.Packets;

namespace CoCSharp.Client.API
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
    /// <param name="packet"></param>
    public delegate void PacketHandler(ICoCClient client, IPacket packet);
}
