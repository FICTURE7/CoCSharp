using CoCSharp.Csv;
using CoCSharp.Data.AssetProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CoCSharp.Data
{
    /// <summary>
    /// Defines multiple modes of locking an <see cref="AssetManager"/>.
    /// </summary>
    [Flags]
    public enum AssetManagerLockMode
    {
        /// <summary>
        /// Locks loading of assets.
        /// </summary>
        Loading = 1,

        /// <summary>
        /// Locks unloading of assets.
        /// </summary>
        Unloading = 2,

        /// <summary>
        /// Locks both loading and unloading of assets.
        /// </summary>
        Both = Loading | Unloading
    }

    /// <summary>
    /// Provides methods to manage Clash of Clans assets.
    /// </summary>
    /// 
    /// <remarks>
    /// <see cref="AssetManager"/> can only load one instance of one type of an asset.
    /// </remarks>
    public sealed class AssetManager : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetManager"/> class with the specified path
        /// pointing to the root of the asset directory.
        /// </summary>
        /// <param name="rootDir">Path pointing to the asset directory.</param>
        /// <exception cref="ArgumentNullException"><paramref name="rootDir"/> is null.</exception>
        /// <exception cref="DirectoryNotFoundException"><paramref name="rootDir"/> does not exists.</exception>
        public AssetManager(string rootDir)
        {
            if (string.IsNullOrWhiteSpace(rootDir))
                throw new ArgumentNullException(nameof(rootDir));
            if (!Directory.Exists(rootDir))
                throw new DirectoryNotFoundException($"Could not find directory at '{Path.GetFullPath(rootDir)}'.");

            _rootDir = rootDir;
            _providers = new Dictionary<Type, AssetProvider>();

            // Register the AssetProviders.
            var tableProvider = new CsvDataTableAssetProvider();
            RegisterProvider(typeof(CsvDataTable<>), tableProvider);
            // Register typeof(CsvDataTable) to the same instance of the type CsvDataRow<>.
            // Which enables us to do things like this AssetManaget.Get<CsvDataTable>();
            RegisterProvider(typeof(CsvDataTableCollection), tableProvider);

            _table = tableProvider.Table;

            var fingerprintJsonPath = Path.Combine(_rootDir, "fingerprint.json");
            var fingerprintJson = File.ReadAllText(fingerprintJsonPath);
            _fingerprint = Fingerprint.FromJson(fingerprintJson);
        }
        #endregion

        #region Fields & Properties
        private bool _disposed;
        private Fingerprint _fingerprint;
        // How the AssetManager has been locked.
        private AssetManagerLockMode _mode;
        // 'Shortcut' to the CsvDataTableAssetProvider's CsvDataTableCollection so that
        // we don't need to look it up the dictionary.
        private readonly CsvDataTableCollection _table;
        private readonly string _rootDir;
        // Dictionary of AssetLoaders that will load assets of the specified type.
        private readonly Dictionary<Type, AssetProvider> _providers;

        /// <summary>
        /// Gets or sets the default <see cref="AssetManager"/> instance.
        /// </summary>
        [Obsolete]
        public static AssetManager Default { get; set; }

        /// <summary>
        /// Gets the current <see cref="AssetManagerLockMode"/> of the <see cref="AssetManager"/>.
        /// </summary>
        public AssetManagerLockMode LockMode => _mode;

        /// <summary>
        /// Gets the path pointing to the asset directory.
        /// </summary>
        public string RootDirectory => _rootDir;

        /// <summary>
        /// Gets the <see cref="Data.Fingerprint"/> of the assets.
        /// </summary>
        public Fingerprint Fingerprint => _fingerprint;

        /// <summary>
        /// Gets the <see cref="CsvDataColumnCollection"/> instance.
        /// </summary>
        /// 
        /// <exception cref="ObjectDisposedException">The current instance of the <see cref="AssetManager"/> is disposed.</exception>
        public CsvDataTableCollection DataTables
        {
            get
            {
                CheckDispose();

                return _table;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Locks the <see cref="AssetManager"/> with the specified <see cref="AssetManagerLockMode"/>.
        /// </summary>
        /// <param name="mode">Mode in which to lock the <see cref="AssetManager"/>.</param>
        public void Lock(AssetManagerLockMode mode)
        {
            CheckDispose();

            _mode |= mode;
        }

        /// <summary>
        /// Loads the specified asset type at the specified path relative to <see cref="RootDirectory"/> in memory.
        /// </summary>
        /// <typeparam name="TAsset">Type of asset to load.</typeparam>
        /// 
        /// <param name="path">Path to asset file relative to <see cref="RootDirectory"/>.</param>
        /// 
        /// <exception cref="ObjectDisposedException">The current instance of the <see cref="AssetManager"/> is disposed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="InvalidOperationException"><typeparamref name="TAsset"/> is already loaded.</exception>
        /// <exception cref="InvalidOperationException">Unknown asset type.</exception>
        /// <exception cref="FileNotFoundException">File at <paramref name="path"/> does not exists.</exception>
        public void Load<TAsset>(string path) where TAsset : new()
        {
            CheckDispose();
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (_mode.HasFlag(AssetManagerLockMode.Loading))
                throw new InvalidOperationException("Loading of assets is locked.");
            if (IsLoaded<TAsset>())
                throw new InvalidOperationException("Asset of specified type is already loaded.");

            // Path pointing to asset relative to _assetPath.
            var newPath = Path.Combine(_rootDir, path);
            if (!File.Exists(newPath))
                throw new FileNotFoundException($"File at '{Path.GetFullPath(newPath)}' does not exists.");

            var type = typeof(TAsset);
            // Gets the AssetLoader associated with the type.
            var provider = GetProvider(type);

            Debug.WriteLine($"Loading asset of type {type} with AssetProvider {provider} at {path}.");

            provider.LoadAsset(type, newPath);
        }

        /// <summary>
        /// Unloads the specified asset type.
        /// </summary>
        ///
        /// <typeparam name="TAsset">Type of asset to unload.</typeparam>
        /// 
        /// <exception cref="ObjectDisposedException">The current instance of the <see cref="AssetManager"/> is disposed.</exception>
        /// <exception cref="InvalidOperationException">Unknown asset type.</exception>
        public void UnloadAsset<TAsset>() where TAsset : new()
        {
            CheckDispose();

            if (_mode.HasFlag(AssetManagerLockMode.Unloading))
                throw new InvalidOperationException("Unloading of assets is locked.");

            var type = typeof(TAsset);
            var provider = GetProvider(type);
            if (!provider.UnloadAsset(type))
                throw new InvalidOperationException("Unable to unload asset of specified type.");
        }

        /// <summary>
        /// Returns a value indicating whether the specified asset type is loaded.
        /// </summary>
        /// <typeparam name="TAsset">Type of asset to check whether its loaded.</typeparam>
        /// 
        /// <returns><c>true</c> if the asset is loaded; otherwise, <c>false</c>.</returns>
        /// 
        /// <exception cref="ObjectDisposedException">The current instance of the <see cref="AssetManager"/> is disposed.</exception>
        /// <exception cref="InvalidOperationException">Unknown asset type.</exception>
        public bool IsLoaded<TAsset>()
        {
            CheckDispose();

            var type = typeof(TAsset);
            var provider = GetProvider(type);
            return provider.GetAsset(type) != null;
        }

        /// <summary>
        /// Returns the asset of the specified type that was loaded.
        /// </summary>
        /// <typeparam name="TAsset">Type of asset to return.</typeparam>
        /// 
        /// <returns><typeparamref name="TAsset"/> that was loaded.</returns>
        /// 
        /// <exception cref="ObjectDisposedException">The current instance of the <see cref="AssetManager"/> is disposed.</exception>
        /// <exception cref="InvalidOperationException">Unknown asset type.</exception>
        public TAsset Get<TAsset>()
        {
            CheckDispose();

            var type = typeof(TAsset);

            var provider = GetProvider(type);
            var asset = provider.GetAsset(type);

            return (TAsset)asset;
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="AssetManager"/> class.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            // Dispose our providers.
            foreach (var vk in _providers)
                vk.Value.Dispose();

            _providers.Clear();
            _disposed = true;
        }

        private AssetProvider GetProvider(Type type)
        {
            var loader = (AssetProvider)null;

            // If the type is generic, we use its type definition.
            // E.g: CsvDataTable<BuildingData> -> CsvDataTable<>.
            if (type.IsGenericType)
                type = type.GetGenericTypeDefinition();

            // Look for loader in dictionary.
            if (!_providers.TryGetValue(type, out loader))
                throw new InvalidOperationException($"Couldn't find loader for the specified type '{type}'.");

            return loader;
        }

        // Registers an AssetProvider for the specified type.
        private void RegisterProvider(Type type, AssetProvider loader)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (loader == null)
                throw new ArgumentNullException(nameof(loader));

            if (_providers.ContainsKey(type))
                throw new Exception("Already have a loader registered for the specified type.");

            _providers.Add(type, loader);
        }

        private void CheckDispose()
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Cannot access disposed AssetManager object.");
        }
        #endregion
    }
}
