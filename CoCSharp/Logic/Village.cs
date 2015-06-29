using CoCSharp.Networking;
using Ionic.Zlib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CoCSharp.Logic
{
    [JsonObject(MemberSerialization.OptIn)]
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
        public List<Building> Buildings { get; set; }
        [JsonProperty("obstacles")]
        public List<Obstacle> Obstacles { get; set; }
        [JsonProperty("traps")]
        public List<Trap> Traps { get; set; }
        [JsonProperty("decos")]
        public List<Decoration> Decorations { get; set; }

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
            var binaryReader = new BinaryReader(new MemoryStream(homeData));
            
            var decompressedLength = binaryReader.ReadInt32();
            var compressedJson = binaryReader.ReadBytes(homeData.Length - 4);
            var json = ZlibStream.UncompressString(compressedJson);
            
            if (decompressedLength != json.Length) throw new InvalidDataException("Json length is not valid.");
            
            FromJson(json);
        }

        public void ToPacketWriter(PacketWriter writer)
        {
            var json = ToJson();
            var decompressedLength = json.Length;
            var compressedJson = ZlibStream.CompressString(json);
            var binaryWriter = new BinaryWriter(new MemoryStream());
            
            binaryWriter.Write(decompressedLength);
            binaryWriter.Write(compressedJson);

            var homeData = ((MemoryStream)binaryWriter.BaseStream).ToArray();
            writer.Write(homeData, 0, homeData.Length);
        }
    }
}
