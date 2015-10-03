using System.Collections.Generic;

namespace CoCSharp.Networking.Packets
{
    public class AllianceWarLogPacket : IPacket
    {
        public ushort ID { get { return 0x5F12; } }

        public List<WarLogEntry> WarEntries;

        public void ReadPacket(PacketReader reader)
        {
            WarEntries = new List<WarLogEntry>();
            var warCount = reader.ReadInt32();
            for (int i = 0; i < warCount; i++)
            {
                var log = new WarLogEntry();
                log.HomeClanID = reader.ReadInt64();
                log.HomeClanName = reader.ReadString();
                log.HomeClanBadge = reader.ReadInt32();
                log.HomeClanLevel = reader.ReadInt32();

                log.EnemyClanID = reader.ReadInt64();
                log.EnemyClanName = reader.ReadString();
                log.EnemyClanBadge = reader.ReadInt32();
                log.EnemyClanLevel = reader.ReadInt32();

                log.HomeClanStarsWon = reader.ReadInt32();
                log.EnemyClanStarsWon = reader.ReadInt32();

                log.HomeClanPercentage = (float)reader.ReadInt32() / 10;
                log.EnemyClanPercentage = (float)reader.ReadInt32() / 10;

                log.Unknown1 = reader.ReadInt32();
                log.Unknown2 = reader.ReadInt32();
                log.Unknown3 = reader.ReadInt32();

                log.HomeClanPointsGained = reader.ReadInt32();

                log.Unknown4 = reader.ReadInt64();
                log.Unknown5 = reader.ReadInt32();
                log.Unknown6 = reader.ReadInt32();
                log.Unknown7 = reader.ReadInt32(); // time since then?
                log.Unknown8 = reader.ReadInt32();
                log.Unknown9 = reader.ReadInt32();
                log.Unknown10 = reader.ReadByte();
                WarEntries.Add(log);
            }
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteInt32(WarEntries.Count);
            for (int i = 0; i < WarEntries.Count; i++)
            {
                var log = WarEntries[i];
                writer.WriteInt64(log.HomeClanID);
                writer.WriteString(log.HomeClanName);
                writer.WriteInt32(log.HomeClanBadge);
                writer.WriteInt32(log.HomeClanLevel);

                writer.WriteInt64(log.EnemyClanID);
                writer.WriteString(log.EnemyClanName);
                writer.WriteInt32(log.EnemyClanBadge);
                writer.WriteInt32(log.EnemyClanLevel);

                writer.WriteInt32(log.HomeClanStarsWon);
                writer.WriteInt32(log.EnemyClanStarsWon);

                writer.WriteInt32((int)log.HomeClanPercentage * 10); // TODO: Find a more consistent solution.
                writer.WriteInt32((int)log.EnemyClanPercentage * 10);

                writer.WriteInt32(log.Unknown1);
                writer.WriteInt32(log.Unknown2);
                writer.WriteInt32(log.Unknown3);

                writer.WriteInt32(log.HomeClanPointsGained);

                writer.WriteInt64(log.Unknown4);
                writer.WriteInt32(log.Unknown5);
                writer.WriteInt32(log.Unknown6);
                writer.WriteInt32(log.Unknown7);
                writer.WriteInt32(log.Unknown8);
                writer.WriteInt32(log.Unknown9);
                writer.WriteByte(log.Unknown10);
            }
        }

        public class WarLogEntry
        {
            public long HomeClanID { get; set; } // not sure about its feel likes some sort of id
            public string HomeClanName { get; set; }
            public int HomeClanBadge { get; set; } // not sure about this
            public int HomeClanLevel { get; set; }

            public long EnemyClanID { get; set; } // not sure about its feel likes some sort of id
            public string EnemyClanName { get; set; }
            public int EnemyClanBadge { get; set; } // not sure about this
            public int EnemyClanLevel { get; set; }

            public int HomeClanStarsWon { get; set; }
            public int EnemyClanStarsWon { get; set; }

            public float HomeClanPercentage { get; set; } // readint / 10 (to 2 decimal place)
            public float EnemyClanPercentage { get; set; } // readint / 10

            public int Unknown1 { get; set; } // number of home clan attacks?
            public int Unknown2 { get; set; } 
            public int Unknown3 { get; set; }

            public int HomeClanPointsGained { get; set; }

            public long Unknown4 { get; set; } // feels like an id
            public int Unknown5 { get; set; }
            public int Unknown6 { get; set; }
            public int Unknown7 { get; set; }
            public int Unknown8 { get; set; } 
            public int Unknown9 { get; set; }
            public byte Unknown10 { get; set; }
        }
    }
}
