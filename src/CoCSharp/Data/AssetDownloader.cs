using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace CoCSharp.Data
{
    /// <summary>
    /// Provides methods to download Clash of Clans assets.
    /// </summary>
    public class AssetDownloader : IDisposable
    {
        /// <summary>
        /// Official server from which assets are downloaded from. This field is readonly.
        /// </summary>
        public static readonly Uri OfficialAssetServer =
            new Uri("http://b46f744d64acd2191eda-3720c0374d47e9a0dd52be4d281c260f.r11.cf2.rackcdn.com/");

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetDownloader"/> class with the specified
        /// master hash. The <see cref="OfficialAssetServer"/> will be used as <see cref="Uri"/>.
        /// </summary>
        /// <param name="masterHash"></param>
        /// <exception cref="ArgumentNullException"><paramref name="masterHash"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="masterHash"/> is not a valid hex-string.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="masterHash"/>'s length is not 40 characters long.</exception>
        public AssetDownloader(string masterHash) : this(masterHash, OfficialAssetServer)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetDownloader"/> class with the specified
        /// master hash and <see cref="Uri"/>.
        /// </summary>
        /// <param name="masterHash">Master hash of the assets.</param>
        /// <param name="uri"><see cref="Uri"/> where to download the assets.</param>
        /// <exception cref="ArgumentNullException"><paramref name="masterHash"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="masterHash"/> is not a valid hex-string.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="masterHash"/>'s length is not 40 characters long.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="uri"/> is null.</exception>
        public AssetDownloader(string masterHash, Uri uri)
        {
            if (masterHash == null)
                throw new ArgumentNullException("masterHash");
            if (!InternalUtils.IsValidHexString(masterHash))
                throw new ArgumentException("masterHash must be a valid hex-string.", "masterHash");
            if (masterHash.Length != 40)
                throw new ArgumentOutOfRangeException("masterHash", "masterHash must be 40 characters long.");
            if (uri == null)
                throw new ArgumentNullException("uri");

            _uri = uri;
            _masterHash = masterHash;
            _sha1 = SHA1.Create();
            _webClient = new WebClient();
        }

        /// <summary>
        /// Gets the master hash of asset directory.
        /// </summary>
        public string MasterHash { get { return _masterHash; } }

        /// <summary>
        /// Gets the <see cref="Uri"/> pointing to the asset server.
        /// </summary>
        public Uri AssetServerUri { get { return _uri; } }

        private bool _disposed;
        private readonly string _masterHash;
        private readonly Uri _uri;
        private readonly SHA1 _sha1;
        private readonly WebClient _webClient;

        /// <summary>
        /// Downloads the assets to the specified destination directory and checks the SHA1 hashes.
        /// The <see cref="AssetDownloader"/> will download the <see cref="Fingerprint"/> from the asset server.
        /// </summary>
        /// <param name="dstDir">Destination directory of the downloads.</param>
        /// <exception cref="ObjectDisposedException">The current instance of the <see cref="AssetDownloader"/> is disposed.</exception>
        /// <exception cref="DirectoryNotFoundException"><paramref name="dstDir"/> does not exist.</exception>
        public void DownloadAssets(string dstDir)
        {
            CheckDispose();

            if (dstDir == null)
                dstDir = string.Empty;
            else if (dstDir != string.Empty && !Directory.Exists(dstDir))
                throw new DirectoryNotFoundException("Could not find directory at '" + dstDir + "'.");

            var uriBuilder = new UriBuilder(_uri) { Path = _masterHash };
            var remoteRootPath = uriBuilder.Uri;
            var fingerprint = DownloadFingerprint(remoteRootPath);

            // checkHash = true by default.
            InternalDownloadAssets(remoteRootPath, fingerprint, dstDir, true);
        }

        /// <summary>
        /// Downloads the assets to the specified destination directory and whether to check SHA1 hashes or not.
        /// The <see cref="AssetDownloader"/> will download the <see cref="Fingerprint"/> from the asset server.
        /// </summary>
        /// <param name="dstDir">Destination directory of the downloads.</param>
        /// <param name="checkHash">
        /// If set to <c>true</c> the <see cref="AssetDownloader"/> will check if the SHA1 hash of the downloads matches
        /// the ones of local files; otherwise no.
        /// </param>
        /// <exception cref="ObjectDisposedException">The current instance of the <see cref="AssetDownloader"/> is disposed.</exception>
        /// <exception cref="DirectoryNotFoundException"><paramref name="dstDir"/> does not exist.</exception>
        public void DownloadAssets(string dstDir, bool checkHash)
        {
            CheckDispose();

            if (dstDir == null)
                dstDir = string.Empty;
            else
            {
                if (dstDir != string.Empty && !Directory.Exists(dstDir))
                    throw new DirectoryNotFoundException("Could not find directory at '" + dstDir + "'.");
            }

            var uriBuilder = new UriBuilder(_uri) { Path = _masterHash };
            var remoteRootPath = uriBuilder.Uri;
            var fingerprint = DownloadFingerprint(remoteRootPath);

            InternalDownloadAssets(remoteRootPath, fingerprint, dstDir, checkHash);
        }

        /// <summary>
        /// Downloads the assets according the specified <see cref="Fingerprint"/> to the specified destination directory and
        /// whether to check SHA1 hashes or not.</summary>
        /// <param name="fingerprint"><see cref="Fingerprint"/> from which SHA1 hashes and paths will be fetched.</param>
        /// <param name="dstDir">Destination directory of the downloads.</param>
        /// <param name="checkHash">
        /// If set to <c>true</c> the <see cref="AssetDownloader"/> will check if the SHA1 hash of the downloads matches
        /// the ones of local files; otherwise no.
        /// </param>
        /// <exception cref="ObjectDisposedException">The current instance of the <see cref="AssetDownloader"/> is disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="fingerprint"/> is null.</exception>
        /// <exception cref="DirectoryNotFoundException"><paramref name="dstDir"/> does not exist.</exception>
        public void DownloadAssets(Fingerprint fingerprint, string dstDir, bool checkHash)
        {
            CheckDispose();

            if (fingerprint == null)
                throw new ArgumentNullException("fingerprint");

            if (dstDir == null)
                dstDir = string.Empty;
            else
            {
                if (dstDir != string.Empty && !Directory.Exists(dstDir))
                    throw new DirectoryNotFoundException("Could not find directory at '" + dstDir + "'.");
            }

            var uriBuilder = new UriBuilder(_uri) { Path = _masterHash };
            var remoteRootPath = uriBuilder.Uri;

            InternalDownloadAssets(remoteRootPath, fingerprint, dstDir, checkHash);
        }

        private void InternalDownloadAssets(Uri remoteRootPath, Fingerprint fingerprint, string dstDir, bool checkHash)
        {
            var localRootDir = Path.Combine(dstDir, _masterHash);

            // Create a new directory called as the value of _masterHash.
            if (!Directory.Exists(localRootDir))
                Directory.CreateDirectory(localRootDir);

            for (int i = 0; i < fingerprint.Count; i++)
            {
                var file = fingerprint[i];

                // Root directory's name of the file's path.
                var dirName = Path.GetDirectoryName(file.Path);
                // Local directory of the file's path 
                var localDirPath = Path.Combine(localRootDir, dirName);

                // Make sure the directory exists first.
                if (!Directory.Exists(localDirPath))
                    Directory.CreateDirectory(localDirPath);

                var localFilePath = Path.Combine(localRootDir, file.Path);

                if (checkHash)
                {
                    // If we're checking the SHA1 and if a file already exists with the same name/path.
                    if (File.Exists(localFilePath))
                    {
                        var existingFileBytes = File.ReadAllBytes(localFilePath);
                        var existingFileHash = _sha1.ComputeHash(existingFileBytes);

                        // If the existing file have the same SHA1 as the one in the fingerprint
                        // we continue and ignore it.
                        if (InternalUtils.CompareByteArray(existingFileHash, file.Hash))
                            continue;
                    }
                }

                var fileBytes = DownloadFile(remoteRootPath, file.Path);

                var args = new AssetDownloadProgressChangedEventArgs()
                {
                    DownloadedCount = i + 1,
                    FileDownloaded = file,
                    NextDownload = fingerprint[i],
                    ProgressPercentage = ((i + 1) / (double)fingerprint.Count) * 100,
                };

                OnDownloadProgressChanged(args);
                File.WriteAllBytes(localFilePath, fileBytes);
            }

            OnDownloadCompleted(new AssetDownloadCompletedEventArgs());
        }

        /// <summary>
        /// The event raised when progress was made during the download process.
        /// </summary>
        public event EventHandler<AssetDownloadProgressChangedEventArgs> DownloadProgressChanged;
        /// <summary>
        /// Use this method to trigger the <see cref="DownloadProgressChanged"/> event.
        /// </summary>
        /// <param name="e">The arguments.</param>
        protected virtual void OnDownloadProgressChanged(AssetDownloadProgressChangedEventArgs e)
        {
            if (DownloadProgressChanged != null)
                DownloadProgressChanged(this, e);
        }

        /// <summary>
        /// The event raised when the whole download is complete.
        /// </summary>
        public event EventHandler<AssetDownloadCompletedEventArgs> DownloadCompleted;
        /// <summary>
        /// Use this method to trigger the <see cref="DownloadCompleted"/> event.
        /// </summary>
        /// <param name="e">The arguments.</param>
        protected virtual void OnDownloadCompleted(AssetDownloadCompletedEventArgs e)
        {
            if (DownloadCompleted != null)
                DownloadCompleted(this, e);
        }

        private Fingerprint DownloadFingerprint(Uri rootPath)
        {
            var builder = new UriBuilder(rootPath);
            builder.Path = Path.Combine(builder.Path, "fingerprint.json");
            var fingerprintPath = builder.Uri;
            var fingerprintJson = _webClient.DownloadString(fingerprintPath);

            return Fingerprint.FromJson(fingerprintJson);
        }

        private byte[] DownloadFile(Uri rootPath, string path)
        {
            var builder = new UriBuilder(rootPath);
            builder.Path = Path.Combine(builder.Path, path);
            var assetPath = builder.Uri;
            var assetBytes = _webClient.DownloadData(assetPath);

            return assetBytes;
        }

        private void CheckDispose()
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Cannot access the AssetDownloader object because it was disposed.");
        }

        /// <summary>
        /// Releases all unmanaged and optionally managed resources used by this <see cref="AssetDownloader"/> instance.
        /// </summary>
        /// <param name="disposing">Whether to dispose managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _sha1.Dispose();
                    _webClient.Dispose();
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// Releases all resources used by this <see cref="AssetDownloader"/> instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
