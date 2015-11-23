using System.Collections.Generic;

namespace CoCSharp.Networking.Packets
{
    public class LastClanWarDataPacket : IPacket
    {
        public ushort ID { get { return 0x5F0F; } }

        public int ClanWarStage; // 4 - Preparation Day, 5 - Battle Day, 6 - War Ended
        public int TimeLeftSeconds;
        public Clan Home;
        public Clan Enemy;
        public int Unknown1;
        public int Unknown2;
        public int WarEventCount;
        public List<WarEventEntry> WarEvents;

        public void ReadPacket(PacketReader reader)
        {
            ClanWarStage = reader.ReadInt32();
            TimeLeftSeconds = reader.ReadInt32();
            Home = new Clan();
            Enemy = new Clan();
            Home.Read(reader);
            Enemy.Read(reader);

            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadInt32();
            WarEventCount = reader.ReadInt32();
            WarEvents = new List<WarEventEntry>();
            for (int i = 0; i < WarEventCount; i++)
            {
                var entry = new WarEventEntry();
                entry.Unknown1 = reader.ReadInt32();
                entry.Unknown2 = reader.ReadInt32();
                entry.ReplayID = reader.ReadInt32();
                entry.TimeLeftSeconds = reader.ReadInt32();
                entry.AttackerClanID = reader.ReadInt64();
                entry.AttackerID = reader.ReadInt64();
                entry.DefenderClanID = reader.ReadInt64();
                entry.DefenderID = reader.ReadInt64();
                entry.AttackerName = reader.ReadString();
                entry.DefenderName = reader.ReadString();
                entry.StarsWon = reader.ReadInt32();
                entry.StarsEarned = reader.ReadInt32();
                entry.Damage = reader.ReadInt32();

                entry.Unknown3 = reader.ReadInt32();
                entry.Unknown4 = reader.ReadInt32();
                entry.Unknown5 = reader.ReadInt32();
                entry.Unknown6 = reader.ReadInt32();
                entry.Unknown7 = reader.ReadByte();
                entry.Unknown8 = reader.ReadInt32();
                entry.Unknown9 = reader.ReadInt32();
                entry.Unknown10 = reader.ReadInt32();
                entry.Unknown11 = reader.ReadInt32();
                WarEvents.Add(entry);
            }
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteInt32(ClanWarStage);
            writer.WriteInt32(TimeLeftSeconds);
            Home.Write(writer);
            Enemy.Write(writer);

            writer.WriteInt32(Unknown1);
            writer.WriteInt32(Unknown2);
            writer.WriteInt32(WarEventCount);
            for (int i = 0; i < WarEventCount; i++)
            {
                var entry = WarEvents[i];
                writer.WriteInt32(entry.Unknown1);
                writer.WriteInt32(entry.Unknown2);
                writer.WriteInt32(entry.ReplayID);
                writer.WriteInt32(entry.TimeLeftSeconds);
                writer.WriteInt64(entry.AttackerClanID);
                writer.WriteInt64(entry.AttackerID);
                writer.WriteInt64(entry.DefenderClanID);
                writer.WriteInt64(entry.DefenderID);
                writer.WriteString(entry.AttackerName);
                writer.WriteString(entry.DefenderName);
                writer.WriteInt32(entry.StarsWon);
                writer.WriteInt32(entry.StarsEarned);
                writer.WriteInt32(entry.Damage);

                writer.WriteInt32(entry.Unknown3);
                writer.WriteInt32(entry.Unknown4);
                writer.WriteInt32(entry.Unknown5);
                writer.WriteInt32(entry.Unknown6);
                writer.WriteByte(entry.Unknown7);
                writer.WriteInt32(entry.Unknown8);
                writer.WriteInt32(entry.Unknown9);
                writer.WriteInt32(entry.Unknown10);
                writer.WriteInt32(entry.Unknown11);
            }
        }

        public class Clan
        {
            public long ClanID { get; set; }
            public string ClanName { get; set; }
            public int ClanBadge { get; set; }
            public int ClanLevel { get; set; }
            public int PlayerCount { get; set; }
            public List<Player> Roster;

