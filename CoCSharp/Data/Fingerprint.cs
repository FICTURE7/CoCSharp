using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CoCSharp.Data
{
    /// <summary>
    /// Represents a Clash of Clans fingerprint.
    /// </summary>
    public class Fingerprint
    {
        /// <summary>
        /// Default server from which to download the assets from fingerprints. This field is constant.
        /// </summary>
        public const string DefaultAssetServer = "http://b46f744d64acd2191eda-3720c0374d47e9a0dd52be4d281c260f.r11.cf2.rackcdn.com/";

        /// <summary>
        /// Initializes a new instance of the <see cref="Fingerprint"/> class.
        /// </summary>
        public Fingerprint()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fingerprint"/> class
        /// from the specified path to the fingerprint.json.
        /// </summary>
        /// <param name="path">Path to fingerprint.json</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="path"/>'s length is 0.</exception>
        /// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
        public Fingerprint(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (path.Length == 0)
                throw new ArgumentException("Empty path is not valid.");
            if (!File.Exists(path))
                throw new FileNotFoundException("Could not find file '" + Path.GetFullPath(path) + "'.");

            var json = File.ReadAllText(path);
            var fingerprint = FromJson(json);
            Files = fingerprint.Files;
            Hash = fingerprint.Hash;
            Version = fingerprint.Version;
        }

        /// <summary>
        /// Gets or sets the of array of <see cref="FingerprintFile"/>.
        /// </summary>
        public FingerprintFile[] Files { get; set; }

        /// <summary>
        /// Gets or sets the SHA-1 hash of the <see cref="Fingerprint"/>.
        /// </summary>
        [JsonProperty("sha")]
        public string Hash { get; set; }

        /// <summary>
        /// Gets or sets the version of the <see cref="Fingerprint"/>.
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        /// Returns a JSON string that represents the current <see cref="Fingerprint"/>.
        /// </summary>
        /// <returns>A JSON string that represents the current <see cref="Fingerprint"/>.</returns>
        public string ToJson()
        {
            return ToJson(false);
        }

        /// <summary>
        /// Returns a JSON string and indented if specified that represents the current <see cref="Fingerprint"/>.
        /// </summary>
        /// <param name="indent">If set to <c>true</c> the returned JSON string will be indented.</param>
        /// <returns>A JSON string and indented if specified that represents the current <see cref="Fingerprint"/>.</returns>
        public string ToJson(bool indent)
        {
            return indent == true ? JsonConvert.SerializeObject(this, Formatting.Indented) : JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Returns a <see cref="Fingerprint"/> that will be deserialize from the specified
        /// JSON string.
        /// </summary>
        /// <param name="value">JSON string that represents the <see cref="Fingerprint"/>.</param>
        /// <returns>A <see cref="Fingerprint"/> that is deserialized from the specified JSON string.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/>'s length is 0.</exception>
        public static Fingerprint FromJson(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (value.Length == 0)
                throw new ArgumentException("Empty json value is not valid.");

            var fingerprint = JsonConvert.DeserializeObject<Fingerprint>(value);
            return fingerprint;
        }

        //TODO: Move this into a whole new class.
        #region Download Methods

        /// <summary>
        /// Downloads files in the <see cref="Files"/> array at the
        /// specified path from the <see cref="DefaultAssetServer"/>.
        /// </summary>
        /// <param name="path">Path where to save the downloads.</param>
        public void DownloadFiles(string path)
        {
            DownloadFiles(path, DefaultAssetServer, false);
        }

        /// <summary>
        /// Downloads files in the <see cref="Files"/> array at the
        /// specified path from the specified server URL.
        /// </summary>
        /// <param name="path">Path where to save the downloads.</param>
        /// <param name="serverUrl">URL of where to download the files.</param>
        public void DownloadFiles(string path, string serverUrl)
        {
            DownloadFiles(path, serverUrl, false);
        }

        /// <summary>
        /// Downloads files in the <see cref="Files"/> array at the
        /// specified path from the specified server URL and check SHA-1 if <paramref name="checksum"/> is set to <c>true</c>.
        /// </summary>
        /// <param name="path">Path where to save the downloads.</param>
        /// <param name="serverUrl">URL of where to download the files.</param>
        /// <param name="checksum">
        /// If set to <c>true</c> then it will calculate the SHA-1 of the files downloaded
        /// and redownload the file if the hash incorrect.
        /// </param>
        public void DownloadFiles(string path, string serverUrl, bool checksum)
        {
            var retryCount = 0;
            var root = Path.Combine(serverUrl, Hash);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // Kinda silly to create an instance if you might not gonna use it.
            using (var sha1 = SHA1.Create())
            {
                using (var webClient = new WebClient())
                {
                    for (int i = 0; i < Files.Length; i++)
                    {
                        var fingerprintFile = Files[i];
                        var downloadUrl = Path.Combine(root, fingerprintFile.Path);
                        var fileBytes = webClient.DownloadData(downloadUrl);

                        if (checksum)
                        {
                            var fileSha = Utils.BytesToString(sha1.ComputeHash(fileBytes));

                            // Restart download if hash check failed.
                            if (fileSha != fingerprintFile.Hash)
                            {
                                retryCount++;
                                i--;
                                continue;
                            }
                        }

                        var savePath = Path.Combine(path, fingerprintFile.Path);
                        var saveDirectory = Path.GetDirectoryName(savePath);

                        if (!Directory.Exists(saveDirectory))
                            Directory.CreateDirectory(saveDirectory);

                        File.WriteAllBytes(savePath, fileBytes);
                        var args = new AssetDownloadProgressChangedEventArgs()
                        {
                            FileDownloaded = fingerprintFile,
                            ProgressPercentage = ((double)(i + 1) / Files.Length) * 100,
                            DownloadedCount = i + 1,
                            DownloadsCount = Files.Length,
                            RetryCount = retryCount
                        };

                        OnDownloadProgressChanged(args);
                        retryCount = 0;
                    }
                }
            }
        }

        /// <summary>
        ///  Downloads files in the <see cref="Files"/> array at the
        /// specified path from the <see cref="DefaultAssetServer"/> asynchronously.
        /// </summary>
        /// <param name="path">Path where to save the downloads.</param>
        public async void DownloadFilesAsync(string path)
        {
            await Task.Run(() => DownloadFiles(path, DefaultAssetServer, false));
        }

        /// <summary>
        /// Downloads files in the <see cref="Files"/> array at the
        /// specified path from the specified server URL asynchronously.
        /// </summary>
        /// <param name="path">Path where to save the downloads.</param>
        /// <param name="serverUrl">URL of where to download the files.</param>
        public async void DownloadFilesAsync(string path, string serverUrl)
        {
            await Task.Run(() => DownloadFiles(path, serverUrl, false));
        }

        /// <summary>
        /// Downloads files in the <see cref="Files"/> array asynchronously at the
        /// specified path from the specified server URL and check SHA-1 if <paramref name="checksum"/> is set to <c>true</c>.
        /// </summary>
        /// <param name="path">Path where to save the downloads.</param>
        /// <param name="serverUrl">URL of where to download the files.</param>
        /// <param name="checksum">
        /// If set to <c>true</c> then it will calculate the SHA-1 of the files downloaded
        /// and redownload the file if the hash incorrect.
        /// </param>
        public async void DownloadFilesAsync(string path, string serverUrl, bool checksum)
        {
            await Task.Run(() => DownloadFiles(path, serverUrl, checksum));
        }

        /// <summary>
        /// The event that is fired when the download progress has changed.
        /// </summary>
        public event EventHandler<AssetDownloadProgressChangedEventArgs> DownloadProgressChanged;
        /// <summary>
        /// Fires the <see cref="DownloadProgressChanged"/> event with the specified <see cref="AssetDownloadProgressChangedEventArgs"/>.
        /// </summary>
        /// <param name="e">The arguments with which to fire the event.</param>
        protected virtual void OnDownloadProgressChanged(AssetDownloadProgressChangedEventArgs e)
        {
            if (DownloadProgressChanged != null)
                DownloadProgressChanged(this, e);
        }

        /// <summary>
        /// The even that is fired when the download has completed.
        /// </summary>
        public event EventHandler<AssetDownloadCompletedEventArgs> DownloadCompleted;
        /// <summary>
        /// Fires the <see cref="DownloadProgressChanged"/> event with the specified <see cref="AssetDownloadProgressChangedEventArgs"/>.
        /// </summary>
        /// <param name="e">The arguments with which to fire the event.</param>
        protected virtual void OnDownloadCompleted(AssetDownloadCompletedEventArgs e)
        {
            if (DownloadCompleted != null)
                DownloadCompleted(this, e);
        }
        #endregion
    }
}
