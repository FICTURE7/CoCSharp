using System;
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
                log.EnemyClanStartsWon = reader.ReadInt32();

                log.HomeClanPercentage = (float)reader.ReadInt32() / 10;
                log.EnemyClanPercentage = (float)reader.ReadInt32() / 10;

                log.Unknown1 = reader.ReadInt32();
                log.Unknown2 = reader.ReadInt32();
                log.Unknown3 = reader.ReadInt32();

                log.HomeClanPointsGained = reader.ReadInt32();

                log.Unknown5 = reader.ReadInt64();
                log.Unknown6 = reader.ReadInt32();
                log.Unknown7 = reader.ReadInt32();
                log.Unknown8 = reader.ReadInt32(); // time since then?
                log.Unknown9 = reader.ReadInt32();
                log.Unknown10 = reader.ReadInt32();
                log.Unknown11 = reader.ReadByte();
                WarEntries.Add(log);
            }
        }

        public void WritePacket(PacketWriter writer)
        {

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
            public int EnemyClanStartsWon { get; set; }

            public float HomeClanPercentage { get; set; } // readint / 10 (to 2 decimal place)
            public float EnemyClanPercentage { get; set; } // readint / 10

            public int Unknown1 { get; set; } // number of home clan attacks?
            public int Unknown2 { get; set; } 
            public int Unknown3 { get; set; }

            public int HomeClanPointsGained { get; set; }

            public long Unknown5 { get; set; } // feels like an id
            public int Unknown6 { get; set; }
            public int Unknown7 { get; set; }
            public int Unknown8 { get; set; }
            public int Unknown9 { get; set; } 
            public int Unknown10 { get; set; }
            public byte Unknown11 { get; set; }
        }
    }
}
