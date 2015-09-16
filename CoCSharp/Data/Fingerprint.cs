using Newtonsoft.Json;
using System;

namespace CoCSharp.Data
{
    /// <summary>
    /// Object version of "fingerprint.json" file.
    /// </summary>
    public class Fingerprint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Fingerprint"/> class.
        /// </summary>
        public Fingerprint()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fingerprint"/> class with
        /// the specified JSON string.
        /// </summary>
        /// <param name="json">JSON string.</param>
        public Fingerprint(string json)
        {
            FromJson(json);
        }

        /// <summary>
        /// Gets or sets an array of <see cref="Fingerprint.FingerprintFile"/> of
        /// the fingerprint.
        /// </summary>
        [JsonProperty("files")]
        public FingerprintFile[] Files { get; set; }

        /// <summary>
        /// Gets or sets the hash of the fingerprint.
        /// </summary>
        [JsonProperty("sha")]
        public string Hash { get; set; }

        /// <summary>
        /// Gets or sets the version of the fingerprint.
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        /// Deserialize this <see cref="Fingerprint"/> object from a JSON string.
        /// </summary>
        /// <param name="json">JSON string.</param>
        public void FromJson(string json)
        {
            var fingerprint = JsonConvert.DeserializeObject<Fingerprint>(json);
            Files = fingerprint.Files;
            Hash = fingerprint.Hash;
            Version = fingerprint.Version;
        }

        /// <summary>
        /// Serialize this <see cref="Fingerprint"/> object to a JSON string.
        /// </summary>
        /// <returns>Serializes JSON string.</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Fingerprint Create(string path)
        {
            /* Should calculate hashes and find the files
             * to make a fingerprint.json.
             */
            throw new NotImplementedException();
        }

        public class FingerprintFile
        {
            [JsonProperty("sha")]
            public string Hash { get; set; }

            [JsonProperty("file")]
            public string Path { get; set; }
        }
    }
}
