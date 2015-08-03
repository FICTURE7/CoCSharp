using CoCSharp.Data;
using CoCSharp.Networking;
using Ionic.Zlib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CoCSharp.Logic
{
    /// <summary>
    /// 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Village
    {
        /// <summary>
        /// 
        /// </summary>
        public Village()
        {
            Buildings = new List<Building>();
            Obstacles = new List<Obstacle>();
            Traps = new List<Trap>();
            Decorations = new List<Decoration>();
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("buildings")]
        public List<Building> Buildings { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("obstacles")]
        public List<Obstacle> Obstacles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("traps")]
        public List<Trap> Traps { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("decos")]
        public List<Decoration> Decorations { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RawJson { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        public void FromJson(string json)
        {
            var village = JsonConvert.DeserializeObject<Village>(json);
            if (village == null) 
                throw new InvalidOperationException("Invalid village Json string.");

            Buildings = village.Buildings;
            Obstacles = village.Obstacles;
            Traps = village.Traps;
            Decorations = village.Decorations;
            RawJson = json;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public void ReadFromPacketReader(PacketReader reader)
        {
            var homeData = reader.ReadByteArray();
            var binaryReader = new BinaryReader(new MemoryStream(homeData));
            
            var decompressedLength = binaryReader.ReadInt32();
            var compressedJson = binaryReader.ReadBytes(homeData.Length - 4);
            var json = ZlibStream.UncompressString(compressedJson);
            
            if (decompressedLength != json.Length) throw new InvalidDataException("Json length is not valid.");
            
            FromJson(json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToPacketWriter(PacketWriter writer)
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