            public void Read(PacketReader reader)
            {
                ClanID = reader.ReadInt64();
                ClanName = reader.ReadString();
                ClanBadge = reader.ReadInt32();
                ClanLevel = reader.ReadInt32();

                PlayerCount = reader.ReadInt32();
                Roster = new List<Player>();
                for (int i = 0; i < PlayerCount; i++)
                {
                    var player = new Player();
                    player.ClanID = reader.ReadInt64();
                    player.ID1 = reader.ReadInt64();
                    player.ID2 = reader.ReadInt64();
                    player.Name = reader.ReadString();
                    player.StarsGivenUp = reader.ReadInt32();
                    player.Damage = reader.ReadInt32();
                    player.Unknown1 = reader.ReadInt32();
                    player.AttacksUsed = reader.ReadInt32();
                    player.TotalDefenses = reader.ReadInt32();
                    player.GoldGained = reader.ReadInt32();
                    player.ElixirGained = reader.ReadInt32();
                    player.DarkElixirGained = reader.ReadInt32();
                    player.GoldAvailable = reader.ReadInt32();
                    player.ElixirAvailable = reader.ReadInt32();
                    player.DarkElixirAvailable = reader.ReadInt32();
                    player.OffenseWeight = reader.ReadInt32();
                    player.DefenseWeight = reader.ReadInt32();
                    player.Unknown2 = reader.ReadInt32();
                    player.TownHall = reader.ReadInt32();

                    player.Unknown3 = reader.ReadByte();
                    player.Unknown4 = reader.ReadInt32();
                    player.Unknown5 = reader.ReadInt32();
                    player.Unknown6 = reader.ReadInt32();

                    player.IsBattleDay = reader.ReadBoolean();
                    if (player.IsBattleDay)
                    {
                        player.Unknown7 = reader.ReadInt32();
                        player.Unknown8 = reader.ReadInt32();

                        player.HasBeenAttacked = reader.ReadBoolean();
                        if (player.HasBeenAttacked)
                        {
                            player.Unknown9 = reader.ReadInt32();
                            player.BestAttackReplayID = reader.ReadInt32();
                        }
                    }

                    player.Unknown10 = reader.ReadInt32();

                    player.ClanCastle = reader.ReadInt32();
                    player.TroopCapacity = reader.ReadInt32();
                    player.RequestMessage = reader.ReadString();
                    player.TroopCount = reader.ReadInt32();

                    List<long> troops = new List<long>();
                    int sizeJ = reader.ReadInt32();
                    troops.Add(sizeJ);
                    for (int j = 0; j < sizeJ; j++)
                    {
                        troops.Add(reader.ReadInt64());
                        int sizeK = reader.ReadInt32();
                        troops.Add(sizeK);
                        for (int k = 0; k < sizeK; k++)
                            troops.Add(reader.ReadInt64());
                    }
                    player.TroopsInside = troops;

                    Roster.Add(player);
                }

                // Some kind of padding
                reader.ReadByte();
                var length = reader.ReadInt32();
                for (int i = 0; i < length + 5; i++)
                    reader.ReadInt32();
                reader.ReadByte();
            }

