using Newtonsoft.Json;

namespace CoCSharp.Data
{
    /// <summary>
    /// Represents a file's fingerprint in a <see cref="Fingerprint"/>.
    /// </summary>
    public class FingerprintFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FingerprintFile"/> class.
        /// </summary>
        public FingerprintFile()
        {
            // Space
        }

        /// <summary>
        /// Gets or sets the SHA-1 hash of the file.
        /// </summary>
        [JsonProperty("sha")]
        public string Hash { get; set; }

        /// <summary>
        /// Gets or sets the path of the file.
        /// </summary>
        [JsonProperty("file")]
        public string Path { get; set; }
    }
}
