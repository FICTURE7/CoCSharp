namespace CoCSharp.Networking.Packets
{
    /// <summary>
    /// Under construction
    /// </summary>
    public class LastClanWarDataPacket : IPacket
    {
        public ushort ID { get { return 0x5F0F; } }

        public int Unknown1;
        public int Unknown2;
        public bool Unknown3;
        public bool Unknown4;
        public bool Unknown5;
        public long Unknown6;
        public string ClanName;
        public int Unknown7;
        public int Unknown8;
        public int PlayersInWarCount;
        public int Unknown9;
        public int Unknown10;
        public int Unknown11;
        public int Unknown12;
        public int Unknown13;
        public int Unknown14;
        public string player1Name;

        public void ReadPacket(PacketReader reader)
        {
            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadBoolean();
            Unknown4 = reader.ReadBoolean();
            Unknown5 = reader.ReadBoolean();
            Unknown6 = reader.ReadInt64();
            reader.BaseStream.Position += 1;
            Unknown7 = reader.ReadInt32();
            Unknown8 = reader.ReadInt32();
            PlayersInWarCount = reader.ReadInt32();
            Unknown9 = reader.ReadInt32();
            Unknown10 = reader.ReadInt32();
            Unknown11 = reader.ReadInt32();
            Unknown12 = reader.ReadInt32();
            Unknown13 = reader.ReadInt32();
            Unknown14 = reader.ReadInt32();

            var p = new WarPlayerInfo();

            p.Name = reader.ReadString();
            p.Unknown1 = reader.ReadInt32();
            p.Unknown2 = reader.ReadInt32();
            p.Unknown3 = reader.ReadInt32();
            p.Unknown4 = reader.ReadInt32();
            p.Unknown5 = reader.ReadInt32();
            p.Unknown6 = reader.ReadInt32();
            p.Unknown7 = reader.ReadInt32();
            p.Unknown8 = reader.ReadInt32();
            p.Unknown9 = reader.ReadInt32();
            p.Unknown10 = reader.ReadInt32();
            p.Unknown11 = reader.ReadInt32();
            p.Unknown12 = reader.ReadInt32();
            p.Unknown13 = reader.ReadInt32();
            p.Unknown14 = reader.ReadInt32();
            p.Unknown15 = reader.ReadInt32();
            p.Unknown16 = reader.ReadBoolean();
            p.Unknown17 = reader.ReadBoolean();
            p.Unknown18 = reader.ReadBoolean();
            p.Unknown19 = reader.ReadInt32();
            p.Unknown20 = reader.ReadInt32();
            p.Unknown21 = reader.ReadInt32();
            p.Unknown22 = reader.ReadInt32();
            p.Unknown23 = reader.ReadInt32();
            p.Unknown24 = reader.ReadInt32();
            p.Unknown25 = reader.ReadInt32();
            p.RequestText = reader.ReadString();
        }

        public void WritePacket(PacketWriter writer)
        {
            //TODO: Implement
        }
    }

    public class WarPlayerInfo
    {
        public string Name;
        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public int Unknown5;
        public int Unknown6;
        public int Unknown7;
        public int Unknown8;
        public int Unknown9;
        public int Unknown10;
        public int Unknown11;
        public int Unknown12;
        public int Unknown13;
        public int Unknown14;
        public int Unknown15;
        public bool Unknown16;
        public bool Unknown17;
        public bool Unknown18;
        public int Unknown19;
        public int Unknown20;
        public int Unknown21;
        public int Unknown22;
        public int Unknown23;
        public int Unknown24;
        public int Unknown25;
        public int AllianceCastleUnitCapacity;
        public string RequestText;
    }
}
