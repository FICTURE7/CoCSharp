using CoCSharp.Logic;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCSharp.Networking.Packets
{
    public class AllianceListResponsePacket : IPacket
    {
        public ushort ID
        { get { return 0x5EF6; } }

        public int SearchStringLength;
        public string SearchString;
        public int ClansFound;
        public List<Clan> Clans;

        public AllianceListResponsePacket()
        {
            Clans = new List<Clan>();
        }

        public void ReadPacket(PacketReader reader)
        {
            SearchStringLength = reader.ReadInt32();
            SearchString = Encoding.UTF8.GetString(reader.ReadBytes(SearchStringLength), 0, SearchStringLength);
            ClansFound = reader.ReadInt32();

            for (int i = 0; i < ClansFound - 1; i++)
            {
                Clan clan = new Clan();
                clan.ID = reader.ReadInt64();
                var nameLength = reader.ReadInt32();
                clan.Name = Encoding.UTF8.GetString(reader.ReadBytes(nameLength), 0, nameLength);
                clan.Unknown1 = reader.ReadInt32();
                clan.Type = reader.ReadInt32();
                clan.Members = reader.ReadInt32();
                clan.Trophies = reader.ReadInt32();
                clan.Unknown2 = reader.ReadInt32();
                clan.WarsWin = reader.ReadInt32();
                clan.WarsLost = reader.ReadInt32();
                clan.WarsDraw = reader.ReadInt32();
                clan.Badge = reader.ReadInt32();
                clan.Unknown3 = reader.ReadInt32();
                clan.Unknown4 = reader.ReadInt32();
                clan.EP = reader.ReadInt32();
                clan.Level = reader.ReadInt32();
                Clans.Add(clan);
            }
        }

        public void WritePacket(PacketWriter writer)
        {
            var x = writer;

        }
    }
}
