using Newtonsoft.Json;

namespace CoCSharp.Data
{
    public class Fingerprint
    {
        public Fingerprint()
        { }

        [JsonProperty("files")]
        public FingerprintFile[] Files { get; set; }

        [JsonProperty("sha")]
        public string Hash { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        public class FingerprintFile
        {
            [JsonProperty("sha")]
            public string Hash { get; set; }

            [JsonProperty("file")]
            public string Path { get; set; }
        }
    }
}