            public void Write(PacketWriter writer)
            {
                writer.WriteInt64(ClanID);
                writer.WriteString(ClanName);
                writer.WriteInt32(ClanBadge);
                writer.WriteInt32(ClanLevel);

                writer.WriteInt32(PlayerCount);
                for (int i = 0; i < PlayerCount; i++)
                {
                    var player = Roster[i];
                    writer.WriteInt64(player.ClanID);
                    writer.WriteInt64(player.ID1);
                    writer.WriteInt64(player.ID2);
                    writer.WriteString(player.Name);
                    writer.WriteInt32(player.StarsGivenUp);
                    writer.WriteInt32(player.Damage);
                    writer.WriteInt32(player.Unknown1);
                    writer.WriteInt32(player.AttacksUsed);
                    writer.WriteInt32(player.TotalDefenses);
                    writer.WriteInt32(player.GoldGained);
                    writer.WriteInt32(player.ElixirGained);
                    writer.WriteInt32(player.DarkElixirGained);
                    writer.WriteInt32(player.GoldAvailable);
                    writer.WriteInt32(player.ElixirAvailable);
                    writer.WriteInt32(player.DarkElixirAvailable);
                    writer.WriteInt32(player.OffenseWeight);
                    writer.WriteInt32(player.DefenseWeight);
                    writer.WriteInt32(player.Unknown2);
                    writer.WriteInt32(player.TownHall);

                    writer.WriteByte(player.Unknown3);
                    writer.WriteInt32(player.Unknown4);
                    writer.WriteInt32(player.Unknown5);
                    writer.WriteInt32(player.Unknown6);

                    writer.WriteBoolean(player.IsBattleDay);
                    if (player.IsBattleDay)
                    {
                        writer.WriteInt32(player.Unknown7);
                        writer.WriteInt32(player.Unknown8);

                        writer.WriteBoolean(player.HasBeenAttacked);
                        if (player.HasBeenAttacked)
                        {
                            writer.WriteInt32(player.Unknown9);
                            writer.WriteInt32(player.BestAttackReplayID);
                        }
                    }

                    writer.WriteInt32(player.Unknown10);

                    writer.WriteInt32(player.ClanCastle);
                    writer.WriteInt32(player.TroopCapacity);
                    writer.WriteString(player.RequestMessage);
                    writer.WriteInt32(player.TroopCount);

                    int listIdx = 0;
                    int sizeJ = (int)player.TroopsInside[listIdx++];
                    writer.WriteInt32(sizeJ);
                    for (int j = 0; j < sizeJ; j++)
                    {
                        writer.WriteInt64(player.TroopsInside[listIdx++]);
                        int sizeK = (int)player.TroopsInside[listIdx++];
                        writer.WriteInt32(sizeK);
                        for (int k = 0; k < sizeK; k++)
                            writer.WriteInt64(player.TroopsInside[listIdx++]);
                    }
                }
            }

            public class Player
            {
                public long ClanID { get; set; }
                public long ID1 { get; set; }
                public long ID2 { get; set; }
                public string Name { get; set; }
                public int StarsGivenUp { get; set; }
                public int Damage { get; set; }
                public int Unknown1 { get; set; }
                public int AttacksUsed { get; set; }
                public int TotalDefenses { get; set; }
                public int GoldGained { get; set; } // Loot gained
                public int ElixirGained { get; set; } // Loot gained
                public int DarkElixirGained { get; set; } // Loot gained
                public int GoldAvailable { get; set; } // Available loot
                public int ElixirAvailable { get; set; } // Available loot
                public int DarkElixirAvailable { get; set; } // Available loot
                public int OffenseWeight { get; set; }
                public int DefenseWeight { get; set; }
                public int Unknown2 { get; set; }
                public int TownHall { get; set; } // Town Hall level minus 1

                public byte Unknown3 { get; set; }
                public int Unknown4 { get; set; }
                public int Unknown5 { get; set; }
                public int Unknown6 { get; set; }

                public bool IsBattleDay { get; set; } // Not entirely sure
                public int Unknown7 { get; set; }
                public int Unknown8 { get; set; }

                public bool HasBeenAttacked { get; set; }
                public int Unknown9 { get; set; }
                public int BestAttackReplayID { get; set; } // Possibly int64 with Unknown9

                public int Unknown10 { get; set; }

                public int ClanCastle { get; set; } // Clan Castle level minus 1
                public int TroopCapacity { get; set; }
                public string RequestMessage { get; set; }
                public int TroopCount { get; set; }

                public List<long> TroopsInside;
            }
        }

        public class WarEventEntry
        {
            public int Unknown1 { get; set; }
            public int Unknown2 { get; set; }
            public int ReplayID { get; set; } // Possibly int64 with Unknown2
            public int TimeLeftSeconds { get; set; }
            public long AttackerClanID { get; set; }
            public long AttackerID { get; set; }
            public long DefenderClanID { get; set; }
            public long DefenderID { get; set; }
            public string AttackerName { get; set; }
            public string DefenderName { get; set; }
            public int StarsWon { get; set; }
            public int StarsEarned { get; set; }
            public int Damage { get; set; }

            public int Unknown3 { get; set; }
            public int Unknown4 { get; set; }
            public int Unknown5 { get; set; }
            public int Unknown6 { get; set; }

            public byte Unknown7 { get; set; }
            public int Unknown8 { get; set; }
            public int Unknown9 { get; set; }
            public int Unknown10 { get; set; }
            public int Unknown11 { get; set; }
        }
    }
}
