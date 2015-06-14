using CoCSharp.Networking;
using Ionic.Zlib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CoCSharp.Logic
{
    public class Village
    {
        public Village()
        {
            this.Buildings = new List<Building>();
            this.Obstacles = new List<Obstacle>();
            this.Traps = new List<Trap>();
            this.Decorations = new List<Decoration>();
        }

        [JsonProperty("buildings")]
        public List<Building> Buildings { get; set; } // should be lists buts its ok for now

        [JsonProperty("obstacles")]
        public List<Obstacle> Obstacles { get; set; }

        [JsonProperty("traps")]
        public List<Trap> Traps { get; set; }

        [JsonProperty("decos")]
        public List<Decoration> Decorations { get; set; }

        [JsonIgnore()]
        public string RawJson { get; set; }

        public void FromJson(string json)
        {
            var village = JsonConvert.DeserializeObject<Village>(json);
            if (village == null) throw new InvalidOperationException("Invalid village Json string.");

            Buildings = village.Buildings;
            Obstacles = village.Obstacles;
            Traps = village.Traps;
            Decorations = village.Decorations;
            RawJson = json;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void FromPacketReader(PacketReader reader)
        {
            var homeData = reader.ReadByteArray();
            var jsonData = new byte[homeData.Length - 4]; // skipping Decompressed data length
            Buffer.BlockCopy(homeData, 4, jsonData, 0, homeData.Length - 4);
            var homeJson = ZlibStream.UncompressString(jsonData);
            FromJson(homeJson);
        }

        public void ToPacketWriter(PacketWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
