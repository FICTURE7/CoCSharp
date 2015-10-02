using CoCSharp.Data;
using CoCSharp.Networking;
using Ionic.Zlib;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace CoCSharp.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public class Village
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Village"/> class.
        /// </summary>
        public Village()
        {
            Traps = new List<Trap>();
            Buildings = new List<Building>();
            Obstacles = new List<Obstacle>();
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
        [JsonProperty("respawnVars")]
        public RespawnVars RespawnVars { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public string RawJson { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        public void FromJson(string json)
        {
            //TODO: Make this method static.
            var village = JsonConvert.DeserializeObject<Village>(json);
            Buildings = village.Buildings;
            Obstacles = village.Obstacles;
            Traps = village.Traps;
            Decorations = village.Decorations;
            RespawnVars = village.RespawnVars;
            RawJson = json;

            for (int i = 0; i < Buildings.Count; i++)
                Buildings[i].Village = this;

            for (int i = 0; i < Obstacles.Count; i++)
                Obstacles[i].Village = this;

            for (int i = 0; i < Traps.Count; i++)
                Traps[i].Village = this;

            for (int i = 0; i < Decorations.Count; i++)
                Decorations[i].Village = this;
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
        public void Read(PacketReader reader)
        {
            var homeData = reader.ReadByteArray();
            if (homeData == null)
                return;
            using (var binaryReader = new BinaryReader(new MemoryStream(homeData)))
            {
                var decompressedLength = binaryReader.ReadInt32();
                var compressedJson = binaryReader.ReadBytes(homeData.Length - 4);
                var json = ZlibStream.UncompressString(compressedJson);
                if (decompressedLength != json.Length)
                    if (decompressedLength - 1 != json.Length) // to prevent for running in error
                        throw new InvalidDataException(string.Format("Json length is not valid. {0} != {1}.", decompressedLength, json.Length));
                FromJson(json);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public void Write(PacketWriter writer)
        {
            var json = ToJson();
            var decompressedLength = json.Length;
            var compressedJson = ZlibStream.CompressString(json);
            using (var binaryWriter = new BinaryWriter(new MemoryStream()))
            {
                binaryWriter.Write(decompressedLength);
                binaryWriter.Write(compressedJson);

                var homeData = ((MemoryStream)binaryWriter.BaseStream).ToArray();
                writer.Write(homeData, 0, homeData.Length);
            }
        }
    }
}
