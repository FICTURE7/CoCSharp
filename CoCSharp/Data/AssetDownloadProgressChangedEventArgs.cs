using System;

namespace CoCSharp.Data
{
    /// <summary>
    /// Provides data for <see cref="Fingerprint.DownloadProgressChanged"/> event. 
    /// Long name I know, if you have a better suggestion create an issue for it. ;]
    /// </summary>
    public class AssetDownloadProgressChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetDownloadProgressChangedEventArgs"/> class.
        /// </summary>
        public AssetDownloadProgressChangedEventArgs()
        {
            // Space           
        }

        /// <summary>
        /// Gets or sets the <see cref="FingerprintFile"/> downloaded.
        /// </summary>
        public FingerprintFile FileDownloaded { get; set; }

        /// <summary>
        /// Gets or sets the progress percentage.
        /// </summary>
        public double ProgressPercentage { get; set; }

        /// <summary>
        /// Gets or sets the amount of files downloaded.
        /// </summary>
        public int DownloadedCount { get; set; }

        /// <summary>
        /// Gets or sets the amount of file to download.
        /// </summary>
        public int DownloadsCount { get; set; } //TODO: Find a better name.

        /// <summary>
        /// Gets or sets the amount of download retried.
        /// </summary>
        public int RetryCount { get; set; }
    }
}
