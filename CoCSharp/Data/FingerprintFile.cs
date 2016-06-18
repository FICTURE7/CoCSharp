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
        public byte[] Hash { get; set; }

        /// <summary>
        /// Gets or sets the path of the file.
        /// </summary>
        public string Path { get; set; }
    }
}
