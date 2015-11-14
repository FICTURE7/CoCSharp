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

                log.HomeClanPercentage = (float)reader.ReadInt32() / 20;
                log.EnemyClanPercentage = (float)reader.ReadInt32() / 20;

                log.Unknown1 = reader.ReadInt32();
                log.Unknown2 = reader.ReadInt32();
                log.HomeAttacksUsed = reader.ReadInt32();

                log.HomeClanPointsGained = reader.ReadInt32();

                log.Unknown3 = reader.ReadInt64();
                log.WarSize = reader.ReadInt32();
                log.WarResult = reader.ReadInt32();
                log.Unknown4 = reader.ReadInt32(); // time since then?
                log.Unknown5 = reader.ReadInt32();
                log.Unknown6 = reader.ReadInt32();
                log.Unknown7 = reader.ReadByte();
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

                writer.WriteInt32((int)log.HomeClanPercentage * 20); // TODO: Find a more consistent solution.
                writer.WriteInt32((int)log.EnemyClanPercentage * 20);

                writer.WriteInt32(log.Unknown1);
                writer.WriteInt32(log.Unknown2);
                writer.WriteInt32(log.HomeAttacksUsed);

                writer.WriteInt32(log.HomeClanPointsGained);

                writer.WriteInt64(log.Unknown3);
                writer.WriteInt32(log.WarSize);
                writer.WriteInt32(log.WarResult);
                writer.WriteInt32(log.Unknown4);
                writer.WriteInt32(log.Unknown5);
                writer.WriteInt32(log.Unknown6);
                writer.WriteByte(log.Unknown7);
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

            public float HomeClanPercentage { get; set; } // readint / 20 (to 2 decimal place)
            public float EnemyClanPercentage { get; set; } // readint / 20

            public int Unknown1 { get; set; }
            public int Unknown2 { get; set; }
            public int HomeAttacksUsed { get; set; }

            public int HomeClanPointsGained { get; set; }

            public long Unknown3 { get; set; } // feels like an id
            public int WarSize { get; set; }
            public int WarResult { get; set; } // 0 = lose, 1 = win
            public int Unknown4 { get; set; }
            public int Unknown5 { get; set; }
            public int Unknown6 { get; set; }
            public byte Unknown7 { get; set; }
        }
    }
}