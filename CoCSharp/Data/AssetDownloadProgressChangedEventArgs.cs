using System;

namespace CoCSharp.Data
{
    /// <summary>
    /// Provides data for <see cref="AssetDownloader.DownloadProgressChanged"/> event.
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
        /// Gets or sets the next <see cref="FingerprintFile"/> to download.
        /// </summary>
        public FingerprintFile NextDownload { get; set; }

        /// <summary>
        /// Gets or sets the percentage progress made based on downloaded count per total downloads.
        /// </summary>
        public double ProgressPercentage { get; set; }

        /// <summary>
        /// Gets or sets the amount of files downloaded.
        /// </summary>
        public int DownloadedCount { get; set; }
    }
}
